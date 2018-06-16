using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Temonis
{
    internal static class NativeMethods
    {
        [DllImport("Winmm.dll", CharSet = CharSet.Unicode)]
        internal static extern int mciSendString(string lpszCommand, StringBuilder lpszReturnString, int cchReturn, IntPtr hwndCallback);

        [DllImport("Winmm.dll", CharSet = CharSet.Unicode)]
        internal static extern uint mciGetDeviceID(string lpszDevice);
    }
}
