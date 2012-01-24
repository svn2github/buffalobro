using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.Kernel;
using Buffalo.DB.BQLCommon.BQLBaseFunction;
using System.Data;
using Buffalo.DB.DataBaseAdapter.IDbAdapters;
using System.Collections;
using Buffalo.DB.BQLCommon;

namespace Buffalo.DB.CommBase.DataAccessBases.AliasTableMappingManagers
{
    public class AliasTableMapping
    {
        

        private BQLAliasHandle _table;

        private Dictionary<string,BQLAliasParamHandle> _dicParams;

        private Dictionary<string, AliasTableMapping> _dicChildTables = new Dictionary<string, AliasTableMapping>();

        TableAliasNameManager _belongManager;//�����Ĺ�����

        EntityMappingInfo _mappingInfo;//�����Ĺ���

        private EntityInfoHandle _entityInfo;

        private Dictionary<string, EntityPropertyInfo> _dicPropertyInfo = new Dictionary<string, EntityPropertyInfo>();

        private List<AliasReaderMapping> _lstReaderMapping =null;

        private AliasReaderMapping _primaryMapping = null;

        private Dictionary<string, EntityBase> _dicInstance = new Dictionary<string, EntityBase>();//�Ѿ�ʵ������ʵ��

        private IList _baseList;

        /// <summary>
        /// ����ӳ��
        /// </summary>
        /// <param name="table"></param>
        /// <param name="aliasName"></param>
        public AliasTableMapping(BQLEntityTableHandle table, TableAliasNameManager belongManager, EntityMappingInfo mappingInfo) 
        {
            _belongManager = belongManager;
            _entityInfo = table.GetEntityInfo();
            _table = new BQLAliasHandle(table, _belongManager.NextTableAliasName());
            _mappingInfo = mappingInfo;
            InitParam(table);
        }

        /// <summary>
        /// ��ʼ����Reader��ӳ����Ϣ
        /// </summary>
        /// <param name="reader"></param>
        public void InitReaderMapping(IDataReader reader) 
        {
            _lstReaderMapping = new List<AliasReaderMapping>(_dicPropertyInfo.Count);
            int fCount=reader.FieldCount;
            EntityPropertyInfo info=null;
            for (int i = 0; i < fCount; i++) 
            {
                string colName = reader.GetName(i);

                if (_dicPropertyInfo.TryGetValue(colName, out info)) 
                {
                    AliasReaderMapping aliasMapping = new AliasReaderMapping(i, info);
                    _lstReaderMapping.Add(aliasMapping);
                    if (info.IsPrimaryKey) 
                    {
                        _primaryMapping = aliasMapping;
                    }
                }
            }

            foreach (KeyValuePair<string, AliasTableMapping> keyPair in _dicChildTables)
            {
                keyPair.Value.InitReaderMapping(reader);
            }

            _baseList = new ArrayList();
        }

        /// <summary>
        /// ��ȡ��Ϣ
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public EntityBase LoadFromReader(IDataReader reader, out bool hasValue) 
        {
            
            IDBAdapter dbAdapter = _entityInfo.DBInfo.CurrentDbAdapter;

            EntityBase objRet = null;
            string pk = null;
            hasValue = true;
            if (_primaryMapping != null)//����оͷ���null
            {


                if (reader.IsDBNull(_primaryMapping.ReaderIndex)) 
                {
                    return null;
                }
                object pkValue = reader[_primaryMapping.ReaderIndex];
                pk = pkValue.ToString();
                _dicInstance.TryGetValue(pk, out objRet);
            }

            
            if (objRet == null)
            {
                objRet = Activator.CreateInstance(_entityInfo.EntityType) as EntityBase;
                
                foreach (AliasReaderMapping readMapping in _lstReaderMapping)
                {
                    int index = readMapping.ReaderIndex;
                    EntityPropertyInfo info = readMapping.PropertyInfo;
                    if (!reader.IsDBNull(index) && info != null)
                    {

                        dbAdapter.SetObjectValueFromReader(reader, index, objRet, info);
                    }
                }
                if (!string.IsNullOrEmpty(pk))
                {
                    _dicInstance[pk.ToString()] = objRet;
                }
                objRet._search_baselist_ = _baseList;
                _baseList.Add(objRet);
                hasValue = false;
            }
            

            foreach (KeyValuePair<string, AliasTableMapping> keyPair in _dicChildTables)
            {
                AliasTableMapping childMapping = keyPair.Value;
                bool hValue = false;
                object child = childMapping.LoadFromReader(reader, out hValue);
                if (child != null)
                {
                    if (childMapping.MappingInfo.IsParent)//��丸��
                    {
                        childMapping.MappingInfo.SetValue(objRet, child);
                    }
                    else //�������
                    {
                        IList lst = childMapping.MappingInfo.GetValue(objRet) as IList;
                        if (lst == null)
                        {
                            lst = Activator.CreateInstance(childMapping.MappingInfo.FieldType) as IList;
                            childMapping.MappingInfo.SetValue(objRet, lst);
                        }
                        
                        lst.Add(child);
                    }
                }
            }
            return objRet;
        }


        

        /// <summary>
        /// ʵ����Ϣ
        /// </summary>
        /// <returns></returns>
        public EntityInfoHandle EntityInfo 
        {
            get 
            {
                return _entityInfo;
            }
        }

        /// <summary>
        /// �����ֶ�
        /// </summary>
        public Dictionary<string, AliasTableMapping> ChildTables
        {
            get 
            {

                return _dicChildTables;
            }
        }
        /// <summary>
        /// ӳ����Ϣ
        /// </summary>
        public EntityMappingInfo MappingInfo 
        {
            get 
            {
                return _mappingInfo;
            }
        }
        /// <summary>
        /// ��ȡ�ֶα�����Ϣ
        /// </summary>
        /// <param name="propertyName">������������</param>
        /// <returns></returns>
        public List<BQLParamHandle> GetParamInfo(string propertyName) 
        {
            List<BQLParamHandle> lstRet = new List<BQLParamHandle>();

            if (propertyName != "*")
            {
                BQLAliasParamHandle handle = null;
                if (_dicParams.TryGetValue(propertyName, out handle))
                {
                    lstRet.Add(handle);
                }
            }
            else 
            {
                foreach (KeyValuePair<string, BQLAliasParamHandle> keyPair in _dicParams) 
                {
                    lstRet.Add(keyPair.Value);
                }
            }
            return lstRet;
        }

        /// <summary>
        /// ����Ϣ
        /// </summary>
        public BQLAliasHandle TableInfo
        {
            get 
            {
                return _table;
            }
        }

        /// <summary>
        /// �����ӱ�
        /// </summary>
        /// <param name="table">�ӱ�</param>
        public AliasTableMapping AddChildTable(BQLEntityTableHandle table, List<KeyWordJoinItem> lstJoin) 
        {
            AliasTableMapping retTable = null;
            Stack<BQLEntityTableHandle> stkTables = new Stack<BQLEntityTableHandle>();
            BQLEntityTableHandle curTable=table;
            do
            {
                stkTables.Push(curTable);
                curTable = curTable.GetParentTable();
            } while (!CommonMethods.IsNull(curTable));

            AliasTableMapping lastTable = null;//��һ����
            while (stkTables.Count > 0) 
            {
                BQLEntityTableHandle cTable=stkTables.Pop();

                string pName = cTable.GetPropertyName();
                if (string.IsNullOrEmpty(pName))
                {
                    if (cTable.GetEntityInfo().EntityType == cTable.GetEntityInfo().EntityType)
                    {
                        lastTable = this;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (!lastTable._dicChildTables.ContainsKey(pName))
                    {
                        
                        EntityMappingInfo mapInfo = _entityInfo.MappingInfo[pName];
                        if (mapInfo != null)
                        {
                            retTable = new AliasTableMapping(cTable, _belongManager, mapInfo);
                            lastTable._dicChildTables[pName] = retTable;
                        }
                        else 
                        {
                            throw new MissingMemberException("ʵ��:" + _entityInfo.EntityType.FullName + "���Ҳ�������:" + pName + "");
                        }
                    }
                }
            }
            return retTable;
        }

       
        /// <summary>
        /// ��ȡ�����ֶ�
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public BQLParamHandle GetAliasParam(string propertyName) 
        {
            BQLAliasParamHandle prm = null;
            BQLParamHandle ret = null;
            if (_dicParams.TryGetValue(propertyName, out prm))
            {
                ret = BQL.ToParam(prm.AliasName);
                ret.ValueDbType = prm.ValueDbType;
            }
            return ret;
        }

        /// <summary>
        /// ��ʼ���ֶ�
        /// </summary>
        /// <param name="table"></param>
        /// <param name="paramIndex"></param>
        private void InitParam(BQLEntityTableHandle table) 
        {
            _dicParams = new Dictionary<string, BQLAliasParamHandle>();

            foreach (EntityPropertyInfo info in table.GetEntityInfo().PropertyInfo) 
            {
                string prmAliasName=_belongManager.NextParamAliasName(_table.GetAliasName());
                BQLAliasParamHandle prm = BQL.Tables[_table.GetAliasName()][info.ParamName].As(prmAliasName);

                
                _dicPropertyInfo[prmAliasName] = info;
                prm.ValueDbType = info.SqlType;
                _dicParams[info.PropertyName]=prm;
            }
        }
        

    }
}