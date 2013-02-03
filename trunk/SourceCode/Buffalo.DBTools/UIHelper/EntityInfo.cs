using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using Buffalo.DBTools.HelperKernel;

namespace Buffalo.DBTools.UIHelper
{
    /// <summary>
    /// ʵ����Ϣ
    /// </summary>
    public class EntityInfo
    {
        private string _fileName;
        /// <summary>
        /// �ļ���
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
        }
       private ClassDesignerInfo _designerInfo;
        /// <summary>
        /// �����ͼ��Ϣ
        /// </summary>
        public ClassDesignerInfo DesignerInfo
        {
            get { return _designerInfo; }
            set { _designerInfo = value; }
        }
        private string _namespace;
        /// <summary>
        /// �����ռ�
        /// </summary>
        public string Namespace
        {
            get { return _namespace; }
        }

        private string _className;
        /// <summary>
        /// ����
        /// </summary>
        public string ClassName
        {
            get { return _className; }
        }
        /// <summary>
        /// ��ȫ��
        /// </summary>
        public string FullName 
        {
            get 
            {
                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrEmpty(_namespace)) 
                {
                    sb.Append(_namespace);
                    sb.Append(".");
                }
                sb.Append(_className);
                return sb.ToString();
            }
        }

        private string _summary;

        /// <summary>
        /// ע��
        /// </summary>
        public string Summary
        {
            get { return _summary; }
        }
         private string _baseTypeName;
        /// <summary>
        /// ������
        /// </summary>
        public string BaseTypeName 
        {
            get
            {

                return _baseTypeName;
            }
        }
        private ClrType _classType;
        /// <summary>
        /// ������
        /// </summary>
        public ClrType ClassType
        {
            get { return _classType; }
        }

        List<UIModelItem> lstProperty;

        /// <summary>
        /// ���Լ���
        /// </summary>
        public List<UIModelItem> Propertys
        {
            get { return lstProperty; }
        }
        private ClrType _baseType;

        /// <summary>
        /// ���� 
        /// </summary>
        public ClrType BaseType
        {
            get { return _baseType; }
            set { _baseType = value; }
        }
        /// <summary>
        /// ��ʼ��������
        /// </summary>
        /// <param name="classShape">��ͼ��</param>
        /// <param name="project">������Ŀ</param>
        public EntityInfo(ClrType ctype, ClassDesignerInfo info) 
        {
            _classType = ctype;
            _designerInfo = info;
            FillClassInfo();
            InitPropertys();
            
        }
        Dictionary<string, List<string>> _dicGenericInfo = new Dictionary<string, List<string>>();
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public Dictionary<string, List<string>> GenericInfo
        {
            get { return _dicGenericInfo; }
        }

        /// <summary>
        /// ��ʼ������
        /// </summary>
        private void InitPropertys() 
        {
            lstProperty = new List<UIModelItem>();
            List<ClrProperty> lstClrProperty = EntityConfig.GetAllMember<ClrProperty>(_classType, true);
            foreach (ClrProperty property in lstClrProperty) 
            {
                UIModelItem item = new UIModelItem(property);
                lstProperty.Add(item);
            }
        }

        /// <summary>
        /// �������Ϣ
        /// </summary>
        private void FillClassInfo()
        {
            ClrType ctype = _classType;
            _className = ctype.Name;

            _baseType = EntityConfig.GetBaseClass(ctype, out _baseTypeName);
            CodeElementPosition ocp = null;
            _fileName = EntityConfig.GetFileName(ctype, out ocp);

            _namespace = ctype.OwnerNamespace.Name;
            _summary = ctype.DocSummary;
            if (ctype.Generic)
            {
                EntityConfig.InitGeneric(ctype, _dicGenericInfo);
            }
        }
    }
}