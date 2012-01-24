using System;
using System.Collections.Generic;
using System.Text;
using Buffalobro.DB.DataBaseAdapter.IDbAdapters;
using System.Data;

namespace Buffalobro.DB.DataBaseAdapter.MySQL5Adapter
{

    public class CommonFunction :ICommonFunction
    {
        public virtual string IsNull(string[] values)
        {
            return "IFNULL(" + values[0] + "," + values[1] + ")";
        }

        public virtual string Len(string[] values)
        {
            return "LENGTH(" + values[0] + ")";
        }

        public virtual string Distinct(string[] values)
        {
            StringBuilder sbValues = new StringBuilder();
            foreach (string value in values)
            {
                sbValues.Append(value + ",");
            }

            if (sbValues.Length > 0)
            {
                sbValues.Remove(sbValues.Length - 1, 1);
            }

            return "distinct " + sbValues.ToString() + " ";
        }


    }
}