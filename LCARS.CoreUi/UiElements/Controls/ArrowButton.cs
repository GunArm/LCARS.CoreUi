using LCARS.CoreUi.Enums;
using LCARS.CoreUi.UiElements.Base;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LCARS.CoreUi.UiElements.Controls
{
    [System.ComponentModel.DefaultEvent("Click")]
    public class ArrowButton : LcarsButtonBase
    {
        #region " Control Design Information "
        public ArrowButton() : base()
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

        private System.ComponentModel.IContainer components;

        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            SuspendLayout();
            //
            //StandardButton
            //
            Name = "ArrowButton";
            Size = new Size(50, 50);
            Text = "";
            ResumeLayout(false);
        }
        #endregion

        #region " Global Variables "
        LcarsArrowDirection ArrowDir = LcarsArrowDirection.Up;
        #endregion

        #region " Properties "
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override string Text
        {
            get { return ""; }
            set { base.Text = ""; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override ContentAlignment ButtonTextAlign
        {
            get { return base.ButtonTextAlign; }
            set { base.ButtonTextAlign = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override int TextHeight
        {
            get { return base.TextHeight; }
            set { base.TextHeight = value; }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool AutoEllipsis { get; set; }

        public LcarsArrowDirection ArrowDirection
        {
            get { return ArrowDir; }
            set
            {
                ArrowDir = value;
                DrawAllButtons();
            }
        }
        #endregion

        #region " Draw Arrow Button "
        public override Bitmap DrawButton()
        {
            Bitmap mybitmap = null;
            Graphics g = null;
            SolidBrush myBrush = new SolidBrush(GetButtonColor());
            Point[] myPoints = new Point[3];

            mybitmap = new Bitmap(Width, Height);
            g = Graphics.FromImage(mybitmap);

            g.FillRectangle(myBrush, 0, 0, mybitmap.Width, mybitmap.Height);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            switch (ArrowDir)
            {
                case LcarsArrowDirection.Up:
                    myPoints[0] = new Point(Width / 2, Height / 5);
                    myPoints[1] = new Point(Width / 5, Height - (Height / 5));
                    myPoints[2] = new Point(Width - (Width / 5), Height - (Height / 5));
                    break;
                case LcarsArrowDirection.Down:
                    myPoints[0] = new Point(Width / 5, Height / 5);
                    myPoints[1] = new Point(Width - (Width / 5), Height / 5);
                    myPoints[2] = new Point(Width / 2, Height - (Height / 5));
                    break;
                case LcarsArrowDirection.Left:
                    myPoints[0] = new Point(Width / 5, Height / 2);
                    myPoints[1] = new Point(Width - (Width / 5), Height / 5);
                    myPoints[2] = new Point(Width - (Width / 5), Height - (Height / 5));
                    break;
                case LcarsArrowDirection.Right:
                    myPoints[0] = new Point(Width - (Width / 5), Height / 2);
                    myPoints[1] = new Point(Width / 5, Height / 5);
                    myPoints[2] = new Point(Width / 5, Height - (Height / 5));
                    break;
            }

            g.FillPolygon(Brushes.Black, myPoints);

            return mybitmap;
        }
        #endregion
    }
}
