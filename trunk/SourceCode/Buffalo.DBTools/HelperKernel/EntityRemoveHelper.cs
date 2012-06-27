using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Buffalo.Kernel;
using EnvDTE;

namespace Buffalo.DBTools.HelperKernel
{
    public class EntityRemoveHelper
    {
        public static void RemoveEntity(EntityConfig entity) 
        {
            FileInfo info = new FileInfo(entity.FileName);

            //ҵ���
            string fileName = info.DirectoryName + "\\Business\\" + entity.ClassName + "Business.cs"; ;
            RemoveFromProject(fileName, entity.CurrentProject);

            //���ݲ�
            string dicPath = info.DirectoryName + "\\DataAccess";
            entity.InitDBConfig();
            DataAccessMappingConfig dalconfig = new DataAccessMappingConfig(entity);
            foreach (ComboBoxItem itype in Generate3Tier.DataAccessTypes)
            {
                string type = itype.Value.ToString();
                string dalPath = dicPath + "\\" + type;
                fileName = dalPath + "\\" + entity.ClassName + "DataAccess.cs";
                RemoveFromProject(fileName, entity.CurrentProject);

                dalconfig.DeleteDal(dalconfig.DataAccessNamespace + "." + type + "." + entity.ClassName + "DataAccess");
            }
            dalconfig.SaveXML();
            string idalPath = dicPath + "\\IDataAccess";
            fileName = idalPath + "\\I" + entity.ClassName + "DataAccess.cs";
            RemoveFromProject(fileName, entity.CurrentProject);
            idalPath = dicPath + "\\Bql";
            fileName = idalPath + "\\" + entity.ClassName + "DataAccess.cs";
            RemoveFromProject(fileName, entity.CurrentProject);
            
            

            //ɾ��BQLEntity
            fileName = dicPath + "\\BQLEntity\\" + entity.ClassName + ".cs";
            RemoveFromProject(fileName, entity.CurrentProject);

            //BEM.xml
            fileName = dicPath+"\\BEM\\" + entity.ClassName + ".BEM.xml";
            RemoveFromProject(fileName, entity.CurrentProject);

            //�Ƴ�ʵ��
            fileName = entity.FileName;
            RemoveFromProject(fileName, entity.CurrentProject);
            fileName = entity.FileName.Replace(".cs", ".extend.cs");
            RemoveFromProject(fileName, entity.CurrentProject);
        }

        /// <summary>
        /// ����Ŀ��ɾ���ļ�
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="project"></param>
        private static void RemoveFromProject(string fileName, Project project) 
        {
            FileInfo finfo = new FileInfo(project.FileName);
            fileName = fileName.Replace(finfo.DirectoryName, "");
            string[] strPart = fileName.Split(new char[] { '\\' });
            ProjectItems curProjects = project.ProjectItems;
            ProjectItem curItem = null;
            foreach (string part in strPart) 
            {
                if (string.IsNullOrEmpty(part)) 
                {
                    continue;
                }
                curItem = FindItem(curProjects, part);
                if (curItem == null) 
                {
                    return;
                }
                curProjects = curItem.ProjectItems;
            }
            curItem.Delete();
        }

        /// <summary>
        /// ͨ�����ֲ�����Ŀ�е���
        /// </summary>
        /// <param name="items"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static ProjectItem FindItem(ProjectItems items, string name) 
        {
            for (int i = 1; i <= items.Count; i++)
            {
                try
                {
                    ProjectItem item = items.Item(i);
                    if (name == item.Name)
                    {
                        return item;
                    }
                }
                catch { }
            }
            return null;
        }
    }
}