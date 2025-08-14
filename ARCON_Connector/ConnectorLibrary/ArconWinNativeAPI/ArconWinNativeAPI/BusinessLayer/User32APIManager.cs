using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static ArconWinNativeAPI.User32;

namespace ArconWinNativeAPI
{
    public class User32APIManager
    {
        #region Variables
        public static IntPtr _pWH_CALLWNDPROCRET = IntPtr.Zero;
        private static uint m_event = 0;
        private  WinEventDelegate m_delegate = null;
        private  IntPtr m_foregroundHwnd = IntPtr.Zero;
        public  event SystemEventEventHandler SystemEventHandler;

        #endregion

        #region Delegates
        public delegate bool EnumWindowsProc(IntPtr hWnd, int lParam);
        public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd,
            int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
        public delegate Int32 HookProcedureDelegate(Int32 iCode, IntPtr pWParam, IntPtr pLParam);
        public delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);
        public delegate void SystemEventEventHandler(IntPtr hWinEventHook, uint eventType,IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
        #endregion

        #region Inbuilt Methods
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref User32.Rect rect);
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr GetShellWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);
        [DllImport("user32.dll")]
        public static extern bool UnhookWinEvent(IntPtr hWinEventHook);
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowsHookEx(User32.HookType hooktype, HookProcedureDelegate callback, IntPtr hMod, UInt32 dwThreadId);
        [DllImport("user32.dll")]
        public static extern Int32 CallNextHookEx(IntPtr hhk, Int32 nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr UnhookWindowsHookEx(IntPtr hhk);
        //[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //public static extern IntPtr SendMessage(IntPtr hwnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref User32.COPYDATASTRUCT lParam);
        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        //[DllImport("user32.dll")]
        //public static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowText(IntPtr hWnd, string Text);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
        [DllImport("user32")]
        public static extern bool AnimateWindow(IntPtr hwnd, int time, int flags);
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint RegisterWindowMessage(string lpString);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SendNotifyMessage(IntPtr hWnd, uint msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern void Mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        [DllImport("user32.dll")]
        public static extern bool EnumThreadWindows(uint dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point point);

        [DllImport("User32.dll")]
        public static extern bool ShowWindow(IntPtr handle, int nCmdShow);

        [DllImport("User32.dll")]
        public static extern bool IsIconic(IntPtr handle);

        [DllImport("User32.dll")]
        public static extern bool BlockInput(bool fBlockIt);

        #endregion

        #region Custom Methods
        public static Process GetProcessDetailByHandle(IntPtr hwnd)
        {
            //IntPtr hwnd = GetForegroundWindow();
            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);
            Process p = Process.GetProcessById((int)pid);
            //p.MainModule.FileName.Dump();
            return p;
        }

        public static bool IsProcessWindowActive(IntPtr hwnd)
        {
            return hwnd.Equals(GetForegroundWindow());
        }

        public static IDictionary<IntPtr, string> GetOpenWindowsFromPID(List<int> lstProcessId)
        {
            IntPtr hShellWindow = GetShellWindow();
            Dictionary<IntPtr, string> dictWindows = new Dictionary<IntPtr, string>();

            EnumWindows(delegate (IntPtr hWnd, int lParam)
            {
                if (hWnd == hShellWindow) return true;
                if (!IsWindowVisible(hWnd)) return true;

                int length = GetWindowTextLength(hWnd);
                if (length == 0) return true;

                uint windowPid;
                GetWindowThreadProcessId(hWnd, out windowPid);

                //if (windowPid != lstProcessID) return true;

                if (!lstProcessId.Where(proc => proc == windowPid).Any()) return true;
                StringBuilder stringBuilder = new StringBuilder(length);
                GetWindowText(hWnd, stringBuilder, length + 1);
                IntPtr MWhandle = FindWindow(null, stringBuilder.ToString());
                IntPtr CWhandle = GetForegroundWindow();
                if (MWhandle != CWhandle)
                    return true;
                dictWindows.Add(hWnd, stringBuilder.ToString());
                return true;
            }, 0);

            return dictWindows;
        }

        public static string GetWindowText(IntPtr handle)
        {
            int length = GetWindowTextLength(handle);
            StringBuilder sb = new StringBuilder(length + 1);
            GetWindowText(handle, sb, sb.Capacity);
            return sb.ToString();
        }
        //public static string GetActiveWindow()
        //{
        //    IntPtr activeAppWindow = IntPtr.Zero;
        //    IntPtr MWhandle = FindWindow(null, "Server Properties - DSK078");
        //    IntPtr CWhandle = GetForegroundWindow();
        //    int length = GetWindowTextLength(MWhandle);
        //    StringBuilder stringBuilder = new StringBuilder(length);
        //    GetWindowText(MWhandle, stringBuilder, length + 1);

        //    length = GetWindowTextLength(CWhandle);
        //    StringBuilder stringBuilder1 = new StringBuilder(length);
        //    GetWindowText(CWhandle, stringBuilder1, length + 1);

        //    if (MWhandle != CWhandle)
        //        return string.Empty;
        //    return stringBuilder.ToString() + " , " + stringBuilder1.ToString();
        //}

        public static void SendStringMessageToApplication(IntPtr hwnd, IntPtr wParam, string message)
        {
            try
            {
                User32.COPYDATASTRUCT cds = new User32.COPYDATASTRUCT();
                cds.cbData = message.Length * 2; ;
                cds.lpData = message;
                SendMessage(hwnd, Constants.WM_COPYDATA, wParam, ref cds);
            }
            catch (Exception ex)
            {
                //Log Error
                throw ex;
            }
        }

        public void SystemEvent(uint SystemEvent, WinEventDelegate m_delegate)
        {
            m_event = SystemEvent;
            //m_delegate = new WinEventDelegate(WinEventProc);
            try
            {
                SetWinEventHook(m_event, m_event, IntPtr.Zero, m_delegate, Convert.ToUInt32(0), Convert.ToUInt32(0), Constants.WINEVENT_OUTOFCONTEXT);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public  void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if ((SystemEventHandler != null) && (SystemEventHandler.GetInvocationList().Length > 0))
            {
                m_foregroundHwnd = hwnd;
                SystemEventHandler?.Invoke(hWinEventHook, eventType, hwnd, idObject, idChild, dwEventThread, dwmsEventTime);
            }
        }

        public static IEnumerable<IntPtr> WindowHandles(Process process)
        {
            var handles = new List<IntPtr>();
            foreach (ProcessThread thread in process.Threads)
                EnumThreadWindows((uint)thread.Id, (hWnd, lParam) => { handles.Add(hWnd); return true; }, IntPtr.Zero);
            return handles;
        }
        #endregion

    }
}
