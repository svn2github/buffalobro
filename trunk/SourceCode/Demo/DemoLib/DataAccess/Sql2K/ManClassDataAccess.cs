using System;
using System.Data;
using Buffalo.DB.CommBase.DataAccessBases;
using Buffalo.DB.DbCommon;
using TestAddIn;
using System.Collections.Generic;
using TestAddIn.DataAccess.IDataAccess;

namespace TestAddIn.DataAccess.Sql2K
{
    ///<summary>
    /// ���ݷ��ʲ�
    ///</summary>
    [IDalAttribute(typeof(IManClassDataAccess))]
    public class ManClassDataAccess : DataAccessModel<ManClass>,IManClassDataAccess
    {
        public ManClassDataAccess(DataBaseOperate oper): base(oper)
        {
            
        }
        public ManClassDataAccess(): base()
        {
        }
    }
}



