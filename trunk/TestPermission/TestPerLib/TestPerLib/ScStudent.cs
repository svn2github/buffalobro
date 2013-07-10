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
    public partial class ScStudent:ScBase
    {
        /// <summary>
        /// 学生名
        /// </summary>
        private string _name;

        /// <summary>
        /// 年龄
        /// </summary>
        private int _age;
        /// <summary>
        /// 学生名
        /// </summary>
        public virtual string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name=value;
            }
        }
        /// <summary>
        /// 年龄
        /// </summary>
        public virtual int Age
        {
            get
            {
                return _age;
            }
            set
            {
                _age=value;
            }
        }
        /// <summary>
        /// 所属班级ID
        /// </summary>
        private int _classId;

        /// <summary>
        /// 所属班级
        /// </summary>
        private  ScClass _belongClass;
        /// <summary>
        /// 所属班级ID
        /// </summary>
        public virtual int ClassId
        {
            get
            {
                return _classId;
            }
            set
            {
                _classId=value;
            }
        }



        /// <summary>
        /// 所属班级
        /// </summary>
        public virtual ScClass BelongClass
        {
            get
            {

                return _belongClass;
            }
            set
            {
                _belongClass = value;
            }
        }

    }
}
