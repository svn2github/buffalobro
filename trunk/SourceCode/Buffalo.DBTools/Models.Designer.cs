﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.5485
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Buffalo.DBTools {
    using System;
    
    
    /// <summary>
    ///   强类型资源类，用于查找本地化字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Models {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Models() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Buffalo.DBTools.Models", typeof(Models).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   为使用此强类型资源类的所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 using System;
        ///using System.Data;
        ///using System.Configuration;
        ///using Buffalo.DB.BQLCommon;
        ///using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
        ///using Buffalo.DB.BQLCommon.BQLConditionCommon;
        ///using System.Collections.Generic;
        ///using Buffalo.DB.QueryConditions;
        ///using &lt;%=BQLEntityNamespace%&gt;;
        ///using &lt;%=EntityNamespace%&gt;;
        ///using Buffalo.DB.DbCommon;
        ///namespace &lt;%=DataAccessNamespace%&gt;.&lt;%=DataBaseType%&gt;
        ///{
        ///    ///&lt;summary&gt;
        ///    /// &lt;%=Summary%&gt;数据访问层
        ///    ///&lt;/summary&gt;
        ///    public class &lt;%=ClassName%&gt;DataAccess :BQ [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string BQLDataAccess {
            get {
                return ResourceManager.GetString("BQLDataAccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 using System;
        ///using System.Data;
        ///using System.Configuration;
        ///using Buffalo.DB.EntityInfos;
        ///using Buffalo.DB.BQLCommon;
        ///using Buffalo.DB.BQLCommon.BQLConditionCommon;
        ///using Buffalo.DB.PropertyAttributes;
        ///namespace &lt;%=BQLEntityNamespace%&gt;
        ///{
        ///    [DataBaseAttribute(&quot;&lt;%=DBName%&gt;&quot;)]
        ///    public partial class &lt;%=DBName%&gt; :BQLDataBaseHandle&lt;&lt;%=DBName%&gt;&gt; 
        ///    {
        ///    }
        ///    
        ///}
        /// 的本地化字符串。
        /// </summary>
        internal static string BQLDB {
            get {
                return ResourceManager.GetString("BQLDB", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 using System;
        ///using System.Data;
        ///using System.Configuration;
        ///using Buffalo.DB.EntityInfos;
        ///using Buffalo.DB.BQLCommon;
        ///using Buffalo.DB.BQLCommon.BQLConditionCommon;
        ///using Buffalo.DB.PropertyAttributes;
        ///namespace &lt;%=BQLEntityNamespace%&gt;
        ///{
        ///&lt;%#IF TableName%&gt;
        ///
        ///    public partial class &lt;%=DBName%&gt; 
        ///    {
        ///        private static &lt;%=BQLClassName%&gt; _&lt;%=ClassName%&gt; = new &lt;%=BQLClassName%&gt;();
        ///    
        ///        public static &lt;%=BQLClassName%&gt; &lt;%=ClassName%&gt;
        ///        {
        ///            get
        ///            {
        ///        [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string BQLEntity {
            get {
                return ResourceManager.GetString("BQLEntity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 using System;
        ///using &lt;%=EntityNamespace%&gt;;
        ///using System.Collections.Generic;
        ///using Buffalo.DB.DataBaseAdapter;
        ///using Buffalo.DB.CommBase.BusinessBases;
        ///using Buffalo.DB.CommBase.DataAccessBases;
        ///using Buffalo.DB.DbCommon;
        ///using Buffalo.DB.QueryConditions;
        ///using Buffalo.DB.CommBase;
        ///namespace &lt;%=BusinessNamespace%&gt;
        ///{
        ///    /// &lt;summary&gt;
        ///    ///  &lt;%=Summary%&gt;业务层
        ///    /// &lt;/summary&gt;
        ///    public class &lt;%=BusinessClassName%&gt;Base&lt;T&gt;: &lt;%=BaseBusinessClass%&gt;&lt;T&gt; where T:&lt;%=ClassName%&gt;,new()
        ///    {
        ///         [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string Business {
            get {
                return ResourceManager.GetString("Business", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///&lt;ClassDiagram MajorVersion=&quot;1&quot; MinorVersion=&quot;1&quot;&gt;
        ///  &lt;Font Name=&quot;微软雅黑&quot; Size=&quot;9&quot; /&gt;
        ///  
        ///&lt;/ClassDiagram&gt; 的本地化字符串。
        /// </summary>
        internal static string ClassDiagram {
            get {
                return ResourceManager.GetString("ClassDiagram", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 using System;
        ///using System.Data;
        ///using Buffalo.DB.CommBase.DataAccessBases;
        ///using Buffalo.DB.DbCommon;
        ///using &lt;%=EntityNamespace%&gt;;
        ///using System.Collections.Generic;
        ///using &lt;%=DataAccessNamespace%&gt;.IDataAccess;
        ///
        ///namespace &lt;%=DataAccessNamespace%&gt;.&lt;%=DataBaseType%&gt;
        ///{
        ///    ///&lt;summary&gt;
        ///    /// &lt;%=Summary%&gt;数据访问层
        ///    ///&lt;/summary&gt;
        ///    public class &lt;%=ClassName%&gt;DataAccess : DataAccessModel&lt;&lt;%=ClassName%&gt;&gt;,I&lt;%=ClassName%&gt;DataAccess
        ///    {
        ///        public &lt;%=ClassName%&gt;DataAccess(DataBaseOperate oper):  [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string DataAccess {
            get {
                return ResourceManager.GetString("DataAccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 &lt;#script type=&quot;linked&quot;&gt;
        ///
        ///&lt;/#script&gt;
        ///
        ///&lt;#script type=&quot;using&quot;&gt;
        ///
        ///&lt;/#script&gt;
        ///
        ///&lt;#script type=&quot;code&quot;&gt;
        ///using System;
        ///using System.Collections.Generic;
        ///using System.Text;
        ///using Buffalo.Permissions.DataViewInfo;
        ///using Buffalo.DB.BQLCommon.BQLConditionCommon;
        ///&lt;#=Entity.GetValue(&quot;Using&quot;)#&gt;
        ///namespace &lt;#=Entity.GetValue(&quot;Namespace&quot;)#&gt;
        ///{
        ///	public class &lt;#=Entity.ClassName #&gt;DataView:DataViewer
        ///	{
        ///		&lt;# foreach(Property pro in SelectedPropertys ) {#&gt;
        ///		private DataItem _&lt;#=GetFieldName(pro.PropertyName)#&gt; [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string DataView {
            get {
                return ResourceManager.GetString("DataView", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 using System;
        ///using System.Collections.Generic;
        ///using System.Text;
        ///using System.Data;
        ///using Buffalo.DB.CommBase;
        ///using Buffalo.Kernel.Defaults;
        ///using Buffalo.DB.PropertyAttributes;
        ///using Buffalo.DB.CommBase.BusinessBases;
        ///namespace &lt;%=EntityNamespace%&gt;
        ///{
        ///	/// &lt;summary&gt;
        ///    /// &lt;%=Summary%&gt;
        ///    /// &lt;/summary&gt;
        ///    public partial class &lt;%=ClassName%&gt;: &lt;%=EntityBaseType%&gt;
        ///    {
        ///&lt;%=EntityFields%&gt;
        ///
        ///&lt;%=EntityRelations%&gt;
        ///
        ///&lt;%=EntityContext%&gt;
        ///    }
        ///}
        /// 的本地化字符串。
        /// </summary>
        internal static string Entity {
            get {
                return ResourceManager.GetString("Entity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 using System;
        ///using System.Collections.Generic;
        ///using System.Text;
        ///using &lt;%=EntityNamespace%&gt;;
        ///using Buffalo.DB.CommBase.DataAccessBases;
        ///namespace &lt;%=DataAccessNamespace%&gt;.IDataAccess
        ///{
        ///	///&lt;summary&gt;
        ///    /// &lt;%=Summary%&gt;数据访问层接口
        ///    ///&lt;/summary&gt;
        ///    public interface I&lt;%=ClassName%&gt;DataAccess : IDataAccessModel&lt;&lt;%=ClassName%&gt;&gt;
        ///    {
        ///		
        ///    }
        ///}
        ///
        ///
        ///
        /// 的本地化字符串。
        /// </summary>
        internal static string IDataAccess {
            get {
                return ResourceManager.GetString("IDataAccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
        ///&lt;root&gt;
        ///
        ///    &lt;configItems&gt;
        ///      &lt;item type=&quot;check&quot;  name=&quot;CanQuery&quot; summary=&quot;查询条件&quot;&gt;&lt;/item&gt;
        ///      &lt;item type=&quot;check&quot; name=&quot;CanAddNew&quot; summary=&quot;可添加&quot;&gt;&lt;/item&gt;
        ///      &lt;item type=&quot;check&quot; name=&quot;CanEdit&quot; summary=&quot;可编辑&quot;&gt;&lt;/item&gt;
        ///      &lt;item type=&quot;combo&quot; name=&quot;SumType&quot; summary=&quot;统计类型&quot; select=&quot;无:None,总相加:Sum,总条数:Count,平均:Avg,自定义:Custom&quot;&gt;&lt;/item&gt;
        ///      &lt;item type=&quot;mtext&quot; name=&quot;CustomCount&quot; summary=&quot;自定义统计&quot;&gt;&lt;/item&gt;
        ///    &lt;/configItems&gt;
        ///  &lt;classItems&gt;
        ///    &lt;item type=&quot;text&quot; name= [字符串的其余部分被截断]&quot;; 的本地化字符串。
        /// </summary>
        internal static string UIConfigItem {
            get {
                return ResourceManager.GetString("UIConfigItem", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 using System;
        ///using System.Data;
        ///using System.Collections.Generic;
        ///using Buffalo.DB.CommBase;
        ///using Buffalo.Kernel.Defaults;
        ///using Buffalo.DB.PropertyAttributes;
        ///namespace &lt;%=EntityNamespace%&gt;
        ///{
        ///	
        ///	/// &lt;summary&gt;
        ///	///  &lt;%=Summary%&gt;
        ///	/// &lt;/summary&gt;
        ///	public partial class &lt;%=ClassFullName%&gt;
        ///	{
        ///		
        ///	}
        ///}
        /// 的本地化字符串。
        /// </summary>
        internal static string UserEntity {
            get {
                return ResourceManager.GetString("UserEntity", resourceCulture);
            }
        }
    }
}
