using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Memcached.ClientLibrary;


/**
 * Memcached C# client, connection pool for Socket IO
 * Copyright (c) 2005
 *
 * This module is Copyright (c) 2005 Tim Gebhardt
 * Based on code from Greg Whalin and Richard Russo from the 
 * Java memcached client api:
 * http://www.whalin.com/memcached/
 * All rights reserved.
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later
 * version.
 *
 * This library is distributed in the hope that it will be
 * useful, but WITHOUT ANY WARRANTY; without even the implied
 * warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR
 * PURPOSE.  See the GNU Lesser General Public License for more
 * details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307  USA
 *
 * @author Tim Gebhardt <tim@gebhardtcomputing.com>
 *
 * @version 1.0
 */
namespace Redissharp
{


    /// <summary>
    /// This class is a connection pool for maintaning a pool of persistent connections
    /// to memcached servers.
    /// 
    /// The pool must be initialized prior to use. This should typically be early on
    /// in the lifecycle of the application instance.
    /// </summary>
    /// <example>
    /// //Using defaults
    ///	String[] serverlist = {"cache0.server.com:12345", "cache1.server.com:12345"};
    ///
    ///	SockIOPool pool = SockIOPool.GetInstance();
    ///	pool.SetServers(serverlist);
    ///	pool.Initialize();	
    ///		
    ///	
    ///	//An example of initializing using defaults and providing weights for servers:
    ///	String[] serverlist = {"cache0.server.com:12345", "cache1.server.com:12345"};
    ///	int[] weights   = new int[]{5, 2};
    /// 
    /// SockIOPool pool = SockIOPool.GetInstance();
    ///	pool.SetServers(serverlist);
    ///	pool.SetWeights(weights);	
    ///	pool.Initialize();	
    ///	
    ///	
    /// //An example of initializing overriding defaults:
    /// String[] serverlist     = {"cache0.server.com:12345", "cache1.server.com:12345"};
    /// int[] weights       = new int[]{5, 2};	
    /// int initialConnections  = 10;
    /// int minSpareConnections = 5;
    /// int maxSpareConnections = 50;	
    /// long maxIdleTime        = 1000 * 60 * 30;	// 30 minutes
    /// long maxBusyTime        = 1000 * 60 * 5;	// 5 minutes
    /// long maintThreadSleep   = 1000 * 5;			// 5 seconds
    /// int	socketTimeOut       = 1000 * 3;			// 3 seconds to block on reads
    /// int	socketConnectTO     = 1000 * 3;			// 3 seconds to block on initial connections.  If 0, then will use blocking connect (default)
    /// boolean failover        = false;			// turn off auto-failover in event of server down	
    /// boolean nagleAlg        = false;			// turn off Nagle's algorithm on all sockets in pool	
    /// 
    /// SockIOPool pool = SockIOPool.getInstance();
    /// pool.Servers = serverlist;
    /// pool.Weights = weights;	
    /// pool.InitConnections = initialConnections;
    /// pool.MinConnections = minSpareConnections;
    /// pool.MaxConnections = maxSpareConnections;
    /// pool.MaxIdle = maxIdleTime;
    /// pool.MaxBusy = maxBusyTime;
    /// pool.MaintenanceSleep = maintThreadSleep;
    /// pool.SocketTimeout = socketTimeOut;
    /// pool.SocketConnectTimeOut = socketConnectTO;	
    /// pool.Nagle = nagleAlg;	
    /// pool.HashingAlg = SockIOPool.HashingAlgorithms.NEW_COMPAT_HASH;	
    /// pool.Initialize();	
    /// 
    /// 
    /// //The easiest manner in which to initialize the pool is to set the servers and rely on defaults as in the first example.
    /// //After pool is initialized, a client will request a Socketobject by calling getSock with the cache key
    /// //The client must always close the Socketobject when finished, which will return the connection back to the pool.
    /// //An example of retrieving a Socketobject:
    /// SockIOPool.Socketsock = SockIOPool.GetInstance().GetSock(key);
    /// try 
    /// {
    ///		sock.write("version\r\n");	
    ///		sock.flush();	
    ///		Console.WriteLine("Version: " + sock.ReadLine());	
    /// }
    ///	catch(IOException ioe) { Console.WriteLine("io exception thrown") }
    /// finally { sock.Close();	}
    /// 
    ///	</example>
    public class SockPool
    {
        // logger
        //private static ILog Log = LogManager.GetLogger(typeof(SockIOPool));

        // store instances of pools
        private static Hashtable Pools = new Hashtable();

        // Pool data
        private MaintenanceThread _maintenanceThread;
        private bool _initialized;
        private int _maxCreate = 1;	// this will be initialized by pool when the pool is initialized
        private Hashtable _createShift;

        // initial, min and max pool sizes
        private int _poolMultiplier = 4;
        private int _initConns = 3;
        private int _minConns = 3;
        private int _maxConns = 10;
        private long _maxIdle = 1000 * 60 * 3; // max idle time for avail sockets (in milliseconds)
        private long _maxBusyTime = 1000 * 60 * 5; // max idle time for avail sockets (in milliseconds)
        private long _maintThreadSleep = 1000 * 5; // maintenance thread sleep time (in milliseconds)
        private int _socketTimeout = 1000 * 10; // default timeout of socket reads
        private int _socketConnectTimeout = 50; // default timeout of socket connections
        private bool _failover = true; // default to failover in event of cache server dead
        private bool _nagle = true; // enable/disable Nagle's algorithm
        private HashingAlgorithm _hashingAlgorithm = HashingAlgorithm.Native; // default to using the native hash as it is the fastest

        // list of all servers
        private ArrayList _servers;
        private ArrayList _weights;
        private ArrayList _buckets;

        // dead server map
        private Hashtable _hostDead;
        private Hashtable _hostDeadDuration;

        // map to hold all available sockets
        private Hashtable _availPool;

        // map to hold busy sockets
        private Hashtable _busyPool;

        // empty constructor
        protected SockPool() { }
        ~SockPool() 
        {
            Shutdown();
        }
        
        /// <summary>
        /// Factory to create/retrieve new pools given a unique poolName.
        /// </summary>
        /// <param name="poolName">unique name of the pool</param>
        /// <returns>instance of SockIOPool</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static SockPool GetInstance(String poolName)
        {
            if(Pools.ContainsKey(poolName))
                return (SockPool)Pools[poolName];

            SockPool pool = new SockPool();
            Pools[poolName] = pool;

            return pool;
        }

        /// <summary>
        /// Single argument version of factory used for back compat.
        /// Simply creates a pool named "default". 
        /// </summary>
        /// <returns>instance of SockIOPool</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static SockPool GetInstance()
        {
            return GetInstance("default instance");
        }

		/// <summary>
		/// Gets the list of all cache servers
		/// </summary>
		/// <value>string array of servers [host:port]</value>
		public ArrayList Servers
		{
			get { return _servers; }
		}

		/// <summary>
		/// Sets the list of all cache servers
		/// </summary>
		/// <param name="servers">string array of servers [host:port]</param>
		public void SetServers(string[] servers)
		{
			SetServers(new ArrayList(servers));
		}

		/// <summary>
		/// Sets the list of all cache servers
		/// </summary>
		/// <param name="servers">string array of servers [host:port]</param>
		public void SetServers(ArrayList servers)
		{
			_servers = servers;
		}

		/// <summary>
		/// Gets or sets the list of weights to apply to the server list
		/// </summary>
		/// <value>
		/// This is an int array with each element corresponding to an element
		/// in the same position in the server string array <see>Servers</see>.
		/// </value>
		public ArrayList Weights
		{
			get { return _weights; }
		}

		/// <summary>
		/// sets the list of weights to apply to the server list
		/// </summary>
		/// <param name="weights">
		/// This is an int array with each element corresponding to an element
		/// in the same position in the server string array <see>Servers</see>.
		/// </param>
		public void SetWeights(int[] weights)
		{
			SetWeights(new ArrayList(weights));
		}

		/// <summary>
		/// sets the list of weights to apply to the server list
		/// </summary>
		/// <param name="weights">
		/// This is an int array with each element corresponding to an element
		/// in the same position in the server string array <see>Servers</see>.
		/// </param>
		public void SetWeights(ArrayList weights)
		{
			_weights = weights;
		}

        /// <summary>
        /// Gets or sets the initial number of connections per server setting in the available pool.
        /// </summary>
        /// <value>int number of connections</value>
        public int InitConnections
        {
            get { return _initConns; }
            set { _initConns = value; }
        }

        /// <summary>
        /// Gets or sets the minimum number of spare connections to maintain in our available pool
        /// </summary>
        public int MinConnections
        {
            get { return _minConns; }
            set { _minConns = value; }
        }

        /// <summary>
        /// Gets or sets the maximum number of spare connections allowed in our available pool.
        /// </summary>
        public int MaxConnections
        {
            get { return _maxConns; }
            set { _maxConns = value; }
        }

        /// <summary>
        /// Gets or sets the maximum idle time for threads in the avaiable pool.
        /// </summary>
        public long MaxIdle
        {
            get { return _maxIdle; }
            set { _maxIdle = value; }
        }

        /// <summary>
        /// Gets or sets the maximum busy time for threads in the busy pool
        /// </summary>
        /// <value>idle time in milliseconds</value>
        public long MaxBusy
        {
            get { return _maxBusyTime; }
            set { _maxBusyTime = value; }
        }

        /// <summary>
        /// Gets or sets the sleep time between runs of the pool maintenance thread.
        /// If set to 0, then the maintenance thread will not be started;
        /// </summary>
        /// <value>sleep time in milliseconds</value>
        public long MaintenanceSleep
        {
            get { return _maintThreadSleep; }
            set { _maintThreadSleep = value; }
        }

        /// <summary>
        /// Gets or sets the socket timeout for reads
        /// </summary>
        /// <value>timeout time in milliseconds</value>
        public int SocketTimeout
        {
            get { return _socketTimeout; }
            set { _socketTimeout = value; }
        }

        /// <summary>
        /// Gets or sets the socket timeout for connects.
        /// </summary>
        /// <value>timeout time in milliseconds</value>
        public int SocketConnectTimeout
        {
            get { return _socketConnectTimeout; }
            set { _socketConnectTimeout = value; }
        }

        /// <summary>
        /// Gets or sets the failover flag for the pool.
        /// 
        /// If this flag is set to true and a socket fails to connect,
        /// the pool will attempt to return a socket from another server
        /// if one exists.  If set to false, then getting a socket
        /// will return null if it fails to connect to the requested server.
        /// </summary>
        public bool Failover
        {
            get { return _failover; }
            set { _failover = value; }
        }

        /// <summary>
        /// Gets or sets the Nagle algorithm flag for the pool.
        /// 
        /// If false, will turn off Nagle's algorithm on all sockets created.
        /// </summary>
        public bool Nagle
        {
            get { return _nagle; }
            set { _nagle = value; }
        }

        /// <summary>
        /// Gets or sets the hashing algorithm we will use.
        /// </summary>
        public HashingAlgorithm HashingAlgorithm
        {
            get { return _hashingAlgorithm; }
            set { _hashingAlgorithm = value; }
        }

        /// <summary>
        /// Internal private hashing method.
        /// 
        /// This is the original hashing algorithm from other clients.
        /// Found to be slow and have poor distribution.
        /// </summary>
        /// <param name="key">string to hash</param>
        /// <returns>hashcode for this string using memcached's hashing algorithm</returns>
        private static int OriginalHashingAlgorithm(string key)
        {
            int hash = 0;
            char[] cArr = key.ToCharArray();

            for(int i = 0; i < cArr.Length; ++i)
            {
                hash = (hash * 33) + cArr[i];
            }

            return hash;
        }

        /// <summary>
        /// Internal private hashing method.
        /// 
        /// This is the new hashing algorithm from other clients.
        /// Found to be fast and have very good distribution.
        /// 
        /// UPDATE: this is dog slow under java.  Maybe under .NET? 
        /// </summary>
        /// <param name="key">string to hash</param>
        /// <returns>hashcode for this string using memcached's hashing algorithm</returns>
        private static int NewHashingAlgorithm(string key)
        {
            CRCTool checksum = new CRCTool();
            checksum.Init(CRCTool.CRCCode.CRC32);
            int crc = (int)checksum.crctablefast(UTF8Encoding.UTF8.GetBytes(key));

            return (crc >> 16) & 0x7fff;
        }

        /// <summary>
        /// Initializes the pool
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Initialize() 
		{
			// check to see if already initialized
            if (_initialized
                && _buckets != null
                && _availPool != null
                && _busyPool != null)
            {

                throw new Exception("initializing initialized pool");

                return;
            }
            
			// initialize empty maps
			_buckets     = new ArrayList();
			_availPool   = new Hashtable(_servers.Count * _initConns);
			_busyPool    = new Hashtable(_servers.Count * _initConns);
			_hostDeadDuration = new Hashtable();
			_hostDead    = new Hashtable();
			_createShift = new Hashtable();
			_maxCreate   = (_poolMultiplier > _minConns) ? _minConns : _minConns / _poolMultiplier;		// only create up to maxCreate connections at once



			// if servers is not set, or it empty, then
			// throw a runtime exception
			if(_servers == null || _servers.Count <= 0) 
			{

				throw new ArgumentException("initialize with no servers");
			}

			for(int i = 0; i < _servers.Count; i++) 
			{
				// add to bucket
				// with weights if we have them 
				if(_weights != null && _weights.Count > i) 
				{
					for(int k = 0; k < ((int)_weights[i]); k++) 
					{
						_buckets.Add(_servers[i]);

					}
				}
				else 
				{
					_buckets.Add(_servers[i]);

				}

				// create initial connections


				for(int j = 0; j < _initConns; j++) 
				{
					Socket socket = CreateSocket((string)_servers[i]);
					if(socket == null) 
					{

						break;
					}

					AddSocketToPool(_availPool, (string)_servers[i], socket);

				}
			}

			// mark pool as initialized
			_initialized = true;

			// start maint thread TODO: re-enable
            
		}
        /// <summary>
        /// ���������߳�
        /// </summary>
        /// <returns></returns>
        public bool OpenMaintenance() 
        {
            if (_maintThreadSleep > 0)
                this.StartMaintenanceThread();
            return true;
        }
        /// <summary>
        /// ���������߳�
        /// </summary>
        /// <returns></returns>
        public bool CloseMaintenance()
        {
            if (_maintThreadSleep > 0)
                this.StartMaintenanceThread();
            return true;
        }
        /// <summary>
        /// Returns the state of the pool
        /// </summary>
        /// <returns>returns <c>true</c> if initialized</returns>
        public bool Initialized
        {
            get { return _initialized; }
        }
        /// <summary>
        /// Gets whether or not the socket is connected.  Returns <c>true</c> if it is.
        /// </summary>
        public bool IsConnected(Socket socket)
        {
            return socket != null && socket.Connected; 
        }

        /// <summary>
        /// �ر�����
        /// </summary>
        /// <param name="socket"></param>
        internal static void TrueCloseSocket(Socket socket) 
        {
            socket.Close();
        }
        private string _host;
        /// <summary>
        /// Creates a new Socketobj for the given server.
        ///
        ///If server fails to connect, then return null and do not try
        ///again until a duration has passed.  This duration will grow
        ///by doubling after each failed attempt to connect.
        /// </summary>
        /// <param name="host">host:port to connect to</param>
        /// <returns>Socketobj or null if failed to create</returns>
        protected Socket CreateSocket(string host)
        {
            Socket socket = null;

            // if host is dead, then we don't need to try again
            // until the dead status has expired
            // we do not try to put back in if failover is off
            if(_failover && _hostDead.ContainsKey(host) && _hostDeadDuration.ContainsKey(host))
            {

                DateTime store = (DateTime)_hostDead[host];
                long expire = ((long)_hostDeadDuration[host]);

                if((store.AddMilliseconds(expire)) > DateTime.Now)
                    return null;
            }
            String[] ip = host.Split(':');
            
            IPAddress ipa = IPAddress.Any;
            if (!IPAddress.TryParse(ip[0], out ipa)) 
            {
                throw new FormatException(host + "������ȷ�ĵ�ַ��ʽ");
            }
            int port = 6379;
            if (ip.Length > 1) 
            {
                if (!int.TryParse(ip[1], out port)) 
                {
                    throw new FormatException(host + "������ȷ�ĵ�ַ��ʽ");
                }
            }
			try
			{
                //socket = new Socket(this, host, _socketTimeout, _socketConnectTimeout, _nagle);
                

                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.NoDelay = true;
                socket.SendTimeout = _socketTimeout;
                socket.Connect(ipa, port);
                if (!IsConnected(socket))
				{

					try
					{
						TrueCloseSocket(socket);
					}
					catch(SocketException ex)
					{
						
						socket = null;
                        throw ex;
					}
				}
			}
			catch(SocketException ex)
			{

				socket = null;
                throw ex;
			}
			catch(ArgumentException ex)
			{

                socket = null;
                throw ex;
			}
			catch(IOException ex)
			{

                socket = null;
                throw ex;
			}

            // if we failed to get socket, then mark
            // host dead for a duration which falls off
            if(socket == null)
            {
                DateTime now = DateTime.Now;
                _hostDead[host] = now;
                long expire = (_hostDeadDuration.ContainsKey(host)) ? (((long)_hostDeadDuration[host]) * 2) : 100;
                _hostDeadDuration[host] = expire;


                // also clear all entries for this host from availPool
                ClearHostFromPool(_availPool, host);
            }
            else
            {

                _hostDead.Remove(host);
                _hostDeadDuration.Remove(host);
                if(_buckets.BinarySearch(host) < 0)
                    _buckets.Add(host);
            }
            _host = host;
            return socket;
        }

        /// <summary>
        /// Returns appropriate Socketobject given
        /// string cache key.
        /// </summary>
        /// <param name="key">hashcode for cache key</param>
        /// <returns>Socketobj connected to server</returns>
        public Socket GetSock(string key)
        {
            return GetSock(key, null);
        }

        /// <summary>
        /// Returns appropriate Socketobject given
        /// string cache key and optional hashcode.
        /// 
        /// Trys to get Socketfrom pool.  Fails over
        /// to additional pools in event of server failure.
        /// </summary>
        /// <param name="key">hashcode for cache key</param>
        /// <param name="hashCode">if not null, then the int hashcode to use</param>
        /// <returns>Socketobj connected to server</returns>
        public Socket GetSock(string key, object hashCode)
        {
			string hashCodeString = "<null>";
			if(hashCode != null)
				hashCodeString = hashCode.ToString();



            if (key == null || key.Length == 0)
            {

                return null;
            }

            if(!_initialized)
            {

                return null;
            }

            // if no servers return null
            if(_buckets.Count == 0)
                return null;

            // if only one server, return it
            if(_buckets.Count == 1)
                return GetConnection((string)_buckets[0]);

            int tries = 0;

            // generate hashcode
            int hv;
            if(hashCode != null)
            {
                hv = (int)hashCode;
            }
            else
            {

                // NATIVE_HASH = 0
                // OLD_COMPAT_HASH = 1
                // NEW_COMPAT_HASH = 2
                switch(_hashingAlgorithm)
                {
                    case HashingAlgorithm.Native:
                        hv = key.GetHashCode();
                        break;

                    case HashingAlgorithm.OldCompatibleHash:
                        hv = OriginalHashingAlgorithm(key);
                        break;

                    case HashingAlgorithm.NewCompatibleHash:
                        hv = NewHashingAlgorithm(key);
                        break;

                    default:
                        // use the native hash as a default
                        hv = key.GetHashCode();
                        _hashingAlgorithm = HashingAlgorithm.Native;
                        break;
                }
            }

            // keep trying different servers until we find one
            while(tries++ <= _buckets.Count)
            {
                // get bucket using hashcode 
                // get one from factory
                int bucket = hv % _buckets.Count;
                if(bucket < 0)
                    bucket += _buckets.Count;

                Socket sock = GetConnection((string)_buckets[bucket]);



                if(sock != null)
                    return sock;

                // if we do not want to failover, then bail here
                if(!_failover)
                    return null;

                // if we failed to get a socket from this server
                // then we try again by adding an incrementer to the
                // current key and then rehashing 
                switch(_hashingAlgorithm)
                {
                    case HashingAlgorithm.Native:
                        hv += ((string)("" + tries + key)).GetHashCode();
                        break;

                    case HashingAlgorithm.OldCompatibleHash:
                        hv += OriginalHashingAlgorithm("" + tries + key);
                        break;

                    case HashingAlgorithm.NewCompatibleHash:
                        hv += NewHashingAlgorithm("" + tries + key);
                        break;

                    default:
                        // use the native hash as a default
                        hv += ((string)("" + tries + key)).GetHashCode();
                        _hashingAlgorithm = HashingAlgorithm.Native;
                        break;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns a Socketobject from the pool for the passed in host.
        /// 
        /// Meant to be called from a more intelligent method
        /// which handles choosing appropriate server
        /// and failover. 
        /// </summary>
        /// <param name="host">host from which to retrieve object</param>
        /// <returns>Socketobject or null if fail to retrieve one</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Socket GetConnection(string host)
        {
            if(!_initialized)
            {

                return null;
            }

            if(host == null)
                return null;

            // if we have items in the pool
            // then we can return it
            if(_availPool != null && !(_availPool.Count == 0))
            {
                // take first connected socket
                Hashtable aSockets = (Hashtable)_availPool[host];

                if(aSockets != null && !(aSockets.Count == 0))
                {
                    foreach(Socket socket in new IteratorIsolateCollection(aSockets.Keys))
                    {
                        if(IsConnected(socket))
                        {


                            // remove from avail pool
                            aSockets.Remove(socket);

                            // add to busy pool
                            AddSocketToPool(_busyPool, host, socket);

                            // return socket
                            return socket;
                        }
                        else
                        {
                            // not connected, so we need to remove it


                            // remove from avail pool
                            aSockets.Remove(socket);
                        }

                    }
                }
            }

            // if here, then we found no sockets in the pool
            // try to create on a sliding scale up to maxCreate
            object cShift = _createShift[host];
            int shift = (cShift != null) ? (int)cShift : 0;

            int create = 1 << shift;
            if(create >= _maxCreate)
            {
                create = _maxCreate;
            }
            else
            {
                shift++;
            }

            // store the shift value for this host
            _createShift[host] = shift;



            for(int i = create; i > 0; i--)
            {
                Socket socket = CreateSocket(host);
                if(socket == null)
                    break;

                if(i == 1)
                {
                    // last iteration, add to busy pool and return sockio
                    AddSocketToPool(_busyPool, host, socket);
                    return socket;
                }
                else
                {
                    // add to avail pool
                    AddSocketToPool(_availPool, host, socket);
                }
            }

            // should never get here
            return null;
        }

        /// <summary>
        /// Adds a socket to a given pool for the given host.
        /// 
        /// Internal utility method. 
        /// </summary>
        /// <param name="pool">pool to add to</param>
        /// <param name="host">host this socket is connected to</param>
        /// <param name="socket">socket to add</param>
        //[MethodImpl(MethodImplOptions.Synchronized)]
        protected static void AddSocketToPool(Hashtable pool, string host, Socket socket)
        {
            if (pool == null)
                return; 

            Hashtable sockets;
            if (host != null && host.Length != 0 && pool.ContainsKey(host))
            {
                sockets = (Hashtable)pool[host];
                if (sockets != null)
                {
                    sockets[socket] = DateTime.Now; //MaxM 1.16.05: Use current DateTime to indicate socker creation time rather than milliseconds since 1970

                    return;
                }
            }

            sockets = new Hashtable();
            sockets[socket] = DateTime.Now; //MaxM 1.16.05: Use current DateTime to indicate socker creation time rather than milliseconds since 1970
            pool[host] = sockets;
        }

        /// <summary>
        /// Removes a socket from specified pool for host.
        /// 
        /// Internal utility method. 
        /// </summary>
        /// <param name="pool">pool to remove from</param>
        /// <param name="host">host pool</param>
        /// <param name="socket">socket to remove</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        protected static void RemoveSocketFromPool(Hashtable pool, string host, Socket socket)
        {
            if (host != null && host.Length == 0 || pool == null)
                return; 

            if(pool.ContainsKey(host))
            {
                Hashtable sockets = (Hashtable)pool[host];
                if(sockets != null)
                {
                    sockets.Remove(socket);
                }
            }
        }

        /// <summary>
        /// Closes and removes all sockets from specified pool for host. 
        /// 
        /// Internal utility method. 
        /// </summary>
        /// <param name="pool">pool to clear</param>
        /// <param name="host">host to clear</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        protected static void ClearHostFromPool(Hashtable pool, string host)
        {
            if (pool == null || host != null && host.Length == 0)
                return;

            if(pool.ContainsKey(host))
            {
                Hashtable sockets = (Hashtable)pool[host];

                if(sockets != null && sockets.Count > 0)
                {
                    foreach(Socket socket in new IteratorIsolateCollection(sockets.Keys))
                    {

                         TrueCloseSocket(socket);


                        sockets.Remove(socket);
                    }
                }
            }
        }

        /// <summary>
        /// Checks a Socketobject in with the pool.
        /// 
        /// This will remove SocketIO from busy pool, and optionally
        /// add to avail pool.
        /// </summary>
        /// <param name="socket">socket to return</param>
        /// <param name="addToAvail">add to avail pool if true</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CheckIn(Socket socket, bool addToAvail)
        {
            if (socket == null)
                return; 

            string host = _host;


            // remove from the busy pool

            RemoveSocketFromPool(_busyPool, host, socket);

            // add to avail pool
            if(addToAvail && IsConnected(socket))
            {

                AddSocketToPool(_availPool, host, socket);
            }
        }

        /// <summary>
        /// Returns a socket to the avail pool.
        /// 
        /// This is called from SockIO.close().  Calling this method
        /// directly without closing the Socketobject first
        /// will cause an IOException to be thrown.
        /// </summary>
        /// <param name="socket">socket to return</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CheckIn(Socket socket)
        {
            CheckIn(socket, true);
        }

        /// <summary>
        /// Closes all sockets in the passed in pool.
        /// 
        /// Internal utility method. 
        /// </summary>
        /// <param name="pool">pool to close</param>
        protected static void ClosePool(Hashtable pool)
        {
            if (pool == null)
                return; 

            foreach(string host in pool.Keys)
            {
                Hashtable sockets = (Hashtable)pool[host];

                foreach(Socket socket in new IteratorIsolateCollection(sockets.Keys))
                {

                        TrueCloseSocket(socket);

                    sockets.Remove(socket);
                }
            }
        }

        /// <summary>
        /// Shuts down the pool.
        /// 
        /// Cleanly closes all sockets.
        /// Stops the maint thread.
        /// Nulls out all internal maps
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Shutdown()
        {

            if(_maintenanceThread != null && _maintenanceThread.IsRunning)
                StopMaintenanceThread();


            ClosePool(_availPool);
            ClosePool(_busyPool);
            _availPool = null;
            _busyPool = null;
            _buckets = null;
            _hostDeadDuration = null;
            _hostDead = null;
            _initialized = false;

        }

        /// <summary>
        /// Starts the maintenance thread.
        /// 
        /// This thread will manage the size of the active pool
        /// as well as move any closed, but not checked in sockets
        /// back to the available pool.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        protected void StartMaintenanceThread()
        {
            if(_maintenanceThread != null)
            {

                if(!_maintenanceThread.IsRunning)
                {
                    _maintenanceThread.Start();
                }
            }
            else
            {
                _maintenanceThread = new MaintenanceThread(this);
                _maintenanceThread.Interval = _maintThreadSleep;
                _maintenanceThread.Start();
            }
        }

        /// <summary>
        /// Stops the maintenance thread.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        protected void StopMaintenanceThread()
        {
            if(_maintenanceThread != null && _maintenanceThread.IsRunning)
                _maintenanceThread.StopThread();
        }

        /// <summary>
        /// Runs self maintenance on all internal pools.
        /// 
        /// This is typically called by the maintenance thread to manage pool size. 
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        protected void SelfMaintain()
        {


            // go through avail sockets and create/destroy sockets
            // as needed to maintain pool settings
            foreach(string host in new IteratorIsolateCollection(_availPool.Keys))
            {
                Hashtable sockets = (Hashtable)_availPool[host];


                // if pool is too small (n < minSpare)
                if(sockets.Count < _minConns)
                {
                    // need to create new sockets
                    int need = _minConns - sockets.Count;


                    for(int j = 0; j < need; j++)
                    {
                        Socket socket = CreateSocket(host);

                        if(socket == null)
                            break;

                        AddSocketToPool(_availPool, host, socket);
                    }
                }
                else if(sockets.Count > _maxConns)
                {
                    // need to close down some sockets
                    int diff = sockets.Count - _maxConns;
                    int needToClose = (diff <= _poolMultiplier)
                        ? diff
                        : (diff) / _poolMultiplier;


                    foreach(Socket socket in new IteratorIsolateCollection(sockets.Keys))
                    {
                        if(needToClose <= 0)
                            break;

                        // remove stale entries
                        DateTime expire = (DateTime)sockets[socket];

                        // if past idle time
                        // then close socket
                        // and remove from pool
                        if((expire.AddMilliseconds(_maxIdle)) < DateTime.Now)
                        {


                                TrueCloseSocket(socket);


                            sockets.Remove(socket);
                            needToClose--;
                        }
                    }
                }

                // reset the shift value for creating new Socketobjects
                _createShift[host] = 0;
            }

            // go through busy sockets and destroy sockets
            // as needed to maintain pool settings
            foreach(string host in _busyPool.Keys)
            {
                Hashtable sockets = (Hashtable)_busyPool[host];


                // loop through all connections and check to see if we have any hung connections
                foreach(Socket socket in new IteratorIsolateCollection(sockets.Keys))
                {
                    // remove stale entries
                    DateTime hungTime = (DateTime)sockets[socket];

                    // if past max busy time
                    // then close socket
                    // and remove from pool
                    if((hungTime.AddMilliseconds(_maxBusyTime)) < DateTime.Now)
                    {

                            TrueCloseSocket(socket);
                       

                        sockets.Remove(socket);
                    }
                }
            }


        }

        /// <summary>
        /// Class which extends thread and handles maintenance of the pool.
        /// </summary>
        private class MaintenanceThread
        {
            private MaintenanceThread() { }

            private Thread _thread;

            private SockPool _pool;
            private long _interval = 1000 * 3; // every 3 seconds
            private bool _stopThread;

            public MaintenanceThread(SockPool pool)
            {
                _thread = new Thread(new ThreadStart(Maintain));
                _pool = pool;
            }

            public long Interval
            {
                get { return _interval; }
                set { _interval = value; }
            }

            public bool IsRunning
            {
                get { return _thread.IsAlive; }
            }

            /// <summary>
            /// Sets stop variable and interups and wait
            /// </summary>
            public void StopThread()
            {
                _stopThread = true;
                _thread.Interrupt();
            }

            /// <summary>
            /// The logic of the thread.
            /// </summary>
            private void Maintain()
            {
                while(!_stopThread)
                {

                        Thread.Sleep((int)_interval);

                        // if pool is initialized, then
                        // run the maintenance method on itself
                        if(_pool.Initialized)
                            _pool.SelfMaintain();


                }
            }

            /// <summary>
            /// Start the thread
            /// </summary>
            public void Start()
            {
                _stopThread = false;
                _thread.Start();
            }
        }

        //private static ResourceManager _resourceManager = new ResourceManager("Memcached.ClientLibrary.StringMessages", typeof(SockIOPool).Assembly);
        //private static string GetLocalizedString(string key)
        //{
        //    return _resourceManager.GetString(key);
        //}
    }
}