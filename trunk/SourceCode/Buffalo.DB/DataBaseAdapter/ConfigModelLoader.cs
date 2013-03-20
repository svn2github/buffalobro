using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Diagnostics;
using Buffalo.DB.CommBase;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.DbCommon;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.EntityInfos;
using Buffalo.Kernel.FastReflection;
using System.IO;
using Buffalo.Kernel;

namespace Buffalo.DB.DataBaseAdapter
{
    /// <summary>
    /// ���ݷ��ʲ�ļ��ض�ȡ��
    /// </summary>
    public class ConfigModelLoader
    {
        
        private static Dictionary<string, Type> _dicLoaderConfig =null;//���ü�¼����
        private static Dictionary<string, Type> _dicEntityLoaderConfig = null;//����ʵ���¼����
        private static Dictionary<string, DBInfo> _dicDBInfo = new Dictionary<string, DBInfo>();//����ʵ���¼����
        
        /// <summary>
        /// �Ƿ��Ѿ���ʼ��
        /// </summary>
        internal static bool HasInit 
        {
            get 
            {
                return _dicLoaderConfig != null;
            }
        }

        /// <summary>
        /// ��ȡ���ݿ���Ϣ
        /// </summary>
        /// <param name="dbName">���ݿ���</param>
        /// <returns></returns>
        public static DBInfo GetDBInfo(string dbName) 
        {
            if (_dicDBInfo == null)
            {
                return null;
            }
            DBInfo ret=null;
            _dicDBInfo.TryGetValue(dbName, out ret);
            return ret;
        }

        /// <summary>
        /// ��ȡ���һ�����ݿ���Ϣ
        /// </summary>
        /// <returns></returns>
        internal static DBInfo GetFristDBInfo() 
        {
            if (_dicDBInfo == null)
            {
                return null;
            }
            foreach (KeyValuePair<string, DBInfo> kvp in _dicDBInfo) 
            {
                return kvp.Value;
            }
            return null;
        }

        #region ��ʼ������
        /// <summary>
        /// �������ݿ���Ϣ
        /// </summary>
        /// <param name="dbinfo"></param>
        public static void AppendDBInfo(DBInfo dbinfo) 
        {
            _dicDBInfo[dbinfo.Name] = dbinfo;
        }
        

        /// <summary>
        /// ��ʼ������
        /// </summary>
        internal static bool InitConfig()
        {
            if (HasInit) 
            {
                return true;
            }
            _dicLoaderConfig = new Dictionary<string, Type>();
            _dicEntityLoaderConfig = new Dictionary<string, Type>();
            Dictionary<string, XmlDocument> dicEntityConfig = new Dictionary<string, XmlDocument>();
            List<ConfigInfo> docs = ConfigXmlLoader.LoadXml("DataAccessConfig");
            if (docs.Count > 0)
            {
                
                DBInfo existsInfo = null;
                foreach (ConfigInfo doc in docs)
                {
                    XmlDocument docInfo = doc.Document;
                    DBInfo dbinfo = GetDBInfo(docInfo);
                    if (!_dicDBInfo.TryGetValue(dbinfo.Name, out existsInfo))
                    {
                        _dicDBInfo[dbinfo.Name] = dbinfo;
                    }
                    else 
                    {
                        if (!existsInfo.ConnectionString.Equals(dbinfo.ConnectionString)) 
                        {
                            throw new Exception("ͬ�����ݿ�:" + dbinfo.Name + "���������ַ�����ͬ");
                        }
                        if (!existsInfo.DbType.Equals(existsInfo.DbType)) 
                        {
                            throw new Exception("ͬ�����ݿ�:" + dbinfo.Name + "�������ݿ����Ͳ�ͬ");
                        }
                    }
                    
                }
                
            }
            if (_dicDBInfo.Count == 0) 
            {
                throw new Exception("û���������ݿ���Ϣ������config�ļ�");
            }

            LoadModel();
            return true;
        }

        /// <summary>
        /// ��ȡ��ǰ�����ļ������ݿ���Ϣ
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static DBInfo GetDBInfo(XmlDocument doc) 
        {
            string dbType = null;
            string connectionString = null;
            string name = null;
            //string output = null;
            string[] attNames = null;
            //Dictionary<string, string> extendDatabaseConnection = new Dictionary<string,string>();
            if (doc == null)
            {
                throw new Exception("�Ҳ��������ļ�");
            }
            XmlNodeList lstConfig = doc.GetElementsByTagName("config");
            if (lstConfig.Count > 0)
            {
                XmlNode node = lstConfig[0];
                foreach (XmlAttribute att in node.Attributes)
                {
                    if (att.Name.Equals("dbType",StringComparison.CurrentCultureIgnoreCase))
                    {
                        dbType = att.InnerText;
                    }
                    //else if (att.Name.Equals("output", StringComparison.CurrentCultureIgnoreCase))
                    //{
                    //    output = att.InnerText;
                    //}
                    else if (att.Name.Equals("name",StringComparison.CurrentCultureIgnoreCase))
                    {
                        name=att.InnerText;
                    }
                    else if (att.Name.Equals("connectionString",StringComparison.CurrentCultureIgnoreCase))
                    {
                        connectionString = att.InnerText;
                    }
                    else if (att.Name.Equals("appnamespace",StringComparison.CurrentCultureIgnoreCase)) 
                    {
                        string names=att.InnerText;

                        if (!string.IsNullOrEmpty(names))
                        {
                            attNames = names.Split(new char[] { '|' });
                        }
                        for (int i = 0; i < attNames.Length; i++) 
                        {
                            string attName = attNames[i];
                            if (!attName.EndsWith(".")) 
                            {
                                attNames[i] = attName + ".";
                            }
                        }
                    }
                    
                }
            }
            else
            {
                throw new Exception("�����ļ�û��config�ڵ�");
            }
            
            DBInfo info=new DBInfo(name, connectionString, dbType);
            info.DataaccessNamespace=attNames;
            return info;
        }

        

        
        /// <summary>
        /// ����ģ����Ϣ
        /// </summary>
        private static void LoadModel()
        {
            List<Assembly> lstAss = GetAllAssembly();
            Dictionary<string, EntityConfigInfo> dicAllEntityInfo = new Dictionary<string, EntityConfigInfo>();//ʵ����Ϣ
            
            foreach (Assembly ass in lstAss) 
            {
                
                string[] resourceNames = ass.GetManifestResourceNames();
                foreach (string name in resourceNames)
                {
                    if (name.EndsWith(".BEM.xml", StringComparison.CurrentCultureIgnoreCase))
                    {
                        try
                        {
                            Stream stm = ass.GetManifestResourceStream(name);
                            XmlDocument doc = new XmlDocument();
                            doc.Load(stm);

                            //��ȡ����
                            XmlNodeList lstNode = doc.GetElementsByTagName("class");
                            if (lstNode.Count > 0)
                            {
                                XmlNode classNode = lstNode[0];
                                XmlAttribute att = classNode.Attributes["ClassName"];
                                if (att != null)
                                {
                                    string className = att.InnerText;
                                    if (!string.IsNullOrEmpty(className))
                                    {
                                        Type cType = ass.GetType(className);

                                        EntityConfigInfo info = new EntityConfigInfo();
                                        info.Type = cType;
                                        info.ConfigXML = doc;
                                        dicAllEntityInfo[className] = info;

                                    }
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                    else if (name.EndsWith(".BDM.xml", StringComparison.CurrentCultureIgnoreCase))
                    {
                        Stream stm = ass.GetManifestResourceStream(name);
                        XmlDocument doc = new XmlDocument();
                        doc.Load(stm);
                        AppendDalLoader(ass, doc);
                    }
                }
            }
            EntityInfoManager.InitAllEntity(dicAllEntityInfo);

        }
        /// <summary>
        /// ���ӵ����ݲ�
        /// </summary>
        /// <param name="doc"></param>
        private static void AppendDalLoader(Assembly ass, XmlDocument doc) 
        {
            XmlNodeList nodes = doc.GetElementsByTagName("dataaccess");
            if (nodes.Count <= 0) 
            {
                return;
            }
            XmlAttribute att = nodes[0].Attributes["name"];
            if (att == null) 
            {
                return;
            }
            string name = att.InnerText;
            DBInfo db = null;
            if (!_dicDBInfo.TryGetValue(name, out db)) 
            {
                return;
            }
            string[] namespaces = db.DataaccessNamespace;
            XmlNodeList dalNodes = doc.GetElementsByTagName("item");
            foreach (XmlNode dalNode in dalNodes) 
            {
                att = dalNode.Attributes["type"];
                if (att == null) 
                {
                    continue;
                }
                string typeName = att.InnerText;
                foreach (string allNameSpace in namespaces) 
                {
                    if (typeName.StartsWith(allNameSpace)) 
                    {
                        Type dalType = ass.GetType(typeName);
                        if (dalType != null) 
                        {
                            att = dalNode.Attributes["interface"];
                            if (att == null)
                            {
                                break;
                            }
                            _dicLoaderConfig[att.InnerText] = dalType;

                            Type[] gTypes = DefaultType.GetGenericType(dalType, true);
                            if (gTypes != null && gTypes.Length > 0)
                            {
                                Type gType = gTypes[0];
                                _dicEntityLoaderConfig[gType.FullName] = dalType;
                            }
                        }

                        break;
                    }
                }
            }
        }


        private static List<Assembly> _modelAssembly = new List<Assembly>();

        /// <summary>
        /// ����Ҫ�����ĳ���
        /// </summary>
        /// <param name="ass">����</param>
        public static void AppendModelAssembly(Assembly ass) 
        {
            
            string key=ass.FullName;
            foreach (Assembly curAss in _modelAssembly) 
            {
                string curKey = curAss.FullName;
                if (key == curKey) 
                {
                    return;
                }
            }
            _modelAssembly.Add(ass);
        }
        /// <summary>
        /// ��ȡ��ģ�������г���
        /// </summary>
        /// <returns></returns>
        private static List<Assembly> GetAllAssembly() 
        {
            return _modelAssembly;
        }

        #endregion

        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="entityType">ʵ������</param>
        /// <returns></returns>
        public static object GetInstanceByEntity(Type entityType, DataBaseOperate oper)
        {
            //InitConfig();
            Type objType = null;
            if (!_dicEntityLoaderConfig.TryGetValue(entityType.FullName, out objType))
            {
                throw new Exception("�Ҳ�����Ӧ������" + entityType.FullName+"�����ã����������ļ�");
            }
            return Activator.CreateInstance(objType, oper);
        }

        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="entityType">ʵ������</param>
        /// <returns></returns>
        public static IDataAccessModel<T> GetInstanceByEntity<T>(DataBaseOperate oper)
                    where T : EntityBase, new()
        {

            return GetInstanceByEntity(typeof(T),oper) as IDataAccessModel<T>;
        }
        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="entityType">ʵ������</param>
        /// <returns></returns>
        public static IViewDataAccess<T> GetViewInstanceByEntity<T>(DataBaseOperate oper)
                    where T : EntityBase, new()
        {
            return GetInstanceByEntity(typeof(T), oper) as IViewDataAccess<T>;
        }
        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="interfaceType">ʵ������</param>
        /// <returns></returns>
        public static object GetInstance(Type interfaceType, DataBaseOperate oper) 
        {
            //InitConfig();
            Type objType = null;
            if (!_dicLoaderConfig.TryGetValue(interfaceType.FullName, out objType)) 
            {
                throw new Exception("�Ҳ����ӿ�" + interfaceType.FullName + "�Ķ�Ӧ���ã����������ļ�");
            }
            return Activator.CreateInstance(objType, oper);
        }
       
        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="DataBaseOperate">����</param>
        /// <returns></returns>
        public static T GetInstance<T>(DataBaseOperate oper)
        {
            return (T)GetInstance(typeof(T),oper);
        }



        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="entityType">ʵ������</param>
        /// <returns></returns>
        public static object GetInstanceByEntity(Type entityType)
        {
            //InitConfig();
            Type objType = null;
            if (!_dicEntityLoaderConfig.TryGetValue(entityType.FullName, out objType))
            {
                throw new Exception("�Ҳ�����Ӧ������");
            }
            return Activator.CreateInstance(objType);
        }

        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="entityType">ʵ������</param>
        /// <returns></returns>
        public static IDataAccessModel<T> GetInstanceByEntity<T>()
                    where T : EntityBase, new()
        {

            return GetInstanceByEntity(typeof(T)) as IDataAccessModel<T>;
        }
        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="entityType">ʵ������</param>
        /// <returns></returns>
        public static IViewDataAccess<T> GetViewInstanceByEntity<T>()
                    where T : EntityBase, new()
        {
            return GetInstanceByEntity(typeof(T)) as IViewDataAccess<T>;
        }
        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="interfaceType">ʵ������</param>
        /// <returns></returns>
        public static object GetInstance(Type interfaceType)
        {
            //InitConfig();
            Type objType = null;
            if (!_dicLoaderConfig.TryGetValue(interfaceType.FullName, out objType))
            {
                throw new Exception("�Ҳ�����Ӧ������");
            }
            return Activator.CreateInstance(objType);
        }

        /// <summary>
        /// ����ʵ�����ͻ�ȡ��Ӧ�����ݷ��ʲ��ʵ��
        /// </summary>
        /// <param name="DataBaseOperate">����</param>
        /// <returns></returns>
        public static T GetInstance<T>()
        {
            return (T)GetInstance(typeof(T));
        }
    }
}