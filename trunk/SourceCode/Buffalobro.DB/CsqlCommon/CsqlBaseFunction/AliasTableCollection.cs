using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.CsqlCommon.CsqlConditionCommon;

namespace Buffalobro.DB.CsqlCommon.CsqlBaseFunction
{
    public class AliasTableCollection
    {
        internal AliasTableCollection() { }
        /// <summary>
        /// ��ȡ������
        /// </summary>
        /// <param name="aliasName">��������</param>
        /// <returns></returns>
        public AliasTabelParamCollection this[string aliasName] 
        {
            get 
            {
                return new AliasTabelParamCollection(aliasName);
            }
        }
    }

    public class AliasTabelParamCollection 
    {
        private string aliasName;
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="aliasName">����</param>
        internal AliasTabelParamCollection(string aliasName) 
        {
            this.aliasName = aliasName;
        }
        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <returns></returns>
        public AliasTabelParamHandle _
        {
            get
            {
                return new AliasTabelParamHandle(aliasName, "*");
            }
        }
        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="propertyName">������</param>
        /// <returns></returns>
        public AliasTabelParamHandle this[string propertyName] 
        {
            get 
            {
                return new AliasTabelParamHandle(aliasName, propertyName);
            }
        }
    }
}