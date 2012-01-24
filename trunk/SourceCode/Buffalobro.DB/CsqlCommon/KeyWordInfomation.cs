using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.IdentityInfos;
using Buffalobro.DB.QueryConditions;
using Buffalobro.DB.DbCommon;
using Buffalobro.DB.DataBaseAdapter;
using Buffalobro.DB.CommBase.DataAccessBases.AliasTableMappingManagers;

namespace Buffalobro.DB.CsqlCommon.CsqlConditionCommon
{
    public class KeyWordInfomation:ICloneable
    {
        public KeyWordInfomation() 
        {
            
        }

        
        //private QueryParamCollection queryParams = new QueryParamCollection();
        //private AliasCollection alias = new AliasCollection();
        private List<IdentityInfo> _identityInfos = new List<IdentityInfo>();

        

       
        private bool _isWhere = false;

        /// <summary>
        /// �Ƿ������������
        /// </summary>
        public bool IsWhere
        {
            get { return _isWhere; }
            set { _isWhere = value; }
        }
        private AbsCondition _condition = null;
        /// <summary>
        /// �����������Ϣ 
        /// </summary>
        public AbsCondition Condition
        {
            set {_condition=value; }
            get { return _condition; }
        }

        private CsqlInfos _infos=null;
        /// <summary>
        /// �������
        /// </summary>
        public CsqlInfos Infos
        {
            get { return _infos; }
            set { _infos = value; }
        }

        private DBInfo _dbInfo;
        /// <summary>
        /// ���ݿ���Ϣ
        /// </summary>
        public DBInfo DBInfo
        {
            get { return _dbInfo; }
            set { _dbInfo = value; }
        }

        ///// <summary>
        ///// ��ѯ��ʾ�����Ժ��ֶεĶ�Ӧ��
        ///// </summary>
        //internal QueryParamCollection QueryParams 
        //{
        //    get 
        //    {
        //        return queryParams;
        //    }
        //}

        ///// <summary>
        ///// ��������������
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
        /// �Զ������ļ���
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
        /// ��ӳ�������
        /// </summary>
        internal TableAliasNameManager AliasManager
        {
            get
            {
                return _aliasManager;
            }
            set
            {
                _aliasManager=value;
            }
        }

        #region ICloneable ��Ա

        public object Clone()
        {
            KeyWordInfomation info = new KeyWordInfomation();
            info._infos = this._infos;
            //info._isPutPropertyName = this._isPutPropertyName;
            //info._isPage = this._isShowTableName;
            info._paramList = _paramList;
            info._dbInfo = this._dbInfo;
            return info;
        }

        #endregion
    }
}