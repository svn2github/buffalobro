using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.IdentityInfos;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DB.CommBase.DataAccessBases.AliasTableMappingManagers;

namespace Buffalo.DB.BQLCommon.BQLConditionCommon
{
    public class KeyWordInfomation : ICloneable
    {
        public KeyWordInfomation()
        {

        }


        //private QueryParamCollection queryParams = new QueryParamCollection();
        //private AliasCollection alias = new AliasCollection();
        private List<IdentityInfo> _identityInfos = new List<IdentityInfo>();


        private bool _hasGroup = false;

        /// <summary>
        /// 是否有聚合语句
        /// </summary>
        public bool HasGroup
        {
            get
            {
                return _hasGroup;
            }
            set
            {
                _hasGroup = value;
            }
        }

        private int _primaryKeys;


        /// <summary>
        /// 主键数
        /// </summary>
        public int PrimaryKeys
        {
            get { return _primaryKeys; }
            set { _primaryKeys = value; }
        }
        private bool _isWhere = false;

        /// <summary>
        /// 是否在输出条件中
        /// </summary>
        public bool IsWhere
        {
            get { return _isWhere; }
            set { _isWhere = value; }
        }
        private AbsCondition _condition = null;
        /// <summary>
        /// 输出的条件信息 
        /// </summary>
        public AbsCondition Condition
        {
            set { _condition = value; }
            get { return _condition; }
        }

        private BQLInfos _infos = null;
        /// <summary>
        /// 输出设置
        /// </summary>
        public BQLInfos Infos
        {
            get { return _infos; }
            set { _infos = value; }
        }

        private DBInfo _dbInfo;
        /// <summary>
        /// 数据库信息
        /// </summary>
        public DBInfo DBInfo
        {
            get { return _dbInfo; }
            set { _dbInfo = value; }
        }

        ///// <summary>
        ///// 查询显示的属性和字段的对应表
        ///// </summary>
        //internal QueryParamCollection QueryParams 
        //{
        //    get 
        //    {
        //        return queryParams;
        //    }
        //}

        ///// <summary>
        ///// 别名表和其属性
        ///// </summary>
        //internal AliasCollection Alias
        //{
        //    get
        //    {
        //        return alias;
        //    }
        //}

        protected ParamList _paramList = null;
        public ParamList ParamList
        {
            get
            {
                return _paramList;
            }
            set
            {
                _paramList = value;
            }
        }

        /// <summary>
        /// 自动增长的集合
        /// </summary>
        internal List<IdentityInfo> IdentityInfos
        {
            get
            {
                return _identityInfos;
            }
        }

        private TableAliasNameManager _aliasManager;

        /// <summary>
        /// 表映射管理器
        /// </summary>
        internal TableAliasNameManager AliasManager
        {
            get
            {
                return _aliasManager;
            }
            set
            {
                _aliasManager = value;
            }
        }


        private bool _outPutModle = false;
        /// <summary>
        /// 输出信息模式
        /// </summary>
        public bool OutPutModle
        {
            get { return _outPutModle; }
            set { _outPutModle = value; }
        }

        #region ICloneable 成员

        public object Clone()
        {
            KeyWordInfomation info = new KeyWordInfomation();
            info._infos = this._infos;
            //info._isPutPropertyName = this._isPutPropertyName;
            //info._isPage = this._isShowTableName;
            info._paramList = _paramList;
            info._dbInfo = this._dbInfo;
            info.OutPutModle = this.OutPutModle;
            return info;
        }

        #endregion
    }
}
