using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Buffalobro.DB.DataBaseAdapter.IDbAdapters;
using Buffalobro.DB.CommBase;
using Buffalobro.DB.EntityInfos;
using Buffalobro.DB.QueryConditions;
using Buffalobro.DB.DbCommon;
using Buffalo.Kernel;
namespace Buffalobro.DB.DataBaseAdapter.SqlServer2KAdapter
{
    public class DBAdapter : IDBAdapter
    {

        /// <summary>
        /// ȫ������ʱ�������ֶ��Ƿ���ʾ����ʽ
        /// </summary>
        public virtual bool IsShowExpression 
        {
            get 
            {
                return false;
            }
        }



        /// <summary>
        /// �Ƿ��¼�������ֶ����ֶ�����
        /// </summary>
        public bool IsSaveIdentityParam
        {
            get 
            {
                return false;
            }
        }

        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        public virtual ParamList CsqlSelectParamList
        {
            get 
            {
                return null;
            }
        }
        /// <summary>
        /// ��ȡ������
        /// </summary>
        /// <param name="paramName">������</param>
        /// <param name="type">�������ݿ�����</param>
        /// <param name="paramValue">����ֵ</param>
        /// <param name="paramDir">������������</param>
        /// <returns></returns>
        public IDataParameter GetDataParameter(string paramName, DbType type, object paramValue, ParameterDirection paramDir) 
        {
            IDataParameter newParam = new SqlParameter();
            newParam.ParameterName = paramName;
            newParam.DbType = type;
            newParam.Value = paramValue;
            newParam.Direction = paramDir;
            return newParam;
        }

        /// <summary>
        /// ��ȡtop�Ĳ�ѯ�ַ���
        /// </summary>
        /// <param name="sql">��ѯ�ַ���</param>
        /// <param name="top">topֵ</param>
        /// <returns></returns>
        public string GetTopSelectSql(SelectCondition sql, int top)
        {
            StringBuilder sbSql = new StringBuilder(500);
            sbSql.Append("select top ");
            sbSql.Append(top.ToString());
            sbSql.Append(" " + sql.SqlParams.ToString() + " from ");
            sbSql.Append(sql.Tables.ToString());
            if (sql.Condition.Length > 0)
            {
                sbSql.Append(" where " + sql.Condition.ToString());
            }

            if (sql.Orders.Length > 0)
            {
                sbSql.Append(" order by ");
                sbSql.Append(sql.Orders.ToString());
            }
            if (sql.Having.Length > 0)
            {
                sbSql.Append(" having ");
                sbSql.Append(sql.Having.ToString());
            }

            return sbSql.ToString();
        }
        /// <summary>
        /// ��ȡ������
        /// </summary>
        /// <param name="tableName">����</param>
        /// <param name="entityInfo">�ֶ���</param>
        /// <returns></returns>
        public string GetSequenceName(string tableName, string paramName)
        {
            return null;
        }

        /// <summary>
        /// ��ʼ��������
        /// </summary>
        /// <param name="seqName"></param>
        public void InitSequence(string seqName,DataBaseOperate oper)
        {
        }
        /// <summary>
        /// ����������ת���ɵ�ǰ���ݿ�֧�ֵ�����
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DbType ToCurrentDbType(DbType type) 
        {
            return CurrentDbType(type);
        }

        internal static DbType CurrentDbType(DbType type) 
        {
            DbType ret = type;
            switch (ret)
            {
                case DbType.Time:
                    ret = DbType.DateTime;
                    break;
                case DbType.UInt16:
                    ret = DbType.Int32;
                    break;
                case DbType.UInt32:
                    ret = DbType.Int64;
                    break;
                case DbType.UInt64:
                    ret = DbType.Int64;
                    break;
                case DbType.Date:
                    ret = DbType.DateTime;
                    break;
                case DbType.SByte:
                    ret = DbType.Byte;
                    break;
                case DbType.VarNumeric:
                    ret = DbType.Currency;
                    break;
                default:
                    break;
            }
            return ret;
        }

        /// <summary>
        /// ��ȡSQL������
        /// </summary>
        /// <returns></returns>
        public IDbCommand GetCommand() 
        {
            IDbCommand comm = new SqlCommand();
            return comm;
        }
        /// <summary>
        /// ��ȡSQL����
        /// </summary>
        /// <returns></returns>
        public virtual IDbConnection GetConnection()
        {
            IDbConnection conn = new SqlConnection();
            return conn;
        }
        /// <summary>
        /// ��ȡSQL������
        /// </summary>
        /// <returns></returns>
        public IDbDataAdapter GetAdapter()
        {
            IDbDataAdapter adapter = new SqlDataAdapter();
            return adapter;
        }

        /// <summary>
        /// ��ʽ���ֶ���
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public string FormatParam(string paramName) 
        {
            
            return "[" + paramName + "]";
        }

        /// <summary>
        /// ��ʽ��������
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public string FormatTableName(string tableName)
        {
            return FormatParam(tableName);
        }
        /// <summary>
        /// ��ʽ��������
        /// </summary>
        /// <param name="pname"></param>
        /// <returns></returns>
        public string FormatValueName(string pname)
        {
            return "@" + pname;
        }

        /// <summary>
        /// ��ʽ�������ļ���
        /// </summary>
        /// <param name="pname"></param>
        /// <returns></returns>
        public string FormatParamKeyName(string pname)
        {
            return "@" + pname;
        }
        /// <summary>
        /// ����ȫ�ļ����Ĳ�ѯ���
        /// </summary>
        /// <param name="paranName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string FreeTextLike(string paranName,string value) 
        {
            return " (contains([" + paranName + "]," + value + "))";
        }
        /// <summary>
        /// ��ȡ��ǰʱ��
        /// </summary>
        /// <returns></returns>
        public string GetNowDate(DbType dbType)
        {
            return "getdate()";
        }
        /// <summary>
        /// �α��ҳ
        /// </summary>
        /// <typeparam name="T">ʵ������</typeparam>
        /// <param name="sql">sql���</param>
        /// <param name="objPage">��ҳʵ��</param>
        /// <param name="oper">���ݿ�����</param>
        /// <returns></returns>
        public IDataReader Query(string sql, PageContent objPage, DataBaseOperate oper)
        {
            return CursorPageCutter.Query(sql, objPage, oper);
        }

        /// <summary>
        /// ��ѯ���ҷ���DataSet(�α��ҳ)
        /// </summary>
        /// <param name="sql">Ҫ��ѯ��SQL���</param>
        /// <param name="objPage">��ҳ����</param>
        /// <param name="oper">���ݿ����</param>
        /// <param name="curType">ӳ���ʵ������(����û����ݿ��ԭ���������Ϊnull)</param>
        /// <returns></returns>
        public DataTable QueryDataTable(string sql, PageContent objPage, DataBaseOperate oper, Type curType)
        {
            return CursorPageCutter.QueryDataTable(sql, objPage, oper, curType);
        }
        /// <summary>
        /// �α��ҳ
        /// </summary>
        /// <typeparam name="T">ʵ������</typeparam>
        /// <param name="lstParam">��������</param>
        /// <param name="sql">sql���</param>
        /// <param name="objPage">��ҳʵ��</param>
        /// <param name="oper">���ݿ�����</param>
        /// <returns></returns>
        public IDataReader Query(string sql,ParamList lstParam, PageContent objPage, DataBaseOperate oper)
        {
            throw new Exception("SqlServer��֧�ִ��������α��ҳ");
        }

        /// <summary>
        /// ��ѯ���ҷ���DataSet(�α��ҳ)
        /// </summary>
        /// <param name="sql">Ҫ��ѯ��SQL���</param>
        /// <param name="lstParam">��������</param>
        /// <param name="objPage">��ҳ����</param>
        /// <param name="oper">���ݿ����</param>
        /// <param name="curType">ӳ���ʵ������(����û����ݿ��ԭ���������Ϊnull)</param>
        /// <returns></returns>
        public DataTable QueryDataTable(string sql,ParamList lstParam, PageContent objPage, DataBaseOperate oper, Type curType)
        {
            throw new Exception("SqlServer��֧�ִ��������α��ҳ");
        }
        /// <summary>
        /// ���ɷ�ҳSQL���
        /// </summary>
        /// <param name="list">�����б�</param>
        /// <param name="oper">���Ӷ���</param>
        /// <param name="objCondition">��������</param>
        /// <param name="objPage">��ҳ��¼��</param>
        /// <returns></returns>
        public virtual string CreatePageSql(ParamList list, DataBaseOperate oper, SelectCondition objCondition, PageContent objPage) 
        {
            return CutPageSqlCreater.CreatePageSql(list, oper, objCondition, objPage);
        }


        
        
        /// <summary>
        /// ��ȡ�ַ���ƴ��SQl���
        /// </summary>
        /// <param name="str">�ַ�������</param>
        /// <returns></returns>
        public string ConcatString(params string[] strs)
        {
            StringBuilder sbRet = new StringBuilder();
            foreach (string curStr in strs)
            {
                sbRet.Append(curStr + "+");
            }
            string ret = sbRet.ToString();
            if (ret.Length > 1)
            {
                ret = ret.Substring(0, ret.Length - 1);
            }
            return ret;
        }
        /// <summary>
        /// ��ȡ�Զ�������SQL
        /// </summary>
        /// <returns></returns>
        public string GetIdentitySQL(EntityInfoHandle info) 
        {
            return "select SCOPE_IDENTITY()";
        }
        /// <summary>
        /// ��ȡ�Զ�����ֵ��SQL
        /// </summary>
        /// <returns></returns>
        public string GetIdentityValueSQL(EntityInfoHandle info)
        {
            return null;
        }
        /// <summary>
        /// �ѱ���ת���SQL����е�ʱ�����ʽ
        /// </summary>
        /// <returns></returns>
        public string GetDateTimeString(object value)
        {
            return "'" + value.ToString().Replace("'","") + "'";
        }

        /// <summary>
        /// ����ʱ��������ֶ���
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string GetIdentityParamName(EntityPropertyInfo info) 
        {
            return null;
        }
        /// <summary>
        /// ����ʱ��������ֶ�ֵ
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string GetIdentityParamValue(EntityInfoHandle entityInfo, EntityPropertyInfo info)
        {
            return null;
        }

        /// <summary>
        /// ����Reader�����ݰ���ֵ����ʵ��
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="index">��ǰReader������</param>
        /// <param name="arg">Ŀ�����</param>
        /// <param name="info">Ŀ�����Եľ��</param>
        public static void ValueFromReader(IDataReader reader,int index,object arg,EntityPropertyInfo info) 
        {
            object val = reader.GetValue(index);
            Type resType = info.FieldType;//�ֶ�ֵ����

            val=CommonMethods.EntityProChangeType(val, resType);
            
            info.SetValue(arg, val);
        }
        /// <summary>
        /// ����Reader�����ݰ���ֵ����ʵ��
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="index">��ǰReader������</param>
        /// <param name="arg">Ŀ�����</param>
        /// <param name="info">Ŀ�����Եľ��</param>
        public void SetObjectValueFromReader(IDataReader reader, int index, object arg, EntityPropertyInfo info)
        {
            ValueFromReader(reader, index, arg, info);
        }
    }
}