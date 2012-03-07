using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner.PresentationModel;
using System.Collections;
using Buffalo.DBTools.DocSummary.VSConfig;
/** 
@author 289323612@qq.com
@version 创建时间：2011-12-1
显示类图注释
*/
namespace Buffalo.DBTools.DocSummary
{
    internal class MemberSummary : ShapeField
    {
        // Fields
        private Connect _FromAddin;
        private SolidBrush BackBrush = new SolidBrush(Color.White);
        private SolidBrush VarBrush = new SolidBrush(Color.Blue);
        private SolidBrush NameBrush = new SolidBrush(Color.Black);
        private SolidBrush SummaryBrush = new SolidBrush(Color.Green);
        // Methods
        public override void DoPaint(DiagramPaintEventArgs e, ShapeElement parentShape)
        {
            base.DoPaint(e, parentShape);
            Font font = this.GetFont(e.View);
            CDCompartment compartment = parentShape as CDCompartment;
            if (compartment != null)
            {
                ListField listField = null;
                foreach (ShapeField field2 in compartment.ShapeFields)
                {
                    if (field2 is ListField)
                    {
                        listField = field2 as ListField;
                        break;
                    }
                }
                if (listField != null)
                {
                    int itemCount = compartment.GetItemCount(listField);
                    for (int i = 0; i < itemCount; i++)
                    {
                        ItemDrawInfo itemDrawInfo = new ItemDrawInfo();
                        
                        compartment.GetItemDrawInfo(listField, i, itemDrawInfo);
                        if (itemDrawInfo.Disabled)
                        {
                            continue;
                        }
                        string[] strArray = itemDrawInfo.Text.Split(new char[] { ':', '(', '{', '<' });
                        Member menberByName = this.GetMenberByName(parentShape.ParentShape, strArray[0].Trim());
                        if ((menberByName == null))
                        {
                            continue;
                        }
                        string docSummary = menberByName.DocSummary;
                        string genericTypeName = "";

                        genericTypeName = menberByName.MemberTypeShortName;
                        
                        BackBrush.Color = Color.White;
                        VarBrush.Color = Color.Blue;
                        NameBrush.Color =Color.Black;
                        SummaryBrush.Color =Color.Green;
                        SelectedShapesCollection seleShapes = this._FromAddin.SelectedShapes;
                        if (seleShapes != null)
                        {
                            foreach (DiagramItem item in seleShapes)
                            {
                                if (((item.Shape == compartment) && (item.Field == listField)) && (item.SubField.SubFieldHashCode == i))
                                {
                                    this.BackBrush.Color = SystemColors.Highlight;
                                    VarBrush.Color = Color.White;
                                    NameBrush.Color = Color.White;
                                    SummaryBrush.Color = Color.White;
                                    break;
                                }
                            }
                        }

                        
                        
                        float recX = (float)itemDrawInfo.ImageMargin;//0.16435f
                        RectangleD bound = parentShape.BoundingBox;

                        float width = (float)bound.Width;
                        //float MemberStartMargin = 0.26f;
                        //float MemberLineHeight = 0.19f;
                        
                        e.Graphics.FillRectangle(this.BackBrush, VSConfigManager.CurConfig.MemberMarginX,
                            (VSConfigManager.CurConfig.MemberStartMargin +
                            VSConfigManager.CurConfig.MemberLineHeight * (float)i), width,
                            VSConfigManager.CurConfig.MemberSummaryHeight);

                        float curX=VSConfigManager.CurConfig.MemberMarginX;
                        string curStr = genericTypeName;

                        if (BackBrush.Color == Color.White)//非选中状态
                        {
                            if (menberByName.MemberType != null && menberByName.MemberType.TypeModifier == TypeModifier.Value)
                            {
                                VarBrush.Color = Color.Blue;
                            }
                            else
                            {
                                VarBrush.Color = Color.DodgerBlue;
                            }
                        }
                        e.Graphics.DrawString(curStr, font, this.VarBrush, curX,
                            (VSConfigManager.CurConfig.MemberStartMargin + VSConfigManager.CurConfig.MemberLineHeight * (float)i + 0.02f));
                        curX += e.Graphics.MeasureString(curStr, font).Width;

                        curStr = " " + menberByName.Name;
                        e.Graphics.DrawString(curStr, font, this.NameBrush, curX,
                            (VSConfigManager.CurConfig.MemberStartMargin + VSConfigManager.CurConfig.MemberLineHeight * (float)i + 0.02f));
                        curX += e.Graphics.MeasureString(curStr, font).Width;

                        curStr = "//" + docSummary;
                        e.Graphics.DrawString(curStr, font, this.SummaryBrush, curX,
                            (VSConfigManager.CurConfig.MemberStartMargin + VSConfigManager.CurConfig.MemberLineHeight * (float)i + 0.02f));
                       
                    }
                }
            }
        }

        private Font GetFont(DiagramClientView View)
        {
            if (View == null)
            {
                return new Font("宋体", 9f, FontStyle.Regular);
            }
            return new Font("宋体", View.Font.Size * View.ZoomFactor, FontStyle.Regular);
        }

        private Member GetMenberByName(ShapeElement clsShape, string Mname)
        {
            ClrType associatedType = null;
            if (clsShape is ClrClassShape)
            {
                associatedType = (clsShape as ClrClassShape).AssociatedType;
            }
            if (clsShape is ClrInterfaceShape)
            {
                associatedType = (clsShape as ClrInterfaceShape).AssociatedType;
            }
            if (associatedType != null)
            {
                foreach (Member member in (IEnumerable)associatedType.Members)
                {
                    if (member.Name == Mname)
                    {
                        return member;
                    }
                }
            }
            return null;
        }

        // Properties
        public Connect FromAddin
        {
            get
            {
                return this._FromAddin;
            }
            set
            {
                this._FromAddin = value;
            }
        }
    }


}
