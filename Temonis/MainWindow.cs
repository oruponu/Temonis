using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Temonis.Resources;
using Timer = System.Timers.Timer;

namespace Temonis
{
    public partial class MainWindow : Form
    {
        public static readonly HttpClient HttpClient = new HttpClient();
        private const int TimeResetInterval = 60;
        private const int EqInfoInterval = 10;
        private readonly Timer _timer = new Timer();
        private static Kyoshin _kyoshin;
        private static Eew _eew;
        private static EqInfo _eqInfo;
        private static StateSet _stateSet;
        private static int _timeResetCount = TimeResetInterval;
        private static int _eqInfoCount = EqInfoInterval;
        private static int _retryCount;

        public static MainWindow Instance { get; private set; }

        public static DateTime LatestTime { get; private set; } = DateTime.Now;

        public MainWindow()
        {
            InitializeComponent();

            Instance = this;
            _kyoshin = new Kyoshin();
            _eew = new Eew();
            _eqInfo = new EqInfo();
            _stateSet = new StateSet();
            Configuration.LoadSettings();

            // フォームを初期化
            SuspendLayout();
            PictureBox_KyoshinMap.Image = Properties.Resources.BaseMap;
            ComboBox_MapType.SelectedIndex = 0;    // インデックスを「リアルタイム震度」に設定
            InitializeLabel();
            SetFormColor();
            SetFormFont();
            ActiveControl = label_KyoshinLatestTime;
            ResumeLayout(false);

            // タイマーを設定
            _timer.Interval = 1000;
            _timer.SynchronizingObject = this;
            _timer.Elapsed += Timer;
            _timer.Start();
            Timer(null, EventArgs.Empty);

            // 電源モード変更イベントを登録
            SystemEvents.PowerModeChanged += PowerModeChanged;
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 電源モード変更イベントを解除
            SystemEvents.PowerModeChanged -= PowerModeChanged;
        }

        private void DataGridView_EqInfoIntensity_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var intensity = (string)DataGridView_EqInfoIntensity[0, e.RowIndex].Value;
            if (intensity == "")
            {
                var i = e.RowIndex;
                while (e.RowIndex > 0)
                {
                    intensity = (string)DataGridView_EqInfoIntensity[0, --i].Value;
                    if (intensity != "") break;
                }
            }

            using (var pen = new Pen(Util.EqInfo.ColorMap[intensity]))
            {
                e.Graphics.DrawLine(pen, 1, e.RowBounds.Top, 1, e.RowBounds.Top + e.RowBounds.Height);
            }
        }

        /// <summary>
        /// ラベルを初期化します。
        /// </summary>
        private void InitializeLabel()
        {
            Label_KyoshinMaxInt.Text = "";
            Label_KyoshinMaxIntDetail.Text = "";
            Label_KyoshinPrefecture.Text = "";
            Label_EewDateTimeHeader.Visible = false;
            Label_EewEpicenterHeader.Visible = false;
            Label_EewDepthHeader.Visible = false;
            Label_EewMagnitudeHeader.Visible = false;
            Label_EewIntensityHeader.Visible = false;
            Label_EewDateTime.Text = "";
            Label_EewEpicenter.Text = "";
            Label_EewDepth.Text = "";
            Label_EewMagnitude.Text = "";
            Label_EewIntensity.Text = "";
            Label_EqInfoDateTime.Text = "";
            Label_EqInfoEpicenter.Text = "";
            Label_EqInfoDepth.Text = "";
            Label_EqInfoMagnitude.Text = "";
            Label_EqInfoComment.Text = "";
        }

        /// <summary>
        /// フォームの色を設定します。
        /// </summary>
        private void SetFormColor()
        {
            BackColor = Util.Black;
            GroupBox_Kyoshin.ForeColor = Util.White;
            GroupBox_Eew.ForeColor = Util.White;
            GroupBox_EqInfo.ForeColor = Util.White;
            DataGridView_EqInfoIntensity.BackgroundColor = Util.Black;
            DataGridView_EqInfoIntensity.DefaultCellStyle.BackColor = Util.Black;
            DataGridView_EqInfoIntensity.DefaultCellStyle.SelectionBackColor = Util.Black;
            DataGridView_EqInfoIntensity.DefaultCellStyle.SelectionForeColor = Util.White;
        }

        /// <summary>
        /// フォントを設定します。
        /// </summary>
        private void SetFormFont()
        {
            using (var fontCollection = new InstalledFontCollection())
            {
                var fontFamilies = fontCollection.Families;
                foreach (var fontFamily in fontFamilies)
                {
                    if (fontFamily.Name == "Yu Gothic UI")
                    {
                        label_KyoshinLatestTime.Font = new Font(fontFamily.Name, label_KyoshinLatestTime.Font.Size);
                        GroupBox_Kyoshin.Font = new Font(fontFamily.Name, GroupBox_Kyoshin.Font.Size);
                        label_KyoshinMaxIntHeader.Font = new Font(fontFamily.Name, label_KyoshinMaxIntHeader.Font.Size);
                        Label_KyoshinMaxInt.Font = new Font(fontFamily.Name, Label_KyoshinMaxInt.Font.Size);
                        Label_KyoshinMaxInt.Location = new Point(Label_KyoshinMaxInt.Location.X + 1, Label_KyoshinMaxInt.Location.Y);
                        Label_KyoshinMaxIntDetail.Font = new Font(fontFamily.Name, Label_KyoshinMaxIntDetail.Font.Size);
                        Label_KyoshinPrefecture.Font = new Font(fontFamily.Name, Label_KyoshinPrefecture.Font.Size);
                        GroupBox_Eew.Font = new Font(fontFamily.Name, GroupBox_Eew.Font.Size);
                        Label_EewMessage.Font = new Font(fontFamily.Name, Label_EewMessage.Font.Size);
                        Label_EewDateTimeHeader.Font = new Font(fontFamily.Name, Label_EewDateTimeHeader.Font.Size);
                        Label_EewDateTime.Font = new Font(fontFamily.Name, Label_EewDateTime.Font.Size);
                        Label_EewEpicenterHeader.Font = new Font(fontFamily.Name, Label_EewEpicenterHeader.Font.Size);
                        Label_EewEpicenter.Font = new Font(fontFamily.Name, Label_EewEpicenter.Font.Size);
                        Label_EewDepthHeader.Font = new Font(fontFamily.Name, Label_EewDepthHeader.Font.Size);
                        Label_EewDepth.Font = new Font(fontFamily.Name, Label_EewDepth.Font.Size);
                        Label_EewMagnitudeHeader.Font = new Font(fontFamily.Name, Label_EewMagnitudeHeader.Font.Size);
                        Label_EewMagnitude.Font = new Font(fontFamily.Name, Label_EewMagnitude.Font.Size);
                        Label_EewIntensityHeader.Font = new Font(fontFamily.Name, Label_EewIntensityHeader.Font.Size);
                        Label_EewIntensity.Font = new Font(fontFamily.Name, Label_EewIntensity.Font.Size);
                        GroupBox_EqInfo.Font = new Font(fontFamily.Name, GroupBox_EqInfo.Font.Size);
                        label_EqInfoDateTimeHeader.Font = new Font(fontFamily.Name, label_EqInfoDateTimeHeader.Font.Size);
                        Label_EqInfoDateTime.Font = new Font(fontFamily.Name, Label_EqInfoDateTime.Font.Size);
                        label_EqInfoEpicenterHeader.Font = new Font(fontFamily.Name, label_EqInfoEpicenterHeader.Font.Size);
                        Label_EqInfoEpicenter.Font = new Font(fontFamily.Name, Label_EqInfoEpicenter.Font.Size);
                        label_EqInfoDepthHeader.Font = new Font(fontFamily.Name, label_EqInfoDepthHeader.Font.Size);
                        Label_EqInfoDepth.Font = new Font(fontFamily.Name, Label_EqInfoDepth.Font.Size);
                        label_EqInfoMagnitudeHeader.Font = new Font(fontFamily.Name, label_EqInfoMagnitudeHeader.Font.Size);
                        Label_EqInfoMagnitude.Font = new Font(fontFamily.Name, Label_EqInfoMagnitude.Font.Size);
                        Label_EqInfoComment.Font = new Font(fontFamily.Name, Label_EqInfoComment.Font.Size);

                        fontFamily.Dispose();
                        break;
                    }

                    fontFamily.Dispose();
                }
            }
        }
        
        /// <summary>
        /// メインタイマー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Timer(object sender, EventArgs e)
        {
            SuspendLayout();

            await SetLatestTimeAsync();

            if (_eqInfoCount >= EqInfoInterval)
            {
                await _eqInfo.UpdateAsync();
                _eqInfoCount = 0;
            }

            await _eew.UpdateAsync();
            
            var successed = await _kyoshin.UpdateAsync();
            if (successed)
            {
                _retryCount = 0;
            }
            else
            {
                Label_KyoshinMaxInt.Text = "";
                Label_KyoshinPrefecture.Text = "";
                LatestTime = await RequestLatestTimeAsync(LatestTime).ConfigureAwait(false);
                _retryCount++;
            }

            label_KyoshinLatestTime.Text = _retryCount < 10 ? LatestTime.ToString("yyyy/MM/dd HH:mm:ss") : "接続しています...";
            await _stateSet.UpdateAsync();

            _timeResetCount++;
            _eqInfoCount++;

            ResumeLayout();
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
                using (var stream = await HttpClient.GetStreamAsync(Properties.Resources.LatestTimeUri).ConfigureAwait(false))
                {
                    var serializer = new DataContractJsonSerializer(typeof(Root));
                    json = (Root)serializer.ReadObject(stream);
                }
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException || ex is SerializationException)
            {
                InternalLog(ex);
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
                LatestTime = await RequestLatestTimeAsync(LatestTime).ConfigureAwait(false);
                _timeResetCount = 0;
            }
            else
            {
                LatestTime = LatestTime.AddSeconds(1);
            }
        }

        /// <summary>
        /// 電源モード変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static async void PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode != PowerModes.Resume) return;
            LatestTime = await RequestLatestTimeAsync(LatestTime).ConfigureAwait(false);
        }

        [Conditional("DEBUG")]
        public static void InternalLog(string str)
        {
            var value = $"{DateTime.Now}\n";
            value += $"[Log]\n{str}\n\n";
            using (var stream = new StreamWriter("Log.txt", true))
            {
                stream.WriteLine(value);
            }
        }

        [Conditional("DEBUG")]
        public static void InternalLog(Exception ex)
        {
            var value = $"{DateTime.Now}\n";
            value += $"[Message]\n{ex.Message}\n";
            value += $"[StackTrace]\n{ex.StackTrace}\n\n";
            using (var stream = new StreamWriter("Exception.txt", true))
            {
                stream.WriteLine(value);
            }
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
}
