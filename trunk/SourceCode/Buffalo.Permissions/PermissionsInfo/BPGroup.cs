using System;
using System.Collections.Generic;
using System.Text;

using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
using System.Data;
using Buffalo.DB.CommBase.BusinessBases;
namespace Buffalo.Permissions.PermissionsInfo
{
    /// <summary>
    /// 权限组
    /// </summary>
    public partial class BPGroup:BPBase
    {
        /// <summary>
        /// 来自外部系统的组标识
        /// </summary>
        private string _groupID;

        /// <summary>
        /// 来自外部系统的表名
        /// </summary>
        private string _groupName;


        /// <summary>
        /// 来自外部系统的组标识
        /// </summary>
        public string GroupID
        {
            get
            {
                return _groupID;
            }
            set
            {
                _groupID=value;
                OnPropertyUpdated("GroupID");
            }
        }
        /// <summary>
        /// 来自外部系统的表名
        /// </summary>
        public string GroupName
        {
            get
            {
                return _groupName;
            }
            set
            {
                _groupName=value;
                OnPropertyUpdated("GroupName");
            }
        }

        

        private static ModelContext<BPGroup> _____baseContext=new ModelContext<BPGroup>();
        /// <summary>
        /// 获取查询关联类
        /// </summary>
        /// <returns></returns>
        public static ModelContext<BPGroup> GetContext() 
        {
            return _____baseContext;
        }
    }
}
