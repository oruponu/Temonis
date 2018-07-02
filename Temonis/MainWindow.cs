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
        private const int TimeResetInterval = 300;
        private const int EqInfoInterval = 10;
        private readonly Timer _timer = new Timer();
        private static Kyoshin _kyoshin;
        private static EEW _eew;
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
            _eew = new EEW();
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

            using (var pen = new Pen(Utility.EqInfo.ColorMap[intensity]))
            {
                e.Graphics.DrawLine(pen, 1, e.RowBounds.Top, 1, e.RowBounds.Top + e.RowBounds.Height);
            }
        }

        /// <summary>
        /// ラベルを初期化
        /// </summary>
        private void InitializeLabel()
        {
            Label_KyoshinMaxInt.Text = "";
            Label_KyoshinMaxIntDetail.Text = "";
            Label_KyoshinPrefecture.Text = "";
            Label_EEWDateTimeHeader.Visible = false;
            Label_EEWEpicenterHeader.Visible = false;
            Label_EEWDepthHeader.Visible = false;
            Label_EEWMagnitudeHeader.Visible = false;
            Label_EEWIntensityHeader.Visible = false;
            Label_EEWDateTime.Text = "";
            Label_EEWEpicenter.Text = "";
            Label_EEWDepth.Text = "";
            Label_EEWMagnitude.Text = "";
            Label_EEWIntensity.Text = "";
            Label_EqInfoDateTime.Text = "";
            Label_EqInfoEpicenter.Text = "";
            Label_EqInfoDepth.Text = "";
            Label_EqInfoMagnitude.Text = "";
            Label_EqInfoMessage.Text = "";
        }

        /// <summary>
        /// フォームの色を設定
        /// </summary>
        private void SetFormColor()
        {
            BackColor = Utility.Black;
            GroupBox_Kyoshin.ForeColor = Utility.White;
            GroupBox_EEW.ForeColor = Utility.White;
            GroupBox_EqInfo.ForeColor = Utility.White;
            DataGridView_EqInfoIntensity.BackgroundColor = Utility.Black;
            DataGridView_EqInfoIntensity.DefaultCellStyle.BackColor = Utility.Black;
            DataGridView_EqInfoIntensity.DefaultCellStyle.SelectionBackColor = Utility.Black;
            DataGridView_EqInfoIntensity.DefaultCellStyle.SelectionForeColor = Utility.White;
        }

        /// <summary>
        /// フォントを設定
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
                        GroupBox_EEW.Font = new Font(fontFamily.Name, GroupBox_EEW.Font.Size);
                        Label_EEWMessage.Font = new Font(fontFamily.Name, Label_EEWMessage.Font.Size);
                        Label_EEWDateTimeHeader.Font = new Font(fontFamily.Name, Label_EEWDateTimeHeader.Font.Size);
                        Label_EEWDateTime.Font = new Font(fontFamily.Name, Label_EEWDateTime.Font.Size);
                        Label_EEWEpicenterHeader.Font = new Font(fontFamily.Name, Label_EEWEpicenterHeader.Font.Size);
                        Label_EEWEpicenter.Font = new Font(fontFamily.Name, Label_EEWEpicenter.Font.Size);
                        Label_EEWDepthHeader.Font = new Font(fontFamily.Name, Label_EEWDepthHeader.Font.Size);
                        Label_EEWDepth.Font = new Font(fontFamily.Name, Label_EEWDepth.Font.Size);
                        Label_EEWMagnitudeHeader.Font = new Font(fontFamily.Name, Label_EEWMagnitudeHeader.Font.Size);
                        Label_EEWMagnitude.Font = new Font(fontFamily.Name, Label_EEWMagnitude.Font.Size);
                        Label_EEWIntensityHeader.Font = new Font(fontFamily.Name, Label_EEWIntensityHeader.Font.Size);
                        Label_EEWIntensity.Font = new Font(fontFamily.Name, Label_EEWIntensity.Font.Size);
                        GroupBox_EqInfo.Font = new Font(fontFamily.Name, GroupBox_EqInfo.Font.Size);
                        label_EqInfoDateTimeHeader.Font = new Font(fontFamily.Name, label_EqInfoDateTimeHeader.Font.Size);
                        Label_EqInfoDateTime.Font = new Font(fontFamily.Name, Label_EqInfoDateTime.Font.Size);
                        label_EqInfoEpicenterHeader.Font = new Font(fontFamily.Name, label_EqInfoEpicenterHeader.Font.Size);
                        Label_EqInfoEpicenter.Font = new Font(fontFamily.Name, Label_EqInfoEpicenter.Font.Size);
                        label_EqInfoDepthHeader.Font = new Font(fontFamily.Name, label_EqInfoDepthHeader.Font.Size);
                        Label_EqInfoDepth.Font = new Font(fontFamily.Name, Label_EqInfoDepth.Font.Size);
                        label_EqInfoMagnitudeHeader.Font = new Font(fontFamily.Name, label_EqInfoMagnitudeHeader.Font.Size);
                        Label_EqInfoMagnitude.Font = new Font(fontFamily.Name, Label_EqInfoMagnitude.Font.Size);
                        Label_EqInfoMessage.Font = new Font(fontFamily.Name, Label_EqInfoMessage.Font.Size);

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
                await RequestLatestTimeAsync();
                _retryCount++;
            }

            label_KyoshinLatestTime.Text = _retryCount < 10 ? LatestTime.ToString("yyyy/MM/dd HH:mm:ss") : "接続しています...";
            await _stateSet.UpdateAsync();

            _timeResetCount++;
            _eqInfoCount++;

            ResumeLayout();
        }

        /// <summary>
        /// 強震モニタの時刻を取得
        /// </summary>
        /// <returns></returns>
        private static async Task RequestLatestTimeAsync()
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

            if (json == default(Root)) return;

            LatestTime = DateTime.Parse(json.LatestTime);
            //LatestTime = LatestTime.AddDays(-10).AddMinutes(16);
        }

        /// <summary>
        /// 強震モニタの時刻を設定
        /// </summary>
        /// <returns></returns>
        private static async Task SetLatestTimeAsync()
        {
            if (_timeResetCount >= TimeResetInterval)
            {
                await RequestLatestTimeAsync();
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
            await RequestLatestTimeAsync();
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
