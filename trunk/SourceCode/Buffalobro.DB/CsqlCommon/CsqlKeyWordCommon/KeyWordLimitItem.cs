using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;
using Buffalobro.DB.QueryConditions;

namespace Buffalobro.DB.CsqlCommon.CsqlKeyWordCommon
{
    /// <summary>
    /// 显示记录的范围
    /// </summary>
    public class KeyWordLimitItem : CsqlQuery
    {
        uint star = 0;
        uint totleRecords = 0;
        /// <summary>
        /// 显示记录的范围
        /// </summary>
        /// <param name="star">开始记录</param>
        /// <param name="totleRecords">要显示多少条记录</param>
        /// <param name="previous">上一个语句</param>
        public KeyWordLimitItem(uint star, uint totleRecords, CsqlQuery previous)
            : base(previous) 
        {
            this.star = star;
            this.totleRecords = totleRecords;
        }

        internal override void LoadInfo(KeyWordInfomation info)
        {
            //info.IsPage = true;
        }

        internal override void Tran(KeyWordInfomation info)
        {
            PageContent objPage = new PageContent();
            objPage.StarIndex = star;
            info.Infos.PagerCount++;
            objPage.PagerIndex = info.Infos.PagerCount;
            
            objPage.PageSize = totleRecords;
            objPage.IsFillTotleRecords = false;
            info.Condition.PageContent = objPage;
        }
    }
}
