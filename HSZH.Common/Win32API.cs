using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace HSZH.Common
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LASTINPUTINFO
    {
        [MarshalAs(UnmanagedType.U4)]
        public int cbSize;
        [MarshalAs(UnmanagedType.U4)]
        public uint dwTime;
    }

    public struct COPYDATASTRUCT
    {
        public IntPtr dwData;
        public int cbData;
        public IntPtr lpData;
    }

    public enum AppBarMessages
    {
        New =
        0x00000000,
        Remove =
        0x00000001,
        QueryPos =
        0x00000002,
        SetPos =
        0x00000003,
        GetState =
        0x00000004,
        GetTaskBarPos =
        0x00000005,
        Activate =
        0x00000006,
        GetAutoHideBar =
        0x00000007,
        SetAutoHideBar =
        0x00000008,
        WindowPosChanged =
        0x00000009,
        SetState =
        0x0000000a
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct APPBARDATA
    {
        public UInt32 cbSize;
        public IntPtr hWnd;
        public UInt32 uCallbackMessage;
        public UInt32 uEdge;
        public Rectangle rc;
        public Int32 lParam;
    }

    public enum AppBarStates
    {
        AutoHide =
        0x00000001,
        AlwaysOnTop =
        0x00000002
    }

    /// <summary>
    /// 窗口与要获得句柄的窗口之间的关系。
    /// </summary>
    public enum GetWindowCmd : uint
    {
        /// <summary>
        /// 返回的句柄标识了在Z序最高端的相同类型的窗口。
        /// 如果指定窗口是最高端窗口，则该句柄标识了在Z序最高端的最高端窗口；
        /// 如果指定窗口是顶层窗口，则该句柄标识了在z序最高端的顶层窗口：
        /// 如果指定窗口是子窗口，则句柄标识了在Z序最高端的同属窗口。
        /// </summary>
        GW_HWNDFIRST = 0,
        /// <summary>
        /// 返回的句柄标识了在z序最低端的相同类型的窗口。
        /// 如果指定窗口是最高端窗口，则该柄标识了在z序最低端的最高端窗口：
        /// 如果指定窗口是顶层窗口，则该句柄标识了在z序最低端的顶层窗口；
        /// 如果指定窗口是子窗口，则句柄标识了在Z序最低端的同属窗口。
        /// </summary>
        GW_HWNDLAST = 1,
        /// <summary>
        /// 返回的句柄标识了在Z序中指定窗口下的相同类型的窗口。
        /// 如果指定窗口是最高端窗口，则该句柄标识了在指定窗口下的最高端窗口：
        /// 如果指定窗口是顶层窗口，则该句柄标识了在指定窗口下的顶层窗口；
        /// 如果指定窗口是子窗口，则句柄标识了在指定窗口下的同属窗口。
        /// </summary>
        GW_HWNDNEXT = 2,
        /// <summary>
        /// 返回的句柄标识了在Z序中指定窗口上的相同类型的窗口。
        /// 如果指定窗口是最高端窗口，则该句柄标识了在指定窗口上的最高端窗口；
        /// 如果指定窗口是顶层窗口，则该句柄标识了在指定窗口上的顶层窗口；
        /// 如果指定窗口是子窗口，则句柄标识了在指定窗口上的同属窗口。
        /// </summary>
        GW_HWNDPREV = 3,
        /// <summary>
        /// 返回的句柄标识了指定窗口的所有者窗口（如果存在）。
        /// GW_OWNER与GW_CHILD不是相对的参数，没有父窗口的含义，如果想得到父窗口请使用GetParent()。
        /// 例如：例如有时对话框的控件的GW_OWNER，是不存在的。
        /// </summary>
        GW_OWNER = 4,
        /// <summary>
        /// 如果指定窗口是父窗口，则获得的是在Tab序顶端的子窗口的句柄，否则为NULL。
        /// 函数仅检查指定父窗口的子窗口，不检查继承窗口。
        /// </summary>
        GW_CHILD = 5,
        /// <summary>
        /// （WindowsNT 5.0）返回的句柄标识了属于指定窗口的处于使能状态弹出式窗口（检索使用第一个由GW_HWNDNEXT 查找到的满足前述条件的窗口）；
        /// 如果无使能窗口，则获得的句柄与指定窗口相同。
        /// </summary>
        GW_ENABLEDPOPUP = 6
    }

    public class Win32API
    {
        private const int SW_HIDE = 0;
        private const int SW_RESTORE = 9;

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);
        [DllImport("user32.dll", EntryPoint = "GetKeyboardState")]
        public static extern int GetKeyboardState(byte[] pbKeyState);
        [DllImport("user32.dll")]
        public static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        [DllImport("User32.dll")]
        public static extern void keybd_event(byte bVK, byte bScan, Int32 dwFlags, int dwExtraInfo);
        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll")]
        public static extern int ShowWindow(int nhwnd, int ncmdShow);
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindow(IntPtr hWnd, GetWindowCmd uCmd);
        [DllImport("user32.dll", EntryPoint = "GetWindowText")]
        public static extern int GetWindowText(
            IntPtr hWnd,
            StringBuilder lpString,
            int nMaxCount
        );
        [DllImport("user32.dll", EntryPoint = "GetClassName")]
        public static extern int GetClassName(
            IntPtr hWnd,
            StringBuilder lpString,
            int nMaxCont
        );
        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hwnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);
        [DllImport("shell32.dll")]
        public static extern UInt32 SHAppBarMessage(UInt32 dwMessage, ref APPBARDATA pData);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WritePrivateProfileSection(string lpAppName, string lpString, string lpFileName);
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string defVal, Byte[] retVal, int size, string filePath);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hwnd, int x, int y, int cx, int cy, bool repaint);
        [DllImport("User32.dll")]
        public static extern IntPtr SetParent(IntPtr hChild, IntPtr hParent);
        [DllImport("User32.dll")]
        public static extern long SetWindowLong(IntPtr hWnd, int nIndex, long dwNewLong);
        [DllImport("User32.dll")]
        public static extern long GetWindowLong(IntPtr hWnd, int nIndex);
        /// <summary>
        /// Set the Taskbar State option
        /// </summary>
        /// <param name="option">AppBarState to activate</param>
        public static void SetTaskbarState(AppBarStates option)
        {
            APPBARDATA msgData = new APPBARDATA();
            msgData.cbSize = (UInt32)Marshal.SizeOf(msgData);
            msgData.hWnd = (IntPtr)FindWindow("System_TrayWnd", null);
            msgData.lParam = (Int32)(option);
            SHAppBarMessage((UInt32)AppBarMessages.SetState, ref msgData);
        }

        /// <summary>
        /// Gets the current Taskbar state
        /// </summary>
        /// <returns>current Taskbar state</returns>
        public static AppBarStates GetTaskbarState()
        {
            APPBARDATA msgData = new APPBARDATA();
            msgData.cbSize = (UInt32)Marshal.SizeOf(msgData);
            msgData.hWnd = (IntPtr)FindWindow("System_TrayWnd", null);
            return (AppBarStates)SHAppBarMessage((UInt32)AppBarMessages.GetState, ref msgData);
        }

        public static void AddKeyBoardINput(byte input)
        {
            keybd_event(input, 0, 0, 0);
            keybd_event(input, 0, 0x02, 0);
        }

        public static void ShowOrHindeShell(int flag)
        {
            IntPtr hStar = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "Button", null);
            switch (flag)
            {
                case 0:
                    ShowWindow(FindWindow("Shell_TrayWnd", null), SW_HIDE);
                    if (hStar != IntPtr.Zero)
                        ShowWindow(hStar, 0);
                    break;
                case 1:
                    ShowWindow(FindWindow("Shell_TrayWnd", null), SW_RESTORE);
                    if (hStar != IntPtr.Zero)
                        ShowWindow(hStar, 1);
                    break;
            }
        }

        public static bool AppIsRun(string appName)
        {
            Process[] process = Process.GetProcessesByName(appName);
            if (process.Length < 1)
            {
                return true;
            }
            int myPprocesid = Process.GetCurrentProcess().Id;
            foreach (Process obj in process)
            {
                if (obj.Id != myPprocesid)
                {
                    obj.Kill();
                }
            }
            return true;
        }

        public static bool ApplicationIsRun(string appName)
        {
            Process[] process = Process.GetProcessesByName(appName);
            if (process.Length < 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void CloseProcess(string appName)
        {
            Process[] process = Process.GetProcessesByName(appName);
            if (process.Length > 0)
            {
                foreach (Process obj in process)
                {
                    obj.Kill();
                }
            }
        }

        public enum PenStyle : int
        {
            PS_SOLID = 0, //The pen is solid.
            PS_DASH = 1, //The pen is dashed.
            PS_DOT = 2, //The pen is dotted.
            PS_DASHDOT = 3, //The pen has alternating dashes and dots.
            PS_DASHDOTDOT = 4, //The pen has alternating dashes and double dots.
            PS_NULL = 5, //The pen is invisible.
            PS_INSIDEFRAME = 6,// Normally when the edge is drawn, it’s centred on the outer edge meaning that half the width of the pen is drawn
            // outside the shape’s edge, half is inside the shape’s edge. When PS_INSIDEFRAME is specified the edge is drawn 
            //completely inside the outer edge of the shape.
            PS_USERSTYLE = 7,
            PS_ALTERNATE = 8,
            PS_STYLE_MASK = 0x0000000F,

            PS_ENDCAP_ROUND = 0x00000000,
            PS_ENDCAP_SQUARE = 0x00000100,
            PS_ENDCAP_FLAT = 0x00000200,
            PS_ENDCAP_MASK = 0x00000F00,

            PS_JOIN_ROUND = 0x00000000,
            PS_JOIN_BEVEL = 0x00001000,
            PS_JOIN_MITER = 0x00002000,
            PS_JOIN_MASK = 0x0000F000,

            PS_COSMETIC = 0x00000000,
            PS_GEOMETRIC = 0x00010000,
            PS_TYPE_MASK = 0x000F0000
        };

        [DllImport("gdi32.dll")]
        public static extern bool MoveToEx(IntPtr hdc, int X, int Y, IntPtr lpPoint);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreatePen(PenStyle fnPenStyle, int nWidth, uint crColor);

        [DllImport("gdi32.dll")]
        public static extern bool Rectangle(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport("gdi32.dll")]
        public static extern bool LineTo(IntPtr hdc, int nXEnd, int nYEnd);

        [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr o);

        //public static bool GetData(Message m, ref byte[] rData)
        //{
        //    if (m.Msg == 0x4a)
        //    {
        //        Type cls = new COPYDATASTRUCT().GetType();
        //        COPYDATASTRUCT lParam = (COPYDATASTRUCT)m.GetLParam(cls);
        //        uint dwData = (uint)((int)lParam.dwData);
        //        rData = new byte[lParam.cbData];
        //        Marshal.Copy(lParam.lpData, rData, 0, rData.Length);
        //        return true;
        //    }
        //    return false;
        //}

        public static int SendData(IntPtr winHandle, byte[] data)
        {
            COPYDATASTRUCT lParam = new COPYDATASTRUCT
            {
                cbData = data.Length,
                lpData = Marshal.AllocHGlobal(data.Length)
            };
            Marshal.Copy(data, 0, lParam.lpData, data.Length);
            return SendMessage(winHandle, 0x4a, 0, ref lParam);
        }

        public static int DoSendData(object data, IntPtr intPtr)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, data);
                byte[] buffer = stream.GetBuffer();
                return SendData(intPtr, buffer);
            }
        }
    }
}
