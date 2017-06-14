using LCARS.CoreUi.Enums;
using LCARS.CoreUi.UiElements.Base;
using System.Drawing;

namespace LCARS.CoreUi.UiElements.Controls
{
    [System.ComponentModel.DefaultEvent("Click")]
    public class FlatButton : LcarsButtonBase
    {
        #region " Control Design Information "
        public FlatButton() : base()
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
            //FlatButtonButton
            //
            Name = "FlatButton";
            Size = new Size(200, 100);
            ResumeLayout(false);

        }
        #endregion

        #region " Draw Flat Button "
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

            g.FillRectangle(myBrush, 0, 0, mybitmap.Width, mybitmap.Height);

            //Set up text:
            TextSize = Size;
            g.Dispose();
            return mybitmap;
        }
        #endregion
    }
}
