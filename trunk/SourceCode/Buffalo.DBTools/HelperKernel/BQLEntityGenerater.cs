using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Buffalo.DB.PropertyAttributes;
using Buffalo.DBTools.ROMHelper;
using EnvDTE;

namespace Buffalo.DBTools.HelperKernel
{
    /// <summary>
    /// BQL实体生成
    /// </summary>
    public class BQLEntityGenerater:GrneraterBase
    {
        public BQLEntityGenerater(DBEntityInfo config, Project project)
            : base(config, project)
        {
        } 

        public BQLEntityGenerater(EntityConfig config) :base(config)
        {
        }

        /// <summary>
        /// 生成DB声明
        /// </summary>
        public void GenerateBQLEntityDB() 
        {
            FileInfo info = new FileInfo(EntityFileName);
            string dicPath = info.DirectoryName + "\\BQLEntity";
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }
            string fileName = dicPath + "\\" + DBName + ".cs";
            
            string model = Models.BQLDB;

            List<string> codes = new List<string>();
            using (StringReader reader = new StringReader(model))
            {
                string tmp = null;
                while ((tmp = reader.ReadLine()) != null)
                {
                    tmp = tmp.Replace("<%=BQLEntityNamespace%>", BQLEntityNamespace);
                    tmp = tmp.Replace("<%=DBName%>", DBName);
                    
                    codes.Add(tmp);
                }
            }
            CodeFileHelper.SaveFile(fileName, codes);
            EnvDTE.ProjectItem newit = CurrentProject.ProjectItems.AddFromFile(fileName);
            newit.Properties.Item("BuildAction").Value = 1;
        }

        /// <summary>
        /// 生成BQL实体
        /// </summary>
        public void GenerateBQLEntity()
        {
            TagManager tag=new TagManager();
            FileInfo info = new FileInfo(EntityFileName);
            string dicPath = info.DirectoryName + "\\BQLEntity";
            if (!Directory.Exists(dicPath))
            {
                Directory.CreateDirectory(dicPath);
            }

            string fileName = dicPath + "\\"+ ClassName + ".cs";
            
            string idal = Models.BQLEntity;
            List<string> codes = new List<string>();
            string baseType = null;
            if (EntityConfig.IsSystemTypeName(EntityBaseTypeName))
            {
                baseType = "BQLEntityTableHandle";
            }
            else 
            {
                baseType = DBName + "_" + EntityBaseTypeShortName;
                
            }
            
            using (StringReader reader = new StringReader(idal))
            {
                string tmp = null;
                while ((tmp = reader.ReadLine()) != null)
                {
                    if (tmp.StartsWith("<%#IF TableName%>")) 
                    {
                        tag.AddTag("TableName");
                    }
                    else if (tmp.StartsWith("<%#ENDIF%>"))
                    {
                        tag.PopTag();
                    }
                    else
                    {
                        if (tag.CurrentTag == "TableName" && string.IsNullOrEmpty(Table.TableName)) 
                        {
                            continue;
                        }
                        tmp = tmp.Replace("<%=EntityNamespace%>", EntityNamespace);
                        tmp = tmp.Replace("<%=BQLEntityNamespace%>", BQLEntityNamespace);
                        tmp = tmp.Replace("<%=Summary%>", Table.Description);
                        tmp = tmp.Replace("<%=DBName%>", DBName);

                        tmp = tmp.Replace("<%=BQLEntityBaseType%>", baseType);
                        tmp = tmp.Replace("<%=DataAccessNamespace%>", DataAccessNamespace);

                        string className = ClassName ;

                        tmp = tmp.Replace("<%=ClassName%>", className);
                        if (GenericInfo != null && GenericInfo.Count != 0) //有泛型
                        {
                            StringBuilder generic = new StringBuilder(200);
                            StringBuilder where = new StringBuilder(200);
                            GetGeneric(generic, where);
                            tmp = tmp.Replace("<%=Generic%>", generic.ToString());
                            tmp = tmp.Replace("<%=GenericWhere%>", where.ToString());
                            tmp = tmp.Replace("<%=HasGeneric%>", "<>");
                        }
                        else 
                        {
                            tmp = tmp.Replace("<%=Generic%>", "");
                            tmp = tmp.Replace("<%=GenericWhere%>", "");
                            tmp = tmp.Replace("<%=HasGeneric%>", "");
                        }
                        string entityClassName = ClassName;
                        tmp = tmp.Replace("<%=EntityClassName%>", entityClassName);
                        tmp = tmp.Replace("<%=PropertyDetail%>", GenProperty());
                        tmp = tmp.Replace("<%=RelationDetail%>", GenRelation());
                        tmp = tmp.Replace("<%=PropertyInit%>", GenInit());
                        codes.Add(tmp);
                    }
                }
            }
            CodeFileHelper.SaveFile(fileName, codes);
            EnvDTE.ProjectItem newit = CurrentProject.ProjectItems.AddFromFile(fileName);
            newit.Properties.Item("BuildAction").Value = 1;
        }
        /// <summary>
        /// 获取泛型字符串
        /// </summary>
        /// <returns></returns>
        private void GetGeneric(StringBuilder generic,StringBuilder where) 
        {
            if (GenericInfo == null || GenericInfo.Count == 0) 
            {
                return;
            }
            generic.Append("<");
            foreach (KeyValuePair<string, List<string>> kvp in GenericInfo) 
            {
                generic.Append(kvp.Key);
                generic.Append(",");
                if (kvp.Value != null && kvp.Value.Count > 0)
                {
                    
                    where.Append("\nwhere " + kvp.Key + ":");
                    foreach (string whereItem in kvp.Value)
                    {
                        if (whereItem.IndexOf("EntityBase") >= 0 || whereItem.IndexOf("ThinModelBase") >= 0) 
                        {
                            where.Append("BQLEntityParamHandle");
                        }
                        else if (whereItem.IndexOf("(") < 0)
                        {
                            where.Append(DBName + "_" + whereItem);
                            
                        }
                        else 
                        {
                            where.Append(whereItem);
                        }
                        where.Append(",");
                    }
                    if (where.Length > 0)
                    {
                        where.Remove(where.Length - 1, 1);
                    }
                }
            }
            if (generic.Length > 0) 
            {
                generic.Remove(generic.Length - 1, 1);
            }

            
            generic.Append(">");
        }
        /// <summary>
        /// 生成属性
        /// </summary>
        /// <returns></returns>
        private string GenProperty() 
        {
            StringBuilder sbProperty = new StringBuilder();
            if (Table.Params == null)
            {
                return sbProperty.ToString();
            }
            foreach (EntityParam epf in Table.Params) 
            {
                //if (!epf.IsGenerate)
                //{
                //    continue;
                //}
                sbProperty.Append("        private BQLEntityParamHandle " + epf.FieldName + " = null;\n");
                sbProperty.Append("        /// <summary>\n");
                sbProperty.Append("        /// " + epf.Description + "\n");
                sbProperty.Append("        /// </summary>\n");
                sbProperty.Append("        public BQLEntityParamHandle " + epf.PropertyName + "\n");
                sbProperty.Append("        {\n");
                sbProperty.Append("            get\n");
                sbProperty.Append("            {\n");
                sbProperty.Append("                return " + epf.FieldName + ";\n");
                sbProperty.Append("            }\n");
                sbProperty.Append("         }\n");
            }
            return sbProperty.ToString();
        }

        /// <summary>
        /// 生成映射属性
        /// </summary>
        /// <returns></returns>
        private string GenRelation()
        {
            StringBuilder sbRelation = new StringBuilder();
            if (Table.RelationItems == null) 
            {
                return sbRelation.ToString();
            }
            foreach (TableRelationAttribute er in Table.RelationItems)
            {
                //if (!er.IsGenerate)
                //{
                //    continue;
                //}
                //string targetType = er.FInfo.MemberTypeShortName;
                if (er.IsParent)
                {
                    string targetType = er.FieldTypeName;
                    sbRelation.Append("        /// <summary>\n");
                    sbRelation.Append("        /// " + er.Description + "\n");
                    sbRelation.Append("        /// </summary>\n");
                    bool isGeneric = false;
                    string type = null;
                    if (GenericInfo!=null && GenericInfo.ContainsKey(targetType))
                    {
                        type = targetType;
                        isGeneric = true;
                    }
                    else 
                    {
                        type = DBName + "_" + targetType;
                    }
                    sbRelation.Append("        public " + type + " " + er.PropertyName + "\n");
                    sbRelation.Append("        {\n");
                    sbRelation.Append("            get\n");
                    sbRelation.Append("            {\n");
                    if (!isGeneric)
                    {
                        sbRelation.Append("               return new " + DBName + "_" + targetType + "(this,\"" + er.PropertyName + "\");\n");
                    }
                    else 
                    {
                        sbRelation.Append("               Type objType = typeof(" + type + ");");

                        sbRelation.Append("               return ("+type+")Activator.CreateInstance(objType, this, \"" + er.PropertyName + "\");");
                    }
                    sbRelation.Append("            }\n");
                    sbRelation.Append("         }\n");
                }
                
            }
            return sbRelation.ToString();
        }

        /// <summary>
        /// 生成映射属性
        /// </summary>
        /// <returns></returns>
        private string GenInit()
        {
            StringBuilder sbInit = new StringBuilder();

            foreach (EntityParam epf in Table.Params)
            {
                //if (!epf.IsGenerate) 
                //{
                //    continue;
                //}
                sbInit.Append("            " + epf.FieldName + "=CreateProperty(\"" + epf.PropertyName + "\");\n");
                
            }
            return sbInit.ToString();
        }
    }
}
