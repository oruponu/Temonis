using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Temonis.MainWindow;

namespace Temonis
{
    public static class EqInfo
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private static readonly IReadOnlyDictionary<string, string> CityAbbreviation = new Dictionary<string, string>
        {
            ["渡島北斗市"] = "北斗市",
            ["渡島森町"] = "森町",
            ["渡島松前町"] = "松前町",
            ["檜山江差町"] = "江差町",
            ["上川中川町"] = "中川町",
            ["上川地方上川町"] = "上川町",
            ["宗谷枝幸町"] = "枝幸町",
            ["胆振伊達市"] = "伊達市",
            ["日高地方日高町"] = "日高町",
            ["十勝清水町"] = "清水町",
            ["十勝池田町"] = "池田町",
            ["十勝大樹町"] = "大樹町",
            ["青森南部町"] = "南部町",
            ["岩手洋野町"] = "洋野町",
            ["宮城加美町"] = "加美町",
            ["宮城美里町"] = "美里町",
            ["宮城川崎町"] = "川崎町",
            ["秋田美郷町"] = "美郷町",
            ["山形金山町"] = "金山町",
            ["山形朝日町"] = "朝日町",
            ["山形川西町"] = "川西町",
            ["山形小国町"] = "小国町",
            ["福島伊達市"] = "伊達市",
            ["福島広野町"] = "広野町",
            ["福島金山町"] = "金山町",
            ["福島昭和村"] = "昭和村",
            ["茨城古河市"] = "古河市",
            ["茨城鹿嶋市"] = "鹿嶋市",
            ["栃木さくら市"] = "さくら市",
            ["栃木那珂川町"] = "那珂川町",
            ["群馬高山村"] = "高山村",
            ["群馬昭和村"] = "昭和村",
            ["群馬上野村"] = "上野村",
            ["群馬南牧村"] = "南牧村",
            ["群馬明和町"] = "明和町",
            ["埼玉美里町"] = "美里町",
            ["埼玉神川町"] = "神川町",
            ["埼玉三芳町"] = "三芳町",
            ["千葉佐倉市"] = "佐倉市",
            ["東京府中市"] = "府中市",
            ["東京利島村"] = "利島村",
            ["神奈川大井町"] = "大井町",
            ["富山朝日町"] = "朝日町",
            ["福井坂井市"] = "坂井市",
            ["福井池田町"] = "池田町",
            ["福井若狭町"] = "若狭町",
            ["山梨北杜市"] = "北杜市",
            ["山梨南部町"] = "南部町",
            ["長野池田町"] = "池田町",
            ["長野高山村"] = "高山村",
            ["長野川上村"] = "川上村",
            ["長野南牧村"] = "南牧村",
            ["長野高森町"] = "高森町",
            ["岐阜山県市"] = "山県市",
            ["岐阜池田町"] = "池田町",
            ["静岡清水町"] = "清水町",
            ["静岡森町"] = "森町",
            ["愛知津島市"] = "津島市",
            ["愛知江南市"] = "江南市",
            ["愛知みよし市"] = "みよし市",
            ["愛知美浜町"] = "美浜町",
            ["三重朝日町"] = "朝日町",
            ["三重明和町"] = "明和町",
            ["三重大紀町"] = "大紀町",
            ["三重紀北町"] = "紀北町",
            ["三重御浜町"] = "御浜町",
            ["滋賀日野町"] = "日野町",
            ["大阪和泉市"] = "和泉市",
            ["大阪岬町"] = "岬町",
            ["大阪太子町"] = "太子町",
            ["兵庫香美町"] = "香美町",
            ["兵庫稲美町"] = "稲美町",
            ["兵庫神河町"] = "神河町",
            ["兵庫太子町"] = "太子町",
            ["奈良川西町"] = "川西町",
            ["奈良川上村"] = "川上村",
            ["和歌山広川町"] = "広川町",
            ["和歌山美浜町"] = "美浜町",
            ["和歌山日高町"] = "日高町",
            ["和歌山印南町"] = "印南町",
            ["鳥取若桜町"] = "若桜町",
            ["鳥取南部町"] = "南部町",
            ["鳥取日野町"] = "日野町",
            ["島根美郷町"] = "美郷町",
            ["岡山美咲町"] = "美咲町",
            ["広島三次市"] = "三次市",
            ["広島府中市"] = "府中市",
            ["徳島三好市"] = "三好市",
            ["愛媛松前町"] = "松前町",
            ["愛媛鬼北町"] = "鬼北町",
            ["高知香南市"] = "香南市",
            ["高知津野町"] = "津野町",
            ["福岡古賀市"] = "古賀市",
            ["福岡川崎町"] = "川崎町",
            ["福岡広川町"] = "広川町",
            ["佐賀鹿島市"] = "鹿島市",
            ["長崎対馬市"] = "対馬市",
            ["熊本小国町"] = "小国町",
            ["熊本高森町"] = "高森町",
            ["熊本美里町"] = "美里町",
            ["宮崎都農町"] = "都農町",
            ["宮崎美郷町"] = "美郷町",
            ["鹿児島出水市"] = "出水市",
            ["鹿児島十島村"] = "十島村"
        };
        private static string _prevInfo;
        private static string _prevId;

        public static string Id { get; set; }

        public static double EpicenterX { get; set; }

        public static double EpicenterY { get; set; }

        /// <summary>
        /// 地震情報を取得
        /// </summary>
        /// <returns></returns>
        public static async Task UpdateAsync()
        {
            var html = "";
            try
            {
                var response = await HttpClient.GetAsync(Properties.Resources.EqInfoUri);
                if (!response.IsSuccessStatusCode)
                    return;
                html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                WriteLog(ex);
            }

            if (string.IsNullOrEmpty(html))
                return;

            var info = Regex.Match(html, "<div id=\"eqinfdtl\" class=\"tracked_mods\">.+?</div>", RegexOptions.Singleline).Value;
            if (!IsUpdated(info))
                return;

            // 発生時刻
            var dateTime = Regex.Match(info, @"<.+>発生時刻(</.+>)+\n(<.+>)+(.+?)</.+>").Groups[3].Value.Replace("ごろ", "");
            if (dateTime == "---")
                return;
            MainWindow.DataContext.EqInfo.DateTime = DateTime.TryParse(dateTime, new CultureInfo("ja-JP"), DateTimeStyles.AssumeLocal, out var arrivalTime) ? arrivalTime.ToString("yyyy年MM月dd日 HH時mm分") : "---";

            // 震源地
            var epicenter = Regex.Match(info, @"<.+>震源地(</.+>)+\n(<.+>)+<.+?>(.+?)</.+>").Groups[3].Value;
            epicenter += Regex.Match(info, @"<.+>震源地(</.+>)+\n(<.+>)+<.+?>(.+?)</.+>(.+?)(</.+>){2}").Groups[4].Value;
            MainWindow.DataContext.EqInfo.Epicenter = epicenter.Replace('/', '／');

            // 震源の緯度
            var latitude = .0;
            var latLon = Regex.Match(info, @"<.+>緯度/経度(</.+>)+\n(<.+>)+(.+?)</.+>").Groups[3].Value.Split('/');
            if (latLon[0] != "---")
            {
                latLon[0] = latLon[0].TrimEnd('度');
                if (latLon[0].StartsWith("北緯"))
                    double.TryParse(latLon[0].Replace("北緯", ""), out latitude);
                else
                    double.TryParse("-" + latLon[0].Replace("南緯", ""), out latitude);
            }

            // 震源の経度
            var longitude = .0;
            if (latLon.Length > 1 && latLon[1] != "---")
            {
                latLon[1] = latLon[1].TrimEnd('度');
                if (latLon[1].StartsWith("東経"))
                    double.TryParse(latLon[1].Replace("東経", ""), out longitude);
                else
                    double.TryParse('-' + latLon[1].Replace("西経", ""), out longitude);
            }

            // 震源の深さ
            MainWindow.DataContext.EqInfo.Depth = Regex.Match(info, @"<.+>深さ(</.+>)+\n\n(<.+>)+(.+?)</.+>").Groups[3].Value;

            // マグニチュード
            MainWindow.DataContext.EqInfo.Magnitude = Regex.Match(info, @"<.+>マグニチュード(</.+>)+\n(<.+>)+(.+?)</.+>").Groups[3].Value;

            // 付加文
            var comment = Regex.Match(info, @"<.+>情報(</.+>)+\n(<.+?>)+(.+?)</?.+>").Groups[3].Value;
            if (comment.Count(text => text == '。') == 2)  // 付加文の情報が2つの場合は2行に分割
                comment = comment.Replace("。", "。\n").TrimEnd('\n');

            MainWindow.DataContext.EqInfo.Comment = comment;

            // 各地の震度
            var intensity = Regex.Match(info, @"<.+class=""yjw_table"">(.+?)</\w+>\n</div>", RegexOptions.Singleline).Groups[1].Value.Replace("\n", "");
            intensity = Regex.Replace(intensity, @"<\w+ \S+><\w+ \S+ \w+>(<\w+>)+", "[");
            intensity = Regex.Replace(intensity, @"(</\w+>)+<\w+ \S+><\w+>", "]");
            intensity = Regex.Replace(intensity, @"<\w+ \S+><\w+ \S+ \S+ \S+><\w+>震度", "");
            intensity = Regex.Replace(intensity, @"(</\w+>)+<\w+ \S+ \S+><.+?>", ":");
            intensity = Regex.Replace(intensity, @"　<\w+>(</\w+>){3}", "");
            intensity = Regex.Replace(intensity, @"(</\w+>)+", ",");
            intensity = intensity.TrimEnd(',');

            var maxInt = "";
            if (intensity != "")
            {
                SetDataGridContext(intensity);
                maxInt = intensity.Split(',')[0].Split(':')[0];
            }
            else
            {
                MainWindow.DataContext.EqInfo.IntensityList = new List<Intensity>();
            }

            // 地震ID
            Id = Regex.Match(html, "<a href=\"/weather/jp/earthquake/(.+).html\">").Groups[1].Value;

            SetEpicenterPoint(latitude, longitude);

            UpdateState(maxInt);
        }

        /// <summary>
        /// ページの更新を確認します。
        /// </summary>
        /// <param name="current">現在保持している HTML</param>
        /// <returns></returns>
        private static bool IsUpdated(string current)
        {
            if (_prevInfo == current)
                return false;
            _prevInfo = current;
            return true;
        }

        private static void SetDataGridContext(string text)
        {
            var intensity = new List<Intensity>();
            foreach (var item in text.Split(','))
            {
                var split = item.Split(':');
                var maxInt = "震度" + split[0];
                var maxIntVisible = true;
                var prefNameVisible = true;
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
                            intensity.Add(new Intensity
                            {
                                MaxInt = maxInt,
                                MaxIntVisible = maxIntVisible,
                                PrefName = prefName,
                                PrefNameVisible = prefNameVisible,
                                CityName = cities.ToString()
                            });
                            cities.Clear();
                            maxIntVisible = false;
                            prefNameVisible = false;
                        }

                        if (cities.Length > 0)
                            cities.Append('　');
                        cities.Append(cityName);
                    }

                    intensity.Add(new Intensity
                    {
                        MaxInt = maxInt,
                        MaxIntVisible = maxIntVisible,
                        PrefName = prefName,
                        PrefNameVisible = prefNameVisible,
                        CityName = cities.ToString()
                    });

                    maxIntVisible = false;
                    prefNameVisible = true;
                }
            }

            MainWindow.DataContext.EqInfo.IntensityList = intensity;
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
            if (!city.StartsWith(pref) && pref != "北海道")
                return city;
            if (city.EndsWith("区") && !city.EndsWith("２３区"))
                return pref == "東京" ? city.Replace("東京", "") : city.Contains("堺市") ? city.Replace("大阪", "") : city;
            return CityAbbreviation.TryGetValue(city, out var value) ? value : city;
        }

        /// <summary>
        /// 震央位置の画像を作成します。
        /// </summary>
        /// <param name="latitude">震源の緯度</param>
        /// <param name="longitude">震源の経度</param>
        private static void SetEpicenterPoint(double latitude, double longitude)
        {
            double lonMin;
            double pxPerDigX;
            if (latitude > 30.0 || longitude > 130.9)
            {
                lonMin = 128.6;
                pxPerDigX = Kyoshin.ImageWidth / 17.3;
            }
            else
            {
                lonMin = 122.5;
                pxPerDigX = (Kyoshin.ImageWidth - 181) / 8.4;
            }

            EpicenterX = (longitude - lonMin) * pxPerDigX;

            double latMin;
            double pxPerDigY;
            if (latitude > 30.0 || longitude > 130.9)
            {
                latMin = 30.0;
                pxPerDigY = Kyoshin.ImageHeight / 16.0;
                EpicenterY = Kyoshin.ImageHeight - (latitude - latMin) * pxPerDigY;
            }
            else
            {
                latMin = 23.6;
                pxPerDigY = (Kyoshin.ImageHeight - 244) / 6.4;
                EpicenterY = Kyoshin.ImageHeight - 193 - (latitude - latMin) * pxPerDigY;
            }
        }

        private static void UpdateState(string maxInt)
        {
            if (maxInt.Contains('弱') || maxInt.Contains('強') || maxInt.Contains('7') || MainWindow.DataContext.EqInfo.Comment.Contains("津波警報"))
                MainWindow.DataContext.EqInfo.Level = Level.Red;
            else if (maxInt.Contains('3') || maxInt.Contains('4'))
                MainWindow.DataContext.EqInfo.Level = Level.Yellow;
            else
                MainWindow.DataContext.EqInfo.Level = Level.White;

            if (!string.IsNullOrEmpty(_prevId))
                Sound.PlayEqInfoAsync(maxInt);

            SetActive();

            _prevId = Id;
        }

        public class Intensity : INotifyPropertyChanged
        {
            private static readonly PropertyChangedEventArgs MaxIntPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(MaxInt));
            private static readonly PropertyChangedEventArgs MaxIntVisiblePropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(MaxIntVisible));
            private static readonly PropertyChangedEventArgs PrefNamePropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(PrefName));
            private static readonly PropertyChangedEventArgs PrefNameVisiblePropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(PrefNameVisible));
            private static readonly PropertyChangedEventArgs CityNamePropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(CityName));
            private string _maxInt;
            private bool _maxIntVisible;
            private string _prefName;
            private bool _prefNameVisible;
            private string _cityName;

            public event PropertyChangedEventHandler PropertyChanged;

            public string MaxInt
            {
                get => _maxInt;
                set
                {
                    _maxInt = value;
                    PropertyChanged?.Invoke(this, MaxIntPropertyChangedEventArgs);
                }
            }

            public bool MaxIntVisible
            {
                get => _maxIntVisible;
                set
                {
                    _maxIntVisible = value;
                    PropertyChanged?.Invoke(this, MaxIntVisiblePropertyChangedEventArgs);
                }
            }

            public string PrefName
            {
                get => _prefName;
                set
                {
                    _prefName = value;
                    PropertyChanged?.Invoke(this, PrefNamePropertyChangedEventArgs);
                }
            }

            public bool PrefNameVisible
            {
                get => _prefNameVisible;
                set
                {
                    _prefNameVisible = value;
                    PropertyChanged?.Invoke(this, PrefNameVisiblePropertyChangedEventArgs);
                }
            }

            public string CityName
            {
                get => _cityName;
                set
                {
                    _cityName = value;
                    PropertyChanged?.Invoke(this, CityNamePropertyChangedEventArgs);
                }
            }
        }

        public class DataContext : INotifyPropertyChanged
        {
            private static readonly PropertyChangedEventArgs LevelPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Level));
            private static readonly PropertyChangedEventArgs DateTimePropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(DateTime));
            private static readonly PropertyChangedEventArgs EpicenterPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Epicenter));
            private static readonly PropertyChangedEventArgs DepthPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Depth));
            private static readonly PropertyChangedEventArgs MagnitudePropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Magnitude));
            private static readonly PropertyChangedEventArgs CommentPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Comment));
            private static readonly PropertyChangedEventArgs IntensityPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(IntensityList));
            private Level _level;
            private string _dateTime;
            private string _epicenter;
            private string _depth;
            private string _magnitude;
            private string _comment;
            private List<Intensity> _intensityList;

            public event PropertyChangedEventHandler PropertyChanged;

            public Level Level
            {
                get => _level;
                set
                {
                    _level = value;
                    PropertyChanged?.Invoke(this, LevelPropertyChangedEventArgs);
                }
            }

            public string DateTime
            {
                get => _dateTime;
                set
                {
                    _dateTime = value;
                    PropertyChanged?.Invoke(this, DateTimePropertyChangedEventArgs);
                }
            }

            public string Epicenter
            {
                get => _epicenter;
                set
                {
                    _epicenter = value;
                    PropertyChanged?.Invoke(this, EpicenterPropertyChangedEventArgs);
                }
            }

            public string Depth
            {
                get => _depth;
                set
                {
                    _depth = value;
                    PropertyChanged?.Invoke(this, DepthPropertyChangedEventArgs);
                }
            }

            public string Magnitude
            {
                get => _magnitude;
                set
                {
                    _magnitude = value;
                    PropertyChanged?.Invoke(this, MagnitudePropertyChangedEventArgs);
                }
            }

            public string Comment
            {
                get => _comment;
                set
                {
                    _comment = value;
                    PropertyChanged?.Invoke(this, CommentPropertyChangedEventArgs);
                }
            }

            public List<Intensity> IntensityList
            {
                get => _intensityList;
                set
                {
                    _intensityList = value;
                    PropertyChanged?.Invoke(this, IntensityPropertyChangedEventArgs);
                }
            }
        }
    }
}
