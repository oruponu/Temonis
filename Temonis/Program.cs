using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Temonis
{
    internal static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }

    internal static class NativeMethods
    {

        [DllImport("winmm.dll", CharSet = CharSet.Unicode)]
        internal static extern int mciSendString(string lpszCommand, StringBuilder lpszReturnString,
            int cchReturn, IntPtr hwndCallback);
    }
}
