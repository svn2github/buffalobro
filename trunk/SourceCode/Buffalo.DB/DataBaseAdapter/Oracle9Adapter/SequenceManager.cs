using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.EntityInfos;
using System.Data;
using Buffalo.DB.DbCommon;

namespace Buffalo.DB.DataBaseAdapter.Oracle9Adapter
{
    /// <summary>
    /// 主键序列管理
    /// </summary>
    public class SequenceManager
    {
        //private static Dictionary<string, bool> dicSequence = new Dictionary<string, bool>();//序列已经初始化的集合

        /// <summary>
        /// 获取该属性的序列
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static string GetSequenceName(string tableName,string paramName) 
        {
            StringBuilder sbSeqName = new StringBuilder(20);
            sbSeqName.Append("seq_");
            sbSeqName.Append(tableName);
            sbSeqName.Append("_");
            sbSeqName.Append(paramName);
            sbSeqName.Replace(" ", "");

            return sbSeqName.ToString() ;
        }

        /// <summary>
        /// 初始化序列
        /// </summary>
        /// <param name="seqName"></param>
        public static string GetInitSequence(string seqName, DataBaseOperate oper)
        {

            if (!IsSequenceExists(seqName, oper)) //判断是否已经存在序列
            {
                string sql = "CREATE SEQUENCE \"" + seqName + "\" INCREMENT BY 1 START WITH 1  NOMAXVALUE  NOCYCLE  NoCACHE";//创建序列
                return sql;
            }
            return null;


        }

        /// <summary>
        /// 获取序列是否存在
        /// </summary>
        /// <param name="seqName">序列名</param>
        /// <param name="oper">数据库链接</param>
        /// <returns></returns>
        internal static bool IsSequenceExists(string seqName, DataBaseOperate oper)
        {
            string sql = "select SEQUENCE_NAME from user_sequences where SEQUENCE_NAME='" + seqName + "'";
            
            IDataReader reader = null;
            int count = 0;
            try
            {
                reader = oper.Query(sql, null);
                if (reader.Read())
                {
                    if (!reader.IsDBNull(0)) 
                    {
                        count = 1;
                    }
                   
                }
            }
            catch (Exception ex) 
            {
                throw new Exception("查询序列时候出现错误:" + ex.Message);
            }
            finally 
            {
                reader.Close();
            }
            return count > 0;
        }

    }
}
