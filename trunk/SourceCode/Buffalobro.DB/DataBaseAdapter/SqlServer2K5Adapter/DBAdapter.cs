using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.QueryConditions;
using Buffalobro.DB.DbCommon;
using System.Data.SqlClient;

namespace Buffalobro.DB.DataBaseAdapter.SqlServer2K5Adapter
{
    public class DBAdapter : SqlServer2KAdapter.DBAdapter
    {
        public override string CreatePageSql(ParamList list, DataBaseOperate oper, SelectCondition objCondition, PageContent objPage)
        {
            return CutPageSqlCreater.CreatePageSql(list, oper, objCondition, objPage);
        }
        /// <summary>
        /// ȫ������ʱ�������ֶ��Ƿ���ʾ����ʽ
        /// </summary>
        public override bool IsShowExpression
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        public override ParamList CsqlSelectParamList
        {
            get
            {
                return new ParamList();
            }
        }
        
    }
}