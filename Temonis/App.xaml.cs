using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Temonis
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        public static void Main()
        {
            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                WriteFatalErrorLog(e.Exception);
                e.SetObserved();
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                var ex = e.ExceptionObject as Exception;
                WriteFatalErrorLog(ex);
                Environment.Exit(-1);
            };

            Configuration.Load();
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            WriteFatalErrorLog(e.Exception);
            e.Handled = true;
        }

        private static void WriteFatalErrorLog(Exception ex)
        {
            var value = $"{DateTime.Now}\n";
            value += $"[Message]\n{ex.GetType().FullName}\n{ex.Message}\n";
            value += $"[StackTrace]\n{ex.StackTrace}\n\n";
            using (var stream = new StreamWriter("FatalError.txt", true))
                stream.WriteLine(value);
        }
    }
}
