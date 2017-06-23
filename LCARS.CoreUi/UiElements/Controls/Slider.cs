using LCARS.CoreUi.Assets.Access;
using LCARS.CoreUi.Colors;
using LCARS.CoreUi.Enums;
using LCARS.CoreUi.Interfaces;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LCARS.CoreUi.UiElements.Controls
{
    [DefaultEvent("ValueChanged")]
    public class Slider : Control, IColorable, IAlertable
    {
        public event EventHandler ValueChanged;

        private bool mouseDown = false;
        private int mouseOffset = 0;
        private int padding = 5;

        public Slider() : base()
        {
            Size = new Size(30, 200);
            this.DoubleBuffered = true;
        }

        #region " Properties "
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LcarsColorManager ColorManager
        {
            get { return colorManager; }
            set
            {
                if (colorManager != null) colorManager.ColorsUpdated -= OnColorsUpdate;
                colorManager = value;
                if (colorManager != null) colorManager.ColorsUpdated += OnColorsUpdate;
                Invalidate();
            }
        }
        private LcarsColorManager colorManager = new LcarsColorManager();

        public Color CustomAlertColor
        {
            get { return customAlertColor; }
            set
            {
                if (value == customAlertColor) return;
                customAlertColor = value;
                if (alertState == LcarsAlert.Custom)
                {
                    Invalidate();
                }
            }
        }
        private Color customAlertColor;

        [DefaultValue(LcarsColorFunction.MiscFunction)]
        public LcarsColorFunction MainColor
        {
            get { return mainColor; }
            set
            {
                if (value == mainColor) return;
                mainColor = value;
                Invalidate();
            }
        }
        private LcarsColorFunction mainColor = LcarsColorFunction.MiscFunction;

        [DefaultValue(LcarsColorFunction.PrimaryFunction)]
        public LcarsColorFunction ButtonColor
        {
            get { return buttonColor; }
            set
            {
                if (value == buttonColor) return;
                buttonColor = value;
                Invalidate();
            }
        }
        private LcarsColorFunction buttonColor = LcarsColorFunction.PrimaryFunction;

        [DefaultValue(true)]
        public bool IsLit
        {
            get { return isLit; }
            set
            {
                if (value == isLit) return;
                isLit = value;
                Invalidate();
            }
        }
        private bool isLit = true;

        [DefaultValue(0)]
        public int Min
        {
            get { return min; }
            set
            {
                if (min == value) return;
                min = value;
                val = ClipValue(val);
                InvalidateBar();
            }
        }
        private int min = 0;


        [DefaultValue(100)]
        public int Max
        {
            get { return max; }
            set
            {
                if (max == value) return;
                max = value;
                val = ClipValue(val);
                InvalidateBar();
            }
        }
        private int max = 100;

        [DefaultValue(50)]
        public int Value
        {
            get { return val; }
            set
            {
                if (val == value)
                {
                    if (mouseDown)
                    {
                        InvalidateBar();
                    }
                }
                else
                {
                    val = ClipValue(value);
                    InvalidateBar();
                    OnValueChanged(new EventArgs());
                }
            }
        }
        private int val = 50;

        [DefaultValue(LcarsAlert.Normal)]
        public LcarsAlert AlertState
        {
            get { return alertState; }
            set
            {
                if (value == alertState) return;
                alertState = value;
                Invalidate();
            }
        }
        private LcarsAlert alertState = LcarsAlert.Normal;

        [DefaultValue(30)]
        public int ButtonHeight
        {
            get { return buttonHeight; }
            set
            {
                if (value == buttonHeight)
                    return;
                //Can't be too small
                if (value < Width / 4)
                {
                    value = Width / 4;
                }
                //Or larger than half the bar
                if (value > (Height - Width / 2 - padding * 2) / 2)
                {
                    value = (Height - Width / 2 - padding * 2) / 2;
                }
                buttonHeight = value;
                InvalidateBar();
            }
        }
        private int buttonHeight = 30;
        #endregion

        private void OnColorsUpdate(object sender, EventArgs e)
        {
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Rectangle btnBounds = ButtonBounds;
            Point mLoc = PointToClient(MousePosition);
            if (e.Button == MouseButtons.Left & btnBounds.Contains(mLoc))
            {
                mouseOffset = mLoc.Y - btnBounds.Top - btnBounds.Height / 2;
                mouseDown = true;
                InvalidateBar();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left & mouseDown)
            {
                mouseDown = false;
                InvalidateBar();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            //Check if mouse is actually within the control's bounds.
            if (Size.Width > e.Location.X &&
                Size.Height > e.Location.Y &&
                e.Location.Y >= 0 &&
                e.Location.X >= 0)
            {
                if (e.Button == MouseButtons.Left &&
                    mouseDown)
                {
                    int h = Height - Width / 2 - 2 * padding - buttonHeight;
                    int y = PointToClient(MousePosition).Y - padding - Width / 4 - buttonHeight / 2 - mouseOffset;
                    int newValue = Convert.ToInt32(Math.Round((double)(y * (min - max) / h + max)));
                    Value = newValue;
                    //Handles the rest of the update
                }
            }
        }

        protected override void OnMouseLeave(System.EventArgs e)
        {
            base.OnMouseLeave(e);
            if (mouseDown)
            {
                mouseDown = false;
                InvalidateBar();
            }
        }

        /// <summary>
        /// Raises the ValueChanged event
        /// </summary>
        /// <param name="e">An EventArgs that contains the event data</param>
        /// <remarks>
        /// Derived classes should override this sub instead of handling the event. This avoids the
        /// use of a delegate that would be required by the event.
        /// </remarks>
        protected virtual void OnValueChanged(EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            buttonHeight = Convert.ToInt32(factor.Height * buttonHeight);
            padding = Convert.ToInt32(factor.Height * padding);
            base.ScaleControl(factor, specified);
        }

        /// <summary>
        /// Bounds of the button from current value or mouse position.
        /// </summary>
        /// <returns>Client-area coordinate rectangle</returns>
        /// <remarks>
        /// If the mouse is down, the rectangle will always be centered on it, minus the original mouse
        /// offset. Otherwise, the rectangle will be centered on the current value.
        /// </remarks>
        protected Rectangle ButtonBounds
        {
            get
            {
                double h = Height - Width / 2 - 2 * padding - buttonHeight;
                double y = 0;
                if (mouseDown)
                {
                    y = PointToClient(MousePosition).Y - buttonHeight / 2 - Width / 4 - padding - mouseOffset;
                    if (y < 0) y = 0;
                    else if (y > h) y = h;
                }
                else
                {
                    y = (double)(val - max) / (min - max) * h;
                }
                return new Rectangle(0, Convert.ToInt32(y) + padding + Width / 4, Width, buttonHeight);
            }
        }

        /// <summary>
        /// Invalidate only the bar section of the control.
        /// </summary>
        /// <remarks>
        /// This saves drawing time by keeping the ellipses we already drew.
        /// </remarks>
        protected void InvalidateBar()
        {
            Invalidate(new Rectangle(0, Width / 4, Width, Height - Width / 2));
        }

        /// <summary>
        /// Clip the given value to lie within the range of [min, max].
        /// </summary>
        /// <remarks>
        /// Min does not have to be less than Max. The value will be clipped to lie between Min and
        /// Max, inclusive.
        /// </remarks>
        protected int ClipValue(int newValue)
        {
            if (min < max)
            {
                if (newValue < min) newValue = min;
                else if (newValue > max) newValue = max;
            }
            else
            {
                if (newValue > min) newValue = min;
                else if (newValue < max) newValue = max;
            }
            return newValue;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Color barColor;
            Color buttonColor;
            switch (alertState)
            {
                case LcarsAlert.Normal:
                    barColor = ColorManager.GetColor(mainColor);
                    break;
                case LcarsAlert.Red:
                    barColor = Color.Red;
                    break;
                case LcarsAlert.White:
                    barColor = Color.White;
                    break;
                case LcarsAlert.Yellow:
                    barColor = Color.Yellow;
                    break;
                case LcarsAlert.Custom:
                    barColor = customAlertColor;
                    break;
                default:
                    barColor = Color.Red;
                    break;
            }
            if (!isLit) barColor = Color.FromArgb(255, barColor.R / 2, barColor.G / 2, barColor.B / 2);

            Rectangle btnRect = ButtonBounds;
            if (alertState == LcarsAlert.Normal)
            {
                if (mouseDown & btnRect.Contains(PointToClient(MousePosition))) buttonColor = Color.Red;
                else buttonColor = ColorManager.GetColor(this.buttonColor);
            }
            else buttonColor = barColor;

            //Setup brushes
            SolidBrush barBrush = new SolidBrush(barColor);
            SolidBrush buttonBrush = new SolidBrush(buttonColor);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            //Draw top/bottom ellipse sections if required
            if (e.ClipRectangle.Height != Height - Width / 2)
            {
                g.FillPie(barBrush, 0, 0, Width, Width / 2, 180, 180);
                g.FillPie(barBrush, 0, Height - Width / 2, Width, Width / 2, 0, 180);
            }

            //Draw main rectangles
            int railStart = Width / 4;
            g.FillRectangle(barBrush, 0, railStart, Width, btnRect.Top - railStart - padding);
            g.FillRectangle(barBrush, 0, btnRect.Bottom + padding, Width, Height - (btnRect.Bottom + padding) - Width / 4);
            //Draw button
            g.FillRectangle(buttonBrush, btnRect);

            //scale font down to fit
            float fontSize = 16;
            Font f;
            SizeF textSize;
            do
            {

                f = FontProvider.Lcars(fontSize);
                textSize = CreateGraphics().MeasureString(val.ToString(), f);
                fontSize -= .5f;
            }
            while (textSize.Width > btnRect.Width);

            var placement = new Rectangle(
                (int)(btnRect.X + (btnRect.Width - textSize.Width) / 2),
                (int)(btnRect.Y + (btnRect.Height - textSize.Height) / 2),
                (int)Math.Ceiling(textSize.Width),
                (int)Math.Ceiling(textSize.Height));

            g.DrawString(val.ToString(), f, Brushes.Black, placement);
        }
    }
}
