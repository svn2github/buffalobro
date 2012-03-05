using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner.PresentationModel;
using System.Drawing;
using System.Drawing.Drawing2D;
/** 
@author 289323612@qq.com
@version 创建时间：2011-12-1
显示类图注释
*/
namespace Buffalo.DBTools.DocSummary
{
    internal class SummaryShape : ShapeField
    {
        // Fields
        private float Overrideheight;
        private SolidBrush PerentBrush = new SolidBrush(Color.Gray);
        private float perentx = 0.03f;
        private float perenty = 0.35f;
        private SolidBrush SumerBrush = new SolidBrush(Color.Maroon);
        private float summeryx = 0.03f;
        private float summeryy = 0.2f;

        // Methods
        public override void DoPaint(DiagramPaintEventArgs e, ShapeElement parentShape)
        {
            base.DoPaint(e, parentShape);
            Font font = this.GetFont(e.View);
            string summrytxt = this.GetSummrytxt(parentShape);
            string perentSummrytxt = this.GetPerentSummrytxt(parentShape);
            bool flag = false;
            this.Overrideheight = 0.15f;
            RectangleF rect = new RectangleF(this.summeryx, this.summeryy, ((float)parentShape.BoundingBox.Width) - (this.summeryx * 2f), this.Overrideheight);
            if (!string.IsNullOrEmpty(perentSummrytxt))
            {
                flag = true;
                this.Overrideheight += this.Overrideheight;
                rect = new RectangleF(this.summeryx, this.summeryy, ((float)parentShape.BoundingBox.Width) - (this.summeryx * 2f), this.Overrideheight);
            }
            LinearGradientBrush brush = new LinearGradientBrush(rect, Color.FromArgb(0xd4, 0xdd, 0xef), Color.White, 0f);
            e.Graphics.FillRectangle(brush, rect);
            e.Graphics.DrawString(summrytxt, font, this.SumerBrush, this.summeryx + 0.05f, this.summeryy);
            if (flag)
            {
                Font font2 = this.GetFont(e.View);
                e.Graphics.DrawString(perentSummrytxt, font2, this.PerentBrush, this.perentx + 0.05f, this.perenty);
            }
        }

        private ClrClass GetClass(ShapeElement parentShape)
        {
            if (parentShape is ClrTypeShape)
            {
                return (((ClrTypeShape)parentShape).AssociatedType as ClrClass);
            }
            return null;
        }

        private Font GetFont(DiagramClientView View)
        {
            if (View == null)
            {
                return new Font("宋体", 9f, FontStyle.Bold);
            }
            return new Font(View.Font.FontFamily, View.Font.Size * View.ZoomFactor, FontStyle.Bold);
        }

        private string GetPerentSummrytxt(ShapeElement parentShape)
        {
            ClrClass class2 = this.GetClass(parentShape);
            if (class2 == null)
            {
                return "";
            }
            if (class2.Inherits == null)
            {
                return "";
            }
            if (class2.Inherits == "Object")
            {
                return "";
            }
            if (class2.Parent is ClrNamespace)
            {
                foreach (ClrType type in ((ClrNamespace)class2.Parent).ClrClasses)
                {
                    if (type.AccessibleName == class2.Inherits)
                    {
                        if (string.IsNullOrEmpty(type.DocSummary))
                        {
                            return string.Format("继承:{0}", class2.Inherits);
                        }
                        return string.Format("继承:{0}", type.DocSummary);
                    }
                }
            }
            return string.Format("继承:{0}", class2.Inherits);
        }

        private string GetSummrytxt(ShapeElement parentShape)
        {
            ClrClass class2 = this.GetClass(parentShape);
            if (class2 == null)
            {
                return "(无摘要)";
            }
            if (string.IsNullOrEmpty(class2.DocSummary))
            {
                return "(无摘要)";
            }
            return class2.DocSummary;
        }
    }


}
