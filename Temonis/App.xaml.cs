using System.Windows;
using System.Windows.Threading;

namespace Temonis;

/// <summary>
/// App.xaml の相互作用ロジック
/// </summary>
public partial class App : Application
{
    private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        Program.WriteFatalErrorLog(e.Exception);
        e.Handled = true;
    }
}
