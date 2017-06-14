using System;

namespace LCARS.CoreUi.UiElements.Base
{
    public partial class LcarsButtonBase
    {
        //Left button constants
        const int lButtonClick = 0x201;
        const int lButtonUp = 0x202;

        const int lButtonDouble = 0x203;
        //Right button constants
        const int rButtonClick = 0x204;
        const int rButtonUp = 0x205;

        const int rButtonDouble = 0x206;
        //Middle button constants
        const int mButtonClick = 0x207;
        const int mButtonUp = 0x208;

        const int mButtonDouble = 0x209;

        [System.Diagnostics.DebuggerStepThrough()]
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            try
            {
                if (!canClick)
                {
                    if (m.Msg == lButtonClick |
                        m.Msg == lButtonUp |
                        m.Msg == lButtonDouble |
                        m.Msg == rButtonClick |
                        m.Msg == rButtonUp |
                        m.Msg == rButtonDouble |
                        m.Msg == mButtonClick |
                        m.Msg == mButtonUp |
                        m.Msg == mButtonDouble)
                    {
                        return;
                    }
                }

                if (m.Msg == lButtonDouble) m.Msg = lButtonClick;

                base.WndProc(ref m);
            }
            catch (Exception ex)
            {
            }
        }

        protected override void ScaleControl(System.Drawing.SizeF factor, System.Windows.Forms.BoundsSpecified specified)
        {
            this.TextHeight = textHeight * (int)factor.Height;
            base.ScaleControl(factor, specified);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            if (isLit != flasherInvertLit)
            {
                e.Graphics.DrawImage(normalButton, 0, 0);
            }
            else
            {
                e.Graphics.DrawImage(unlitButton, 0, 0);
            }
            if (TextVisible)
            {
                DrawText(e.Graphics);
            }
        }
        
        protected override bool ScaleChildren
        {
            get { return false; }
        }

        /// <summary>
        /// Disposes of the current instance of LcarsButtonBase
        /// </summary>
        /// <param name="disposing">Boolean indicating whether to dispose of internal components</param>
        /// <remarks>Overriden to clean up the component list</remarks>
        protected override void Dispose(bool disposing)
        {
            //Needed to end the flashing thread and allow button to be disposed at all
            if (flashing)
            {
                flasher.Abort();
            }
            if (disposing)
            {
                if ((components != null))
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}
