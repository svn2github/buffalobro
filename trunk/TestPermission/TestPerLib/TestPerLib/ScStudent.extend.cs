using System;
using System.Data;
using System.Collections.Generic;
using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
namespace TestPerLib
{
	
	/// <summary>
	///  
	/// </summary>
	public partial class ScStudent
	{
        /// <summary>
        /// �����༶��
        /// </summary>
        public string BelongClassName
        {
            get 
            {
                return BelongClass.ClassName;
            }
        }
	}
}