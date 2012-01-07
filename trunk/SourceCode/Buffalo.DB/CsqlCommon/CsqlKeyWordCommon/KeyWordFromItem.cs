using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.DB.CsqlCommon.CsqlConditionCommon;
using Buffalo.DB.CommBase.DataAccessBases.AliasTableMappingManagers;

namespace Buffalo.DB.CsqlCommon.CsqlKeyWordCommon
{
    public class KeyWordFromItem : CsqlQuery
    {
        protected CsqlTableHandle[] _tables;
        
        /// <summary>
        /// From关键字项
        /// </summary>
        /// <param name="tables">表集合</param>
        /// <param name="previous">上一个关键字</param>
        public KeyWordFromItem(CsqlTableHandle[] tables, CsqlQuery previous)
            : base(previous) 
        {
            this._tables = tables;
        }

        ///// <summary>
        ///// 要查询的字段
        ///// </summary>
        //internal CsqlTableHandle[] Tables 
        //{
        //    get 
        //    {
        //        return tables;
        //    }
        //}

        /// <summary>
        /// 左连接
        /// </summary>
        /// <param name="jionTable">表</param>
        /// <param name="on">条件</param>
        /// <returns></returns>
        public KeyWordJoinItem LeftJoin(CsqlTableHandle joinTable, CsqlCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "left", this);
            return item;
        }
        /// <summary>
        /// 右连接
        /// </summary>
        /// <param name="jionTable">表</param>
        /// <param name="on">条件</param>
        /// <returns></returns>
        public KeyWordJoinItem RightJoin(CsqlTableHandle joinTable, CsqlCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "right", this);
            return item;
        }
        /// <summary>
        /// 内连接
        /// </summary>
        /// <param name="jionTable">表</param>
        /// <param name="on">条件</param>
        /// <returns></returns>
        public KeyWordJoinItem InnerJoin(CsqlTableHandle joinTable, CsqlCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "inner", this);
            return item;
        }
        /// <summary>
        /// 交叉连接
        /// </summary>
        /// <param name="jionTable">表</param>
        /// <param name="on">条件</param>
        /// <returns></returns>
        public KeyWordJoinItem CrossJoin(CsqlTableHandle joinTable, CsqlCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "cross", this);
            return item;
        }
        /// <summary>
        /// 全连接
        /// </summary>
        /// <param name="jionTable">表</param>
        /// <param name="on">条件</param>
        /// <returns></returns>
        public KeyWordJoinItem FullJoin(CsqlTableHandle joinTable, CsqlCondition on)
        {
            KeyWordJoinItem item = new KeyWordJoinItem(joinTable, on, "full", this);
            return item;
        }
        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public KeyWordWhereItem Where(CsqlCondition condition) 
        {
            KeyWordWhereItem item = new KeyWordWhereItem(condition, this);
            return item;
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="paramhandles"></param>
        /// <returns></returns>
        public KeyWordOrderByItem OrderBy(params CsqlParamHandle[] paramhandles)
        {
            KeyWordOrderByItem item = new KeyWordOrderByItem(paramhandles, this);
            return item;
        }
        /// <summary>
        /// 分组
        /// </summary>
        /// <param name="paramhandles"></param>
        /// <returns></returns>
        public KeyWordGroupByItem GroupBy(params CsqlParamHandle[] paramhandles)
        {
            KeyWordGroupByItem item = new KeyWordGroupByItem(paramhandles, this);
            return item;
        }

        ///// <summary>
        ///// 查询范围
        ///// </summary>
        ///// <param name="star">开始条数(从0开始)</param>
        ///// <param name="totleRecord">显示条数</param>
        ///// <returns></returns>
        //public KeyWordLimitItem Limit(uint star, uint totleRecord) 
        //{
        //    KeyWordLimitItem item = new KeyWordLimitItem(star, totleRecord, this);
        //    return item;
        //}

        /// <summary>
        /// 加载表的别名信息
        /// </summary>
        /// <param name="info"></param>
        internal override void LoadInfo(KeyWordInfomation info) 
        {
            foreach (CsqlTableHandle tab in _tables) 
            {
                tab.FillInfo(info);
            }
        }

        /// <summary>
        /// from关键字
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        internal override void Tran(KeyWordInfomation info)
        {
            StringBuilder ret = new StringBuilder();


            if (info.AliasManager == null)
            {
                for (int i = 0; i < _tables.Length; i++)
                {
                    CsqlTableHandle table = _tables[i];

                    ret.Append(table.DisplayValue(info));
                    if (i < _tables.Length - 1)
                    {
                        ret.Append(",");
                    }
                }
                info.Condition.Tables.Append(ret.ToString());
            }
            else 
            {
                for (int i = 0; i < _tables.Length; i++)
                {
                    CsqlTableHandle table = _tables[i];

                    CsqlAliasHandle ahandle= info.AliasManager.GetPrimaryAliasHandle(table);
                    if (!Buffalo.Kernel.CommonMethods.IsNull(ahandle)) 
                    {
                        _tables[i] = ahandle;
                    }
                }


                KeyWordFromItem from=info.AliasManager.ToInnerTable(this,info);
                TableAliasNameManager manager = info.AliasManager;
                info.AliasManager = null;
                Stack<KeyWordFromItem> stkFrom = new Stack<KeyWordFromItem>();
                while (from != null) 
                {
                    stkFrom.Push(from);
                    from = from.Previous as KeyWordFromItem;
                }
                while (stkFrom.Count > 0)
                {
                    from = stkFrom.Pop();
                    from.Tran(info);
                }
                info.AliasManager = manager;
            }
            
            
        }



        
    }
}
