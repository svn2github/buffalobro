using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Buffalo.Kernel.Defaults
{
    public class DefaultType
    {
        private static TypeItem[] dbTypeMapping;
        private static Dictionary<string, TypeItem> dic = InitDefauleValues();
        

        private static Dictionary<string, TypeItem> InitDefauleValues()
        {
            Dictionary<string, TypeItem> ret = new Dictionary<string, TypeItem>();
            dbTypeMapping = new TypeItem[28];


            TypeItem item = new TypeItem(typeof(string), null, DbType.AnsiString);
            ret.Add(item.ItemType.FullName, item);
            dbTypeMapping[(int)item.DbType] = item;
            dbTypeMapping[(int)DbType.AnsiStringFixedLength] = item;
            dbTypeMapping[(int)DbType.String] = item;
            dbTypeMapping[(int)DbType.StringFixedLength] = item;


            item = new TypeItem(typeof(DateTime), DefaultValue.DefaultDateTimeValue,DbType.DateTime);
            ret.Add(item.ItemType.FullName, item);
            dbTypeMapping[(int)item.DbType] = item;
            dbTypeMapping[(int)DbType.Date] = item;
            dbTypeMapping[(int)DbType.Time] = item;
            dbTypeMapping[26] = item;//DateTime2
            dbTypeMapping[27] = item;//DateTimeOffset

            

            item=new TypeItem(typeof(byte[]), null, DbType.Binary);
            ret.Add(item.ItemType.FullName, item);
            dbTypeMapping[(int)item.DbType] = item;

            


            item=new TypeItem(typeof(System.DBNull), System.DBNull.Value, DbType.Object);
            ret.Add(item.ItemType.FullName, item);
            dbTypeMapping[(int)item.DbType] = item;

            


            item=new TypeItem(typeof(Guid), DefaultValue.DefaultGuidValue,DbType.Guid);
            ret.Add(item.ItemType.FullName, item);
            dbTypeMapping[(int)item.DbType] = item;


            item=new TypeItem(typeof(int), DefaultValue.DefaultIntValue,DbType.Int32);
            ret.Add(item.ItemType.FullName, item);
            dbTypeMapping[(int)item.DbType] = item;


            item = new TypeItem(typeof(double), DefaultValue.DefaultDoubleValue, DbType.Double);
            ret.Add(item.ItemType.FullName, item);
            dbTypeMapping[(int)item.DbType] = item;


            item = new TypeItem(typeof(float), DefaultValue.DefaultFloatValue, DbType.Single);
            ret.Add(item.ItemType.FullName, item);
            dbTypeMapping[(int)item.DbType] = item;


            item = new TypeItem(typeof(decimal), DefaultValue.DefaultDecimalValue, DbType.Decimal);
            ret.Add(item.ItemType.FullName, item);
            dbTypeMapping[(int)item.DbType] = item;
            dbTypeMapping[(int)DbType.Currency] = item;
            dbTypeMapping[(int)DbType.VarNumeric] = item;

            item = new TypeItem(typeof(sbyte), DefaultValue.DefaultSbyteValue, DbType.SByte);
            ret.Add(item.ItemType.FullName, item);
            dbTypeMapping[(int)item.DbType] = item;


            item = new TypeItem(typeof(short), DefaultValue.DefaultShortValue, DbType.Int16);
            ret.Add(item.ItemType.FullName, item);
            dbTypeMapping[(int)item.DbType] = item;


            item = new TypeItem(typeof(long), DefaultValue.DefaultLongValue, DbType.Int64);
            ret.Add(item.ItemType.FullName, item);
            dbTypeMapping[(int)item.DbType] = item;

            item = new TypeItem(typeof(bool), null, DbType.Boolean);
            ret.Add(item.ItemType.FullName, item);
            dbTypeMapping[(int)item.DbType] = item;


            item = new TypeItem(typeof(object), null, DbType.Object);
            ret.Add(item.ItemType.FullName, item);
            dbTypeMapping[(int)item.DbType] = item;

            return ret;
        }

        /// <summary>
        /// 获取C#类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetCSharpVariableType(DbType type)
        {
            int index = (int)type;
            if (index >= 0 && index < dbTypeMapping.Length) 
            {
                TypeItem item = dbTypeMapping[index];
                if (item != null) 
                {
                    return item.ItemType;
                }
            }
            return null;
        }

        /// <summary>
        /// 判断是否默认值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDefaultValue(object value) 
        {
            if (value == null) 
            {
                return true;
            }
            string typeName = value.GetType().FullName;
            TypeItem item = null;
            if (dic.TryGetValue(typeName, out item)) 
            {
                return value.Equals(item.DefaultValue);
            }
            return false;
        }

        /// <summary>
        /// 获取该类型的默认值
        /// </summary>
        /// <param name="objType"></param>
        /// <returns></returns>
        public static TypeItem GetDefaultValue(Type objType) 
        {
            string typeName = objType.FullName;
            TypeItem item = null;
            dic.TryGetValue(typeName, out item);
            return item;
        }

        public readonly static Type IntType = typeof(int?);
        public readonly static Type DoubleType = typeof(double?);
        public readonly static Type StringType = typeof(string);
        public readonly static Type FloatType = typeof(float?);
        public readonly static Type DateTimeType = typeof(DateTime);
        public readonly static Type DecimalType = typeof(decimal?);
        public readonly static Type ByteType = typeof(byte?);
        public readonly static Type SbyteType = typeof(sbyte?);
        public readonly static Type ShortType = typeof(short?);
        public readonly static Type LongType = typeof(long?);
        public readonly static Type BytesType = typeof(byte[]);
        public readonly static Type BooleanType = typeof(bool?);
        public readonly static Type DBNullType = typeof(System.DBNull);
        public readonly static Type UShortType = typeof(ushort?);
        public readonly static Type UIntType = typeof(uint?);
        public readonly static Type ULongType = typeof(ulong?);
        public readonly static Type GUIDType = typeof(Guid);
        /// <summary>
        /// 判断是否该类型的子类
        /// </summary>
        /// <param name="type">当前类</param>
        /// <param name="baseType">基类</param>
        /// <returns></returns>
        public static bool IsInherit(Type type, Type baseType)
        {
            if (type.BaseType == null) return false;
            if (type.BaseType == baseType) return true;
            return IsInherit(type.BaseType, baseType);
        }

        /// <summary>
        /// 判断变量类型是否此类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="objType"></param>
        /// <returns></returns>
        public static bool EqualType(Type vType, Type objType) 
        {

            if (vType == objType) 
            {
                return true;
            }
            Type rType = GetRealValueType(objType);
            if (vType == rType) 
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 获取泛型类里边的类型
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static Type GetRealValueType(Type dataType) 
        {
            if (dataType.IsGenericType)
            {
                Type[] genTypes = dataType.GetGenericArguments();
                if (genTypes.Length > 0)
                {
                    return genTypes[0];
                }
            }
            return dataType;
        }

        /// <summary>
        /// 获取泛型里边的类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="isherit"></param>
        /// <returns></returns>
        public static Type[] GetGenericType(Type type,bool isherit) 
        {
            Type rootType = typeof(object);
            do
            {
                if (type.IsGenericType)
                {
                    return type.GetGenericArguments();
                }
                type = type.BaseType;
                if (type==null)
                {
                    break;
                }
            } while (isherit);
            return null;
        }

        /// <summary>
        /// 判断是否实现了该接口
        /// </summary>
        /// <param name="type">当前类</param>
        /// <param name="interfaceType">接口类型</param>
        /// <returns></returns>
        public bool IsImplement(Type type, Type interfaceType)
        {
            if (!interfaceType.IsInterface) 
            {
                return false;
            }
            Type[] infTypes = type.GetInterfaces();
            foreach (Type objType in infTypes) 
            {
                if (objType == interfaceType) 
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 把C#类型转换成SQL类型
        /// </summary>
        /// <param name="valueType"></param>
        /// <returns></returns>
        public static DbType ToDbType(Type valueType)
        {
            TypeItem item = GetDefaultValue(valueType);
            if (item != null) 
            {
                return item.DbType;
            }
            return DbType.Object;
        }

    }
}
