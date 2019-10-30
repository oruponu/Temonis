using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using static Temonis.MainWindow;

namespace Temonis
{
    internal static class Program
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

            var latestVersion = GetLatestVersionAsync().GetAwaiter().GetResult();
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            if (latestVersion != null && latestVersion > version)
            {
                var result = MessageBox.Show($"新しいバージョン {latestVersion} がリリースされました。\nダウンロードページを開きますか？", "Temonis", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = Properties.Resources.TemonisUri,
                        UseShellExecute = true
                    };
                    Process.Start(startInfo);
                    Environment.Exit(0);
                }
            }

            Settings.Load();

            var app = new App();
            app.InitializeComponent();
            app.Run();
        }

        public static void WriteFatalErrorLog(Exception ex)
        {
            var value = $"{DateTime.Now}\n";
            value += $"[Message]\n{ex.GetType().FullName}\n{ex.Message}\n";
            value += $"[StackTrace]\n{ex.StackTrace}\n\n";
            using var stream = new StreamWriter("FatalError.txt", true);
            stream.WriteLine(value);
        }

        private static async Task<Version> GetLatestVersionAsync()
        {
            string[] split = null;
            try
            {
                using var client = new HttpClient();
                var response = await client.GetAsync($"{Temonis.Properties.Resources.TemonisUri}version");
                if (!response.IsSuccessStatusCode)
                    return null;
                split = (await response.Content.ReadAsStringAsync()).Split('.');
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                WriteLog(ex);
            }

            if (split != null && split.Length == 4 && int.TryParse(split[0], out var major) && int.TryParse(split[1], out var minor) && int.TryParse(split[2], out var build) && int.TryParse(split[3], out var revision))
                return new Version(major, minor, build, revision);
            return null;
        }
    }
}
