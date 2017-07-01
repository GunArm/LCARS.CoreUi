using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LCARS.CoreUi.UiElements
{
    public partial class LcarsForm
    {
        private FormWindowState lastWindowState;
        private FormBorderStyle lastBorderStyle;
        private bool lastTopMost;
        private Rectangle lastBounds;

        public bool FullScreen
        {
            get { return fullScreen; }
            set
            {
                if (value == fullScreen) return;
                if (value)
                {
                    // remember state
                    lastWindowState = WindowState;
                    lastBorderStyle = FormBorderStyle;
                    lastTopMost = TopMost;
                    lastBounds = Bounds;

                    // go fullscreen
                    WindowState = FormWindowState.Maximized;
                    FormBorderStyle = FormBorderStyle.None;
                    TopMost = true;
                    SetFullScreen(Handle);
                }
                else
                {
                    // restore state
                    WindowState = lastWindowState;
                    FormBorderStyle = lastBorderStyle;
                    TopMost = lastTopMost;
                    Bounds = lastBounds;
                }
                fullScreen = value;
            }
        }
        private bool fullScreen = false;

        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        private static extern int GetSystemMetrics(int which);

        [DllImport("user32.dll")]
        private static extern void SetWindowPos(IntPtr formHandle, IntPtr insertAfter, int X, int Y, int width, int height, uint flags);

        private const int SWP_SHOWWINDOW = 64; // 0x0040

        private static void SetFullScreen(IntPtr hwnd)
        {
            SetWindowPos(
                hwnd,
                IntPtr.Zero,
                0,
                0,
                GetSystemMetrics(0),
                GetSystemMetrics(1),
                SWP_SHOWWINDOW);
        }
    }
}

