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
using Timers = System.Timers;

namespace Temonis
{
    public partial class MainWindow : Form
    {
        public static readonly Color Black = Color.FromArgb(26, 26, 36);
        public static readonly Color White = Color.FromArgb(226, 226, 226);
        public static readonly Color Red = Color.FromArgb(255, 40, 0);
        public static readonly Color Blue = Color.FromArgb(0, 40, 255);
        public static readonly Color Yellow = Color.FromArgb(250, 245, 0);
        public static readonly Color Purple = Color.FromArgb(200, 0, 255);
        private static readonly HttpClient HttpClient = new HttpClient();
        private const string LatestTimeUri = "http://www.kmoni.bosai.go.jp/new/webservice/server/pros/latest.json";
        private const int TimeResetInterval = 300;
        private const int EqInfoInterval = 10;
        private static Kyoshin _kyoshin;
        private static EEW _eew;
        private static EqInfo _eqInfo;
        private static Sound _sound;
        private static int _timeResetCount = TimeResetInterval;
        private static int _eqInfoCount = EqInfoInterval;
        private static int _retryCount;

        public static DateTime LatestTime { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            _kyoshin = new Kyoshin(this);
            _eew = new EEW(this);
            _eqInfo = new EqInfo(this);
            _sound = new Sound(this);
            Settings.LoadSettings();

            // フォームを初期化
            pictureBox_kyoshinMap.Image = Properties.Resources.BaseMap;
            InitializeLabel();
            SetFormColor();
            SetFormFont();
            ActiveControl = label_kyoshinLatestTime;
            // タイマー定義
            var timer = new Timers.Timer(1000);
            timer.Elapsed += Timer;
            timer.SynchronizingObject = this;
            timer.Start();
            Timer(null, EventArgs.Empty);
            // 電源モード変更イベントを登録
            SystemEvents.PowerModeChanged += PowerModeChanged;
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            // 電源モード変更イベントを解除
            SystemEvents.PowerModeChanged -= PowerModeChanged;
        }

        /// <summary>
        /// ラベルを初期化
        /// </summary>
        private void InitializeLabel()
        {
            label_kyoshinMaxInt.Text = "";
            label_kyoshinMaxIntDetail.Text = "";
            label_kyoshinPrefecture.Text = "";
            label_eewTimeHeader.Visible = false;
            label_eewEpicenterHeader.Visible = false;
            label_eewDepthHeader.Visible = false;
            label_eewMagnitudeHeader.Visible = false;
            label_eewIntensityHeader.Visible = false;
            label_eewTime.Text = "";
            label_eewEpicenter.Text = "";
            label_eewDepth.Text = "";
            label_eewMagnitude.Text = "";
            label_eewIntensity.Text = "";
            label_eqinfoTime.Text = "";
            label_eqinfoEpicenter.Text = "";
            label_eqinfoDepth.Text = "";
            label_eqinfoMagnitude.Text = "";
            label_eqinfoMessage.Text = "";
        }

        /// <summary>
        /// フォームの色を設定
        /// </summary>
        private void SetFormColor()
        {
            BackColor = Black;
            label_kyoshinLatestTime.ForeColor = White;
            groupBox_Kyoshin.ForeColor = White;
            groupBox_EEW.ForeColor = White;
            groupBox_EqInfo.ForeColor = White;
            textBox_eqInfoIntensity.BackColor = Black;
            textBox_eqInfoIntensity.ForeColor = White;
        }

        /// <summary>
        /// フォントを設定
        /// </summary>
        private void SetFormFont()
        {
            var name = "Meiryo UI";
            var fontCollection = new InstalledFontCollection();
            var fontFamilies = fontCollection.Families;
            foreach (var fontFamily in fontFamilies)
            {
                if (fontFamily.Name == "Yu Gothic UI")
                {
                    name = fontFamily.Name;
                }
            }
            label_kyoshinLatestTime.Font = new Font(name, label_kyoshinLatestTime.Font.Size);
            groupBox_Kyoshin.Font = new Font(name, groupBox_Kyoshin.Font.Size);
            label_kyoshinMaxIntHeader.Font = new Font(name, label_kyoshinMaxIntHeader.Font.Size);
            label_kyoshinMaxInt.Font = new Font(name, label_kyoshinMaxInt.Font.Size);
            label_kyoshinMaxIntDetail.Font = new Font(name, label_kyoshinMaxIntDetail.Font.Size);
            label_kyoshinPrefecture.Font = new Font(name, label_kyoshinPrefecture.Font.Size);
            groupBox_EEW.Font = new Font(name, groupBox_EEW.Font.Size);
            label_eewMessage.Font = new Font(name, label_eewMessage.Font.Size);
            label_eewTimeHeader.Font = new Font(name, label_eewTimeHeader.Font.Size);
            label_eewTime.Font = new Font(name, label_eewTime.Font.Size);
            label_eewEpicenterHeader.Font = new Font(name, label_eewEpicenterHeader.Font.Size);
            label_eewEpicenter.Font = new Font(name, label_eewEpicenter.Font.Size);
            label_eewDepthHeader.Font = new Font(name, label_eewDepthHeader.Font.Size);
            label_eewDepth.Font = new Font(name, label_eewDepth.Font.Size);
            label_eewMagnitudeHeader.Font = new Font(name, label_eewMagnitudeHeader.Font.Size);
            label_eewMagnitude.Font = new Font(name, label_eewMagnitude.Font.Size);
            label_eewIntensityHeader.Font = new Font(name, label_eewIntensityHeader.Font.Size);
            label_eewIntensity.Font = new Font(name, label_eewIntensity.Font.Size);
            groupBox_EqInfo.Font = new Font(name, groupBox_EqInfo.Font.Size);
            label_eqinfoTimeHeader.Font = new Font(name, label_eqinfoTimeHeader.Font.Size);
            label_eqinfoTime.Font = new Font(name, label_eqinfoTime.Font.Size);
            label_eqinfoEpicenterHeader.Font = new Font(name, label_eqinfoEpicenterHeader.Font.Size);
            label_eqinfoEpicenter.Font = new Font(name, label_eqinfoEpicenter.Font.Size);
            label_eqinfoDepthHeader.Font = new Font(name, label_eqinfoDepthHeader.Font.Size);
            label_eqinfoDepth.Font = new Font(name, label_eqinfoDepth.Font.Size);
            label_eqinfoMagnitudeHeader.Font = new Font(name, label_eqinfoMagnitudeHeader.Font.Size);
            label_eqinfoMagnitude.Font = new Font(name, label_eqinfoMagnitude.Font.Size);
            label_eqinfoMessage.Font = new Font(name, label_eqinfoMessage.Font.Size);
        }

        /// <summary>
        /// レベルを変更
        /// </summary>
        private void ChangeLevel()
        {
            //強震モニタ
            if (Kyoshin.OnTrigger) //1点赤ではレベルを変更しない
            {
                if (label_kyoshinMaxInt.Text.Contains("弱") || label_kyoshinMaxInt.Text.Contains("強") || label_kyoshinMaxInt.Text.Contains("7（"))
                {
                    groupBox_Kyoshin.BorderColor = Red;
                }
                else if (label_kyoshinMaxInt.Text.Contains("3（") || label_kyoshinMaxInt.Text.Contains("4（"))
                {
                    groupBox_Kyoshin.BorderColor = Yellow;
                }
                else
                {
                    groupBox_Kyoshin.BorderColor = White;
                }
            }
            else
            {
                groupBox_Kyoshin.BorderColor = White;
            }
            //緊急地震速報
            if (EEW.OnTrigger)
            {
                if (label_eewMessage.Text.Contains("警報"))
                {
                    groupBox_EEW.BorderColor = Red;
                }
                else if (label_eewMessage.Text.Contains("予報"))
                {
                    groupBox_EEW.BorderColor = Yellow;
                }
            }
            else
            {
                groupBox_EEW.BorderColor = White;
            }
            //地震情報
            if (textBox_eqInfoIntensity.Text.Contains("弱") || textBox_eqInfoIntensity.Text.Contains("強") ||
                textBox_eqInfoIntensity.Text.Contains("7") || label_eqinfoMessage.Text.Contains("警報"))
            {
                groupBox_EqInfo.BorderColor = Red;
            }
            else if (textBox_eqInfoIntensity.Text.Contains("3") || textBox_eqInfoIntensity.Text.Contains("4") ||
                     label_eqinfoMessage.Text.Contains("注意報"))
            {
                groupBox_EqInfo.BorderColor = Yellow;
            }
            else
            {
                groupBox_EqInfo.BorderColor = White;
            }
        }

        /// <summary>
        /// メインタイマー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Timer(object sender, EventArgs e)
        {
            try
            {
                await SetLatestTimeAsync();
            }
            catch (Exception ex)
            {
                Logger(ex);
            }
            try
            {
                await _eew.UpdateEEWAsync();
            }
            catch (Exception ex)
            {
                Logger(ex);
            }
            if (_eqInfoCount >= EqInfoInterval)
            {
                try
                {
                    await _eqInfo.UpdateEqInfoAsync();
                }
                catch (Exception ex)
                {
                    Logger(ex);
                }
                _eqInfoCount = 0;
            }
            try
            {
                await _kyoshin.UpdateKyoshinAsync();
                _retryCount = 0;
            }
            catch (Exception ex)
            {
                await RequestLatestTimeAsync();
                _retryCount++;
                Logger(ex);
            }
            label_kyoshinLatestTime.Text = _retryCount < 10 ? LatestTime.ToString("yyyy/MM/dd HH:mm:ss") : "接続しています...";
            ChangeLevel();
            try
            {
                await _sound.PlaySound().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Logger(ex);
            }
            _timeResetCount++;
            _eqInfoCount++;
        }

        /// <summary>
        /// 強震モニタの時刻を取得
        /// </summary>
        /// <returns></returns>
        private static async Task RequestLatestTimeAsync()
        {
            try
            {
                using (var stream = await HttpClient.GetStreamAsync(LatestTimeUri))
                {
                    var serializer = new DataContractJsonSerializer(typeof(LatestTimeJson));
                    var json = (LatestTimeJson)serializer.ReadObject(stream);
                    LatestTime = DateTime.Parse(json.LatestTime);
                }
            }
            catch (Exception ex)
            {
                Logger(ex);
            }
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
        private static void Logger(Exception ex)
        {
            using (var stream = new StreamWriter("Error.txt", true))
            {
                stream.WriteLine(DateTime.Now);
                stream.WriteLine($"[Message]\r\n{ex.Message}");
                stream.WriteLine($"[StackTrace]\r\n{ex.StackTrace}");
                stream.WriteLine();
            }
        }
    }

    /// <summary>
    /// JSONクラス
    /// </summary>
    [DataContract]
    public class LatestTimeJson
    {
        [DataMember(Name = "security")]
        public SecurityJson Security { get; set; }

        [DataMember(Name = "latest_time")]
        public string LatestTime { get; set; }

        [DataMember(Name = "request_time")]
        public string RequestTime { get; set; }

        [DataMember(Name = "result")]
        public ResultJson Result { get; set; }

        [DataContract]
        public class SecurityJson
        {
            [DataMember(Name = "realm")]
            public string Realm { get; set; }

            [DataMember(Name = "hash")]
            public string Hash { get; set; }
        }

        [DataContract]
        public class ResultJson
        {
            [DataMember(Name = "status")]
            public string Status { get; set; }

            [DataMember(Name = "message")]
            public string Message { get; set; }
        }
    }
}
