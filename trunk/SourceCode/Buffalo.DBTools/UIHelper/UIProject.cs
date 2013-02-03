using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// 项目项
    /// </summary>
    public class UIProject
    {
        private string _name;
        /// <summary>
        /// 项目名
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private List<UIProjectItem> _lstItems=new List<UIProjectItem>();

        /// <summary>
        /// 项目项
        /// </summary>
        public List<UIProjectItem> LstItems
        {
            get { return _lstItems; }
        }

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <returns></returns>
        public string GenerateCode(EntityInfo entityInfo, UIConfigItem classConfig,
            List<UIModelItem> selectPropertys) 
        {
            foreach (UIProjectItem pitem in _lstItems) 
            {
                string mPath = UIConfigItem.FormatParameter(pitem.ModelPath, entityInfo);
                string tPath = UIConfigItem.FormatParameter(pitem.TargetPath, entityInfo);

            }
        }

    }
}
