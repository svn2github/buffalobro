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
    ///  ��ģ��Ȩ��
    /// </summary>
    public partial class PermissionDB_BPSysModelItem : PermissionDB_BPBase
    {
        private BQLEntityParamHandle _sysModelId = null;
        /// <summary>
        /// ����ģ��ID
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
        /// ������
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
        /// ����ע��
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
        /// �����ʶ
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
        /// ��Ȩ��
        /// </summary>
        public BQLEntityParamHandle DefaultRight
        {
            get
            {
                return _defaultRight;
            }
         }

        /// <summary>
        /// ����ģ��
        /// </summary>
        public PermissionDB_BPSysModel BelongModel
        {
            get
            {
               return new PermissionDB_BPSysModel(this,"BelongModel");
            }
         }


		/// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
        public PermissionDB_BPSysModelItem(BQLEntityTableHandle parent,string propertyName) 
        :this(typeof(Buffalo.Permissions.PermissionsInfo.BPSysModelItem),parent,propertyName)
        {
			
        }
        /// <summary>
        /// ��ʼ���������Ϣ
        /// </summary>
        /// <param name="parent">������Ϣ</param>
        /// <param name="propertyName">������</param>
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
        /// ��ʼ���������Ϣ
        /// </summary>
        public PermissionDB_BPSysModelItem() 
            :this(null,null)
        {
        }
    }
}