using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using Buffalo.DB.PropertyAttributes;

namespace Buffalo.DBTools.HelperKernel
{
    /// <summary>
    /// ���ݲ�ӳ���ļ�����
    /// </summary>
    public class DataAccessMappingConfig : GrneraterBase
    {
        XmlDocument _doc = EntityMappingConfig.NewXmlDocument();
        XmlNode _rootNode;
        string _fileName = null;
        public DataAccessMappingConfig(EntityConfig entity) :base(entity)
        {
            FileInfo classFile = new FileInfo(EntityFileName);
            string dicName = classFile.DirectoryName + "\\BEM\\";
            if (!Directory.Exists(dicName))
            {
                Directory.CreateDirectory(dicName);
            }
            _fileName = dicName + DBName + ".BDM.xml";
            if (File.Exists(_fileName))
            {
                try
                {
                    _doc.Load(_fileName);
                    XmlNodeList rootNodes = _doc.GetElementsByTagName("dataaccess");
                    if (rootNodes.Count<=0) 
                    {
                        _doc = EntityMappingConfig.NewXmlDocument();
                    }else
                    {
                        _rootNode=rootNodes[0];
                    }
                }
                catch 
                {

                }
            }
            if(_rootNode==null)
            {
                XmlNode dalNode = _doc.CreateElement("dataaccess");
                _doc.AppendChild(dalNode);
                XmlAttribute att = _doc.CreateAttribute("name");
                att.InnerText = DBName;
                dalNode.Attributes.Append(att);
                _rootNode = dalNode;
            }
        }

        /// <summary>
        /// ����XML��Ϣ
        /// </summary>
        /// <param name="entity"></param>
        public void SaveXML() 
        {
            
            
            
            EntityMappingConfig.SaveXML(_fileName, _doc);
            EnvDTE.ProjectItem newit = CurrentProject.ProjectItems.AddFromFile(_fileName);
            newit.Properties.Item("BuildAction").Value = 3;
        }

        
        /// <summary>
        /// ʵ������XML����
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void AppendDal(string typeName,string interfaceName) 
        {
            if (Exists(typeName)) 
            {
                return;
            }
            XmlNode node = _doc.CreateElement("item");
            XmlAttribute att = _doc.CreateAttribute("type");
            att.InnerText = typeName;
            node.Attributes.Append(att);

            att = _doc.CreateAttribute("interface");
            att.InnerText = interfaceName;
            node.Attributes.Append(att);

            _rootNode.AppendChild(node);
        }

        /// <summary>
        /// �ж��Ƿ����
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public bool Exists(string typeName) 
        {
            XmlNodeList itemNodes = _doc.GetElementsByTagName("item");
            foreach (XmlNode node in itemNodes) 
            {
                XmlAttribute att = node.Attributes["type"];
                if (att != null) 
                {
                    if (att.InnerText == typeName) 
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        
    }
}