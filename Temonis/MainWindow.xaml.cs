using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Temonis
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly HttpClient HttpClient = new();
        private const int TimeResetInterval = 60;
        private const int EqInfoInterval = 60;
        private readonly DispatcherTimer _timer = new();
        private static int _timeResetCount = TimeResetInterval;
        private static int _eqInfoCount = EqInfoInterval;
        private static int _retryCount;
        private static DateTime _latestTime = DateTime.Now;

        public static MainWindow Instance { get; private set; }

        public static DateTime LatestTime
        {
            get => _latestTime.AddSeconds(DataContext.Kyoshin.SliderValue);
            private set => _latestTime = value;
        }

        public new static DataContext DataContext { get; } = new();

        public MainWindow()
        {
            Instance = this;
            ((FrameworkElement)this).DataContext = DataContext;

            _timer.Interval = TimeSpan.FromSeconds(1.0);
            _timer.Tick += Timer;
            _timer.Start();
            Timer(null, EventArgs.Empty);
        }

        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var decorator = (Decorator)VisualTreeHelper.GetChild((DataGrid)sender, 0);
            var scrollViewer = (ScrollViewer)decorator.Child;
            if (e.Delta < 0)
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + 1.0);
            else
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - 1.0);
        }

        private static async void Timer(object sender, EventArgs e)
        {
            await SetLatestTimeAsync();

            if (_eqInfoCount >= EqInfoInterval)
            {
                Sound.PlayDummy();
                await Jmaxml.RequestFeedAsync();
                _eqInfoCount = 0;
            }

            await Eew.UpdateAsync();

            var isSucceeded = await Kyoshin.UpdateAsync();
            if (isSucceeded)
            {
                _retryCount = 0;
            }
            else
            {
                DataContext.Kyoshin.MaxIntString = "";
                DataContext.Kyoshin.Prefecture = "";
                LatestTime = await RequestLatestTimeAsync(LatestTime.AddSeconds(-DataContext.Kyoshin.SliderValue));
                _retryCount++;
            }

            DataContext.LatestTimeString = _retryCount < 10 ? LatestTime.ToString("yyyy/MM/dd HH:mm:ss") : "接続しています...";

            _timeResetCount++;
            _eqInfoCount++;
        }

        /// <summary>
        /// 強震モニタから時刻を取得します。
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private static async Task<DateTime> RequestLatestTimeAsync(DateTime dateTime)
        {
            var json = default(Json);
            try
            {
                var requestUri = new Uri(Properties.Resources.LatestTimeUri);
                var response = await HttpClient.GetAsync(requestUri);
                if (!response.IsSuccessStatusCode)
                    return dateTime;
                await using var stream = await response.Content.ReadAsStreamAsync();
                json = await JsonSerializer.DeserializeAsync<Json>(stream);
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                WriteLog(ex);
            }

            return json == default(Json) ? dateTime : DateTime.Parse(json.LatestTime);
        }

        /// <summary>
        /// 強震モニタの時刻を設定します。
        /// </summary>
        /// <returns></returns>
        private static async Task SetLatestTimeAsync()
        {
            if (_timeResetCount >= TimeResetInterval)
            {
                LatestTime = await RequestLatestTimeAsync(LatestTime.AddSeconds(-DataContext.Kyoshin.SliderValue));
                _timeResetCount = 0;
            }
            else
            {
                LatestTime = LatestTime.AddSeconds(-DataContext.Kyoshin.SliderValue).AddSeconds(1);
            }
        }

        /// <summary>
        /// ウィンドウをアクティブにします。
        /// </summary>
        public static void SetActive()
        {
            if (!Settings.JsonClass.Behavior.ActivateWindow)
                return;
            if (Instance.WindowState == WindowState.Minimized)
                Instance.WindowState = WindowState.Normal;
            Instance.Activate();
            Instance.Topmost = true;
            Instance.Topmost = false;
            Instance.Focus();
        }

        /// <summary>
        /// GZIP圧縮されたリソースファイルを展開します。
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public static byte[] DecompressResource(Stream stream, int bufferSize)
        {
            var bytes = new byte[bufferSize];
            using var zipStream = new GZipStream(stream, CompressionMode.Decompress);
            using var decompressedMemoryStream = new MemoryStream();
            while (true)
            {
                var size = zipStream.Read(bytes, 0, bytes.Length);
                if (size == 0)
                    break;
                decompressedMemoryStream.Write(bytes, 0, size);
            }

            return bytes;
        }

        [Conditional("DEBUG")]
        public static void WriteLog(string str)
        {
            var value = DateTime.Now + "\n";
            value += $"[Log]\n{str}\n\n";
            File.AppendAllText("Log.txt", value);
        }

        [Conditional("DEBUG")]
        public static void WriteLog(Exception ex)
        {
            var value = DateTime.Now + "\n";
            value += $"[Message]\n{ex.GetType().FullName}: {ex.Message}\n";
            value += $"[StackTrace]\n{ex.StackTrace}\n\n";
            File.AppendAllText("Exception.txt", value);
        }

        private class Json
        {
            [JsonPropertyName("latest_time")]
            public string LatestTime { get; init; }
        }
    }

    public enum Level
    {
        White,
        Yellow,
        Red
    }
}
