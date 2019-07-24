using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
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
        public static readonly HttpClient HttpClient = new HttpClient();
        private const int TimeResetInterval = 60;
        private const int EqInfoInterval = 10;
        private readonly DispatcherTimer _timer = new DispatcherTimer();
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

        public new static DataContext DataContext { get; } = new DataContext();

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
                await EqInfo.UpdateAsync();
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
                LatestTime = await RequestLatestTimeAsync(LatestTime.AddSeconds(-DataContext.Kyoshin.SliderValue)).ConfigureAwait(false);
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
            var json = default(Root);
            try
            {
                var response = await HttpClient.GetAsync(Properties.Resources.LatestTimeUri).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                    return dateTime;
                using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    json = (Root)new DataContractJsonSerializer(typeof(Root)).ReadObject(stream);
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException || ex is SerializationException)
            {
                WriteLog(ex);
            }

            return json == default(Root) ? dateTime : DateTime.Parse(json.LatestTime);
        }

        /// <summary>
        /// 強震モニタの時刻を設定します。
        /// </summary>
        /// <returns></returns>
        private static async Task SetLatestTimeAsync()
        {
            if (_timeResetCount >= TimeResetInterval)
            {
                LatestTime = await RequestLatestTimeAsync(LatestTime.AddSeconds(-DataContext.Kyoshin.SliderValue)).ConfigureAwait(false);
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
            if (!Settings.RootClass.Behavior.ForceActive)
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
        /// <param name="buffer"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public static byte[] DecompressResource(byte[] buffer, int bufferSize)
        {
            var bytes = new byte[bufferSize];
            using (var originalMemoryStream = new MemoryStream(buffer))
            using (var zipStream = new GZipStream(originalMemoryStream, CompressionMode.Decompress))
            using (var decompressedMemoryStream = new MemoryStream())
            {
                while (true)
                {
                    var size = zipStream.Read(bytes, 0, bytes.Length);
                    if (size == 0)
                        break;
                    decompressedMemoryStream.Write(bytes, 0, size);
                }
            }

            return bytes;
        }

        [Conditional("DEBUG")]
        public static void WriteLog(string str)
        {
            var value = $"{DateTime.Now}\n";
            value += $"[Log]\n{str}\n\n";
            using (var stream = new StreamWriter("Log.txt", true))
                stream.WriteLine(value);
        }

        [Conditional("DEBUG")]
        public static void WriteLog(Exception ex)
        {
            var value = $"{DateTime.Now}\n";
            value += $"[Message]\n{ex.Message}\n";
            value += $"[StackTrace]\n{ex.StackTrace}\n\n";
            using (var stream = new StreamWriter("Exception.txt", true))
                stream.WriteLine(value);
        }

        /// <summary>
        /// JSONクラス
        /// </summary>
        [DataContract]
        public class Root
        {
            [DataMember(Name = "security")]
            public SecurityClass Security { get; set; }

            [DataMember(Name = "latest_time")]
            public string LatestTime { get; set; }

            [DataMember(Name = "request_time")]
            public string RequestTime { get; set; }

            [DataMember(Name = "result")]
            public ResultClass Result { get; set; }

            [DataContract]
            public class SecurityClass
            {
                [DataMember(Name = "realm")]
                public string Realm { get; set; }

                [DataMember(Name = "hash")]
                public string Hash { get; set; }
            }

            [DataContract]
            public class ResultClass
            {
                [DataMember(Name = "status")]
                public string Status { get; set; }

                [DataMember(Name = "message")]
                public string Message { get; set; }
            }
        }
    }

    public enum Level
    {
        White,
        Yellow,
        Red
    }
}
