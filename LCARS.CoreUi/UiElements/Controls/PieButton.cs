using LCARS.CoreUi.UiElements.Base;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LCARS.CoreUi.UiElements.Controls
{
    [System.ComponentModel.DefaultEvent("Click")]
    public class PieButton : LcarsButtonBase
    {
        #region " Control Design Information "
        public PieButton() : base()
        {
            Resize += PieButton_Resize;

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

        private System.ComponentModel.IContainer components;

        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            SuspendLayout();
            //
            //FlatButtonButton
            //
            Name = "FlatButton";
            Size = new Size(200, 100);
            ResumeLayout(false);
        }
        #endregion

        #region " Enums "
        public enum PieButtonStyles
        {
            UpperLeft,
            UpperRight,
            LowerLeft,
            LowerRight
        }
        #endregion

        #region " Properties "
        public PieButtonStyles ButtonStyle
        {
            get { return buttonStyle; }
            set
            {
                buttonStyle = value;
                DrawAllButtons();
            }
        }
        PieButtonStyles buttonStyle = PieButtonStyles.UpperLeft;

        public int CircleRadius
        {
            get { return circleRadius; }
            set
            {
                circleRadius = value;
                DrawAllButtons();
            }
        }
        int circleRadius = 0;

        public Point CircleLocation
        {
            get { return circleLocation; }
            set
            {
                circleLocation = value;
                DrawAllButtons();
            }
        }
        Point circleLocation = new Point(0, 0);
        #endregion

        #region " Subs "
        private void PieButton_Resize(object sender, System.EventArgs e)
        {
            TextSize = Size;
        }
        #endregion

        #region " Draw Pie Button "
        public override Bitmap DrawButton()
        {
            Bitmap mybitmap = null;
            Graphics g = null;
            SolidBrush myBrush = new SolidBrush(GetButtonColor());

            try
            {
                mybitmap = new Bitmap(Size.Width, Size.Height);
            }
            catch
            {
                mybitmap = new Bitmap(1, 1);
                mybitmap.SetPixel(0, 0, Color.FromArgb(0, 0, 0, 0));
            }

            g = Graphics.FromImage(mybitmap);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            g.FillRectangle(Brushes.Black, 0, 0, mybitmap.Width, mybitmap.Height);

            Point[] points = new Point[3];
            GraphicsPath gPath = null;
            byte[] pointTypes = new byte[3];
            pointTypes[0] = Convert.ToByte(PathPointType.Line);
            pointTypes[1] = Convert.ToByte(PathPointType.Line);
            pointTypes[2] = Convert.ToByte(PathPointType.Line);

            gPath = new GraphicsPath();
            gPath.AddRectangle(new Rectangle(0, 0, Width, Height));

            switch (buttonStyle)
            {
                case PieButtonStyles.UpperLeft:
                    points[0] = new Point(0, 0);
                    points[1] = new Point(Width, 0);
                    points[2] = new Point(Width, Height);
                    gPath = new GraphicsPath(points, pointTypes);
                    g.FillPath(myBrush, gPath);

                    points[0] = new Point(0, 0);
                    points[1] = new Point(0, Height);
                    points[2] = new Point(Width, Height);
                    break;
                case PieButtonStyles.LowerLeft:
                    points[0] = new Point(0, 0);
                    points[1] = new Point(0, Height);
                    points[2] = new Point(Width, Height);
                    gPath = new GraphicsPath(points, pointTypes);
                    g.FillPath(myBrush, gPath);

                    points[0] = new Point(0, 0);
                    points[1] = new Point(0, Height);
                    points[2] = new Point(Width, Height);
                    break;
            }

            g.FillEllipse(Brushes.Black, new Rectangle(circleLocation.X - circleRadius, circleLocation.Y - circleRadius, circleRadius * 2, circleRadius * 2));
            gPath.AddEllipse(new Rectangle(circleLocation.X - circleRadius, circleLocation.Y - circleRadius, circleRadius * 2, circleRadius * 2));
            gPath.AddLines(points);
            g.Dispose();

            Region = new Region(gPath);
            return mybitmap;
        }
        #endregion
    }
}
