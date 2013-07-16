using System;
using System.Collections.Generic;
using System.Text;

using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
using System.Data;
using Buffalo.DB.CommBase.BusinessBases;
namespace TestPerLib
{
    public partial class ScClass:ScBase
    {
        private string _className;
        /// <summary>
        /// 
        /// </summary>
        public virtual string ClassName
        {
            get
            {
                return _className;
            }
            set
            {
                _className=value;
            }
        }

        public override string ToString()
        {
            return ClassName;
        }

        protected List<ScStudent> _lstStudent;
        /// <summary>
        /// 
        /// </summary>
        public virtual List<ScStudent> LstStudent
        {
            get{ return _lstStudent; }
            set{ _lstStudent = value; }
        }
    }
}
