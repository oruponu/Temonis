using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Temonis
{
    internal class EqInfo
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private const string Uri = "https://typhoon.yahoo.co.jp/weather/jp/earthquake/";
        private static MainWindow _instance;
        private static string _bufferLastEqInfo;

        public static Bitmap BitmapEpicenter { get; private set; }

        public static string Id { get; private set; }

        public static DateTime DateTime { get; private set; }

        public static string Intensity { get; private set; }

        public EqInfo(MainWindow instance)
        {
            _instance = instance;
        }

        // 地震情報を取得（Yahoo!天気・災害）
        public async Task UpdateEqInfo()
        {
            var html = await HttpClient.GetStringAsync(Uri);
            var epicenter = new double[2];

            // 地震ID
            Id = Regex.Match(html, "<a href=\"/weather/jp/earthquake/(?<text>.+?).html\">").Groups["text"].Value;

            html = Regex.Match(html, "<div id=\"eqinfdtl\" class=\"tracked_mods\">.+</div>", RegexOptions.Singleline).Value;
            if (!IsUpdated(html)) return;

            // 発生時刻
            var str = Regex.Match(html, @"<\w+>発生時刻</\w+></\w+>\n<.+><\w+>(?<text>.+?)</\w+>").Groups["text"].Value.Replace("ごろ", "");
            DateTime = DateTime.Parse(str, new CultureInfo("ja-JP"), DateTimeStyles.AssumeLocal);
            _instance.label_eqinfoTime.Text = DateTime.ToString("yyyy年MM月dd日 HH時mm分");
            // 震源地
            str = Regex.Match(html, @"<\w+>震源地</\w+></\w+>\n<.+><\w+><.+?>(?<text>.+?)</\w+></\w+>").Groups["text"].Value.Replace("</a>", "");
            if (str == "") str = "---"; // 震度速報の場合
            _instance.label_eqinfoEpicenter.Text = str;
            // 緯度
            var latitude = Regex.Match(html, @"<\w+>緯度</\w+></\w+>\n<.+><\w+>(?<text>.+?)</\w+>").Groups["text"].Value;
            if (latitude == "---")
            {
                epicenter[0] = 0.0;
            }
            else
            {
                latitude = latitude.Replace("度", "");
                epicenter[0] = latitude.Contains("北緯")
                    ? double.Parse(latitude.Replace("北緯", ""))
                    : double.Parse("-" + latitude.Replace("南緯", ""));
            }
            // 経度
            var longitude = Regex.Match(html, @"<\w+>経度</\w+></\w+>\n<.+><\w+>(?<text>.+?)</\w+>").Groups["text"].Value;
            if (longitude == "---")
            {
                epicenter[1] = 0.0;
            }
            else
            {
                longitude = longitude.Replace("度", "");
                epicenter[1] = longitude.Contains("東経")
                    ? double.Parse(longitude.Replace("東経", ""))
                    : double.Parse("-" + longitude.Replace("西経", ""));
            }
            // 深さ
            str = Regex.Match(html, @"<\w+>深さ</\w+></\w+>\n\n<.+><\w+>(?<text>.+?)</\w+>").Groups["text"].Value;
            _instance.label_eqinfoDepth.Text = str;
            // マグニチュード
            str = Regex.Match(html, @"<\w+>マグニチュード</\w+></\w+>\n<.+><\w+>(?<text>.+?)</\w+>").Groups["text"].Value;
            _instance.label_eqinfoMagnitude.Text = str;
            // 情報
            str = Regex.Match(html, @"<\w+>情報</\w+></\w+>\n<.+><\w+>(?<text>.+?)</\w+>").Groups["text"].Value;
            _instance.label_eqinfoMessage.Font = new Font(_instance.label_eqinfoMessage.Font.FontFamily, 12.0f);
            if (str.Length > 32)
            {
                _instance.label_eqinfoMessage.Font = new Font(_instance.label_eqinfoMessage.Font.FontFamily, 11.0f);
            }
            if (_instance.label_eqinfoTime.Text != "---" && _instance.label_eqinfoEpicenter.Text == "---")
            {
                str = "今後の情報に注意してください。";
            }
            if (str.Count(x => x == '。') == 2) str = str.Replace("。", "。\n");   // 付加文の情報が2つの場合は2行に分割
            _instance.label_eqinfoMessage.Text = str;
            // 各地の震度
            _instance.textBox_eqInfoIntensity.Clear();
            str = Regex.Match(html, @"<\w+ \S+ \S+ \S+ \S+ class=""yjw_table"">(?<text>.+?)</\w+>\n</div>",
                RegexOptions.Singleline).Groups["text"].Value.Replace("\n", "");
            str = Regex.Replace(str, @"<\w+ \S+><\w+ \S+ \S+><\w+><\w+>", "［");
            str = Regex.Replace(str, @"</\w+></\w+></\w+><\w+ \S+><\w+>", "］");
            str = Regex.Replace(str, @"<\w+ \S+><\w+ \S+ \S+ \S+><\w+>", "");
            str = Regex.Replace(str, @"</\w+></\w+><\w+ \S+ \S+><.+?>", "\r\n");
            str = Regex.Replace(str, @"　<\w+></\w+></\w+></\w+>", "\r\n");
            str = Regex.Replace(str, @"</\w+></\w+></\w+>", "\r\n");
            Intensity = str.Trim();
            _instance.textBox_eqInfoIntensity.Text = Intensity;

            CreateEpicenterImg(epicenter);
        }

        // 震央位置の画像を作成
        private static void CreateEpicenterImg(double[] epicenter)
        {
            double lotMin;
            double pxPerDigX;
            if (epicenter[0] > 30.0 || epicenter[1] > 130.9)
            {
                lotMin = 128.6;
                pxPerDigX = _instance.pictureBox_kyoshinMap.Width / 17.3;
            }
            else
            {
                lotMin = 122.5;
                pxPerDigX = (_instance.pictureBox_kyoshinMap.Width - 181) / 8.4;
            }
            var x = (epicenter[1] - lotMin) * pxPerDigX;

            double latMin;
            double pxPerDigY;
            double y;
            if (epicenter[0] > 30.0 || epicenter[1] > 130.9)
            {
                latMin = 30.0;
                pxPerDigY = _instance.pictureBox_kyoshinMap.Height / 16.0;
                y = _instance.pictureBox_kyoshinMap.Height - (epicenter[0] - latMin) * pxPerDigY;
            }
            else
            {
                latMin = 23.6;
                pxPerDigY = (_instance.pictureBox_kyoshinMap.Height - 244) / 6.4;
                y = _instance.pictureBox_kyoshinMap.Height - 193 - (epicenter[0] - latMin) * pxPerDigY;
            }

            BitmapEpicenter = new Bitmap(_instance.pictureBox_kyoshinMap.Width, _instance.pictureBox_kyoshinMap.Height);
            var graphicsEpicenter = Graphics.FromImage(BitmapEpicenter);
            graphicsEpicenter.DrawImage(Properties.Resources.Epicenter,
                (int)(Math.Round(x) - Properties.Resources.Epicenter.Width / 2),
                (int)(Math.Round(y) - Properties.Resources.Epicenter.Height / 2));
            graphicsEpicenter.Dispose();
        }

        // ページ更新チェック
        private static bool IsUpdated(string lastUpdating)
        {
            if (_bufferLastEqInfo == lastUpdating) return false;
            _bufferLastEqInfo = lastUpdating;
            return true;
        }
    }
}
