using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace Buffalo.Kernel.NetClients
{
    public class NetConnection:IDisposable
    {
        Socket client;

        
        IPEndPoint endPoint;
        
        
        public NetConnection(string ip,int port,ProtocolType type) 
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, type);
            endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            
        }
        public NetConnection(string ip, int port)
            :this(ip,port,ProtocolType.Tcp)
        {

        }

        public NetConnection(int port)
            : this("127.0.0.1", port, ProtocolType.Tcp)
        {

        }

        public NetConnection(Socket client)
        {
            this.client = client;
            endPoint = (IPEndPoint)client.RemoteEndPoint;
        }
        public Socket Client
        {
            get { return client; }

        }
        /// <summary>
        /// IP״̬
        /// </summary>
        public IPEndPoint EndPoint
        {
            get { return endPoint; }

        }
        /// <summary>
        /// ������Ϣ��ָ��λ��
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public int SendTo(byte[] content)
        {
            OpenConnection();
            int totalbytes = content.Length;
            client.Send(BitConverter.GetBytes(totalbytes));
            int sbytes = 0;
            while (sbytes < totalbytes)
                sbytes += client.Send(content, sbytes, totalbytes - sbytes, 0);
            return 0;
        }

        /// <summary>
        /// ������Ϣ��ָ��λ��
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public int SendTo(string message)
        {
            return SendTo(System.Text.Encoding.Default.GetBytes(message));
        }

        /// <summary>
        /// ������Ϣ��ָ��λ��
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public byte[] Receive()
        {
            OpenConnection();
            
            //if (isMsg)
            //    ret = GetSocketMsg(client);
            //else
            //    ret = GetSocketData(client);

            return GetData(client);
        }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="stm"></param>
        /// <returns></returns>
        public static byte[] GetData(Socket socket)
        {
            int bytes = 0;
            byte[] lengthBuffer=new byte[4];
            bytes=socket.Receive(lengthBuffer, 0);
            int length = BitConverter.ToInt32(lengthBuffer, 0);
            byte[] buffer = new byte[length];
            int recs = 0;
            while (recs < length)
            {
                recs+=socket.Receive(buffer, recs, length - recs,0);
            }
            return buffer;
        }

        /// <summary>
        /// ��ȡ Socket ����Ϣ��
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static byte[] GetSocketMsg(Socket socket)
        {
            byte[] buffer = new byte[1024 * 1024];
            int b = socket.Receive(buffer, 0);
            if (b > 0)
                return buffer;
            else
                return null;
        }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="stm"></param>
        /// <returns></returns>
        public static byte[] GetSocketData(Socket socket)
        {
            MemoryStream stm=new MemoryStream(2048);
            byte[] buffer=new byte[512];
            int bytes=0;
            do
            {
                bytes = socket.Receive(buffer, bytes == 0 ? 0 : bytes, buffer.Length, 0);
                if (bytes > 0)
                {
                    stm.Write(buffer, 0, bytes);
                }
            } while (bytes > 0);

            return stm.ToArray();
        }

        private void OpenConnection() 
        {
            if (!client.Connected) 
            {
                client.SendTimeout = 3000;
                client.Connect(endPoint);
                endPoint = client.LocalEndPoint as IPEndPoint;
            }
        }

        /// <summary>
        /// �ر�����
        /// </summary>
        public void CloseConnection() 
        {
            if (client!=null && client.Connected)
            {
                
                client.Close();
                endPoint = null;
            }
        }

        #region IDisposable ��Ա

        public void Dispose()
        {
            CloseConnection();
        }

        #endregion
        ~NetConnection()
        {
            CloseConnection();
        }
    }
}