using System;
using System.Data;
using System.Configuration;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.PropertyAttributes;
namespace Buffalo.Permissions.PermissionsInfo.BQLEntity
{

    public partial class PermissionDB
    {
        private static PermissionDB_BPSysModelItem _BPSysModelItem = new PermissionDB_BPSysModelItem();
    
        public static PermissionDB_BPSysModelItem BPSysModelItem
        {
            get
            {
                return _BPSysModelItem;
            }
        }
    }

    /// <summary>
    ///  子模块权限
    /// </summary>
    public partial class PermissionDB_BPSysModelItem : PermissionDB_BPBase
    {
        private BQLEntityParamHandle _sysModelId = null;
        /// <summary>
        /// 所属模块ID
        /// </summary>
        public BQLEntityParamHandle SysModelId
        {
            get
            {
                return _sysModelId;
            }
         }
        private BQLEntityParamHandle _itemName = null;
        /// <summary>
        /// 子项名
        /// </summary>
        public BQLEntityParamHandle ItemName
        {
            get
            {
                return _itemName;
            }
         }
        private BQLEntityParamHandle _itemDescription = null;
        /// <summary>
        /// 子项注释
        /// </summary>
        public BQLEntityParamHandle ItemDescription
        {
            get
            {
                return _itemDescription;
            }
         }
        private BQLEntityParamHandle _itemIdentify = null;
        /// <summary>
        /// 子项标识
        /// </summary>
        public BQLEntityParamHandle ItemIdentify
        {
            get
            {
                return _itemIdentify;
            }
         }
        private BQLEntityParamHandle _defaultRight = null;
        /// <summary>
        /// 项权限
        /// </summary>
        public BQLEntityParamHandle DefaultRight
        {
            get
            {
                return _defaultRight;
            }
         }

        /// <summary>
        /// 所属模块
        /// </summary>
        public PermissionDB_BPSysModel BelongModel
        {
            get
            {
               return new PermissionDB_BPSysModel(this,"BelongModel");
            }
         }


		/// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public PermissionDB_BPSysModelItem(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(Buffalo.Permissions.PermissionsInfo.BPSysModelItem),parent,propertyName)
        {
			
        }
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        /// <param name="parent">父表信息</param>
        /// <param name="propertyName">属性名</param>
        public PermissionDB_BPSysModelItem(Type entityType,BQLEntityTableHandle parent,string propertyName) 
        :base(entityType,parent,propertyName)
        {
            _sysModelId=CreateProperty("SysModelId");
            _itemName=CreateProperty("ItemName");
            _itemDescription=CreateProperty("ItemDescription");
            _itemIdentify=CreateProperty("ItemIdentify");
            _defaultRight=CreateProperty("DefaultRight");

        }
        
        /// <summary>
        /// 初始化本类的信息
        /// </summary>
        public PermissionDB_BPSysModelItem() 
            :this(null,null)
        {
        }
    }
}
