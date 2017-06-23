using LCARS.CoreUi.Assets.Access;
using LCARS.CoreUi.Enums;
using LCARS.CoreUi.UiElements.Controls;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LCARS.CoreUi.UiElements.Lightweight
{
    /// <summary>
    /// Lightweight implementation of a 
    /// <see cref="LCARS.Controls.ComplexButton">complex button</see>
    /// </summary>
    public class LCComplexButton : LCStandardButton
    {
        /// <summary>
        /// Redraws the lightweight complex button
        /// </summary>
        public override void Redraw()
        {
            if (holdDraw) return;
            if (bounds.Height <= 0) return;
            if (bounds.Width <= 0) return;

            Font sideFont = FontProvider.Lcars(Height + (Height / 2.9f), GraphicsUnit.Pixel);
            //Set up brushes
            SolidBrush myBrush = new SolidBrush(ColorManager.GetColor(ColorFunction));
            SolidBrush sideBrush;
            SolidBrush sideTextBrush;
            myBrush = GetBrush();
            if (alertState == LcarsAlert.Normal && !isPressed)
            {
                sideBrush = new SolidBrush(ColorManager.GetColor(sideBlockColor));
                sideTextBrush = new SolidBrush(ColorManager.GetColor(sideTextColor));
            }
            else
            {
                sideBrush = myBrush;
                sideTextBrush = myBrush;
            }

            //Set up graphics and image
            myBitmap = new Bitmap(bounds.Width, bounds.Height);
            Graphics g = Graphics.FromImage(myBitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.Clear(Color.Black);

            //Left orange block
            g.FillRectangle(sideBrush, 0, 0, Height / 2, Height);
            int curLeft = Height / 2;
            SizeF sideTextSize = g.MeasureString(sideText.ToUpper(), sideFont);

            if (sideTextWidth > -1)
            {
                curLeft += sideTextWidth - (int)sideTextSize.Width;
            }
            else
            {
                curLeft -= Height / 5;
            }

            //draw the side text
            g.DrawString(sideText.ToUpper(), sideFont, sideTextBrush, curLeft, -Height / 4.7f);
            if (!string.IsNullOrEmpty(sideText))
            {
                curLeft = (int)((curLeft + sideTextSize.Width) - (Height / 6f));
            }
            else
            {
                curLeft = Height / 2;
            }
            //Draw text and remainder of button
            Rectangle textRect = new Rectangle(curLeft, 0, (Width - curLeft) - Height / 2, Height);
            g.FillRectangle(myBrush, textRect);
            g.FillEllipse(myBrush, Width - Height, 0, Height, Height);
            DrawText(textRect, g);
            ChangeLit(g);
            g.Dispose();
            DoEvent(LightweightEvents.Update);
        }

        /// <summary>
        /// Text to display on the side of the control
        /// </summary>
        /// <remarks>
        /// If <see cref="LCARS.LightweightControls.LCComplexButton.SideTextWidth">SideTextWidth</see>
        /// is set, the text will be constrained to a fixed area. If it has not been set,
        /// the text area will resize to fit.
        /// </remarks>
        public string SideText
        {
            get { return sideText; }
            set
            {
                sideText = value;
                Redraw();
            }
        }
        string sideText = "47";

        /// <summary>
        /// Color of the side text
        /// </summary>
        public LcarsColorFunction SideTextColor
        {
            get { return sideTextColor; }
            set
            {
                if (value == sideTextColor) return;
                sideTextColor = value;
                Redraw();
            }
        }
        LcarsColorFunction sideTextColor = LcarsColorFunction.Orange;


        /// <summary>
        /// Color of the side block on the control
        /// </summary>
        public LcarsColorFunction SideBlockColor
        {
            get { return sideBlockColor; }
            set
            {
                if (value == sideBlockColor) return;
                sideBlockColor = value;
                Redraw();
            }
        }
        LcarsColorFunction sideBlockColor = LcarsColorFunction.Orange;

        /// <summary>
        /// Sets a static text width for the side text.
        /// </summary>
        /// <remarks>
        /// If <see cref="LCARS.LightweightControls.LCComplexButton.SideTextWidth">SideTextWidth</see>
        /// is set, the text will be constrained to a fixed area. If it has not been set,
        /// the text area will resize to fit.
        /// </remarks>
        public int SideTextWidth
        {
            get { return sideTextWidth; }
            set
            {
                if (value == sideTextWidth) return;
                sideTextWidth = value;
                Redraw();
            }
        }
        int sideTextWidth = -1;

        #region " Conversion "
        public static implicit operator ComplexButton(LCComplexButton o)
        {
            ComplexButton newButton = new ComplexButton();
            newButton.Text = o.Text;
            newButton.Bounds = o.Bounds;
            newButton.Data = o.Data;
            newButton.Data2 = o.Data2;
            newButton.ColorManager = o.ColorManager;
            newButton.ColorFunction = o.ColorFunction;
            newButton.AutoEllipsis = o.AutoEllipsis;
            newButton.ButtonTextAlign = o.TextAlign;
            newButton.Clickable = o.Clickable;
            newButton.Flash = o.Flashing;
            newButton.FlashInterval = Convert.ToInt32(o.FlashInterval);
            newButton.Font = o.Font;
            newButton.HoldDraw = o.HoldDraw;
            newButton.CustomAlertColor = o.CustomAlertColor;
            newButton.AlertState = o.AlertState;
            newButton.ForceCaps = o.ForceCaps;
            newButton.SideBlockColor = o.SideBlockColor;
            newButton.SideText = o.SideText;
            newButton.SideTextColor = o.SideTextColor;
            newButton.SideTextWidth = o.SideTextWidth;
            return newButton;
        }

        public static explicit operator LCComplexButton(ComplexButton o)
        {
            LCComplexButton newButton = new LCComplexButton();
            newButton.Text = o.Text;
            newButton.Bounds = o.Bounds;
            newButton.Data = o.Data;
            newButton.Data2 = o.Data2;
            newButton.ColorManager = o.ColorManager;
            newButton.ColorFunction = o.ColorFunction;
            newButton.AutoEllipsis = o.AutoEllipsis;
            newButton.TextAlign = o.ButtonTextAlign;
            newButton.Clickable = o.Clickable;
            newButton.Flashing = o.Flash;
            newButton.FlashInterval = Convert.ToInt32(o.FlashInterval);
            newButton.Font = o.Font;
            newButton.HoldDraw = o.HoldDraw;
            newButton.CustomAlertColor = o.CustomAlertColor;
            newButton.AlertState = o.AlertState;
            newButton.ForceCaps = o.ForceCaps;
            newButton.SideBlockColor = o.SideBlockColor;
            newButton.SideText = o.SideText;
            newButton.SideTextColor = o.SideTextColor;
            newButton.SideTextWidth = o.SideTextWidth;
            return newButton;
        }
        #endregion
    }
}
