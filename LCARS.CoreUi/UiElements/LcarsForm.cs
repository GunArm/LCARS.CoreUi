using LCARS.CoreUi.Helpers;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LCARS.CoreUi.UiElements
{
    /// <summary>
    /// A base Form class to handle common LCARS functions
    /// </summary>
    /// <remarks>
    /// Any form that inherits from this class will have default handlers for LCARS events, and be bound
    /// to the working area when maximized. This eliminates the previous requirement that LCARS apps 
    /// register their main window with LCARS to get position and size updates.
    /// 
    /// Supported events:
    ///  - Alerts initiated/ended
    ///  - Colors changed
    ///  - Beeping updated
    ///  - LCARS closing
    /// 
    /// Default handlers are supplied for color changing, beep updating, and LCARS closing.
    /// </remarks>
    public class LcarsForm : Form
    {
        #region " Windows API "
        [StructLayout(LayoutKind.Sequential)]
        private struct POINTAPI
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MINMAXINFO
        {
            public POINTAPI ptReserved;
            public POINTAPI ptMaxSize;
            public POINTAPI ptMaxPosition;
            public POINTAPI ptMinTrackSize;
            public POINTAPI ptMaxTrackSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MONITORINFO
        {
            public Int32 cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public Int32 dwFlags;
        }
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        private static extern Int32 MonitorFromWindow(Int32 hwnd, Int32 dwFlags);
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        private static extern Int32 MonitorFromPoint(POINTAPI pt, Int32 dwFlags);
        [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]

        private static extern int GetMonitorInfo(Int32 hMonitor, ref MONITORINFO lpmi);
        [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

        private static extern int RegisterWindowMessageA(string lpString);
        private const Int32 MONITOR_DEFAULTTONEAREST = 0x2;
        private const Int32 MONITOR_DEFAULTTOPRIMARY = 0x1;
        private const int WM_MINMAXINFO = 0x24;
        private int X32_MSG;
        #endregion

        #region " Events "
        /// <summary>
        /// Raised when an alert is initiated.
        /// </summary>
        /// <param name="AlertID">ID of the alert initiated</param>
        public event AlertInitiatedEventHandler AlertInitiated;
        public delegate void AlertInitiatedEventHandler(int AlertID);
        /// <summary>
        /// Raised when the current alert has ended
        /// </summary>
        /// <remarks>
        /// If an alert ends because another alert has replaced it, this event will not be raised, only a
        /// new <see cref="AlertInitiated">AlertInitiated</see> event
        /// </remarks>
        public event EventHandler AlertEnded;
        #endregion
        /// <summary>
        /// Handles LCARS messages and maximized bounds
        /// </summary>
        /// <param name="m">Window message</param>
        /// <remarks>
        /// Any messages of type WM_MINMAXINFO or LCARS_X32_MSG will be handled, and not passed to the
        /// default handler.
        /// </remarks>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == X32_MSG)
            {

                m.Result = (IntPtr)1;
                switch ((int)m.LParam)
                {
                    case 2:
                        OnColorsChange();
                        break;
                    case 3:
                        OnBeepingUpdate(bool.Parse(new SettingsStore("LCARS").Load("Application", "ButtonBeep", "TRUE")));
                        break;
                    case 11:
                        OnAlertInitiated((int)m.WParam);
                        break;
                    case 7:
                        OnAlertEnded();
                        break;
                    case 13:
                        OnLCARSClosing();
                        break;
                }

            }
            else if (m.Msg == WM_MINMAXINFO)
            {
                MINMAXINFO mmi = (MINMAXINFO) Marshal.PtrToStructure(m.LParam, typeof(MINMAXINFO));
                int monitor = MonitorFromWindow((int)Handle, MONITOR_DEFAULTTONEAREST);
                POINTAPI pt0 = new POINTAPI
                {
                    X = 0,
                    Y = 0
                };
                int primary = MonitorFromPoint(pt0, MONITOR_DEFAULTTOPRIMARY);
                if (monitor != 0 && primary != 0)
                {
                    MONITORINFO minfo = default(MONITORINFO);
                    MONITORINFO pminfo = default(MONITORINFO);
                    minfo.cbSize = Marshal.SizeOf(minfo);
                    pminfo.cbSize = Marshal.SizeOf(pminfo);
                    if (GetMonitorInfo(monitor, ref minfo) != 0 && GetMonitorInfo(primary, ref pminfo) !=0)
                    {
                        // This looks wrong, but Windows assumes the coordinates are for the primary
                        // monitor, and then adjusts them. We need to undo the adjustments so that
                        // the result is what we actually want.

                        // First, we account for the position change between the primary and actual monitors
                        mmi.ptMaxPosition.X = minfo.rcWork.Left - minfo.rcMonitor.Left;
                        mmi.ptMaxPosition.Y = minfo.rcWork.Top - minfo.rcMonitor.Top;

                        // For some reason setting the max track size bypasses the size adjustment.
                        mmi.ptMaxTrackSize.X = minfo.rcWork.Right - minfo.rcWork.Left;
                        mmi.ptMaxTrackSize.Y = minfo.rcWork.Bottom - minfo.rcWork.Top;

                        Marshal.StructureToPtr(mmi, m.LParam, true);
                        m.Result = (IntPtr)1;
                        return;
                    }
                }
                m.Result = (IntPtr) 0;
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        //Register LCARS_X32_MSG
        protected override void OnLoad(EventArgs e)
        {
            X32_MSG = RegisterWindowMessageA("LCARS_X32_MSG");
            base.OnLoad(e);
        }

        /// <summary>
        /// Called when LCARS updates the color scheme
        /// </summary>
        protected virtual void OnColorsChange()
        {
            Util.UpdateColors(this);
        }

        /// <summary>
        /// Called when beeping setting is updated
        /// </summary>
        /// <param name="beep">New beeping setting</param>
        protected virtual void OnBeepingUpdate(bool beep)
        {
            Util.SetBeeping(this, beep);
        }

        /// <summary>
        /// Called when an alert is initiated.
        /// </summary>
        /// <param name="alertID">ID of current alert</param>
        protected virtual void OnAlertInitiated(int alertID)
        {
            AlertInitiated?.Invoke(alertID);
        }

        /// <summary>
        /// Called when all alerts have ended
        /// </summary>
        protected virtual void OnAlertEnded()
        {
            AlertEnded?.Invoke(this, null);
        }

        /// <summary>
        /// Called when LCARS is closing. By default, will close this window.
        /// </summary>
        protected virtual void OnLCARSClosing()
        {
            Close();
        }
    }
}
