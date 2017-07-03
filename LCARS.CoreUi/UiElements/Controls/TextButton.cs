using LCARS.CoreUi.Assets.Access;
using LCARS.CoreUi.Enums;
using LCARS.CoreUi.UiElements.Base;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LCARS.CoreUi.UiElements.Controls
{
    [System.ComponentModel.DefaultEvent("Click")]
    public class TextButton : LcarsButtonBase
    {
        #region " Control Design Information "
        public TextButton() : base()
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
            Name = "TextButton";
            Size = new Size(200, 100);
            TextHeight = 24;
            ResumeLayout(false);
        }
        #endregion

        #region " Enums"
        public enum TextButtonType
        {
            NoPills = 0,
            DoublePills = 1,
            LeftPill = 2,
            RightPill = 3
        }

        public struct FontData
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
            public int Width;
            public int Height;
        }
        #endregion

        #region " Properties "
        public TextButtonType ButtonType
        {
            get { return buttonType; }
            set
            {
                if (buttonType == value) return;
                buttonType = value;
                DrawAllButtons();
            }
        }
        TextButtonType buttonType = TextButtonType.DoublePills;

        public override ContentAlignment ButtonTextAlign
        {
            get { return buttonTextAlign; }
            set
            {
                if (buttonTextAlign == value) return;
                buttonTextAlign = value;
                DrawAllButtons();
            }
        }
        ContentAlignment buttonTextAlign = ContentAlignment.MiddleRight;

        public new int TextHeight
        {
            get { return textHeight; }
            set
            {
                if (textHeight == value) return;
                textHeight = value;
                font = FontProvider.Lcars(value);
                DrawAllButtons();
            }
        }

        public override string ButtonText
        {
            get { return buttonText; }
            set
            {
                if (buttonText == value) return;
                buttonText = value;
                DrawAllButtons();
            }
        }
        #endregion

        #region " Functions "
        private FontData GetFontDimensions(Font myFont, string Text)
        {
            FontData myData = new FontData();
            int x = 0;
            int y = 0;
            Graphics myG = null;
            SizeF textSize = CreateGraphics().MeasureString(Text, myFont);
            if (string.IsNullOrEmpty(Text))
            {
                return new FontData();
            }

            Bitmap mybitmap = new Bitmap(Convert.ToInt16(textSize.Width), Convert.ToInt16(textSize.Height));

            myG = Graphics.FromImage(mybitmap);
            myG.SmoothingMode = SmoothingMode.AntiAlias;
            myG.PixelOffsetMode = PixelOffsetMode.HighQuality;

            myG.DrawString(Text, myFont, Brushes.Black, 0, 0);

            myData.Left = mybitmap.Width;
            myData.Top = mybitmap.Height;
            myData.Bottom = 0;
            myData.Right = 0;

            for (x = 0; x <= mybitmap.Width - 1; x++)
            {
                for (y = 0; y <= mybitmap.Height - 1; y++)
                {
                    if (mybitmap.GetPixel(x, y).ToArgb() != Color.Black.ToArgb()) continue;

                    if (myData.Left > x) myData.Left = x;
                    if (myData.Top > y) myData.Top = y;
                    if (myData.Right < x) myData.Right = x;
                    if (myData.Bottom < y) myData.Bottom = y;
                }
            }

            myData.Height = myData.Bottom - myData.Top;
            myData.Width = myData.Right - myData.Left;

            return myData;
        }
        #endregion

        #region " Draw TextButton "
        public override Bitmap DrawButton()
        {
            if (Width > 0 & Height > 0)
            {
                Bitmap mybitmap = new Bitmap(1, 1);

                if (TextHeight > 0)
                {
                    Graphics g = null;
                    SolidBrush mybrush = new SolidBrush(GetButtonColor());

                    int drawHeight = 0;
                    string drawString = ForceCaps ? ButtonText.ToUpper() : ButtonText;

                    FontData fontDims = GetFontDimensions(font, drawString);
                    if (fontDims.Height == 0)
                    {
                        fontDims = new FontData();
                        fontDims.Height = Height;
                    }
                    TextVisible = false;
                    mybitmap = new Bitmap(Size.Width, fontDims.Height);
                    g = Graphics.FromImage(mybitmap);

                    if (ButtonText.ToUpper().Contains("Q"))
                    {
                        drawHeight = fontDims.Height - (fontDims.Height / 10);
                        Height = mybitmap.Height;
                    }
                    else
                    {
                        drawHeight = fontDims.Height;
                        Height = drawHeight;
                    }

                    //Set graphics to use smoothing
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    //Draw black background
                    g.FillRectangle(Brushes.Black, 0, 0, mybitmap.Width, mybitmap.Height);
                    g.FillRectangle(mybrush, drawHeight + 6, 0, (Width - (drawHeight * 2)) - 12, drawHeight); // full middle bar

                    switch (buttonType)
                    {
                        case TextButtonType.DoublePills:
                            g.FillEllipse(mybrush, 0, 0, drawHeight, drawHeight); // left pill
                            g.FillRectangle(mybrush, drawHeight / 2, 0, drawHeight / 2, drawHeight); // left pill bulk
                            g.FillEllipse(mybrush, Width - drawHeight, 0, drawHeight, drawHeight); // right pill
                            g.FillRectangle(mybrush, Width - drawHeight, 0, drawHeight / 2, drawHeight); // right pill bulk        
                            break;
                        case TextButtonType.LeftPill:
                            g.FillEllipse(mybrush, 0, 0, drawHeight, drawHeight); // left pill
                            g.FillRectangle(mybrush, drawHeight / 2, 0, drawHeight / 2, drawHeight); // left pill bulk
                            g.FillRectangle(mybrush, Width - drawHeight, 0, drawHeight, drawHeight); // right pill bulk        
                            break;
                        case TextButtonType.RightPill:
                            g.FillRectangle(mybrush, 0, 0, drawHeight, drawHeight); // left pill bulk
                            g.FillEllipse(mybrush, Width - drawHeight, 0, drawHeight, drawHeight); // right pill
                            g.FillRectangle(mybrush, Width - drawHeight, 0, drawHeight / 2, drawHeight); // right pill bulk   
                            break;
                        case TextButtonType.NoPills:
                            g.FillRectangle(mybrush, 0, 0, drawHeight, drawHeight); // left pill bulk
                            g.FillRectangle(mybrush, Width - drawHeight, 0, drawHeight, drawHeight); // right pill bulk   
                            break;
                    }
                    
                    if (!string.IsNullOrEmpty(ButtonText))
                    {
                        if (buttonTextAlign.ToString().ToLower().Contains("right"))
                        {
                            g.FillRectangle(Brushes.Black, Width - ((fontDims.Width + drawHeight) + 12), 0, fontDims.Width + 6, drawHeight);
                            g.DrawString(drawString, font, Brushes.Orange, Width - (((fontDims.Width + fontDims.Left) + drawHeight) + 6), -fontDims.Top);
                        }

                        if (buttonTextAlign.ToString().ToLower().Contains("left"))
                        {
                            g.FillRectangle(Brushes.Black, drawHeight + 6, 0, fontDims.Width + 6, drawHeight);
                            g.DrawString(drawString, font, Brushes.Orange, (drawHeight - fontDims.Left) + 6, -fontDims.Top);
                        }

                        if (buttonTextAlign.ToString().ToLower().Contains("center"))
                        {
                            g.FillRectangle(Brushes.Black, (Width / 2) - ((fontDims.Width + 12) / 2), 0, fontDims.Width + 12, drawHeight);
                            g.DrawString(drawString, font, Brushes.Orange, ((Width / 2) - (fontDims.Width / 2)) - fontDims.Left, -fontDims.Top);
                        }
                    }
                }
                return mybitmap;
            }
            else
            {
                return new Bitmap(1, 1);
            }
        }
        #endregion
    }
}
