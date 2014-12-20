using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using Buffalo.DBTools.ROMHelper;
using EnvDTE;
using Buffalo.Win32Kernel;
using Buffalo.DBTools.UIHelper;

namespace Buffalo.DBTools.HelperKernel
{
    /// <summary>
    /// 生成数据层
    /// </summary>
    public class Generate3Tier : GrneraterBase
    {
        DataAccessMappingConfig dmt = null;
        
        public Generate3Tier(EntityConfig entity) 
            :base(entity)
        {
            dmt = new DataAccessMappingConfig(entity);
        }

        public Generate3Tier(DBEntityInfo entity,ClassDesignerInfo info)
            : base(entity, info)
        {
            dmt = new DataAccessMappingConfig(entity, info);
        }

        /// <summary>
        /// 生成业务层
        /// </summary>
        /// <param name="entity"></param>
        public void GenerateBusiness() 
        {
            FileInfo info = new FileInfo(ClassDesignerFileName);
            

            string dicPath = info.DirectoryName + "\\Business";
            if (!Directory.Exists(dicPath)) 
            {
                Directory.CreateDirectory(dicPath);
            }
            string fileName = dicPath + "\\" + ClassName + "Business.cs";
            if (File.Exists(fileName)) 
            {
                return;
            }


            string model = Models.Business;
            
            string baseClass = null;
            
            string businessClassName=ClassName+"Business";

            if (EntityConfig.IsSystemTypeName(EntityBaseTypeName))
            {
                baseClass = "BusinessModelBase";
            }
            else
            {
                baseClass = BaseNamespace + ".Business." + EntityBaseTypeShortName + "BusinessBase";
            }
            
            

            List<string> codes = new List<string>();
            TagManager tag = new TagManager();

            using (StringReader reader = new StringReader(model))
            {
                string tmp = null;
                while ((tmp = reader.ReadLine()) != null)
                {
                    if (tmp.StartsWith("<%#IF TableName%>"))
                    {
                        tag.AddTag("TableName");
                    }
                    else if (tmp.StartsWith("<%#ENDIF%>"))
                    {
                        tag.PopTag();
                    }
                    else
                    {
                        if (tag.CurrentTag == "TableName" && string.IsNullOrEmpty(Table.TableName))
                        {
                            continue;
                        }
                        tmp = tmp.Replace("<%=EntityNamespace%>", EntityNamespace);
                        tmp = tmp.Replace("<%=Summary%>", Table.Description);
                        tmp = tmp.Replace("<%=BusinessClassName%>", businessClassName);
                        tmp = tmp.Replace("<%=ClassName%>", ClassName);
                        tmp = tmp.Replace("<%=BusinessNamespace%>", BusinessNamespace);
                        tmp = tmp.Replace("<%=BaseBusinessClass%>", baseClass);
                        codes.Add(tmp);
                    }
                }
            }
            
            CodeFileHelper.SaveFile(fileName, codes);
            EnvDTE.ProjectItem newit = DesignerInfo.CurrentProject.ProjectItems.AddFromFile(fileName);
            newit.Properties.Item("BuildAction").Value = (int)BuildAction.Code;
        }

        /// <summary>
        /// 数据层的类型
        /// </summary>
        public static readonly ComboBoxItemCollection DataAccessTypes = InitItems();

        /// <summary>
        /// 数据层的类型
        /// </summary>
        public static readonly ComboBoxItemCollection CacheTypes = InitCacheItems();

        //private static Dictionary<string, string> _dicConnString = InitConnStrings();
        ///// <summary>
        ///// 初始化参考字符串
        ///// </summary>
        ///// <returns></returns>
        //private static Dictionary<string, string> InitConnStrings() 
        //{
        //    Dictionary<string, string> dic = new Dictionary<string, string>();
        //    dic["Sql2K"] = "server={server};database={database};uid={username};pwd={pwd}";
        //    dic["Sql2K5"] = "server={server};database={database};uid={username};pwd={pwd}";
        //    dic["Sql2K8"] = "server={server};database={database};uid={username};pwd={pwd}";
        //    dic["Oracle9"] = "server={server};user id={username};password={pwd}";
        //    dic["MySQL5"] = "User ID={username};Password={pwd};Host={server};Port=3306;Database={database};charset=utf8";
        //    dic["SQLite"] = "Data Source={databasePath}";
        //    dic["DB2v9"] = "server={server}:50000;DATABASE ={database};UID={username};PWD={pwd}";
        //    dic["Psql9"] = "Server={server};Port=5432;User Id={username};Password={pwd};Database={database}";
        //    return dic;
        //}

        ///// <summary>
        ///// 获取参考字符串
        ///// </summary>
        ///// <param name="dbType">数据库类型</param>
        ///// <returns></returns>
        //public static string GetConnString(string dbType) 
        //{
        //    string conn = null;
        //    if(_dicConnString.TryGetValue(dbType,out conn))
        //    {
        //        return conn;
        //    }
        //    return null;
        //}

        /// <summary>
        /// 初始化数据库类型
        /// </summary>
        /// <returns></returns>
        private static ComboBoxItemCollection InitCacheItems()
        {
            ComboBoxItemCollection types = new ComboBoxItemCollection();
            ComboBoxItem item = null;
            item = new ComboBoxItem("无", "");
            item.Tag = "";
            types.Add(item);
            item = new ComboBoxItem("内存", "system");
            item.Tag = "";
            types.Add(item);
            
#if (NET_2_0)
#else
            item = new ComboBoxItem("Memcached", "memcached");
            item.Tag = "server=127.0.0.1:11211,127.0.0.1:11212;expir=30;maxsize=30;throw=0";
            types.Add(item);
            item = new ComboBoxItem("Redis", "redis");
            item.Tag = "server=127.0.0.1:6379,127.0.0.1:6380;readserver=127.0.0.1:6381,127.0.0.1:6382;expir=30;maxsize=30;throw=0";
            types.Add(item);
#endif



            return types;
        }

        /// <summary>
        /// 初始化数据库类型
        /// </summary>
        /// <returns></returns>
        private static ComboBoxItemCollection InitItems() 
        {
            ComboBoxItemCollection types = new ComboBoxItemCollection();
            ComboBoxItem item = new ComboBoxItem("SQL Server 2000", "Sql2K");
            item.Tag = "server=127.0.0.1;database=mydb;uid=sa;pwd=sa";
            types.Add(item);
            item = new ComboBoxItem("SQL Server 2005", "Sql2K5");
            item.Tag = "server=127.0.0.1;database=mydb;uid=sa;pwd=sa";
            types.Add(item);
            item = new ComboBoxItem("SQL Server 2008 或以上", "Sql2K8");
            item.Tag = "server=127.0.0.1;database=mydb;uid=sa;pwd=sa";
            types.Add(item);
            item = new ComboBoxItem("Oracle 9 或以上", "Oracle9");
            item.Tag = "server=Myserver;user id=username;password=pwd";
            types.Add(item);
            item = new ComboBoxItem("MySQL 5.0 或以上", "Buffalo.Data.MySQL");
            item.Tag = "User ID=root;Password=pwd;Host=127.0.0.1;Port=3306;Database=mydb;";
            types.Add(item);
            item = new ComboBoxItem("SQLite", "Buffalo.Data.SQLite");
            item.Tag = "Data Source=D:\\db.s3db";
            types.Add(item);
            item = new ComboBoxItem("IBM DB2 v9或以上", "Buffalo.Data.DB2");
            item.Tag = "server=127.0.0.1:50000;DATABASE =mydb;UID=DB2Admin;PWD=pwd";
            types.Add(item);
            item = new ComboBoxItem("Postgresql9或以上", "Buffalo.Data.PostgreSQL");
            item.Tag = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=pwd;Database=mydb";
            types.Add(item);
            item = new ComboBoxItem("Access", "Access");
            item.Tag = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=c:\\db.mdb; Jet OLEDB:Database Password=pwd";
            types.Add(item);
            return types;
        }

        public static readonly ComboBoxItemCollection Tiers = InitTiers();
        /// <summary>
        /// 初始化架构层数
        /// </summary>
        /// <returns></returns>
        private static ComboBoxItemCollection InitTiers()
        {
            ComboBoxItemCollection tiers = new ComboBoxItemCollection();
            ComboBoxItem item = new ComboBoxItem("三层架构", 3);
            tiers.Add(item);
            item = new ComboBoxItem("单层架构", 1);
            tiers.Add(item);

            return tiers;
        }


        /// <summary>
        /// 生成数据层
        /// </summary>
        /// <param name="entity"></param>
        public void GenerateDataAccess()
        {
            //FileInfo info = new FileInfo(EntityFileName);


            string dicPath = GenerateBasePath + "\\DataAccess";
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }
            string dal = Models.DataAccess;
            foreach (ComboBoxItem itype in DataAccessTypes) 
            {
                if (!this.BbConfig.IsAllDal && !this.BbConfig.DbType.Equals(itype.Value)) 
                {
                    continue;
                }
                string type = itype.Value.ToString();
                string dalPath = dicPath + "\\" + type;
                if (!Directory.Exists(dalPath))
                {
                    Directory.CreateDirectory(dalPath);
                }
                string fileName = dalPath + "\\" + ClassName + "DataAccess.cs";
                if (File.Exists(fileName))
                {
                    continue;
                }

                List<string> codes = new List<string>();
                using (StringReader reader = new StringReader(dal))
                {
                    string tmp = null;
                    while ((tmp = reader.ReadLine()) != null)
                    {
                        tmp = tmp.Replace("<%=EntityNamespace%>", EntityNamespace);
                        tmp = tmp.Replace("<%=Summary%>", Table.Description);
                        tmp = tmp.Replace("<%=DataAccessNamespace%>", DataAccessNamespace);
                        tmp = tmp.Replace("<%=DataBaseType%>", type);
                        tmp = tmp.Replace("<%=ClassName%>", ClassName);
                        codes.Add(tmp);
                    }
                }
                dmt.AppendDal(DataAccessNamespace + "." + type + "." + ClassName + "DataAccess", DataAccessNamespace + ".IDataAccess.I" + ClassName + "DataAccess");
                CodeFileHelper.SaveFile(fileName, codes);
                EnvDTE.ProjectItem newit = DesignerInfo.CurrentProject.ProjectItems.AddFromFile(fileName);
                newit.Properties.Item("BuildAction").Value = (int)BuildAction.Code;
            }
            dmt.SaveXML();
        }
        /// <summary>
        /// 生成IDataAccess
        /// </summary>
        /// <param name="entity"></param>
        public void GenerateIDataAccess() 
        {
            FileInfo info = new FileInfo(ClassDesignerFileName);
            string dicPath = info.DirectoryName + "\\DataAccess";
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }
            string idalPath = dicPath + "\\IDataAccess";
            if (!Directory.Exists(idalPath))
            {
                Directory.CreateDirectory(idalPath);
            }
            string fileName = idalPath + "\\I" + ClassName + "DataAccess.cs";
            if (File.Exists(fileName))
            {
                return;
            }
            string idal = Models.IDataAccess;
            List<string> codes = new List<string>();
            using (StringReader reader = new StringReader(idal))
            {
                string tmp = null;
                while ((tmp = reader.ReadLine()) != null)
                {
                    tmp = tmp.Replace("<%=EntityNamespace%>", EntityNamespace);
                    tmp = tmp.Replace("<%=Summary%>", Table.Description);
                    tmp = tmp.Replace("<%=DataAccessNamespace%>", DataAccessNamespace);
                    tmp = tmp.Replace("<%=ClassName%>", ClassName);
                    codes.Add(tmp);
                }
            }
            CodeFileHelper.SaveFile(fileName, codes);
            EnvDTE.ProjectItem newit =DesignerInfo.CurrentProject.ProjectItems.AddFromFile(fileName);
            newit.Properties.Item("BuildAction").Value = (int)BuildAction.Code;
        }

        /// <summary>
        /// 生成BQL数据层
        /// </summary>
        public void GenerateBQLDataAccess() 
        {
            FileInfo info = new FileInfo(ClassDesignerFileName);
            string dicPath = info.DirectoryName + "\\DataAccess";
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }
            string type = "Bql";
            string idalPath = dicPath + "\\" + type;
            if (!Directory.Exists(idalPath))
            {
                Directory.CreateDirectory(idalPath);
            }
            string fileName = idalPath + "\\" + ClassName + "DataAccess.cs";
            if (File.Exists(fileName))
            {
                return;
            }
            string idal = Models.BQLDataAccess;
            List<string> codes = new List<string>();
            using (StringReader reader = new StringReader(idal))
            {
                string tmp = null;
                while ((tmp = reader.ReadLine()) != null)
                {
                    tmp = tmp.Replace("<%=EntityNamespace%>", EntityNamespace);
                    tmp = tmp.Replace("<%=Summary%>", Table.Description);
                    tmp = tmp.Replace("<%=DataAccessNamespace%>", DataAccessNamespace);
                    tmp = tmp.Replace("<%=DataBaseType%>", type);
                    tmp = tmp.Replace("<%=ClassName%>", ClassName);
                    tmp = tmp.Replace("<%=BQLEntityNamespace%>", BQLEntityNamespace);
                    codes.Add(tmp);
                }
            }
            CodeFileHelper.SaveFile(fileName, codes);
            EnvDTE.ProjectItem newit = DesignerInfo.CurrentProject.ProjectItems.AddFromFile(fileName);
            newit.Properties.Item("BuildAction").Value = (int)BuildAction.Code;
        }

    }
}
