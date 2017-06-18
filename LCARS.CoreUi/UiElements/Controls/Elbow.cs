using LCARS.CoreUi.Enums;
using LCARS.CoreUi.UiElements.Base;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms.Design.Behavior;

namespace LCARS.CoreUi.UiElements.Controls
{
    [System.ComponentModel.DefaultEvent("Click"), Designer(typeof(ElbowDesigner))]
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

        #region " Global Variables "
        LcarsElbowStyle Style;
        int barWidth = 200;
        int barHeight = 25;
        Point ratio = new Point(1, 1);
        #endregion

        #region " Properties "
        public Point ElbowRatio
        {
            get { return ratio; }
            set
            {
                ratio = value;
                DrawAllButtons();
            }
        }

        public int ButtonWidth
        {
            get { return barWidth; }
            set
            {
                barWidth = value;
                DrawAllButtons();
            }
        }

        public int ButtonHeight
        {
            get { return barHeight; }
            set
            {
                barHeight = value;
                DrawAllButtons();
            }
        }

        public LcarsElbowStyle ElbowStyle
        {
            get { return Style; }
            set
            {
                Style = value;
                DrawAllButtons();
            }
        }
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

            g.FillRectangle(mybrush, 0, (Height) / 2, barWidth, (Height / 2) + (Height / 4));

            g.FillRectangle(mybrush, Height / 2, 0, Width - (Height / 2), barHeight);

            g.FillRectangle(mybrush, Height / 2, barHeight, barWidth - (Height / 2), Height - barHeight);

            g.FillRectangle(Brushes.Black, barWidth, barHeight, Width - barWidth, Height - barHeight);

            g.FillRectangle(mybrush, barWidth, barHeight, Height / 4, Height / 4);

            g.FillEllipse(Brushes.Black, barWidth, barHeight, Height / 2, Height / 2);

            Bitmap buffer = new Bitmap(mybitmap);
            Point[] myPoints = new Point[3];

            switch (Style)
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
                        s.Add(new SnapLine(SnapLineType.Top, p.Height - p.ButtonHeight, SnapLinePriority.Always));
                        s.Add(new SnapLine(SnapLineType.Right, p.Width - p.ButtonWidth, SnapLinePriority.Always));
                        s.Add(new SnapLine(SnapLineType.Horizontal, p.Height - p.ButtonHeight - p.Margin.Top, "Margin.Top", SnapLinePriority.Always));
                        s.Add(new SnapLine(SnapLineType.Vertical, p.Width - p.ButtonWidth + p.Margin.Right, "Margin.Right", SnapLinePriority.Always));
                        break;
                    case LcarsElbowStyle.LowerRight:
                        s.Add(new SnapLine(SnapLineType.Top, p.Height - p.ButtonHeight, SnapLinePriority.Always));
                        s.Add(new SnapLine(SnapLineType.Left, p.ButtonWidth, SnapLinePriority.Always));
                        s.Add(new SnapLine(SnapLineType.Horizontal, p.Height - p.ButtonHeight - p.Margin.Top, "Margin.Top", SnapLinePriority.Always));
                        s.Add(new SnapLine(SnapLineType.Vertical, p.ButtonWidth - p.Margin.Left, "Margin.Left", SnapLinePriority.Always));
                        break;
                    case LcarsElbowStyle.UpperLeft:
                        s.Add(new SnapLine(SnapLineType.Bottom, p.ButtonHeight, SnapLinePriority.Always));
                        s.Add(new SnapLine(SnapLineType.Right, p.ButtonWidth, SnapLinePriority.Always));
                        s.Add(new SnapLine(SnapLineType.Horizontal, p.ButtonHeight + p.Margin.Bottom, "Margin.Bottom", SnapLinePriority.Always));
                        s.Add(new SnapLine(SnapLineType.Vertical, p.ButtonWidth + p.Margin.Right, "Margin.Right", SnapLinePriority.Always));
                        break;
                    case LcarsElbowStyle.UpperRight:
                        s.Add(new SnapLine(SnapLineType.Bottom, p.ButtonHeight, SnapLinePriority.Always));
                        s.Add(new SnapLine(SnapLineType.Left, p.Width - p.ButtonWidth, SnapLinePriority.Always));
                        s.Add(new SnapLine(SnapLineType.Horizontal, p.ButtonHeight + p.Margin.Bottom, "Margin.Bottom", SnapLinePriority.Always));
                        s.Add(new SnapLine(SnapLineType.Vertical, p.Width - p.ButtonWidth - p.Margin.Right, "Margin.Left", SnapLinePriority.Always));
                        break;
                }
                return s;
            }
        }
    }
    #endregion
}
