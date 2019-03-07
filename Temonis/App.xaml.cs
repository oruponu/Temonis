using System;
using System.Windows;

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
            Configuration.Load();
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
