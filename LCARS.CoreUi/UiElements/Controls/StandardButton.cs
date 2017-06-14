using LCARS.CoreUi.Enums;
using LCARS.CoreUi.UiElements.Base;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LCARS.CoreUi.UiElements.Controls
{
    [System.ComponentModel.DefaultEvent("Click")]
    public class StandardButton : LcarsButtonBase
    {
        #region " Control Design Information "
        public StandardButton() : base()
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
            Name = "StandardButton";
            Size = new Size(200, 100);
            ResumeLayout(false);

        }
        #endregion

        #region " Global Varibles "
        LcarsButtonStyles myButtonType = LcarsButtonStyles.Pill;
        #endregion

        #region " Properties "
        public LcarsButtonStyles ButtonStyle
        {
            get { return myButtonType; }
            set
            {
                myButtonType = value;
                DrawAllButtons();
            }
        }
        #endregion

        #region " Structures "
        public enum LcarsButtonStyles
        {
            Pill = 0,
            RoundedSquare = 1,
            RoundedSquareSlant = 2,
            RoundedSquareBackSlant = 3
        }
        #endregion

        #region " Draw Standard Button "
        public override Bitmap DrawButton()
        {
            Bitmap mybitmap = null;
            Graphics g = null;
            SolidBrush myBrush = new SolidBrush(ColorManager.GetColor(ColorFunction));
            int halfHeight = 0;
            int quarterHeight = 0;
            int quarterWidth = 0;
            if (AlertState == LcarsAlert.Red)
            {
                myBrush = new SolidBrush(Color.Red);
            }
            else if (AlertState == LcarsAlert.White)
            {
                myBrush = new SolidBrush(Color.White);
            }
            else if (AlertState == LcarsAlert.Yellow)
            {
                myBrush = new SolidBrush(Color.Yellow);
            }
            else if (AlertState == LcarsAlert.Custom)
            {
                myBrush = new SolidBrush(CustomAlertColor);
            }

            mybitmap = new Bitmap(Size.Width, Size.Height);
            g = Graphics.FromImage(mybitmap);

            g.FillRectangle(Brushes.Black, 0, 0, mybitmap.Width, mybitmap.Height);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            halfHeight = Height / 2;
            quarterHeight = Height / 4;
            quarterWidth = Width / 4;

            if (myButtonType == LcarsButtonStyles.Pill)
            {
                g.FillEllipse(myBrush, 0, 0, Size.Height, Size.Height);
                g.FillRectangle(myBrush, halfHeight, 0, Size.Width - Size.Height, Size.Height);
                g.FillEllipse(myBrush, Size.Width - Size.Height, 0, Size.Height, Size.Height);

                textLocation = new Point(Height / 2, 0);
                textSize = new Size(Width - Height, Height);
            }
            else
            {
                g.FillEllipse(myBrush, new Rectangle(0, 0, quarterHeight, quarterHeight));
                g.FillEllipse(myBrush, new Rectangle(Width - quarterHeight, 0, quarterHeight, quarterHeight));
                g.FillEllipse(myBrush, new Rectangle(0, Height - quarterHeight, quarterHeight, quarterHeight));
                g.FillEllipse(myBrush, new Rectangle(Width - quarterHeight, Height - quarterHeight, quarterHeight, quarterHeight));

                g.FillRectangle(myBrush, new Rectangle(quarterHeight / 2, 0, Width - quarterHeight, quarterHeight));
                g.FillRectangle(myBrush, new Rectangle(quarterHeight / 2, Height - quarterHeight, Width - quarterHeight, quarterHeight));
                g.FillRectangle(myBrush, new Rectangle(0, quarterHeight / 2, quarterHeight, Height - quarterHeight));
                g.FillRectangle(myBrush, new Rectangle(Width - quarterHeight, quarterHeight / 2, quarterHeight, Height - quarterHeight));
                g.FillRectangle(myBrush, new Rectangle(quarterHeight / 2, quarterHeight / 2, Width - quarterHeight, Height - quarterHeight));

                Bitmap slant;
                Point[] mypoints;
                GraphicsUnit pageUnit = GraphicsUnit.Pixel;

                switch (myButtonType)
                {
                    case LcarsButtonStyles.RoundedSquareSlant:
                        slant = new Bitmap(mybitmap);

                        mypoints = new Point[3];
                        mypoints[0] = new Point(Width / 4, 0);
                        mypoints[1] = new Point(Width, 0);
                        mypoints[2] = new Point(0, Height);

                        g.FillRectangle(Brushes.Black, mybitmap.GetBounds(ref pageUnit));
                        g.DrawImage(slant, mypoints);

                        textLocation = new Point(Width / 4, 0);
                        textSize = new Size(Width - (Width / 2), Height);
                        break;
                    case LcarsButtonStyles.RoundedSquareBackSlant:
                        slant = new Bitmap(mybitmap);

                        mypoints = new Point[3];
                        mypoints[0] = new Point(0, 0);
                        mypoints[1] = new Point(Width - (Width / 4), 0);
                        mypoints[2] = new Point(Width / 4, Height);

                        g.FillRectangle(Brushes.Black, mybitmap.GetBounds(ref pageUnit));
                        g.DrawImage(slant, mypoints);

                        textLocation = new Point(Width / 4, 0);
                        textSize = new Size(Width - (Width / 2), Height);
                        break;
                    default:
                        textLocation = new Point(0, 0);
                        textSize = new Size(Width, Height);
                        break;
                }
            }
            g.Dispose();
            return mybitmap;
        }
        #endregion
    }
}
