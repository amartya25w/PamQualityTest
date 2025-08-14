using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArconConnector.Forms
{
    public class Win32Window : IWin32Window
    {
        public IntPtr Handle { get; private set; }
        public Win32Window(IntPtr hwnd) { Handle = hwnd; }
    }
}
