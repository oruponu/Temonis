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
        private static string _lastInfo;

        public static Bitmap BitmapEpicenter { get; private set; }

        public static string Id { get; private set; }

        public static DateTime ArrivalTime { get; private set; }

        public static string Intensity { get; private set; }

        public EqInfo(MainWindow instance)
        {
            _instance = instance;
        }

        /// <summary>
        /// 地震情報を取得
        /// </summary>
        /// <returns></returns>
        public async Task UpdateEqInfoAsync()
        {
            var html = await HttpClient.GetStringAsync(Uri);

            // 地震ID
            Id = Regex.Match(html, "<a href=\"/weather/jp/earthquake/(.+).html\">").Groups[1].Value;

            html = Regex.Match(html, "<div id=\"eqinfdtl\" class=\"tracked_mods\">.+?</div>", RegexOptions.Singleline).Value;
            if (!IsUpdated(html)) return;

            // 発生時刻
            var str = Regex.Match(html, @"<.+>発生時刻(</.+>)+\n(<.+>)+(.+?)</.+>").Groups[3].Value.Replace("ごろ", "");
            ArrivalTime = DateTime.Parse(str, new CultureInfo("ja-JP"), DateTimeStyles.AssumeLocal);
            _instance.label_eqinfoTime.Text = ArrivalTime.ToString("yyyy年MM月dd日 HH時mm分");
            // 震源地
            str = Regex.Match(html, @"<.+>震源地(</.+>)+\n(<.+>)+<.+?>(.+?)</.+>").Groups[3].Value;
            str += Regex.Match(html, @"<.+>震源地(</.+>)+\n(<.+>)+<.+?>(.+?)</.+>(.+?)(</.+>){2}").Groups[4].Value;
            _instance.label_eqinfoEpicenter.Text = str;
            // 緯度
            double latitude, longitude;
            str = Regex.Match(html, @"<.+>緯度(</.+>)+\n(<.+>)+(.+?)</.+>").Groups[3].Value;
            if (str == "---")
            {
                latitude = 0.0;
            }
            else
            {
                str = str.Replace("度", "");
                latitude = str.Contains("北緯")
                    ? double.Parse(str.Replace("北緯", ""))
                    : double.Parse("-" + str.Replace("南緯", ""));
            }
            // 経度
            str = Regex.Match(html, @"<.+>経度(</.+>)+\n(<.+>)+(.+?)</.+>").Groups[3].Value;
            if (str == "---")
            {
                longitude = 0.0;
            }
            else
            {
                str = str.Replace("度", "");
                longitude = str.Contains("東経")
                    ? double.Parse(str.Replace("東経", ""))
                    : double.Parse("-" + str.Replace("西経", ""));
            }
            // 深さ
            str = Regex.Match(html, @"<.+>深さ(</.+>)+\n\n(<.+>)+(.+?)</.+>").Groups[3].Value;
            _instance.label_eqinfoDepth.Text = str;
            // マグニチュード
            str = Regex.Match(html, @"<.+>マグニチュード(</.+>)+\n(<.+>)+(.+?)</.+>").Groups[3].Value;
            _instance.label_eqinfoMagnitude.Text = str;
            // 情報
            str = Regex.Match(html, @"<.+>情報(</.+>)+\n(<.+?>)+(.+?)</?.+>").Groups[3].Value;
            _instance.label_eqinfoMessage.Font = new Font(_instance.label_eqinfoMessage.Font.FontFamily, 12f);
            if (str.Length > 32)
            {
                _instance.label_eqinfoMessage.Font = new Font(_instance.label_eqinfoMessage.Font.FontFamily, 11f);
            }
            if (str.Count(x => x == '。') == 2)  // 付加文の情報が2つの場合は2行に分割
            {
                str = str.Replace("。", "。\n");
            }
            _instance.label_eqinfoMessage.Text = str;
            // 各地の震度
            _instance.textBox_eqInfoIntensity.Clear();
            str = Regex.Match(html, @"<.+class=""yjw_table"">(.+?)</\w+>\n</div>",
                RegexOptions.Singleline).Groups[1].Value.Replace("\n", "");
            str = Regex.Replace(str, @"<\w+ \S+><\w+ \S+ \w+>(<\w+>)+", "［");
            str = Regex.Replace(str, @"(</\w+>)+<\w+ \S+><\w+>", "］");
            str = Regex.Replace(str, @"<\w+ \S+><\w+ \S+ \S+ \S+><\w+>", "");
            str = Regex.Replace(str, @"(</\w+>)+<\w+ \S+ \S+><.+?>", "\r\n");
            str = Regex.Replace(str, @"　<\w+>(</\w+>){3}", "\r\n");
            str = Regex.Replace(str, @"(</\w+>)+", "\r\n");
            Intensity = str.Trim();
            _instance.textBox_eqInfoIntensity.Text = Intensity;

            CreateEpicenterImg(latitude, longitude);
        }

        /// <summary>
        /// 震央位置の画像を作成
        /// </summary>
        /// <param name="latitude">震央の緯度</param>
        /// <param name="longitude">震央の経度</param>
        private static void CreateEpicenterImg(double latitude, double longitude)
        {
            double lonMin;
            double pxPerDigX;
            if (latitude > 30.0 || longitude > 130.9)
            {
                lonMin = 128.6;
                pxPerDigX = _instance.pictureBox_kyoshinMap.Width / 17.3;
            }
            else
            {
                lonMin = 122.5;
                pxPerDigX = (_instance.pictureBox_kyoshinMap.Width - 181) / 8.4;
            }
            var x = (longitude - lonMin) * pxPerDigX;

            double latMin;
            double pxPerDigY;
            double y;
            if (latitude > 30.0 || longitude > 130.9)
            {
                latMin = 30.0;
                pxPerDigY = _instance.pictureBox_kyoshinMap.Height / 16.0;
                y = _instance.pictureBox_kyoshinMap.Height - (latitude - latMin) * pxPerDigY;
            }
            else
            {
                latMin = 23.6;
                pxPerDigY = (_instance.pictureBox_kyoshinMap.Height - 244) / 6.4;
                y = _instance.pictureBox_kyoshinMap.Height - 193 - (latitude - latMin) * pxPerDigY;
            }

            BitmapEpicenter = new Bitmap(_instance.pictureBox_kyoshinMap.Width, _instance.pictureBox_kyoshinMap.Height);
            var graphicsEpicenter = Graphics.FromImage(BitmapEpicenter);
            graphicsEpicenter.DrawImage(Properties.Resources.Epicenter,
                (int)(Math.Round(x) - Properties.Resources.Epicenter.Width / 2),
                (int)(Math.Round(y) - Properties.Resources.Epicenter.Height / 2));
            graphicsEpicenter.Dispose();
        }

        /// <summary>
        /// ページ更新チェック
        /// </summary>
        /// <param name="current">現在保持している HTML</param>
        /// <returns></returns>
        private static bool IsUpdated(string current)
        {
            if (_lastInfo == current) return false;
            _lastInfo = current;
            return true;
        }
    }
}
