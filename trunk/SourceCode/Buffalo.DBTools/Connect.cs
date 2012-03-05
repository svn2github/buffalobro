using System;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner.PresentationModel;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using System.Runtime.InteropServices;
using Buffalo.DB.DataBaseAdapter;
using Buffalo.DBTools.HelperKernel;
using Buffalo.DBTools.DocSummary;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner;
using System.Collections;

namespace Buffalo.DBTools
{
	/// <summary>用于实现外接程序的对象。</summary>
	/// <seealso class='IDTExtensibility2' />
	public class Connect : IDTExtensibility2, IDTCommandTarget
	{
		/// <summary>实现外接程序对象的构造函数。请将您的初始化代码置于此方法内。</summary>
		public Connect()
		{
		}

        private DTE2 _applicationObject;
        private AddIn _addInInstance;
        private IServiceProvider m_serviceProvider;
        Commands2 _commands = null;
        object[] _contextGUIDS = null;
        private Command AddToCommand(string name,string buttonText,string tooltip,bool msoButton,object bitmap) 
        {

            return _commands.AddNamedCommand2(_addInInstance, name, buttonText, tooltip, msoButton, bitmap, ref _contextGUIDS, (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled, (int)vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton);
        }

        /// <summary>
        /// 是否此命令
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        private bool IsCommand(string commandName,string modelName) 
        {
            string cmd = this.GetType().FullName + "." + modelName;
            return cmd.Equals(commandName);
        }


		/// <summary>实现 IDTExtensibility2 接口的 OnConnection 方法。接收正在加载外接程序的通知。</summary>
		/// <param term='application'>宿主应用程序的根对象。</param>
		/// <param term='connectMode'>描述外接程序的加载方式。</param>
		/// <param term='addInInst'>表示此外接程序的对象。</param>
		/// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            _applicationObject = (DTE2)application;
            _addInInstance = (AddIn)addInInst;
            m_serviceProvider = new Microsoft.VisualStudio.Shell.ServiceProvider(_applicationObject as Microsoft.VisualStudio.OLE.Interop.IServiceProvider);
            if (connectMode == ext_ConnectMode.ext_cm_UISetup)
            {
               _contextGUIDS = new object[] { };
                _commands = (Commands2)_applicationObject.Commands;

                Microsoft.VisualStudio.CommandBars.CommandBar calssComm = ((Microsoft.VisualStudio.CommandBars.CommandBars)_applicationObject.CommandBars)[CommandBarId.ClassDesignerContextMenu];
                Microsoft.VisualStudio.CommandBars.CommandBar designerComm = ((Microsoft.VisualStudio.CommandBars.CommandBars)_applicationObject.CommandBars)[CommandBarId.ClassDiagramContextMenu];

                try
                {
                    //将一个命令添加到 Commands 集合:
                    Command command = AddToCommand("BuffaloEntityConfig", "配置Buffalo实体", "配置Buffalo实体", true, 50);
                    Command commandCreater = AddToCommand("BuffaloDBCreater", "Buffalo实体到表", "通过Buffalo实体生成到数据库的表", true, 50);
                    Command commandROM = AddToCommand("BuffaloDBToEntity", "表到Buffalo实体", "通过数据层的表生成Buffalo实体", true, 29);
                    Command commandSummary = AddToCommand("ShowHideSummery", "显示/隐藏注释", "显示/隐藏注释", true, 29);
                    Command commandDBAll = AddToCommand("BuffaloDBCreateAll", "生成数据库", "生成数据库", true, 29);
                    //将对应于该命令的控件添加到类菜单
                    if ((command != null) && (calssComm != null))
                    {
                        command.AddControl(calssComm, 1);
                        commandCreater.AddControl(calssComm, 2);
                    }
                    //将对应于该命令的控件添加到总类图菜单
                    if ((command != null) && (designerComm != null))
                    {
                        commandROM.AddControl(designerComm, 1);
                        commandSummary.AddControl(designerComm, 2);
                        commandDBAll.AddControl(designerComm, 3);
                    }
                }
                catch (System.ArgumentException)
                {
                    //如果出现此异常，原因很可能是由于具有该名称的命令
                    //  已存在。如果确实如此，则无需重新创建此命令，并且
                    //  可以放心忽略此异常。
                }
            }
        }

       

		/// <summary>实现 IDTExtensibility2 接口的 OnDisconnection 方法。接收正在卸载外接程序的通知。</summary>
		/// <param term='disconnectMode'>描述外接程序的卸载方式。</param>
		/// <param term='custom'>特定于宿主应用程序的参数数组。</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
		{
            
		}

		/// <summary>实现 IDTExtensibility2 接口的 OnAddInsUpdate 方法。当外接程序集合已发生更改时接收通知。</summary>
		/// <param term='custom'>特定于宿主应用程序的参数数组。</param>
		/// <seealso class='IDTExtensibility2' />		
		public void OnAddInsUpdate(ref Array custom)
		{
		}

		/// <summary>实现 IDTExtensibility2 接口的 OnStartupComplete 方法。接收宿主应用程序已完成加载的通知。</summary>
		/// <param term='custom'>特定于宿主应用程序的参数数组。</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnStartupComplete(ref Array custom)
		{
		}

		/// <summary>实现 IDTExtensibility2 接口的 OnBeginShutdown 方法。接收正在卸载宿主应用程序的通知。</summary>
		/// <param term='custom'>特定于宿主应用程序的参数数组。</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnBeginShutdown(ref Array custom)
		{
		}
		
		/// <summary>实现 IDTCommandTarget 接口的 QueryStatus 方法。此方法在更新该命令的可用性时调用</summary>
		/// <param term='commandName'>要确定其状态的命令的名称。</param>
		/// <param term='neededText'>该命令所需的文本。</param>
		/// <param term='status'>该命令在用户界面中的状态。</param>
		/// <param term='commandText'>neededText 参数所要求的文本。</param>
		/// <seealso class='Exec' />
        public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
        {
            if (neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
            {

                if (commandName == "BuffaloEntityConfig.Connect" ||
                    commandName == "BuffaloDBCreater.Connect")
                {
                    SelectedShapesCollection selectedShapes = SelectedShapes;
                    if (selectedShapes == null)
                    {
                        status = (vsCommandStatus)vsCommandStatus.vsCommandStatusUnsupported |
                            vsCommandStatus.vsCommandStatusInvisible;
                        return;
                    }
                    bool findClass = false;
                    for (int i = 0; i < selectedShapes.Count; i++)
                    {
                        if (!(selectedShapes.TopLevelItems[i].Shape is ClrTypeShape))
                        {
                            continue;
                        }
                        
                        ClrTypeShape sp = selectedShapes.TopLevelItems[i].Shape as ClrTypeShape;
                        if (!(sp.AssociatedType is ClrClass))
                        {
                            continue;
                        }
                        status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported |
                            vsCommandStatus.vsCommandStatusEnabled;
                        findClass = true;
                        break;
                    }
                    if (findClass == false)
                    {
                        status = (vsCommandStatus)vsCommandStatus.vsCommandStatusUnsupported | 
                            vsCommandStatus.vsCommandStatusInvisible;
                        return;
                    }
                }
                
                status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported | 
                    vsCommandStatus.vsCommandStatusEnabled;

            }
        }

        /// <summary>
        /// 当前项目
        /// </summary>
        public Project CurrentProject
        {
            get
            {
                if (_applicationObject.SelectedItems.Count == 0) return null;
                EnvDTE.SelectedItem item = _applicationObject.SelectedItems.Item(1);
                if (item.Project != null)
                {
                    return item.Project;
                }
                else
                {
                    return item.ProjectItem.ProjectItems.ContainingProject;
                }
            }
        }
        /// <summary>
        /// 选中的图形
        /// </summary>
        public SelectedShapesCollection SelectedShapes
        {
            get
            {
                ClassDesignerDocView docView=SelectDocView;
                if (docView == null)
                {
                    return null;
                }
                
                return docView.CurrentDesigner.Selection;
            }
        }



        /// <summary>
        /// 选中的图
        /// </summary>
        public Diagram SelectedDiagram
        {
            get
            {
                ClassDesignerDocView docView=SelectDocView;
                if (docView == null)
                {
                    return null;
                }
                return docView.CurrentDiagram;
            }
        }

        /// <summary>
        /// 选中的文档视图
        /// </summary>
        private ClassDesignerDocView SelectDocView
        {
            get
            {
                Microsoft.VisualStudio.Shell.Interop.ISelectionContainer selectionContainer;
                Microsoft.VisualStudio.Shell.Interop.IVsMonitorSelection monitorService = m_serviceProvider.GetService(typeof(Microsoft.VisualStudio.Shell.Interop.IVsMonitorSelection)) as Microsoft.VisualStudio.Shell.Interop.IVsMonitorSelection;
                if (monitorService != null)
                {
                    IntPtr ppHier, ppSC;
                    uint pitemid;
                    Microsoft.VisualStudio.Shell.Interop.IVsMultiItemSelect ppMIS;
                    Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(monitorService.GetCurrentSelection(out ppHier, out pitemid, out ppMIS, out ppSC));
                    if (ppSC != IntPtr.Zero)
                    {
                        selectionContainer = Marshal.GetObjectForIUnknown(ppSC) as Microsoft.VisualStudio.Shell.Interop.ISelectionContainer;
                        //ISelectionService SelectionContainer = selectionContainer as ISelectionService;
                        return selectionContainer as Microsoft.VisualStudio.EnterpriseTools.ClassDesigner.ClassDesignerDocView;
                    }
                }
                return null;
            }
        }

		/// <summary>实现 IDTCommandTarget 接口的 Exec 方法。此方法在调用该命令时调用。</summary>
		/// <param term='commandName'>要执行的命令的名称。</param>
		/// <param term='executeOption'>描述该命令应如何运行。</param>
		/// <param term='varIn'>从调用方传递到命令处理程序的参数。</param>
		/// <param term='varOut'>从命令处理程序传递到调用方的参数。</param>
		/// <param term='handled'>通知调用方此命令是否已被处理。</param>
		/// <seealso class='Exec' />
		public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
		{
			handled = false;
			if(executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
			{
                if (IsCommand(commandName, "BuffaloEntityConfig"))
				{
                    SelectedShapesCollection selectedShapes = SelectedShapes;
                    if (selectedShapes == null) return;
                    for (int i = 0; i < selectedShapes.Count; i++)
                    {
                        if (!(selectedShapes.TopLevelItems[i].Shape is ClrTypeShape)) continue;
                        ClrTypeShape sp = selectedShapes.TopLevelItems[i].Shape as ClrTypeShape;
                        if (!(sp.AssociatedType is ClrClass))
                        {
                            continue;
                        }
                        using (FrmClassDesigner st = new FrmClassDesigner())
                        {
                            Diagram selDiagram=SelectedDiagram;
                            st.SelectedClass = sp;
                            st.SelectDocView = SelectDocView;
                            st.CurrentProject = CurrentProject;
                            st.SelectedDiagram = selDiagram;
                            st.ShowDialog();
                        }
                    }
					handled = true;
					return;
				}
                else if (IsCommand(commandName, "BuffaloDBCreater")) 
                {
                    SelectedShapesCollection selectedShapes = SelectedShapes;
                    if (selectedShapes == null) return;
                    List<ClrClass> lstClass = new List<ClrClass>();
                    for (int i = 0; i < selectedShapes.Count; i++)
                    {
                        if (!(selectedShapes.TopLevelItems[i].Shape is ClrTypeShape))
                        {
                            continue;
                        }
                        ClrTypeShape sp = selectedShapes.TopLevelItems[i].Shape as ClrTypeShape;
                        ClrClass classType = sp.AssociatedType as ClrClass;
                        if (classType==null)
                        {
                            continue;
                        }
                        lstClass.Add(classType);
                    }
                    using (FrmDBCreate st = new FrmDBCreate())
                    {
                        Diagram selDiagram = SelectedDiagram;
                        st.SelectedClass = lstClass;
                        st.SelectDocView = SelectDocView;
                        st.CurrentProject = CurrentProject;
                        st.SelectedDiagram = selDiagram;
                        st.ShowDialog();
                    }
                    handled = true;
                    return;
                }

                else if (IsCommand(commandName, "BuffaloDBToEntity"))
                {
                    Diagram dia = SelectedDiagram;
                    if (!(dia is ShapeElement))
                    {
                        return;
                    }
                    using (FrmAllTables frmTables = new FrmAllTables()) 
                    {
                        frmTables.SelectedDiagram = dia;
                        frmTables.SelectDocView = SelectDocView;
                        frmTables.CurrentProject = CurrentProject;
                        frmTables.ShowDialog();
                    }

                }

                else if (IsCommand(commandName, "ShowHideSummery"))
                {
                    Diagram dia = this.SelectedDiagram;
                    if (dia != null)
                    {
                        ShapeSummaryDisplayer.ShowOrHideSummary(dia,this);
                        this.SelectDocView.CurrentDesigner.ScrollDown();
                        this.SelectDocView.CurrentDesigner.ScrollUp();
                        handled = true;
                    }

                }
                else if (IsCommand(commandName, "BuffaloDBCreateAll"))
                {
                    ShapeElementMoveableCollection nestedChildShapes = SelectedDiagram.NestedChildShapes;
                    IEnumerable shapes = nestedChildShapes as IEnumerable;
                    if (shapes == null) 
                    {
                        return;
                    }
                    List<ClrClass> lstClass = new List<ClrClass>();
                    foreach (ShapeElement element in shapes)
                    {
                        ClrClass classType = SummaryShape.GetClass(element);
                        if (classType != null)
                        {
                            lstClass.Add(classType);
                        }

                    }
                    using (FrmDBCreate st = new FrmDBCreate())
                    {
                        Diagram selDiagram = SelectedDiagram;
                        st.SelectedClass = lstClass;
                        st.SelectDocView = SelectDocView;
                        st.CurrentProject = CurrentProject;
                        st.SelectedDiagram = selDiagram;
                        st.ShowDialog();
                    }
                    handled = true;
                    return;

                }
			}
		}

	}
}