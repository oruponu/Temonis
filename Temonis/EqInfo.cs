using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Temonis.MainWindow;

namespace Temonis
{
    public static class EqInfo
    {
        private static readonly HttpClient HttpClient = new(new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.All
        });
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
        private static string _prevId = "";

        public static string Id { get; private set; }

        public static double EpicenterX { get; private set; }

        public static double EpicenterY { get; private set; }

        /// <summary>
        /// 地震情報を取得
        /// </summary>
        /// <returns></returns>
        public static async Task<Report> RequestAsync(string uri)
        {
            try
            {
                var response = await HttpClient.GetAsync(uri);
                if (!response.IsSuccessStatusCode)
                    return null;
                var serializer = new XmlSerializer(typeof(Report));
                await using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                return (Report)serializer.Deserialize(stream);
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                WriteLog(ex);
                return null;
            }
        }

        public static void Update(Report report, string eventId = "")
        {
            var infoKind = report.Control.Title switch
            {
                "震度速報" => InfoKind.Shindo,
                "震源に関する情報" => InfoKind.Shingen,
                _ => InfoKind.ShingenShindo
            };

            // 地震ID
            Id = report.Head.EventId;

            var earthquake = report.Body.Earthquake;
            if (infoKind != InfoKind.Shindo)
            {
                // 発生時刻
                MainWindow.DataContext.EqInfo.DateTime = earthquake.ArrivalTime.ToString("yyyy年MM月dd日 HH時mm分");

                // 震源地
                var epicenter = earthquake.Hypocenter.Area.Name;
                if (earthquake.Hypocenter.Area.DetailedName is not null)
                    epicenter += $"（{earthquake.Hypocenter.Area.DetailedName}）";
                if (earthquake.Hypocenter.Area.NameFromMark is not null)
                    epicenter += $"（{earthquake.Hypocenter.Area.NameFromMark}）";
                MainWindow.DataContext.EqInfo.Epicenter = epicenter;

                // 震源の深さ
                var (latitude, longitude, depth) = earthquake.Hypocenter.Area.Coordinate.Split();
                depth = depth != "不明" ? depth : "---";
                MainWindow.DataContext.EqInfo.Depth = depth;
                SetEpicenterPoint(latitude, longitude);

                // マグニチュード
                MainWindow.DataContext.EqInfo.Magnitude = earthquake.Magnitude.Normalize();
            }
            else if (eventId.Length == 0)
            {
                MainWindow.DataContext.EqInfo.DateTime = report.Head.TargetDateTime.ToString("yyyy年MM月dd日 HH時mm分");
                MainWindow.DataContext.EqInfo.Epicenter = "---";
                MainWindow.DataContext.EqInfo.Depth = "---";
                MainWindow.DataContext.EqInfo.Magnitude = "---";
            }

            if (eventId.Length == 0)
            {
                // 付加文
                MainWindow.DataContext.EqInfo.Comment = report.Body.Comments.ForecastComment.Text;
            }

            // 各地の震度
            var intensity = report.Body.Intensity;
            if (intensity is not null)
            {
                SetDataGridContext(infoKind, intensity.Observation);
                UpdateState(intensity.Observation.MaxInt, infoKind);
            }
            else if (eventId.Length == 0)
            {
                MainWindow.DataContext.EqInfo.IntensityList = new List<IntensityLine>();
                UpdateState(report.Head.Title == "遠地地震に関する情報" ? "D" : "", infoKind);
            }
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

        private static void SetDataGridContext(InfoKind infoKind, Observation observation)
        {
            var prefList = observation.Pref.Select(pref => new
            {
                pref.Name,
                Code = int.Parse(pref.Code),
                Area = pref.Area.OrderBy(area => int.Parse(area.Code)).Select(area => new
                {
                    area.Name,
                    Int = area.MaxInt
                }),
                City = pref.Area.SelectMany(area => area.City).OrderBy(city => int.Parse(city.Code)).Select(city => new
                {
                    Name = AbbreviateCityName(city.Name, pref.Name),
                    Int = city.MaxInt
                }),
                Station = pref.Area.SelectMany(area => area.City).SelectMany(city => city.IntensityStation.Select(station => new
                {
                    Name = station.Name.Replace(city.Name, AbbreviateCityName(city.Name, pref.Name)),
                    station.Code,
                    station.Int
                })).OrderBy(station => int.Parse(station.Code)).Select(station => new
                {
                    Name = station.Name.TrimEnd('＊'),
                    station.Int
                })
            }).OrderBy(pref => pref.Code).ToArray();

            var intensityList = new List<IntensityLine>();
            foreach (var intensity in new[] { "7", "6+", "6-", "5+", "5-", "4", "3", "2", "1" })
            {
                var maxIntVisible = true;
                var prefNameVisible = true;
                foreach (var pref in prefList)
                {
                    var cities = new StringBuilder();
                    foreach (var city in infoKind == InfoKind.Shindo ? pref.Area : Settings.JsonClass.Appearance.ShowIntensityStation ? pref.Station : pref.City)
                    {
                        if (city.Int != intensity)
                            continue;
                        if (cities.Length + ("　" + city.Name).Length > 28)
                        {
                            intensityList.Add(new IntensityLine
                            {
                                MaxInt = NormalizeIntensity(intensity),
                                MaxIntVisible = maxIntVisible,
                                PrefName = pref.Name,
                                PrefNameVisible = prefNameVisible,
                                CityName = cities.ToString()
                            });
                            cities.Clear();
                            maxIntVisible = false;
                            prefNameVisible = false;
                        }

                        if (cities.Length > 0)
                            cities.Append('　');
                        cities.Append(city.Name);
                    }

                    if (cities.Length == 0)
                        continue;

                    intensityList.Add(new IntensityLine
                    {
                        MaxInt = NormalizeIntensity(intensity),
                        MaxIntVisible = maxIntVisible,
                        PrefName = pref.Name,
                        PrefNameVisible = prefNameVisible,
                        CityName = cities.ToString()
                    });

                    maxIntVisible = false;
                    prefNameVisible = true;
                }
            }

            MainWindow.DataContext.EqInfo.IntensityList = intensityList;
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

        private static string NormalizeIntensity(string str)
        {
            if (str.Length == 0)
                return str;
            str = "震度" + str;
            if (str.Contains('-'))
                return str.Replace('-', '弱');
            return str.Contains('+') ? str.Replace('+', '強') : str;
        }

        private static void UpdateState(string maxInt, InfoKind infoKind)
        {
            if (maxInt.Contains('-') || maxInt.Contains('+') || maxInt.Contains('7') || MainWindow.DataContext.EqInfo.Comment.Contains("津波警報"))
                MainWindow.DataContext.EqInfo.Level = Level.Red;
            else if (maxInt.Contains('3') || maxInt.Contains('4'))
                MainWindow.DataContext.EqInfo.Level = Level.Yellow;
            else
                MainWindow.DataContext.EqInfo.Level = Level.White;

            if (infoKind != InfoKind.Shingen && maxInt.Length != 0 && _prevId.Length != 0)
            {
                Sound.PlayEqInfo(maxInt);
                SetActive();
            }

            _prevId = Id;
        }

        public class IntensityLine : BindableBase
        {
            private readonly string _maxInt;
            private readonly bool _maxIntVisible;
            private readonly string _prefName;
            private readonly bool _prefNameVisible;
            private readonly string _cityName;

            public string MaxInt
            {
                get => _maxInt;
                init => SetProperty(ref _maxInt, value);
            }

            public bool MaxIntVisible
            {
                get => _maxIntVisible;
                init => SetProperty(ref _maxIntVisible, value);
            }

            public string PrefName
            {
                get => _prefName;
                init => SetProperty(ref _prefName, value);
            }

            public bool PrefNameVisible
            {
                get => _prefNameVisible;
                init => SetProperty(ref _prefNameVisible, value);
            }

            public string CityName
            {
                get => _cityName;
                init => SetProperty(ref _cityName, value);
            }
        }

        public class DataContext : BindableBase
        {
            private Level _level;
            private string _dateTime;
            private string _epicenter;
            private string _depth;
            private string _magnitude;
            private string _comment;
            private List<IntensityLine> _intensityList;

            public Level Level
            {
                get => _level;
                set => SetProperty(ref _level, value);
            }

            public string DateTime
            {
                get => _dateTime;
                set => SetProperty(ref _dateTime, value);
            }

            public string Epicenter
            {
                get => _epicenter;
                set => SetProperty(ref _epicenter, value);
            }

            public string Depth
            {
                get => _depth;
                set => SetProperty(ref _depth, value);
            }

            public string Magnitude
            {
                get => _magnitude;
                set => SetProperty(ref _magnitude, value);
            }

            public string Comment
            {
                get => _comment;
                set => SetProperty(ref _comment, value);
            }

            public List<IntensityLine> IntensityList
            {
                get => _intensityList;
                set => SetProperty(ref _intensityList, value);
            }
        }

        [XmlRoot(Namespace = "http://xml.kishou.go.jp/jmaxml1/")]
        public class Report
        {
            public Control Control { get; init; }

            [XmlElement(Namespace = "http://xml.kishou.go.jp/jmaxml1/informationBasis1/")]
            public Head Head { get; init; }

            [XmlElement(Namespace = "http://xml.kishou.go.jp/jmaxml1/body/seismology1/")]
            public Body Body { get; init; }
        }

        public class Control
        {
            public string Title { get; init; }
        }

        public class Head
        {
            public string Title { get; init; }

            public DateTime TargetDateTime { get; init; }

            [XmlElement(ElementName = "EventID")]
            public string EventId { get; init; }
        }

        public class Body
        {
            public Earthquake Earthquake { get; init; }

            public Intensity Intensity { get; init; }

            public Comments Comments { get; init; }
        }

        public class Earthquake
        {
            public DateTime ArrivalTime { get; init; }

            public Hypocenter Hypocenter { get; init; }

            [XmlElement(Namespace = "http://xml.kishou.go.jp/jmaxml1/elementBasis1/")]
            public Magnitude Magnitude { get; init; }
        }

        public class Hypocenter
        {
            public HypoArea Area { get; init; }
        }

        public class HypoArea
        {
            public string Name { get; init; }

            [XmlElement(Namespace = "http://xml.kishou.go.jp/jmaxml1/elementBasis1/")]
            public Coordinate Coordinate { get; init; }

            public string DetailedName { get; init; }

            public string NameFromMark { get; init; }
        }

        public class Coordinate
        {
            [XmlText]
            public string Value { get; init; }

            public (float latitude, float longitude, string depth) Split()
            {
                var isoStr = Value.Remove(Value.Length - 1);
                var parts = isoStr.Split(new[] { '+', '-' }, StringSplitOptions.None);

                // 震源の緯度
                var latitude = float.Parse(parts[1]);
                if (isoStr[0] == '-')
                    latitude = -latitude;

                // 震源の経度
                var longitude = float.Parse(parts[2]);
                if (isoStr[parts[1].Length + 1] == '-')
                    longitude = -longitude;

                // 震源の深さ
                string depth;
                if (parts.Length >= 4)
                {
                    depth = parts[3] switch
                    {
                        "0" => "ごく浅い",
                        "700000" => "700km以上",
                        _ => parts[3].Remove(parts[3].Length - 3, 3) + "km"
                    };
                }
                else
                {
                    depth = "不明";
                }

                return (latitude, longitude, depth);
            }
        }

        public class Magnitude
        {
            [XmlAttribute(AttributeName = "description")]
            public string Description { get; init; }

            public string Normalize()
            {
                if (Description.Length == 0)
                    return "---";

                var charArray = Description.Replace("Ｍ", "").ToCharArray();
                for (var i = 0; i < charArray.Length; i++)
                {
                    var ch = charArray[i];
                    if ('０' <= ch && ch <= '９')
                    {
                        charArray[i] = (char)(ch - 0xFEE0);
                    }
                    else if (ch == '．')
                    {
                        charArray[i] = '.';
                    }
                }

                return new string(charArray);
            }
        }

        public class Intensity
        {
            public Observation Observation { get; init; }
        }

        public class Observation
        {
            public string MaxInt { get; init; }

            [XmlElement]
            public Pref[] Pref { get; init; }
        }

        public class Pref
        {
            public string Name { get; init; }

            public string Code { get; init; }

            [XmlElement]
            public Area[] Area { get; init; }
        }

        public class Area
        {
            public string Name { get; init; }

            public string Code { get; init; }

            public string MaxInt { get; init; }

            [XmlElement]
            public City[] City { get; init; }
        }

        public class City
        {
            public string Name { get; init; }

            public string Code { get; init; }

            public string MaxInt { get; init; }

            [XmlElement]
            public IntensityStation[] IntensityStation { get; init; }
        }

        public class IntensityStation
        {
            public string Name { get; init; }

            public string Code { get; init; }

            public string Int { get; init; }
        }

        public class Comments
        {
            public ForecastComment ForecastComment { get; init; }
        }

        public class ForecastComment
        {
            public string Text { get; init; }
        }

        private enum InfoKind
        {
            Shindo,
            Shingen,
            ShingenShindo
        }
    }
}
