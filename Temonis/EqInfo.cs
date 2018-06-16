using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Temonis.Resources;
using static Temonis.MainWindow;

namespace Temonis
{
    internal class EqInfo
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private static string _lastInfo;

        public static Bitmap EpicenterBitmap { get; private set; }

        public static string Id { get; private set; }

        public static string Epicenter { get; private set; }

        public static string Intensity { get; private set;}

        public static string MaxInt { get; private set; } = "";

        /// <summary>
        /// 地震情報を取得
        /// </summary>
        /// <returns></returns>
        public async Task UpdateAsync()
        {
            var html = "";
            try
            {
                html = await HttpClient.GetStringAsync(Properties.Resources.EqInfoUri);
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                Logger(ex);
            }

            if (string.IsNullOrEmpty(html)) return;

            html = Regex.Match(html, "<div id=\"eqinfdtl\" class=\"tracked_mods\">.+?</div>", RegexOptions.Singleline).Value;
            if (!IsUpdated(html)) return;

            // 発生時刻
            var time = Regex.Match(html, @"<.+>発生時刻(</.+>)+\n(<.+>)+(.+?)</.+>").Groups[3].Value.Replace("ごろ", "");
            var arrivalTime = DateTime.Parse(time, new CultureInfo("ja-JP"), DateTimeStyles.AssumeLocal);
            Instance.Label_EqInfoTime.Text = arrivalTime.ToString("yyyy年MM月dd日 HH時mm分");

            // 震源地
            var epicenter = Regex.Match(html, @"<.+>震源地(</.+>)+\n(<.+>)+<.+?>(.+?)</.+>").Groups[3].Value;
            epicenter += Regex.Match(html, @"<.+>震源地(</.+>)+\n(<.+>)+<.+?>(.+?)</.+>(.+?)(</.+>){2}").Groups[4].Value;
            Epicenter = epicenter;
            Instance.Label_EqInfoEpicenter.Text = epicenter;

            // 緯度
            double parsedLatitude, parsedLongitude;
            var latitude = Regex.Match(html, @"<.+>緯度(</.+>)+\n(<.+>)+(.+?)</.+>").Groups[3].Value;
            if (latitude == "---")
            {
                parsedLatitude = 0.0;
            }
            else
            {
                latitude = latitude.Replace("度", "");
                parsedLatitude = latitude.Contains("北緯") ? double.Parse(latitude.Replace("北緯", "")) : double.Parse("-" + latitude.Replace("南緯", ""));
            }

            // 経度
            var longitude = Regex.Match(html, @"<.+>経度(</.+>)+\n(<.+>)+(.+?)</.+>").Groups[3].Value;
            if (longitude == "---")
            {
                parsedLongitude = 0.0;
            }
            else
            {
                longitude = longitude.Replace("度", "");
                parsedLongitude = longitude.Contains("東経") ? double.Parse(longitude.Replace("東経", "")) : double.Parse("-" + longitude.Replace("西経", ""));
            }

            // 深さ
            Instance.Label_EqInfoDepth.Text = Regex.Match(html, @"<.+>深さ(</.+>)+\n\n(<.+>)+(.+?)</.+>").Groups[3].Value;

            // マグニチュード
            Instance.Label_EqInfoMagnitude.Text = Regex.Match(html, @"<.+>マグニチュード(</.+>)+\n(<.+>)+(.+?)</.+>").Groups[3].Value;

            // 情報
            var message = Regex.Match(html, @"<.+>情報(</.+>)+\n(<.+?>)+(.+?)</?.+>").Groups[3].Value;
            Instance.Label_EqInfoMessage.Font = new Font(Instance.Label_EqInfoMessage.Font.FontFamily, 12f);
            if (message.Length > 32)
            {
                Instance.Label_EqInfoMessage.Font = new Font(Instance.Label_EqInfoMessage.Font.FontFamily, 11f);
            }

            if (message.Count(x => x == '。') == 2)  // 付加文の情報が2つの場合は2行に分割
            {
                message = message.Replace("。", "。\n");
            }

            Instance.Label_EqInfoMessage.Text = message;

            // 各地の震度
            var intensity = Regex.Match(html, @"<.+class=""yjw_table"">(.+?)</\w+>\n</div>", RegexOptions.Singleline).Groups[1].Value.Replace("\n", "");
            intensity = Regex.Replace(intensity, @"<\w+ \S+><\w+ \S+ \w+>(<\w+>)+", "[");
            intensity = Regex.Replace(intensity, @"(</\w+>)+<\w+ \S+><\w+>", "]");
            intensity = Regex.Replace(intensity, @"<\w+ \S+><\w+ \S+ \S+ \S+><\w+>震度", "");
            intensity = Regex.Replace(intensity, @"(</\w+>)+<\w+ \S+ \S+><.+?>", ":");
            intensity = Regex.Replace(intensity, @"　<\w+>(</\w+>){3}", "");
            intensity = Regex.Replace(intensity, @"(</\w+>)+", ",");
            intensity = intensity.TrimEnd(',');

            Instance.DataGridView_EqInfoIntensity.Rows.Clear();
            if (intensity != "")
            {
                SetDataGridView(intensity);

                Instance.DataGridView_EqInfoIntensity.Columns[Instance.DataGridView_EqInfoIntensity.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                MaxInt = intensity.Split(',')[0].Split(':')[0];
            }
            else
            {
                MaxInt = "";
            }

            Intensity = intensity;
            
            // 地震ID
            Id = Regex.Match(html, "<a href=\"/weather/jp/earthquake/(.+).html\">").Groups[1].Value;

            CreateEpicenterImage(parsedLatitude, parsedLongitude);
        }

        /// <summary>
        /// DataGridView に情報を設定
        /// </summary>
        /// <param name="text"></param>
        private static void SetDataGridView(string text)
        {
            foreach (var item in text.Split(','))
            {
                var split = item.Split(':');
                var intensity = "震度" + split[0];
                foreach (var pref in split[1].TrimStart('[').Split('['))
                {
                    split = pref.Split(']');

                    var prefName = split[0];
                    var cities = new StringBuilder();
                    foreach (var city in split[1].Split('　'))
                    {
                        var cityName = AbbreviateCityName(city, split[0]);
                        if (cities.Length + ("　" + cityName).Length > 28)
                        {
                            if (Instance.DataGridView_EqInfoIntensity.Rows.Count > 0)
                            {
                                var cell = Instance.DataGridView_EqInfoIntensity[0, Instance.DataGridView_EqInfoIntensity.Rows.Count - 1];
                                if (cell.Value != null && cell.Value.ToString() == intensity)
                                {
                                    intensity = "";
                                    cell = Instance.DataGridView_EqInfoIntensity[1, Instance.DataGridView_EqInfoIntensity.Rows.Count - 1];
                                    if (cell.Value != null && cell.Value.ToString() == split[0])
                                    {
                                        prefName = "";
                                    }
                                }
                            }

                            Instance.DataGridView_EqInfoIntensity.Rows.Add(intensity, prefName, cities.ToString());
                            cities.Clear();
                        }

                        if (cities.Length > 0)
                        {
                            cities.Append("　");
                        }

                        cities.Append(cityName);
                    }

                    if (Instance.DataGridView_EqInfoIntensity.Rows.Count > 0)
                    {
                        var cell = Instance.DataGridView_EqInfoIntensity[0, Instance.DataGridView_EqInfoIntensity.Rows.Count - 1];
                        if (cell.Value != null && cell.Value.ToString() == intensity)
                        {
                            intensity = "";
                            cell = Instance.DataGridView_EqInfoIntensity[1, Instance.DataGridView_EqInfoIntensity.Rows.Count - 1];
                            if (cell.Value != null && cell.Value.ToString() == split[0])
                            {
                                prefName = "";
                            }
                        }
                    }

                    Instance.DataGridView_EqInfoIntensity.Rows.Add(intensity, prefName, cities.ToString());
                }
            }
        }

        /// <summary>
        /// 市町村名を省略
        /// </summary>
        /// <param name="city">市町村名</param>
        /// <param name="pref">属する都道府県</param>
        /// <returns></returns>
        private static string AbbreviateCityName(string city, string pref)
        {
            pref = pref.TrimEnd('都').TrimEnd('府').TrimEnd('県');
            if (!city.StartsWith(pref) && pref != "北海道") return city;
            if (city.EndsWith("区") && !city.Contains("堺市"))
            {
                return pref == "東京" ? city.Replace("東京", "") : city;
            }

            return General.EqInfo.CityAbbreviation.TryGetValue(city, out var value) ? value : city;
        }

        /// <summary>
        /// 震央位置の画像を作成
        /// </summary>
        /// <param name="latitude">震央の緯度</param>
        /// <param name="longitude">震央の経度</param>
        private static void CreateEpicenterImage(double latitude, double longitude)
        {
            double lonMin;
            double pxPerDigX;
            if (latitude > 30.0 || longitude > 130.9)
            {
                lonMin = 128.6;
                pxPerDigX = Instance.PictureBox_KyoshinMap.Width / 17.3;
            }
            else
            {
                lonMin = 122.5;
                pxPerDigX = (Instance.PictureBox_KyoshinMap.Width - 181) / 8.4;
            }

            var x = (longitude - lonMin) * pxPerDigX;
            double latMin;
            double pxPerDigY;
            double y;
            if (latitude > 30.0 || longitude > 130.9)
            {
                latMin = 30.0;
                pxPerDigY = Instance.PictureBox_KyoshinMap.Height / 16.0;
                y = Instance.PictureBox_KyoshinMap.Height - (latitude - latMin) * pxPerDigY;
            }
            else
            {
                latMin = 23.6;
                pxPerDigY = (Instance.PictureBox_KyoshinMap.Height - 244) / 6.4;
                y = Instance.PictureBox_KyoshinMap.Height - 193 - (latitude - latMin) * pxPerDigY;
            }

            EpicenterBitmap = new Bitmap(Instance.PictureBox_KyoshinMap.Width, Instance.PictureBox_KyoshinMap.Height);
            var graphicsEpicenter = Graphics.FromImage(EpicenterBitmap);
            graphicsEpicenter.DrawImage(Properties.Resources.Epicenter, (int)(Math.Round(x) - Properties.Resources.Epicenter.Width / 2.0f), (int)(Math.Round(y) - Properties.Resources.Epicenter.Height / 2.0f));
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
