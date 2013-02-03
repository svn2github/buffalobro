using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Buffalo.Kernel;
namespace Buffalo.Kernel.FastReflection
{
    public class FastValueGetSet
    {
        private static Dictionary<string, FastInvokeHandler> dicMethod = new Dictionary<string, FastInvokeHandler>();
        private static Dictionary<string, PropertyInfoHandle> dicProperty = new Dictionary<string, PropertyInfoHandle>();//属性缓存
        private static IDictionary<string, CreateInstanceHandler> _invokerInstance = new Dictionary<string, CreateInstanceHandler>();
        public const BindingFlags AllBindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.SuppressChangeType | BindingFlags.Instance;
        /// <summary>
        /// 获取属性的信息
        /// </summary>
        /// <param name="proName">属性名</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static PropertyInfoHandle GetPropertyInfoHandle(string proName, Type type)
        {
            string fullName = type.FullName + "." + proName;
            PropertyInfoHandle propertyHandle = null;
            using (Lock objLock = new Lock(dicProperty))
            {
                if (!dicProperty.TryGetValue(fullName, out propertyHandle))
                {
                    propertyHandle = GetPropertyInfoHandleWithOutCache(proName, type);
                    dicProperty.Add(fullName, propertyHandle);
                }
            }
            return propertyHandle;
        }


        

        /// <summary>
        /// 获取属性的信息(不使用缓存)
        /// </summary>
        /// <param name="proName">属性名</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static PropertyInfoHandle GetPropertyInfoHandleWithOutCache(string proName, Type type)
        {
            FastInvokeHandler getHandle = GetGetMethodInfo(proName, type);
            FastInvokeHandler setHandle = GetSetMethodInfo(proName, type);
            PropertyInfo pinf = type.GetProperty(proName, AllBindingFlags);//获取子元素集合的属性
            Type proType = null;
            if (pinf != null)
            {
                proType = pinf.PropertyType;
            }
            PropertyInfoHandle propertyHandle = new PropertyInfoHandle(type, getHandle, setHandle, proType, proName);
            return propertyHandle;
        }

        /// <summary>
        /// 获取获取值的方法接口
        /// </summary>
        /// <param name="proName">属性名</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static FastInvokeHandler GetGetMethodInfo(string proName,Type type)
        {
            MethodInfo methodInfo = type.GetMethod("get_" + proName,AllBindingFlags);
            if (methodInfo == null)
            {
                return null;
            }else if(methodInfo.GetParameters().Length>0)
            {
                return null;
            }
            FastInvokeHandler fastInvoker = FastInvoke.GetMethodInvoker(methodInfo);
            return fastInvoker;
        }


        /// <summary>
        /// 获取设置值的方法接口
        /// </summary>
        /// <param name="proName">属性名</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static FastInvokeHandler GetSetMethodInfo(string proName, Type type)
        {
            MethodInfo methodInfo = type.GetMethod("set_" + proName, AllBindingFlags);
            if (methodInfo == null) 
            {
                return null;
            }
            else if (methodInfo.GetParameters().Length != 1)
            {
                return null;
            }
            FastInvokeHandler fastInvoker = FastInvoke.GetMethodInvoker(methodInfo);

            return fastInvoker;
        }
        /// <summary>
        /// 获取该类型的指定方法的委托
        /// </summary>
        /// <param name="objectType">所属类的类型</param>
        /// <param name="methodName">方法名</param>
        /// <param name="parametersType">参数列表</param>
        /// <returns></returns>
        public static FastInvokeHandler GetCustomerMethodInfo(Type objectType,string methodName,Type[] parametersType)
        {
            MethodInfo methodInfo = objectType.GetMethod(methodName, AllBindingFlags, null, parametersType, null);
            if (methodInfo != null)
            {
                return GetCustomerMethodInfo(methodInfo);
            }
            return null;
        }
        /// <summary>
        /// 获取该类型的指定方法的委托
        /// </summary>
        /// <param name="methodInfo">函数信息</param>
        /// <param name="parametersType">参数列表</param>
        /// <returns></returns>
        public static FastInvokeHandler GetCustomerMethodInfo(MethodInfo methodInfo)
        {
            string ptypes = "";
            ParameterInfo[] info = methodInfo.GetParameters();
            foreach (ParameterInfo ptype in info)
            {
                if (ptype.IsOut) 
                {
                    ptypes += "out ";
                }
                else if (ptype.IsRetval)
                {
                    ptypes += "Retva ";
                }
                else if (ptype.IsLcid)
                {
                    ptypes += "Lcid ";
                }
                else if (ptype.IsOptional)
                {
                    ptypes += "Optional ";
                }
                ptypes += ptype.ParameterType.FullName + ",";
            }
            if (ptypes.Length > 0)
            {
                ptypes = ptypes.Substring(0, ptypes.Length - 1);
            }
            string fullName = methodInfo.DeclaringType.FullName + "." + methodInfo.Name + "(" + ptypes + ")";
            FastInvokeHandler fastInvokerHandle = null;
            using (Lock objLock = new Lock(dicMethod))
            {
                if (!dicMethod.TryGetValue(fullName, out fastInvokerHandle))
                {
                    if (methodInfo == null)
                    {
                        return null;
                    }
                    FastInvokeHandler fastInvoker = FastInvoke.GetMethodInvoker(methodInfo);
                    dicMethod.Add(fullName, fastInvoker);
                    fastInvokerHandle = fastInvoker;
                }
            }
            return fastInvokerHandle;
        }

        /// <summary>
        /// 对对象赋值
        /// </summary>
        /// <param name="args">对象</param>
        /// <param name="value">值</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="objType">对象类型</param>
        public static void SetValue(object args, object value, string propertyName,Type objType) 
        {
            PropertyInfoHandle fastInvoker = GetPropertyInfoHandle(propertyName, objType);
            fastInvoker.SetValue(args,value);
        }

        /// <summary>
        /// 获取对象赋值
        /// </summary>
        /// <param name="args">对象</param>
        /// <param name="value">值</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="objType">对象类型</param>
        public static object GetValue(object args, string propertyName, Type objType)
        {
            PropertyInfoHandle fastInvoker = GetPropertyInfoHandle(propertyName, objType);
            return fastInvoker.GetValue(args);
        }

        
        /// <summary>
        /// 生成实体类
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object CreateObject(Type type)
        {
            CreateInstanceHandler create = GetCreateInstanceHandler(type);
            return create.Invoke();
        }

        /// <summary>
        /// 根据类型查找指定的创建对象的代理
        /// </summary>
        /// <param name="type">类型</param> 
        /// <returns></returns> 
        public static CreateInstanceHandler GetCreateInstanceHandler(Type type) 
        {
            CreateInstanceHandler create = null;
            string key = type.FullName;
            using (Lock objLock = new Lock(_invokerInstance))
            {
                if (!_invokerInstance.TryGetValue(key, out create))
                {
                    create = GetCreateInstanceHandlerWithOutCache(type);
                    _invokerInstance.Add(key, create);
                }
            }
            return create;
        }

        /// <summary>
        /// 根据类型查找指定的创建对象的代理
        /// </summary>
        /// <param name="type">类型</param> 
        /// <returns></returns> 
        public static CreateInstanceHandler GetCreateInstanceHandlerWithOutCache(Type type)
        {
            CreateInstanceHandler create = null;
            create = FastInvoke.GetInstanceCreator(type);
            return create;
        }
    }
}
