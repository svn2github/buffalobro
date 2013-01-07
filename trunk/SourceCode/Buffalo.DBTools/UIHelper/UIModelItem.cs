using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DBTools.HelperKernel;

namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// UI需要的信息
    /// </summary>
    public class UIModelItem
    {
        /// <summary>
        /// UI信息
        /// </summary>
        /// <param name="dicCheckItem">选中项</param>
        /// <param name="belongProperty">所属属性</param>
        public UIModelItem(Dictionary<string, object> dicCheckItem, EntityParamField belongProperty) 
        {
            _dicCheckItem = dicCheckItem;
            _belongProperty = belongProperty;
        }

        private Dictionary<string, object> _dicCheckItem;

        /// <summary>
        /// 获取是否选中项
        /// </summary>
        /// <param name="itemName">项名称</param>
        /// <returns></returns>
        public bool HasItem(string itemName) 
        {
            object ret = false;
            if (_dicCheckItem.TryGetValue(itemName, out ret)) 
            {
                if(ret is bool)
                {
                    return (bool)ret;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public string GetValue(string itemName) 
        {
            object ret = false;
            if (_dicCheckItem.TryGetValue(itemName, out ret))
            {
                return ret.ToString();
            }
            return null;
        }

        private EntityParamField _belongProperty;
        /// <summary>
        /// 所属属性
        /// </summary>
        public EntityParamField BelongProperty
        {
            get { return _belongProperty; }
        }
    }
}
