﻿using System;
using System.Collections.Generic;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.PropertyAttributes;
namespace Buffalo.DB.DataBaseAdapter.IDbAdapters
{
    public interface IDBStructure
    {
        /// <summary>
        /// 获取所有表名
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        List<string> GetAllTableName(DBInfo info);
        /// <summary>
        /// 添加字段的语句
        /// </summary>
        /// <returns></returns>
        string GetAddParamSQL();
        /// <summary>
        /// 获取所有关系
        /// </summary>
        /// <param name="chileName">子表名，查询有这里为nulls</param>
        /// <returns></returns>
        List<TableRelationAttribute> GetRelation(DBInfo info, string childName);
    }
}