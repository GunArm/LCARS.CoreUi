using LCARS.CoreUi.Enums;
using LCARS.CoreUi.UiElements.Base;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LCARS.CoreUi.UiElements.Controls
{
    [System.ComponentModel.DefaultEvent("Click")]
    public class ComplexButton : LcarsButtonBase
    {
        #region " Control Design Information "
        public ComplexButton() : base()
        {
            ParentChanged += OnParentChanged;

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
            Name = "ComplexButton";
            Size = new Size(200, 100);
            ResumeLayout(false);
        }
        #endregion

        #region " Properties "
        public string SideText
        {
            get { return sideText; }
            set
            {
                if (sideText == value) return;
                sideText = value;
                DrawAllButtons();
            }
        }
        string sideText = "47";

        public int SideTextWidth
        {
            get { return sideTextWidth; }
            set
            {
                if (sideTextWidth == value) return;
                sideTextWidth = value;
                DrawAllButtons();
            }
        }
        int sideTextWidth = -1;

        public LcarsColorFunction SideTextColor
        {
            get { return sideTextColor; }
            set
            {
                if (SideTextColor == value) return;
                sideTextColor = value;
                DrawAllButtons();
            }
        }
        LcarsColorFunction sideTextColor = LcarsColorFunction.Orange;

        public LcarsColorFunction SideBlockColor
        {
            get { return sideBlockColor; }
            set
            {
                if (sideBlockColor == value) return;
                sideBlockColor = value;
                DrawAllButtons();
            }
        }
        LcarsColorFunction sideBlockColor = LcarsColorFunction.Orange;
        #endregion

        #region " Subs "
        protected void OnParentChanged(object sender, System.EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
        }
        #endregion

        #region " Draw Complex Button "
        public override Bitmap DrawButton()
        {
            Bitmap mybitmap = null;
            Graphics g = null;
            SizeF buttonTextSize = default(SizeF);
            SizeF sideTextSize = default(SizeF);
            int curLeft = 0;
            Font textFont = new Font("LCARS", (Height / 2) + 4, FontStyle.Regular, GraphicsUnit.Pixel);
            Font sideFont = new Font("LCARS", (float)(Height / 2.9) + Height, FontStyle.Regular, GraphicsUnit.Pixel);
            SolidBrush myBrush = new SolidBrush(GetButtonColor());
            SolidBrush sideBrush = new SolidBrush(GetButtonColor(sideBlockColor));
            SolidBrush sideTextBrush = new SolidBrush(GetButtonColor(sideTextColor));

            if (sideText == null)
            {
                sideText = "47";
            }

            //initialize the graphics
            mybitmap = new Bitmap(Width, Height);
            g = Graphics.FromImage(mybitmap);

            g.FillRectangle(Brushes.Black, 0, 0, mybitmap.Width, mybitmap.Height);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //get the width and height of the fonts
            buttonTextSize = g.MeasureString(ButtonText.ToUpper(), textFont);
            sideTextSize = g.MeasureString(sideText.ToUpper(), sideFont);

            //draw the left orange block.  If the mouse is down, draw it white.
            g.FillRectangle(sideBrush, 0, 0, Height / 2, Height);

            //set the curleft to the right side of what we have already drawn.
            curLeft = Height / 2;

            if (sideTextWidth > -1)
            {
                curLeft += sideTextWidth - (int)sideTextSize.Width;
            }
            else
            {
                curLeft -= Height / 5;
            }

            //draw the side text
            g.DrawString(sideText.ToUpper(), sideFont, sideTextBrush, curLeft, (-1) * (float)Height / (float)4.7);

            if (!string.IsNullOrEmpty(sideText))
            {
                curLeft = (curLeft + (int)sideTextSize.Width) - (Height / 6);
            }
            else
            {
                curLeft = curLeft + (Height / 10);
            }

            //draw the main button area
            g.FillRectangle(myBrush, curLeft, 0, (Width - curLeft) - (Height + (Height / 10)), Height);
            TextLocation = new Point(curLeft, 0);
            TextSize = new Size((Width - curLeft) - (Height / 2), Height);
            curLeft += (Width - curLeft) - (Height + (Height / 10));

            curLeft -= Height / 10;

            //draw the straight section of the right side pill shape
            g.FillRectangle(myBrush, curLeft, 0, Height / 2, Height);

            //draw the curved end
            g.FillEllipse(myBrush, curLeft, 0, Height, Height);
            return mybitmap;
        }
        #endregion
    }
}
