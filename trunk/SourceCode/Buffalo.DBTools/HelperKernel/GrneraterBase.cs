using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DBTools.ROMHelper;

namespace Buffalo.DBTools.HelperKernel
{
    /// <summary>
    /// 生成器基类
    /// </summary>
    public class GrneraterBase
    {
        //protected EntityConfig _entity;

        ///// <summary>
        ///// 实体信息
        ///// </summary>
        //public EntityConfig Entity
        //{
        //    get
        //    {
        //        return _entity;
        //    }
        //}

        private KeyWordTableParamItem _table;

        /// <summary>
        /// 表信息
        /// </summary>
        public KeyWordTableParamItem Table
        {
            get { return _table; }
        }

        private Project _currentProject;

        /// <summary>
        /// 当前项目
        /// </summary>
        public Project CurrentProject 
        {
            get 
            {
                return _currentProject;
            }
        }

        private string _entityBaseTypeName;

        /// <summary>
        /// 实体的基类
        /// </summary>
        public string EntityBaseTypeName 
        {
            get 
            {
                return _entityBaseTypeName;
            }
        }
        private string _entityBaseTypeShortName;

        /// <summary>
        /// 实体的基类名
        /// </summary>
        public string EntityBaseTypeShortName 
        {
            get 
            {
                return _entityBaseTypeShortName;
            }
        }
        private string _entityFileName;

        /// <summary>
        /// 实体文件名
        /// </summary>
        public string EntityFileName 
        {
            get
            {
                return _entityFileName;
            }
        }

        private string _entityNamespace;

        /// <summary>
        /// 实体命名空间
        /// </summary>
        public string EntityNamespace
        {
            get
            {
                return _entityNamespace;
            }
        }

        private string _BQLEntityNamespace;
        /// <summary>
        /// BQL实体的命名空间
        /// </summary>
        public string BQLEntityNamespace 
        {
            get 
            {
                return _BQLEntityNamespace;
            }
        }

        private string _className;
        /// <summary>
        /// 实体名称
        /// </summary>
        public string ClassName
        {
            get
            {
                return _className;
            }
        }


        private string _businessNamespace;
        /// <summary>
        /// 业务层命名空间
        /// </summary>
        public string BusinessNamespace
        {
            get
            {
                return _businessNamespace;
            }
        }

        private string _dataAccessNamespace;
        /// <summary>
        /// 数据层命名空间
        /// </summary>
        public string DataAccessNamespace
        {
            get
            {
                return _dataAccessNamespace;
            }
        }

        private string _DBName;

        /// <summary>
        /// 数据库名
        /// </summary>
        public string DBName 
        {
            get 
            {
                return _DBName;
            }
        }

        private DBConfigInfo _dbConfig;

        /// <summary>
        /// 数据库配置
        /// </summary>
        public DBConfigInfo BbConfig
        {
            get { return _dbConfig; }
            set { _dbConfig = value; }
        }

        Dictionary<string, List<string>> _dicGenericInfo;
        /// <summary>
        /// 泛型信息
        /// </summary>
        public Dictionary<string, List<string>> GenericInfo
        {
            get { return _dicGenericInfo; }
        }

        /// <summary>
        /// 获取基类短名
        /// </summary>
        /// <param name="baseType"></param>
        /// <returns></returns>
        private string GetBaseTypeShortName(string baseType) 
        {
            baseType = GetBaseTypeName(baseType);
            int lastDot = baseType.LastIndexOf('.');
            if (lastDot >= 0)
            {
                return baseType.Substring(lastDot + 1, baseType.Length - lastDot - 1);
            }
            return baseType;
        }
        /// <summary>
        /// 获取基类名
        /// </summary>
        /// <returns></returns>
        private string GetBaseTypeName(string baseType) 
        {
            int genTypeIndex = baseType.IndexOf('<');
            if (genTypeIndex >= 0)
            {
                baseType = baseType.Substring(0, genTypeIndex);
            }
            return baseType;
        }

        public GrneraterBase(DBEntityInfo entity,Project curProject) 
        {
            _table = entity.ToTableInfo();
            _className = entity.ClassName;
            _currentProject = curProject;
            _entityBaseTypeName = GetBaseTypeName(entity.BaseType);
           

            _entityBaseTypeShortName =GetBaseTypeShortName( entity.BaseType);
           
            _entityFileName = entity.FileName;
            _entityNamespace = entity.EntityNamespace;
            _BQLEntityNamespace = entity.EntityNamespace + ".BQLEntity";
            _businessNamespace = entity.EntityNamespace + ".Business";
            _dataAccessNamespace = entity.EntityNamespace + ".DataAccess";
            _DBName = entity.CurrentDBConfigInfo.DbName;
            _dbConfig = entity.CurrentDBConfigInfo;
        }

        /// <summary>
        /// 格式化名字
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string FormatName(string name) 
        {
            return name.Replace(" ", "_");
        }

        public GrneraterBase(EntityConfig entity) 
        {
            //_entity = entity;
            //_tableName = entity.TableName;
            _table = entity.ToTableInfo();
            _currentProject = entity.CurrentProject;
            _entityBaseTypeName = GetBaseTypeName(entity.BaseTypeName);

            
            _entityBaseTypeShortName = GetBaseTypeShortName(entity.BaseTypeName);
            _entityFileName = entity.FileName;
            _entityNamespace = entity.Namespace;
            //_summary = entity.Summary;
            _className = entity.ClassName;
            _BQLEntityNamespace = entity.Namespace + ".BQLEntity";
            _businessNamespace = entity.Namespace + ".Business";
            _dataAccessNamespace = entity.Namespace + ".DataAccess";
            _DBName = entity.CurrentDBConfigInfo.DbName;
            _dbConfig = entity.CurrentDBConfigInfo;
            _dicGenericInfo = entity.GenericInfo;
        }

        
    }
}
