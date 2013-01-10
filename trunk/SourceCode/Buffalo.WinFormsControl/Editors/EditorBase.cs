using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Buffalo.Kernel.Defaults;

namespace Buffalo.WinFormsControl.Editors
{
    public delegate void ValueChangeHandle(object sender,object oldValue,object newValue);
    public class EditorBase:UserControl
    {
        public ValueChangeHandle OnValueChange;

        private string _bindPropertyName;
        /// <summary>
        /// 绑定的属性名
        /// </summary>
        public string BindPropertyName
        {
            get { return _bindPropertyName; }
            set { _bindPropertyName = value; }
        }

        /// <summary>
        /// 值
        /// </summary>
        public virtual object Value 
        {
            get { return null; }
            set { }
        }
        private object _oldvalue;
        /// <summary>
        /// 引发值改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected void DoValueChange(object sender, object newValue)
        {
            if (OnValueChange != null) 
            {
                object ovalue = _oldvalue;
                if (ovalue==null && newValue != null) 
                {
                    ovalue = DefaultValue.DefaultForType(newValue.GetType());
                }
                OnValueChange(sender, ovalue, newValue);
                _oldvalue = newValue;
            }
        }
        /// <summary>
        /// 引发值改变事件
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected void DoValueChange(object newValue)
        {
            DoValueChange(this, newValue);
        }
    }
}
