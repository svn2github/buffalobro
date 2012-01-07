using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.DataBaseAdapter.IDbAdapters;
using Buffalobro.DB.DataBaseAdapter;

namespace Buffalobro.DB.CsqlCommon.CsqlConditionCommon
{
    /// <summary>
    /// 字段别名
    /// </summary>
    public class AliasTabelParamHandle:CsqlParamHandle
    {
        private string aliasTableName;
        private string propertyName;
        private string resault = null;
        public AliasTabelParamHandle(string aliasTableName, string propertyName)
        {
            this.aliasTableName = aliasTableName;
            this.propertyName = propertyName;
        }

        

        private void CreateSql(KeyWordInfomation info,bool isFillInfo)
        {
            IDBAdapter idba = info.DBInfo.CurrentDbAdapter;
            //QueryParamCollection prms = info.Alias[aliasTableName];
            if (propertyName == "*")//查询全部字段时候
            {
                //if (isFillInfo)
                //{
                //    foreach (ParamInfo pinfo in prms)
                //    {
                //        ParamInfo prmInfo = new ParamInfo(pinfo.PropertyName, pinfo.ParamName, pinfo.DataValueType);
                //        info.QueryParams[pinfo.PropertyName] = prmInfo;
                //    }
                //}
                resault = idba.FormatTableName(aliasTableName) + ".*";
            }
            else 
            {
                //if (isFillInfo)
                //{
                //    ParamInfo prmInfo = new ParamInfo(propertyName, prms[propertyName].ParamName, prms[propertyName].DataValueType);
                //    info.QueryParams[propertyName] = prmInfo;
                //}
                resault = idba.FormatTableName(aliasTableName) + "." + idba.FormatParam(propertyName);
                //this.valueDataType = prms[propertyName].DataValueType;
            }

        }
        internal override void FillInfo(KeyWordInfomation info) 
        {
            CreateSql(info, true);
        }
        internal override string DisplayValue(KeyWordInfomation info)
        {
            if (resault == null) 
            {
                CreateSql(info, false);
            }
            return resault;
        }


    }
}
