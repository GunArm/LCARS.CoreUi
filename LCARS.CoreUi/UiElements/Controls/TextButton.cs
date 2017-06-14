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

        #region " Global Variables "
        TextButtonType myType = TextButtonType.DoublePills;
        ContentAlignment myTextAlign = ContentAlignment.MiddleRight;

        FontData fontDims;
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
            get { return myType; }
            set
            {
                myType = value;
                DrawAllButtons();
            }
        }

        public override ContentAlignment ButtonTextAlign
        {
            get { return myTextAlign; }
            set
            {
                myTextAlign = value;
                DrawAllButtons();
            }
        }

        public new int TextHeight
        {
            get { return textHeight; }
            set
            {
                textHeight = value;
                fontDims = GetFontDimensions(new Font("LCARS", value, FontStyle.Regular, GraphicsUnit.Point), ButtonText);
                DrawAllButtons();
            }
        }

        public override string ButtonText
        {
            get { return myText; }
            set
            {
                myText = value.ToUpper();
                fontDims = GetFontDimensions(new Font("LCARS", textHeight, FontStyle.Regular, GraphicsUnit.Point), myText);
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
            myFont = new Font("LCARS", textHeight, FontStyle.Regular, GraphicsUnit.Point);
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
                    SolidBrush mybrush = new SolidBrush(ColorManager.GetColor(ColorFunction));
                    Font myfont = new Font("LCARS", TextHeight, FontStyle.Regular, GraphicsUnit.Point);
                    int drawHeight = 0;

                    if (AlertState == LcarsAlert.Red)
                    {
                        mybrush = new SolidBrush(Color.Red);
                    }
                    else if (AlertState == LcarsAlert.White)
                    {
                        mybrush = new SolidBrush(Color.White);
                    }
                    else if (AlertState == LcarsAlert.Yellow)
                    {
                        mybrush = new SolidBrush(Color.Yellow);
                    }
                    else if (AlertState == LcarsAlert.Custom)
                    {
                        mybrush = new SolidBrush(CustomAlertColor);
                    }

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

                    //Draw black background
                    g.FillRectangle(Brushes.Black, 0, 0, mybitmap.Width, mybitmap.Height);

                    //Set graphics to use smoothing
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    switch (myType)
                    {
                        case TextButtonType.DoublePills:
                            g.FillEllipse(mybrush, 0, 0, drawHeight, drawHeight);
                            g.FillEllipse(mybrush, Width - drawHeight, 0, drawHeight, drawHeight);
                            g.FillRectangle(Brushes.Black, drawHeight / 2, 0, Width - drawHeight, drawHeight);
                            g.FillRectangle(mybrush, drawHeight / 2, 0, drawHeight / 2, drawHeight);
                            g.FillRectangle(mybrush, drawHeight + 6, 0, (Width - (drawHeight * 2)) - 12, drawHeight);
                            g.FillRectangle(mybrush, Width - drawHeight, 0, drawHeight / 2, drawHeight);
                            break;
                    }
                    if (!string.IsNullOrEmpty(ButtonText))
                    {
                        if (myTextAlign.ToString().ToLower().Contains("right"))
                        {
                            g.FillRectangle(Brushes.Black, Width - ((fontDims.Width + drawHeight) + 12), 0, fontDims.Width + 6, drawHeight);
                            g.DrawString(ButtonText, myfont, Brushes.Orange, Width - (((fontDims.Width + fontDims.Left) + drawHeight) + 6), -fontDims.Top);
                        }

                        if (myTextAlign.ToString().ToLower().Contains("left"))
                        {
                            g.FillRectangle(Brushes.Black, drawHeight + 6, 0, fontDims.Width + 6, drawHeight);
                            g.DrawString(ButtonText, myfont, Brushes.Orange, (drawHeight - fontDims.Left) + 6, -fontDims.Top);
                        }

                        if (myTextAlign.ToString().ToLower().Contains("center"))
                        {
                            g.FillRectangle(Brushes.Black, (Width / 2) - ((fontDims.Width + 12) / 2), 0, fontDims.Width + 12, drawHeight);
                            g.DrawString(ButtonText, myfont, Brushes.Orange, ((Width / 2) - (fontDims.Width / 2)) - fontDims.Left, -fontDims.Top);
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
