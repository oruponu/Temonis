using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Temonis
{
    public partial class MainWindow : Form
    {
        public static readonly Color Black = Color.FromArgb(32, 32, 32);
        public static readonly Color White = Color.FromArgb(224, 224, 224);
        public static readonly Color Red = Color.FromArgb(224, 56, 0);
        public static readonly Color Blue = Color.FromArgb(0, 56, 224);
        public static readonly Color Yellow = Color.FromArgb(224, 224, 56);
        public static readonly Color Purple = Color.FromArgb(224, 56, 224);
        private static readonly HttpClient HttpClient = new HttpClient();
        private static int _timeResetCount = TimeResetInterval;
        private static int _eqInfoCount = EqInfoInterval;
        private const string LatestTimeUri = "http://www.kmoni.bosai.go.jp/new/webservice/server/pros/latest.json";
        private const int TimeResetInterval = 300;
        private const int EqInfoInterval = 10;
        private static Kyoshin _kyoshin;
        private static EEW _eew;
        private static EqInfo _eqInfo;
        private static Sound _sound;
        private static int _retryCount;

        public static DateTime LatestTime { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            _kyoshin = new Kyoshin(this);
            _eew = new EEW(this);
            _eqInfo = new EqInfo(this);
            _sound = new Sound();
            // 設定を読み込む
            Settings.LoadSettings();
            //ピクチャーボックスを初期化
            pictureBox_kyoshinMap.Image = Properties.Resources.BaseMap;
            // ラベルを初期化
            InitializeLabel();
            // フォームの色を変更
            ChangeFormColor();
            // フォーカスをコントロールから外す
            ActiveControl = label_LatestTime;
            // タイマー定義
            var timer = new System.Timers.Timer(1000);
            timer.Elapsed += Timer;
            timer.SynchronizingObject = this;
            timer.Start();
            Timer(null, EventArgs.Empty);
        }

        // ラベルを初期化
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

        // フォームの色を変更
        private void ChangeFormColor()
        {
            BackColor = Black;
            label_LatestTime.ForeColor = White;
            groupBox_Kyoshin.ForeColor = White;
            groupBox_EEW.ForeColor = White;
            groupBox_EqInfo.ForeColor = White;
            textBox_eqInfoIntensity.BackColor = Black;
            textBox_eqInfoIntensity.ForeColor = White;
        }
        
        // レベルを変更
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

        // メインタイマー
        private async void Timer(object sender, EventArgs e)
        {
            try
            {
                await SetLatestTime();
            }
            catch (Exception ex)
            {
                Logger(ex);
            }
            try
            {
                await _eew.UpdateEEW();
            }
            catch (Exception ex)
            {
                Logger(ex);
            }
            if (_eqInfoCount >= EqInfoInterval)
            {
                try
                {
                    await _eqInfo.UpdateEqInfo();
                }
                catch (Exception ex)
                {
                    Logger(ex);
                }
                _eqInfoCount = 0;
            }
            try
            {
                await _kyoshin.UpdateKyoshin();
                _retryCount = 0;
            }
            catch (Exception ex)
            {
                await RequestLatestTime();
                _retryCount++;
                Logger(ex);
            }
            label_LatestTime.Text = _retryCount < 10 ? LatestTime.ToString("yyyy/MM/dd HH:mm:ss") : "接続しています...";
            ChangeLevel();
            try
            {
                _sound.PlaySound();
            }
            catch (Exception ex)
            {
                Logger(ex);
            }
            _timeResetCount++;
            _eqInfoCount++;
        }

        // 強震モニタの時刻を取得
        private static async Task RequestLatestTime()
        {
            try
            {
                using (var stream = await HttpClient.GetStreamAsync(LatestTimeUri))
                {
                    var latestTime = (LatestTimeJson)new DataContractJsonSerializer(typeof(LatestTimeJson)).ReadObject(stream);
                    LatestTime = DateTime.Parse(latestTime.LatestTime);
                }
            }
            catch (Exception ex)
            {
                Logger(ex);
            }
        }

        // 強震モニタの時刻を設定
        private static async Task SetLatestTime()
        {
            if (_timeResetCount >= TimeResetInterval)
            {
                await RequestLatestTime();
                _timeResetCount = 0;
            }
            else
            {
                LatestTime = LatestTime.AddSeconds(1);
            }
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

    // JSONクラス
    [DataContract]
    public class LatestTimeJson
    {
        [DataMember(Name = "security")]
        public SecurityLatest Security { get; set; }

        [DataMember(Name = "latest_time")]
        public string LatestTime { get; set; }

        [DataMember(Name = "request_time")]
        public string RequestTime { get; set; }

        [DataMember(Name = "result")]
        public ResultLatest Result { get; set; }
    }

    [DataContract]
    public class SecurityLatest
    {
        [DataMember(Name = "realm")]
        public string Realm { get; set; }

        [DataMember(Name = "hash")]
        public string Hash { get; set; }
    }

    [DataContract]
    public class ResultLatest
    {
        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
