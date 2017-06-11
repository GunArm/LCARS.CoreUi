using LCARS.CoreUi.Enums;
using System.Collections.Generic;
using System.Drawing;

namespace LCARS.CoreUi.UiElements
{
    public partial class LcarsButtonBase
    {
        /// <summary>
        /// Redraws the button and stores the bitmaps to memory.
        /// </summary>
        public void DrawAllButtons()
        {
            if (holdDraw) return;

            if (Width == 0 || Height == 0) return;

            if (normalButton != null)
            {
                //UnlitButton does not require null check
                normalButton.Dispose();
                unlitButton.Dispose();
            }

            normalButton = DrawButton(); // Lit Button
            unlitButton = DrawUnlitButton(normalButton); // Unlit button

            Invalidate();
        }

        public virtual Bitmap DrawButton()
        {
            Bitmap mybitmap = default(Bitmap);
            Graphics g = default(Graphics);

            var alertColorMapping = new Dictionary<LcarsAlert, Color>()
            {
                {LcarsAlert.Normal, ColorManager.GetColor(colorFunction) },
                {LcarsAlert.Red, Color.Red },
                {LcarsAlert.White, Color.White},
                {LcarsAlert.Yellow, Color.Yellow},
                {LcarsAlert.Custom, customAlertColor},
            };
            Color drawColor = alertColorMapping[alertState];

            // turn color white when pressed, unless already white, then turn it red
            if (isPressed) drawColor = (drawColor == Color.White) ? Color.Red : Color.White;

            SolidBrush myBrush = new SolidBrush(drawColor);

            mybitmap = new Bitmap(this.Size.Width, this.Size.Height);
            g = Graphics.FromImage(mybitmap);

            g.FillRectangle(Brushes.Black, 0, 0, mybitmap.Width, mybitmap.Height);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            g.FillEllipse(myBrush, 0, 0, this.Size.Height, this.Size.Height);
            g.FillRectangle(myBrush, this.Size.Height / 2, 0, this.Size.Width - this.Size.Height, this.Size.Height);
            g.FillEllipse(myBrush, this.Size.Width - this.Size.Height, 0, this.Size.Height, this.Size.Height);
            //Draw text:
            this.TextLocation = new Point(0, 0);
            this.TextSize = (Size)new Point(this.Width - this.Height, this.Height);
            g.Dispose();
            return mybitmap;
        }

        private Bitmap DrawUnlitButton(Bitmap normal)
        {
            Color fadeColor = Color.FromArgb(128, 0, 0, 0);
            Bitmap mybitmap = new Bitmap(normal);
            //make a copy of the normal button.
            Graphics g = Graphics.FromImage(mybitmap);
            SolidBrush mybrush = new SolidBrush(fadeColor);

            g.FillRectangle(mybrush, 0, 0, mybitmap.Width, mybitmap.Height);
            return mybitmap;
        }

        /// <summary>
        /// Draws the text of the control in specified area.
        /// </summary>
        /// <param name="g">Graphics object being used to draw the control.</param>
        /// <remarks>This sub will handle all auto-ellipsis functionality and alignment.
        /// </remarks>
        protected void DrawText(Graphics g)
        {
            if (!textVisible) return;

            Rectangle area = new Rectangle(textLocation, textSize);
            StringFormat format = new StringFormat();
            format.FormatFlags = StringFormatFlags.NoWrap;

            if (textScrollTimer.Enabled | !autoEllipsis)
            {
                if (ellipsisMode == EllipsisModes.Character) format.Trimming = StringTrimming.Character;
                else format.Trimming = StringTrimming.Word;
            }
            else
            {
                if (ellipsisMode == EllipsisModes.Character)  format.Trimming = StringTrimming.EllipsisCharacter;
                else format.Trimming = StringTrimming.EllipsisWord;
            }

            if (textScrollTimer.Enabled)  format.Alignment = StringAlignment.Near;
            else
            {
                if (textAlign == ContentAlignment.BottomCenter | textAlign == ContentAlignment.MiddleCenter | textAlign == ContentAlignment.TopCenter)
                {
                    format.Alignment = StringAlignment.Center;
                }
                else if (textAlign == ContentAlignment.BottomRight | textAlign == ContentAlignment.MiddleRight | textAlign == ContentAlignment.TopRight)
                {
                    format.Alignment = StringAlignment.Far;
                }
                else if (textAlign == ContentAlignment.BottomLeft | textAlign == ContentAlignment.MiddleLeft | textAlign == ContentAlignment.TopLeft)
                {
                    format.Alignment = StringAlignment.Near;
                }
            }

            if (textAlign == ContentAlignment.TopCenter | textAlign == ContentAlignment.TopLeft | textAlign == ContentAlignment.TopRight)
            {
                format.LineAlignment = StringAlignment.Near;
            }
            else if (textAlign == ContentAlignment.MiddleCenter | textAlign == ContentAlignment.MiddleLeft | textAlign == ContentAlignment.MiddleRight)
            {
                format.LineAlignment = StringAlignment.Center;
            }
            else if (textAlign == ContentAlignment.BottomCenter | textAlign == ContentAlignment.BottomLeft | textAlign == ContentAlignment.BottomRight)
            {
                format.LineAlignment = StringAlignment.Far;
            }

            if (ForceCaps) g.DrawString(currentTextScrollRotation.ToUpper(), font, Brushes.Black, area, format);
            else g.DrawString(currentTextScrollRotation, font, Brushes.Black, area, format);
        }
    }
}
