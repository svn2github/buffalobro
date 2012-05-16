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
    /// 用户组的模块权限
    /// </summary>
    public partial class BPGroupModelRight:BPBase
    {
        /// <summary>
        /// 组模块ID
        /// </summary>
        private int? _modelId;
        /// <summary>
        /// 组ID
        /// </summary>
        private int? _groupId;

        /// <summary>
        /// 模块权限
        /// </summary>
        private BPModelRights _ModelRight;

        /// <summary>
        /// 所属模块
        /// </summary>
        private BPSysModel _belongModel;
        /// <summary>
        /// 所属分组
        /// </summary>
        private BPGroup _belongGroup;

        /// <summary>
        /// 此组的子项权限
        /// </summary>
        private List<BPGroupItemRight> _lstGroupItemRight;

        /// <summary>
        /// 组模块ID
        /// </summary>
        public int? ModelId
        {
            get
            {
                return _modelId;
            }
            set
            {
                _modelId=value;
                OnPropertyUpdated("ModelId");
            }
        }
        /// <summary>
        /// 组ID
        /// </summary>
        public int? GroupId
        {
            get
            {
                return _groupId;
            }
            set
            {
                _groupId=value;
                OnPropertyUpdated("GroupId");
            }
        }
        /// <summary>
        /// 模块权限
        /// </summary>
        public BPModelRights ModelRight
        {
            get
            {
                return _ModelRight;
            }
            set
            {
                _ModelRight=value;
                OnPropertyUpdated("ModelRight");
            }
        }

        private static ModelContext<BPGroupModelRight> _____baseContext=new ModelContext<BPGroupModelRight>();
        /// <summary>
        /// 获取查询关联类
        /// </summary>
        /// <returns></returns>
        public static ModelContext<BPGroupModelRight> GetContext() 
        {
            return _____baseContext;
        }
        /// <summary>
        /// 所属模块
        /// </summary>
        public BPSysModel BelongModel
        {
            get
            {
               if (_belongModel == null)
               {
                   FillParent("BelongModel");
               }
                return _belongModel;
            }
            set
            {
                _belongModel = value;
                OnPropertyUpdated("BelongModel");
            }
        }
        /// <summary>
        /// 所属分组
        /// </summary>
        public BPGroup BelongGroup
        {
            get
            {
               if (_belongGroup == null)
               {
                   FillParent("BelongGroup");
               }
                return _belongGroup;
            }
            set
            {
                _belongGroup = value;
                OnPropertyUpdated("BelongGroup");
            }
        }
        /// <summary>
        /// 此组的子项权限
        /// </summary>
        public List<BPGroupItemRight> LstGroupItemRight
        {
            get
            {
               if (_lstGroupItemRight == null)
               {
                   FillChild("LstGroupItemRight");
               }
                return _lstGroupItemRight;
            }
        }
    }
}
