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
    /// 系统模块
    /// </summary>
    public partial class BPSysModel:BPBase
    {
        public BPSysModel() { }
        /// <summary>
        /// 系统模块
        /// </summary>
        /// <param name="modelIdentify">模块标识</param>
        /// <param name="modelName">模块名</param>
        /// <param name="modelType">模块类型</param>
        /// <param name="modelDescription">模块注释</param>
        public static BPSysModel CreateSysModel(string modelIdentify, string modelName, 
            string modelType,string modelDescription) 
        {
            BPSysModel model = new BPSysModel();
            model.ModelIdentify = modelIdentify;
            model.ModelName = modelName;
            model.ModelType = modelType;
            model.ModelDescription = modelDescription;

            return model;
        }

        /// <summary>
        /// 模块标识
        /// </summary>
        private string _modelIdentify;
        /// <summary>
        /// 模块名称
        /// </summary>
        private string _modelName;

        /// <summary>
        /// 模块注释
        /// </summary>
        private string _modelDescription;

        /// <summary>
        /// 模块分类
        /// </summary>
        private string _modelType;
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModelName
        {
            get
            {
                return _modelName;
            }
            set
            {
                _modelName=value;
                OnPropertyUpdated("ModelName");
            }
        }
        /// <summary>
        /// 模块注释
        /// </summary>
        public string ModelDescription
        {
            get
            {
                return _modelDescription;
            }
            set
            {
                _modelDescription=value;
                OnPropertyUpdated("ModelDescription");
            }
        }
        /// <summary>
        /// 模块分类
        /// </summary>
        public string ModelType
        {
            get
            {
                return _modelType;
            }
            set
            {
                _modelType=value;
                OnPropertyUpdated("ModelType");
            }
        }
        /// <summary>
        /// 默认权限
        /// </summary>
        private BPModelRights _defaultModelRight;
        

        private List<BPSysModelItem> _lstModelItem;




        private static ModelContext<BPSysModel> _____baseContext=new ModelContext<BPSysModel>();
        /// <summary>
        /// 获取查询关联类
        /// </summary>
        /// <returns></returns>
        public static ModelContext<BPSysModel> GetContext() 
        {
            return _____baseContext;
        }
        /// <summary>
        /// 
        /// </summary>
        public List<BPSysModelItem> LstModelItem
        {
            get
            {
               if (_lstModelItem == null)
               {
                   FillChild("LstModelItem");
               }
                return _lstModelItem;
            }
        }

        /// <summary>
        /// 添加模块子项
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="description"></param>
        public void AddModelItem(string itemIdentify,string itemName, string description) 
        {
            if(_lstModelItem==null)
            {
                _lstModelItem=new List<BPSysModelItem>();
            }

            BPSysModelItem item = new BPSysModelItem();
            item.BelongModel = this;
            item.ItemName = itemName;
            item.ItemIdentify = itemIdentify;
            item.ItemDescription = description;
            
            _lstModelItem.Add(item);
        }

        /// <summary>
        /// 模块标识
        /// </summary>
        public string ModelIdentify
        {
            get
            {
                return _modelIdentify;
            }
            set
            {
                _modelIdentify=value;
                OnPropertyUpdated("ModelIdentify");
            }
        }
        /// <summary>
        /// 默认权限
        /// </summary>
        public BPModelRights DefaultModelRight
        {
            get
            {
                return _defaultModelRight;
            }
            set
            {
                _defaultModelRight=value;
                OnPropertyUpdated("DefaultModelRight");
            }
        }
    }
}
