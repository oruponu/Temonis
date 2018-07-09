﻿using System;
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
                InternalLog(ex);
            }

            if (string.IsNullOrEmpty(html)) return;

            var info = Regex.Match(html, "<div id=\"eqinfdtl\" class=\"tracked_mods\">.+?</div>", RegexOptions.Singleline).Value;
            if (!IsUpdated(info)) return;

            // 発生時刻
            var time = Regex.Match(info, @"<.+>発生時刻(</.+>)+\n(<.+>)+(.+?)</.+>").Groups[3].Value.Replace("ごろ", "");
            Instance.Label_EqInfoDateTime.Text = DateTime.TryParse(time, new CultureInfo("ja-JP"), DateTimeStyles.AssumeLocal, out var arrivalTime) ? arrivalTime.ToString("yyyy年MM月dd日 HH時mm分") : "---";

            // 震源地
            var epicenter = Regex.Match(info, @"<.+>震源地(</.+>)+\n(<.+>)+<.+?>(.+?)</.+>").Groups[3].Value;
            epicenter += Regex.Match(info, @"<.+>震源地(</.+>)+\n(<.+>)+<.+?>(.+?)</.+>(.+?)(</.+>){2}").Groups[4].Value;
            Epicenter = epicenter;
            Instance.Label_EqInfoEpicenter.Text = epicenter;

            // 緯度
            var latitude = 0.0f;
            var latLon = Regex.Match(info, @"<.+>緯度/経度(</.+>)+\n(<.+>)+(.+?)</.+>").Groups[3].Value.Split('/');
            if (latLon[0] != "---")
            {
                latLon[0] = latLon[0].Replace("度", "");
                if (latLon[0].Contains("北緯"))
                {
                    float.TryParse(latLon[0].Replace("北緯", ""), out latitude);
                }
                else
                {
                    float.TryParse("-" + latLon[0].Replace("南緯", ""), out latitude);
                }
            }

            // 経度
            var longitude = 0.0f;
            if (latLon.Length > 1 && latLon[1] != "---")
            {
                latLon[1] = latLon[1].Replace("度", "");
                if (latLon[1].Contains("東経"))
                {
                    float.TryParse(latLon[1].Replace("東経", ""), out longitude);
                }
                else
                {
                    float.TryParse('-' + latLon[1].Replace("西経", ""), out longitude);
                }
            }

            // 深さ
            Instance.Label_EqInfoDepth.Text = Regex.Match(info, @"<.+>深さ(</.+>)+\n\n(<.+>)+(.+?)</.+>").Groups[3].Value;

            // マグニチュード
            Instance.Label_EqInfoMagnitude.Text = Regex.Match(info, @"<.+>マグニチュード(</.+>)+\n(<.+>)+(.+?)</.+>").Groups[3].Value;

            // 情報
            var comment = Regex.Match(info, @"<.+>情報(</.+>)+\n(<.+?>)+(.+?)</?.+>").Groups[3].Value;
            Instance.Label_EqInfoComment.Font = new Font(Instance.Label_EqInfoComment.Font.FontFamily, 12f);
            if (comment.Length > 32)
            {
                Instance.Label_EqInfoComment.Font = new Font(Instance.Label_EqInfoComment.Font.FontFamily, 11f);
            }

            if (comment.Count(x => x == '。') == 2)  // 付加文の情報が2つの場合は2行に分割
            {
                comment = comment.Replace("。", "。\n");
            }

            Instance.Label_EqInfoComment.Text = comment;

            // 各地の震度
            var intensity = Regex.Match(info, @"<.+class=""yjw_table"">(.+?)</\w+>\n</div>", RegexOptions.Singleline).Groups[1].Value.Replace("\n", "");
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

            CreateEpicenterImage(latitude, longitude);
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
                        if (cities.Length + ('　' + cityName).Length > 28)
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
                            cities.Append('　');
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
        /// 市町村名を省略します。
        /// </summary>
        /// <param name="city">市町村名</param>
        /// <param name="pref">属する都道府県</param>
        /// <returns></returns>
        private static string AbbreviateCityName(string city, string pref)
        {
            pref = pref.TrimEnd('都').TrimEnd('府').TrimEnd('県');
            if (!city.StartsWith(pref) && pref != "北海道") return city;
            if (city.EndsWith("区") && !city.EndsWith("２３区") && !city.Contains("堺市"))
            {
                return pref == "東京" ? city.Replace("東京", "") : city;
            }

            return Utility.EqInfo.CityAbbreviation.TryGetValue(city, out var value) ? value : city;
        }

        /// <summary>
        /// 震央位置の画像を作成します。
        /// </summary>
        /// <param name="latitude">震央の緯度</param>
        /// <param name="longitude">震央の経度</param>
        private static void CreateEpicenterImage(float latitude, float longitude)
        {
            float lonMin;
            float pxPerDigX;
            if (latitude > 30.0f || longitude > 130.9f)
            {
                lonMin = 128.6f;
                pxPerDigX = Instance.PictureBox_KyoshinMap.Width / 17.3f;
            }
            else
            {
                lonMin = 122.5f;
                pxPerDigX = (Instance.PictureBox_KyoshinMap.Width - 181) / 8.4f;
            }

            var x = (longitude - lonMin) * pxPerDigX;
            float latMin;
            float pxPerDigY;
            float y;
            if (latitude > 30.0f || longitude > 130.9f)
            {
                latMin = 30.0f;
                pxPerDigY = Instance.PictureBox_KyoshinMap.Height / 16.0f;
                y = Instance.PictureBox_KyoshinMap.Height - (latitude - latMin) * pxPerDigY;
            }
            else
            {
                latMin = 23.6f;
                pxPerDigY = (Instance.PictureBox_KyoshinMap.Height - 244) / 6.4f;
                y = Instance.PictureBox_KyoshinMap.Height - 193 - (latitude - latMin) * pxPerDigY;
            }

            EpicenterBitmap = new Bitmap(Instance.PictureBox_KyoshinMap.Width, Instance.PictureBox_KyoshinMap.Height);
            using (var graphicsEpicenter = Graphics.FromImage(EpicenterBitmap))
            {
                graphicsEpicenter.DrawImage(Properties.Resources.Epicenter, (int)(Math.Round(x) - Properties.Resources.Epicenter.Width / 2.0f), (int)(Math.Round(y) - Properties.Resources.Epicenter.Height / 2.0f));
            }
        }

        /// <summary>
        /// ページの更新を確認します。
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
