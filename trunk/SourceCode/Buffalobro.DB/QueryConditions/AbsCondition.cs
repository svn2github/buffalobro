﻿using System;
using System.Text;
using Buffalobro.DB.DbCommon;
using System.Collections.Generic;
using System.Data;
using Buffalobro.DB.DataBaseAdapter;
using Buffalobro.DB.CommBase.DataAccessBases.AliasTableMappingManagers;
namespace Buffalobro.DB.QueryConditions
{
    public abstract class AbsCondition
    {
        public AbsCondition(DBInfo db) 
        {
            _dbInfo = db;
        }


        protected string _itemName = null;


        public virtual StringBuilder Condition 
        {
            get
            {
                throw new Exception(_itemName + "语句没有条件");
            } 
        }
        public virtual string GetSql() 
        {
            return null;
        }

        protected DBInfo _dbInfo;
        public DBInfo DBinfo 
        {
            get 
            {
                return _dbInfo;
            }
            set 
            {
                _dbInfo = value;
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
        /// <summary>
        /// 插入字段的数值类型
        /// </summary>
        public virtual List<DbType> ParamTypes
        {
            get { return null; }
        }
        protected DataBaseOperate _oper;

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        public DataBaseOperate Oper
        {
            get { return _oper; }
            set { _oper = value; }
        }

        protected ParamList _paramList = null;
        public ParamList DbParamList 
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
        /// 主键
        /// </summary>
        private StringBuilder _primaryKey = new StringBuilder();
        /// <summary>
        /// 主键
        /// </summary>
        public StringBuilder PrimaryKey
        {
            get { return _primaryKey; }
        }
        public virtual StringBuilder GroupBy
        {
            get { throw new Exception(_itemName + "语句没有分组"); }
        }
        public virtual StringBuilder Having
        {
            get { throw new Exception(_itemName + "语句没有Having"); }
        }
        public virtual StringBuilder Orders 
        { 
            get { throw new Exception(_itemName + "语句没有排序"); }
        }
        public virtual PageContent PageContent 
        {
            get { throw new Exception(_itemName + "语句没有分页"); }
            set { throw new Exception(_itemName + "语句没有分页"); }
        }
        //public virtual StringBuilder PrimaryKey { get; }
        public virtual StringBuilder SqlParams 
        {
            get { throw new Exception(_itemName + "语句没有字段"); }
        }
        public virtual StringBuilder Tables 
        {
            get { throw new Exception(_itemName + "语句没有表名"); }
        }
        public virtual StringBuilder SqlValues 
        {
            get { throw new Exception(_itemName + "语句没有Values"); }
        }
        public virtual StringBuilder UpdateSetValue 
        {
            get { throw new Exception(_itemName + "语句没有Set语句"); }
        }
    }
}