using System;
using System.Runtime.InteropServices;

namespace LCARS.CoreUi.UiElements
{
    partial class LcarsForm
    {
        private const int WM_MINMAXINFO = 0x24;

        enum MonitorOptions : uint
        {
            DefaultNull = 0x00000000,
            DefaultToPrimary = 0x00000001,
            DefaultToNearest = 0x00000002
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct PointStruct
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MINMAXINFO
        {
            public PointStruct Reserved;
            public PointStruct MaxSize;
            public PointStruct MaxPosition;
            public PointStruct MinTrackSize;
            public PointStruct MaxTrackSize;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RectStruct
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MONITORINFO
        {
            public int Size;
            public RectStruct Monitor;
            public RectStruct WorkArea;
            public uint Flags;
        }

        [DllImport("user32.dll")]
        static extern IntPtr MonitorFromWindow(IntPtr hwnd, MonitorOptions dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr MonitorFromPoint(PointStruct pt, MonitorOptions dwFlags);

        [DllImport("user32.dll")]
        static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        static extern uint RegisterWindowMessageA(string lpString);
    }
}
