using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;
using Buffalobro.DB.CsqlCommon.CsqlAggregateFunctions;
using System.Data;
using Buffalobro.Kernel.Defaults;
using Buffalobro.DB.DataBaseAdapter;

namespace Buffalobro.DB.CsqlCommon.CsqlExtendFunction
{
    public delegate string DelConvertFunction(string value,DbType type,DBInfo info);
    public delegate string DelConvertFunction2(string value, string value1, DBInfo info);
    public class CsqConvertFunction : CsqlParamHandle
    {
        public DelConvertFunction funHandle1;
        public DelConvertFunction2 funHandle2;
        private string format;
        private CsqlValueItem value;
        //private DbType convertType;
        public CsqConvertFunction(CsqlValueItem value, DbType convertType, DelConvertFunction funHandle)
        {
            this.funHandle1 = funHandle;
            this.value = value;
            this._valueDbType = convertType;
            //SetType(convertType);
        }

        public CsqConvertFunction(CsqlValueItem value, string format, DelConvertFunction2 funHandle)
        {
            this.funHandle2 = funHandle;
            this.value = value;
            this._valueDbType = DbType.Object;
            this.format = format;
        }
        internal override void FillInfo(KeyWordInfomation info)
        {
            
                value.FillInfo(info);
            
        }

        ///// <summary>
        ///// �������ݿ�����ָ����������
        ///// </summary>
        ///// <param name="type"></param>
        //private void SetType(DbType type) 
        //{
        //    if (type == DbType.AnsiString || type == DbType.AnsiStringFixedLength || type == DbType.String ||
        //        type == DbType.StringFixedLength)
        //    {
        //        this.ValueDataType = DefaultType.StringType;
        //    }
        //    else if (type == DbType.Int64 ||
        //         type == DbType.Int32 || type == DbType.Int16 || type == DbType.SByte)
        //    {
        //        this.ValueDataType = DefaultType.IntType;
        //    }
        //    else if (type == DbType.UInt32 || type == DbType.UInt64 || type == DbType.UInt16 || type == DbType.Byte || type == DbType.Boolean)
        //    {
        //        this.ValueDataType = DefaultType.IntType;
        //    }
        //    else if (type == DbType.Decimal || type == DbType.Double || type == DbType.Double
        //        || type == DbType.Currency || type == DbType.VarNumeric)
        //    {
        //        this.ValueDataType = DefaultType.DecimalType;
        //    }
        //    else if (type == DbType.DateTime || type == DbType.Time || type == DbType.Date)
        //    {
        //        this.ValueDataType = DefaultType.DateTimeType;
        //    }
        //}

        internal override string DisplayValue(KeyWordInfomation info)
        {
            
            string pValue1 = value.DisplayValue(info);
            if (funHandle2!=null)
            {
                return funHandle2(pValue1, format,info.DBInfo);
            }
            else
            {
                return funHandle1(pValue1, _valueDbType,info.DBInfo);
            }
        }
    }
}