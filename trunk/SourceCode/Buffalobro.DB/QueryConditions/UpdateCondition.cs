using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.DbCommon;
using Buffalobro.DB.DataBaseAdapter;

namespace Buffalobro.DB.QueryConditions
{
    public class UpdateCondition : AbsCondition
    {

        public UpdateCondition(DBInfo db)
            : base(db) 
        {
            _itemName = "Update";
        }

        /// <summary>
        /// ��ѯ����
        /// </summary>
        private StringBuilder _condition = new StringBuilder();
        /// <summary>
        /// ����
        /// </summary>
        public override StringBuilder Condition
        {
            get { return _condition; }
        }

        private StringBuilder _updateSetValue = new StringBuilder();

        public override StringBuilder UpdateSetValue
        {
            get { return _updateSetValue; }
        }

        public override string GetSql()
        {
            StringBuilder sbRet = new StringBuilder(2000);
            sbRet.Append("update  ");
            sbRet.Append(_tables.ToString());
            sbRet.Append(" set ");
            sbRet.Append(_updateSetValue.ToString());

            if (_condition.Length > 0)
            {
                sbRet.Append(" where ");
                sbRet.Append(_condition.ToString());
            }

            return sbRet.ToString();
        }

        

        /// <summary>
        /// ��ѯ�ı�
        /// </summary>
        private StringBuilder _tables = new StringBuilder();
        /// <summary>
        /// ��
        /// </summary>
        public override StringBuilder Tables
        {
            get { return _tables; }
        }
    }
}