using LCARS.CoreUi.Enums;
using LCARS.CoreUi.UiElements.Base;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms.Design.Behavior;

namespace LCARS.CoreUi.UiElements.Controls
{
    [DefaultEvent("Click"), Designer(typeof(ElbowDesigner))]
    public class Elbow : LcarsButtonBase
    {
        #region " Control Design Information "
        public Elbow() : base()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if ((components != null))
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private IContainer components;

        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            SuspendLayout();
            //
            //StandardButton
            //
            Name = "Elbow";
            Size = new Size(200, 100);
            ResumeLayout(false);
        }
        #endregion

        #region " Properties "
        public int VerticalBarWidth
        {
            get { return verticalBarWidth; }
            set
            {
                if (verticalBarWidth == value) return;
                verticalBarWidth = value;
                DrawAllButtons();
            }
        }
        int verticalBarWidth = 200;

        public int HorizantalBarHeight
        {
            get { return horizantalBarHeight; }
            set
            {
                if (horizantalBarHeight == value) return;
                horizantalBarHeight = value;
                DrawAllButtons();
            }
        }
        int horizantalBarHeight = 25;

        public LcarsElbowStyle ElbowStyle
        {
            get { return elbowStyle; }
            set
            {
                if (elbowStyle == value) return;
                elbowStyle = value;
                DrawAllButtons();
            }
        }
        LcarsElbowStyle elbowStyle;
        #endregion

        #region " Draw LCARS Style Elbows "
        public override Bitmap DrawButton()
        {
            Bitmap mybitmap = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(mybitmap);
            SolidBrush mybrush = null;

            //set the brush to the selected color
            mybrush = new SolidBrush(GetButtonColor());

            //Draw smooth curves (takes longer but looks oh so good!)
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //fill the background with black
            g.FillRectangle(Brushes.Black, 0, 0, mybitmap.Width, mybitmap.Height);

            //draw the first elipse the size of the elbow
            g.FillEllipse(mybrush, 0, 0, Height, Height);
            g.FillRectangle(mybrush, 0, (Height) / 2, verticalBarWidth, (Height / 2) + (Height / 4));
            g.FillRectangle(mybrush, Height / 2, 0, Width - (Height / 2), horizantalBarHeight);
            g.FillRectangle(mybrush, Height / 2, horizantalBarHeight, verticalBarWidth - (Height / 2), Height - horizantalBarHeight);
            g.FillRectangle(Brushes.Black, verticalBarWidth, horizantalBarHeight, Width - verticalBarWidth, Height - horizantalBarHeight);
            g.FillRectangle(mybrush, verticalBarWidth, horizantalBarHeight, Height / 4, Height / 4);
            g.FillEllipse(Brushes.Black, verticalBarWidth, horizantalBarHeight, Height / 2, Height / 2);

            Bitmap buffer = new Bitmap(mybitmap);
            Point[] myPoints = new Point[3];

            switch (elbowStyle)
            {
                case LcarsElbowStyle.UpperRight:
                    myPoints[0] = new Point(Width, 0);
                    myPoints[1] = new Point(0, 0);
                    myPoints[2] = new Point(Width, Height);
                    break;
                case LcarsElbowStyle.LowerRight:
                    myPoints[0] = new Point(Width, Height);
                    myPoints[1] = new Point(0, Height);
                    myPoints[2] = new Point(Width, 0);
                    break;
                case LcarsElbowStyle.LowerLeft:
                    myPoints[0] = new Point(0, Height);
                    myPoints[1] = new Point(Width, Height);
                    myPoints[2] = new Point(0, 0);
                    break;
            }

            g.DrawImage(buffer, myPoints);
            TextSize = Size;
            g.Dispose();
            return mybitmap;
        }
        #endregion
    }

    #region " Designer "
    internal class ElbowDesigner : LcarsButtonBaseDesigner
    {
        public override System.Collections.IList SnapLines
        {
            get
            {
                System.Collections.IList s = base.SnapLines;
                Elbow p = (Elbow)Control;
                if (p == null)
                {
                    return s;
                }
                switch (p.ElbowStyle)
                {
                    case LcarsElbowStyle.LowerLeft:
                        s.Add(new SnapLine(SnapLineType.Top, p.Height - p.HorizantalBarHeight, SnapLinePriority.Always));
                        s.Add(new SnapLine(SnapLineType.Right, p.Width - p.VerticalBarWidth, SnapLinePriority.Always));
                        s.Add(new SnapLine(SnapLineType.Horizontal, p.Height - p.HorizantalBarHeight - p.Margin.Top, "Margin.Top", SnapLinePriority.Always));
                        s.Add(new SnapLine(SnapLineType.Vertical, p.Width - p.VerticalBarWidth + p.Margin.Right, "Margin.Right", SnapLinePriority.Always));
                        break;
                    case LcarsElbowStyle.LowerRight:
                        s.Add(new SnapLine(SnapLineType.Top, p.Height - p.HorizantalBarHeight, SnapLinePriority.Always));
                        s.Add(new SnapLine(SnapLineType.Left, p.VerticalBarWidth, SnapLinePriority.Always));
                        s.Add(new SnapLine(SnapLineType.Horizontal, p.Height - p.HorizantalBarHeight - p.Margin.Top, "Margin.Top", SnapLinePriority.Always));
                        s.Add(new SnapLine(SnapLineType.Vertical, p.VerticalBarWidth - p.Margin.Left, "Margin.Left", SnapLinePriority.Always));
                        break;
                    case LcarsElbowStyle.UpperLeft:
                        s.Add(new SnapLine(SnapLineType.Bottom, p.HorizantalBarHeight, SnapLinePriority.Always));
                        s.Add(new SnapLine(SnapLineType.Right, p.VerticalBarWidth, SnapLinePriority.Always));
                        s.Add(new SnapLine(SnapLineType.Horizontal, p.HorizantalBarHeight + p.Margin.Bottom, "Margin.Bottom", SnapLinePriority.Always));
                        s.Add(new SnapLine(SnapLineType.Vertical, p.VerticalBarWidth + p.Margin.Right, "Margin.Right", SnapLinePriority.Always));
                        break;
                    case LcarsElbowStyle.UpperRight:
                        s.Add(new SnapLine(SnapLineType.Bottom, p.HorizantalBarHeight, SnapLinePriority.Always));
                        s.Add(new SnapLine(SnapLineType.Left, p.Width - p.VerticalBarWidth, SnapLinePriority.Always));
                        s.Add(new SnapLine(SnapLineType.Horizontal, p.HorizantalBarHeight + p.Margin.Bottom, "Margin.Bottom", SnapLinePriority.Always));
                        s.Add(new SnapLine(SnapLineType.Vertical, p.Width - p.VerticalBarWidth - p.Margin.Right, "Margin.Left", SnapLinePriority.Always));
                        break;
                }
                return s;
            }
        }
    }
    #endregion
}
