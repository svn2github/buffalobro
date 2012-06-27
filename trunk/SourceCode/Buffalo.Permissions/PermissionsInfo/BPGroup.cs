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
    /// Ȩ����
    /// </summary>
    public partial class BPGroup:BPBase
    {
        /// <summary>
        /// �����ⲿϵͳ�����ʶ
        /// </summary>
        private string _groupID;

        /// <summary>
        /// �����ⲿϵͳ�ı���
        /// </summary>
        private string _groupName;


        /// <summary>
        /// �����ⲿϵͳ�����ʶ
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
        /// �����ⲿϵͳ�ı���
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
        /// ��ȡ��ѯ������
        /// </summary>
        /// <returns></returns>
        public static ModelContext<BPGroup> GetContext() 
        {
            return _____baseContext;
        }
    }
}