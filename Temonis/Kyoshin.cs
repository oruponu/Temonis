using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static Temonis.MainWindow;

namespace Temonis
{
    public static class Kyoshin
    {
        public static readonly double ImageWidth = (double)Instance.FindResource("ImageWidth");
        public static readonly double ImageHeight = (double)Instance.FindResource("ImageHeight");

        /// <summary>
        /// 観測点リスト
        /// </summary>
        private static readonly IReadOnlyList<Station> Stations = Encoding.UTF8.GetString(DecompressResource(Properties.Resources.Stations, 48141)).TrimEnd('\0').Split('\n').Select(line => new Station
        {
            IsEnabled = Convert.ToBoolean(int.Parse(line.Split(',')[0])),
            Name = line.Split(',')[1],
            PrefName = line.Split(',')[2],
            X = int.Parse(line.Split(',')[3]),
            Y = int.Parse(line.Split(',')[4])
        }).ToList();

        /// <summary>
        /// 地図の種類
        /// </summary>
        private static readonly IReadOnlyList<string> MapType = new[]
        {
            "jma",
            "acmap",
            "vcmap",
            "dcmap",
            "rsp0125",
            "rsp0250",
            "rsp0500",
            "rsp1000",
            "rsp2000",
            "rsp4000"
        };

        /// <summary>
        /// リアルタイム震度の色
        /// </summary>
        private static readonly IReadOnlyDictionary<int, double> ColorMap = new Dictionary<int, double>
        {
            [205] = -3.0,
            [258] = -2.9,
            [410] = -2.8,
            [659] = -2.7,
            [1007] = -2.6,
            [1523] = -2.5,
            [2080] = -2.4,
            [2736] = -2.3,
            [3489] = -2.2,
            [4341] = -2.1,
            [5434] = -2.0,
            [7463] = -1.9,
            [10028] = -1.8,
            [12760] = -1.7,
            [16081] = -1.6,
            [19794] = -1.5,
            [23592] = -1.4,
            [28061] = -1.3,
            [32561] = -1.2,
            [37786] = -1.1,
            [43403] = -1.0,
            [45290] = -0.9,
            [48505] = -0.8,
            [54345] = -0.7,
            [65905] = -0.6,
            [81871] = -0.5,
            [105030] = -0.4,
            [141432] = -0.3,
            [183152] = -0.2,
            [235703] = -0.1,
            [312601] = 0.0,
            [484424] = 0.1,
            [744017] = 0.2,
            [1063042] = 0.3,
            [1505935] = 0.4,
            [2016662] = 0.5,
            [2691604] = 0.6,
            [3506984] = 0.7,
            [4394776] = 0.8,
            [5516308] = 0.9,
            [6816306] = 1.0,
            [7479401] = 1.1,
            [8306933] = 1.2,
            [9063436] = 1.3,
            [10002898] = 1.4,
            [11006078] = 1.5,
            [11916872] = 1.6,
            [13042903] = 1.7,
            [14062039] = 1.8,
            [15318018] = 1.9,
            [16646400] = 2.0,
            [16450065] = 2.1,
            [16448568] = 2.2,
            [16446600] = 2.3,
            [16445145] = 2.4,
            [16638019] = 2.5,
            [16441820] = 2.6,
            [16634736] = 2.7,
            [16438593] = 2.8,
            [16631551] = 2.9,
            [16630216] = 3.0,
            [16432433] = 3.1,
            [16429089] = 3.2,
            [16425873] = 3.3,
            [16423164] = 3.4,
            [16614499] = 3.5,
            [16417340] = 3.6,
            [16609264] = 3.7,
            [16412345] = 3.8,
            [16604176] = 3.9,
            [16602111] = 4.0,
            [16405560] = 4.1,
            [16403448] = 4.2,
            [16401705] = 4.3,
            [16399833] = 4.4,
            [16592611] = 4.5,
            [16396668] = 4.6,
            [16589475] = 4.7,
            [16393953] = 4.8,
            [16587000] = 4.9,
            [16585999] = 5.0,
            [16390785] = 5.1,
            [16197193] = 5.2,
            [16005217] = 5.3,
            [15814851] = 5.4,
            [15626089] = 5.5,
            [15438978] = 5.6,
            [15253392] = 5.7,
            [15069392] = 5.8,
            [14886972] = 5.9,
            [14706125] = 6.0,
            [13481272] = 6.1,
            [12167000] = 6.2,
            [11089567] = 6.3,
            [9938375] = 6.4,
            [8998912] = 6.5,
            [8000000] = 6.6,
            [7077888] = 6.7,
            [6331625] = 6.8,
            [5545233] = 6.9
        };
        private static readonly IReadOnlyList<Color> RealTimeReplaceColor = new List<Color>
        {
            Color.FromRgb(0, 0, 0),
            ((SolidColorBrush)Instance.FindResource("Black")).Color
        };
        private static readonly IReadOnlyList<Color> PsWaveReplaceColor = new List<Color>
        {
            Color.FromRgb(0, 0, 255),
            ((SolidColorBrush)Instance.FindResource("Blue")).Color,
            Color.FromRgb(255, 0, 0),
            ((SolidColorBrush)Instance.FindResource("Red")).Color
        };
        private static readonly Color[] Colors = new Color[256];
        private static readonly Intensity Observation = new Intensity
        {
            Prefs = new List<Intensity.Pref>(47),
            Stations = new List<Intensity.Station>(Stations.Count())
        };
        private static bool _isTriggerWait;
        private static int _offTriggerTime;
        private static int _maxInt;
        private static bool _prevTriggerOn;
        private static bool _prevTriggerWait;
        private static int _prevMaxInt;
        private static WriteableBitmap _readTimeBitmap;

        public static bool IsTriggerOn { get; private set; }

        public static async Task<bool> UpdateAsync()
        {
            var isSucceeded = false;
            var time = LatestTime.ToString("yyyyMMdd/yyyyMMddHHmmss");
            var visual = new DrawingVisual();
            using (var context = visual.RenderOpen())
            {
                var imageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/BaseMap.png", UriKind.Absolute));
                imageSource.Freeze();
                var rect = new Rect(.0, .0, ImageWidth, ImageHeight);
                context.DrawImage(imageSource, rect);

                try
                {
                    await DrawRealTimeImageAsync(context, time);
                    isSucceeded = true;
                }
                catch (Exception ex) when (ex is HttpRequestException || ex is IOException || ex is TaskCanceledException || ex is ArgumentException)
                {
                    InternalLog(ex);
                }

                try
                {
                    var source = await DownloadImageAsync($"{Properties.Resources.KyoshinUri}RealTimeImg/jma_s/{time}.jma_s.gif");
                    source.Freeze();
                    var converted = new FormatConvertedBitmap(source, PixelFormats.Bgr32, null, .0);
                    converted.Freeze();
                    _readTimeBitmap = new WriteableBitmap(converted);
                    _readTimeBitmap.Lock();
                    GetRealtimeIntensity(context);
                    _readTimeBitmap.Unlock();
                    _readTimeBitmap.Freeze();
                    isSucceeded = true;
                }
                catch (Exception ex) when (ex is HttpRequestException || ex is IOException || ex is TaskCanceledException || ex is ArgumentException)
                {
                    InternalLog(ex);
                }

                if (Eew.IsTriggerOn)
                {
                    try
                    {
                        await DrawPsWaveImageAsync(context, time);
                        isSucceeded = true;
                    }
                    catch (Exception ex) when (ex is HttpRequestException || ex is IOException || ex is TaskCanceledException || ex is ArgumentException)
                    {
                        InternalLog(ex);
                    }
                }
                else if (EqInfo.EpicenterX != default || EqInfo.EpicenterY != default)
                {
                    var image = new BitmapImage(new Uri("pack://application:,,,/Resources/Epicenter.png", UriKind.Absolute));
                    image.Freeze();
                    context.DrawImage(image, new Rect((int)(EqInfo.EpicenterX - image.Width / 2.0), (int)(EqInfo.EpicenterY - image.Height / 2.0), image.Width, image.Height));
                }

                imageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/BaseMapBorder.png", UriKind.Absolute));
                imageSource.Freeze();
                context.DrawImage(imageSource, rect);
            }

            var target = new RenderTargetBitmap((int)ImageWidth, (int)ImageHeight, 96, 96, PixelFormats.Pbgra32);
            target.Render(visual);
            target.Freeze();
            MainWindow.DataContext.Kyoshin.ImageSource = target;

            UpdateState();

            return isSucceeded;
        }

        /// <summary>
        /// 画像をダウンロードします。
        /// </summary>
        /// <param name="requestUri">要求の送信先 URI</param>
        /// <returns></returns>
        private static async Task<BitmapSource> DownloadImageAsync(string requestUri)
        {
            var bytes = await MainWindow.HttpClient.GetByteArrayAsync(requestUri).ConfigureAwait(false);
            var image = new BitmapImage();
            image.BeginInit();
            using (var stream = new MemoryStream(bytes))
            {
                image.StreamSource = stream;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
                image.Freeze();
            }

            return image;
        }

        /// <summary>
        /// 強震モニタの画像を描画します。
        /// </summary>
        /// <param name="context">描画対象のオブジェクト</param>
        /// <param name="time">取得する時刻</param>
        /// <returns></returns>
        private static async Task DrawRealTimeImageAsync(DrawingContext context, string time)
        {
            var mapType = MapType[MainWindow.DataContext.Kyoshin.ComboBoxSelectedIndex];
            var mapSb = MainWindow.DataContext.Kyoshin.RadioButton == DataContext.RadioButtonEnum.Surface ? "s" : "b";
            var image = await DownloadImageAsync($"{Properties.Resources.KyoshinUri}RealTimeImg/{mapType}_{mapSb}/{time}.{mapType}_{mapSb}.gif");
            image.Freeze();
            var rect = new Rect(.0, .0, ImageWidth, ImageHeight);
            context.DrawImage(ReplaceColorPalette(image, in RealTimeReplaceColor), rect);
        }

        /// <summary>
        /// 緊急地震速報 P波・S波到達予想円を描画します。
        /// </summary>
        /// <param name="context">描画対象のオブジェクト</param>
        /// <param name="time">取得する時刻</param>
        /// <returns></returns>
        private static async Task DrawPsWaveImageAsync(DrawingContext context, string time)
        {
            var image = await DownloadImageAsync($"{Properties.Resources.KyoshinUri}PSWaveImg/eew/{time}.eew.gif");
            image.Freeze();
            var rect = new Rect(.0, .0, ImageWidth, ImageHeight);
            context.DrawImage(ReplaceColorPalette(image, in PsWaveReplaceColor), rect);
        }

        /// <summary>
        /// 指定された色のカラーパレットを置換します。
        /// </summary>
        /// <param name="image"></param>
        /// <param name="replaceColors"></param>
        /// <returns></returns>
        private static BitmapSource ReplaceColorPalette(BitmapSource image, in IReadOnlyList<Color> replaceColors)
        {
            for (var i = 0; i < Colors.Length; i++)
            {
                var color = image.Palette.Colors[i];
                for (var j = 0; j < replaceColors.Count; j += 2)
                {
                    if (color == replaceColors[j])
                        color = replaceColors[j + 1];
                }

                Colors[i] = color;
            }

            var palette = new BitmapPalette(Colors);
            var bitmap = new WriteableBitmap(image);
            var height = bitmap.PixelHeight;
            var stride = bitmap.BackBufferStride;
            return BitmapSource.Create(image.PixelWidth, image.PixelHeight, image.DpiX, image.DpiY, PixelFormats.Indexed8, palette, bitmap.BackBuffer, height * stride, stride);
        }

        /// <summary>
        /// 画像から最大地表震度を取得します。
        /// </summary>
        /// <param name="context">描画対象のオブジェクト</param>
        private static void GetRealtimeIntensity(DrawingContext context)
        {
            Observation.MaxInt = -3.0;
            Observation.Prefs.Clear();
            Observation.Stations.Clear();
            for (var i = 0; i < Stations.Count; i++)
            {
                var realtimeInt = GetInstIntensity(Stations[i].X, Stations[i].Y);
                if (!Stations[i].IsEnabled || realtimeInt <= -3.0)
                    continue;
                if (Observation.MaxInt < realtimeInt)
                    Observation.MaxInt = realtimeInt;

                var station = new Intensity.Station
                {
                    Name = Stations[i].Name,
                    PrefName = Stations[i].PrefName,
                    Int = realtimeInt,
                    Point = new Point(Stations[i].X, Stations[i].Y)
                };
                Observation.Stations.Add(station);

                // 都道府県リストを作成
                if (realtimeInt < .5)
                    continue;
                if (!station.PrefName.Contains('県') && !station.PrefName.Contains('府') &&
                    !station.PrefName.Contains('道') && !station.PrefName.Contains('都'))
                    continue;
                if (!Observation.Prefs.Select(pref => pref.Name).Contains(Stations[i].PrefName))
                {
                    var pref = new Intensity.Pref
                    {
                        Name = Stations[i].PrefName,
                        MaxInt = realtimeInt,
                        Number = 1
                    };
                    Observation.Prefs.Add(pref);
                }
                else
                {
                    var index = Observation.Prefs.FindIndex(pref => pref.Name == Stations[i].PrefName);
                    if (Observation.Prefs[index].MaxInt < realtimeInt)
                        Observation.Prefs[index].MaxInt = realtimeInt;
                    Observation.Prefs[index].Number++;
                }
            }

            Observation.Stations = Observation.Stations.OrderByDescending(station => station.Int).ToList();
            var firstIntStation = Observation.Stations[0];
            var secondIntStation = Observation.Stations.ElementAt(1);
            var thirdIntStation = Observation.Stations.ElementAt(2);

            // トリガチェック
            var nearest = Observation.Stations.OrderBy(station => Math.Sqrt((station.Point.X - firstIntStation.Point.X) * (station.Point.X - firstIntStation.Point.X) + (station.Point.Y - firstIntStation.Point.Y) * (station.Point.Y - firstIntStation.Point.Y))).ElementAt(1);
            var range = Math.Sqrt((nearest.Point.X - firstIntStation.Point.X) * (nearest.Point.X - firstIntStation.Point.X) + (nearest.Point.Y - firstIntStation.Point.Y) * (nearest.Point.Y - firstIntStation.Point.Y)) * 1.1;
            var minRange = SetComputeRange(firstIntStation.PrefName);
            if (range < minRange)
                range = minRange;
            var stations = Observation.Stations.Where(station => Math.Sqrt((station.Point.X - firstIntStation.Point.X) * (station.Point.X - firstIntStation.Point.X) + (station.Point.Y - firstIntStation.Point.Y) * (station.Point.Y - firstIntStation.Point.Y)) <= range).ToArray();
            var firstScore = ComputeScore(stations);
            nearest = Observation.Stations.OrderBy(station => Math.Sqrt((station.Point.X - secondIntStation.Point.X) * (station.Point.X - secondIntStation.Point.X) + (station.Point.Y - secondIntStation.Point.Y) * (station.Point.Y - secondIntStation.Point.Y))).ElementAt(1);
            range = Math.Sqrt((nearest.Point.X - secondIntStation.Point.X) * (nearest.Point.X - secondIntStation.Point.X) + (nearest.Point.Y - secondIntStation.Point.Y) * (nearest.Point.Y - secondIntStation.Point.Y)) * 1.1;
            minRange = SetComputeRange(secondIntStation.PrefName);
            if (range < minRange)
                range = minRange;
            stations = Observation.Stations.Skip(1).Where(station => Math.Sqrt((station.Point.X - secondIntStation.Point.X) * (station.Point.X - secondIntStation.Point.X) + (station.Point.Y - secondIntStation.Point.Y) * (station.Point.Y - secondIntStation.Point.Y)) <= range).ToArray();
            var secondScore = 0;
            if (secondIntStation.Int >= .5)
                secondScore = ComputeScore(stations);
            nearest = Observation.Stations.OrderBy(station => Math.Sqrt((station.Point.X - thirdIntStation.Point.X) * (station.Point.X - thirdIntStation.Point.X) + (station.Point.Y - thirdIntStation.Point.Y) * (station.Point.Y - thirdIntStation.Point.Y))).ElementAt(1);
            range = Math.Sqrt((nearest.Point.X - thirdIntStation.Point.X) * (nearest.Point.X - thirdIntStation.Point.X) + (nearest.Point.Y - thirdIntStation.Point.Y) * (nearest.Point.Y - thirdIntStation.Point.Y)) * 1.1;
            minRange = SetComputeRange(thirdIntStation.PrefName);
            if (range < minRange)
                range = minRange;
            stations = Observation.Stations.Skip(2).Where(station => Math.Sqrt((station.Point.X - thirdIntStation.Point.X) * (station.Point.X - thirdIntStation.Point.X) + (station.Point.Y - thirdIntStation.Point.Y) * (station.Point.Y - thirdIntStation.Point.Y)) <= range).ToArray();
            var thirdScore = 0;
            if (thirdIntStation.Int >= .5)
                thirdScore = ComputeScore(stations);
            var firstPref = firstIntStation;
            var maxScore = firstScore;
            if (maxScore < secondScore)
            {
                firstPref = secondIntStation;
                maxScore = secondScore;
                if (firstIntStation.Int <= secondIntStation.Int)
                    firstIntStation = secondIntStation;

                if (maxScore < thirdScore)
                {
                    firstPref = thirdIntStation;
                    maxScore = thirdScore;
                    if (secondIntStation.Int <= thirdIntStation.Int)
                        firstIntStation = thirdIntStation;
                }
            }

            if (maxScore >= 100)
            {
                IsTriggerOn = true;
                _isTriggerWait = false;
            }
            else if (IsTriggerOn)
            {
                if (firstIntStation.Int < .5)
                {
                    if (++_offTriggerTime >= 10)
                    {
                        IsTriggerOn = false;
                        _isTriggerWait = false;
                        _offTriggerTime = 0;
                    }
                    else
                    {
                        _isTriggerWait = true;
                    }
                }
                else
                {
                    _isTriggerWait = true;
                    _offTriggerTime = 0;
                }
            }

            _maxInt = ToIntensityInt(Observation.MaxInt);
            Observation.Prefs = Observation.Prefs.Where(pref => pref.Number > 2).OrderByDescending(pref => pref.MaxInt).ThenBy(pref => pref.Number).ToList();

            // 最大震度を検知した地点に円を描画
            var color = GetColor((int)firstIntStation.Point.X, (int)firstIntStation.Point.Y);
            var brush = new SolidColorBrush(color);
            var pen = new Pen(brush, 1.0);
            context.DrawEllipse(null, pen, firstIntStation.Point, 12.0, 12.0);

            // 最大震度を検知した地点をラベルに設定
            MainWindow.DataContext.Kyoshin.MaxIntString = $"{ToIntensityString(Observation.MaxInt)}（{firstIntStation.PrefName} {firstIntStation.Name}）";

            // 最大震度（気象庁震度階級）を検知した地点数
            var maxIntNum = Observation.Stations.Where(station => station.Int >= .5).Count(station => ToIntensityInt(station.Int) == _maxInt);
            MainWindow.DataContext.Kyoshin.MaxIntDetail = maxIntNum > 1 ? $"他 {(maxIntNum - 1).ToString()} 地点" : "";

            // 地表リアルアイム震度0.5以上を検知した都道府県をラベルに設定
            if (IsTriggerOn && !_isTriggerWait && firstIntStation.Int >= .5)
            {
                if (Observation.Prefs.Any())
                {
                    var text = string.Join("　", Observation.Prefs.Take(9).Select(pref => pref.Name));
                    if (Observation.Prefs.Count >= 11)
                        text += $"　他 {(Observation.Prefs.Count - 9).ToString()} 都道府県";
                    else if (Observation.Prefs.Count >= 10)
                        text += '　' + Observation.Prefs.Last().Name;

                    var space = text.Length - text.Replace("　", "").Length;
                    var insert = (int)Math.Ceiling(space * .5);
                    if (insert >= 3)
                    {
                        var split = text.Split('　');
                        var builder = new StringBuilder();
                        for (var i = 0; i < split.Length; i++)
                        {
                            if (insert == i + 1)
                                builder.Append(split[i] + '\n');
                            else
                                builder.Append(split[i] + '　');
                        }

                        text = builder.ToString().TrimEnd();
                    }

                    MainWindow.DataContext.Kyoshin.Prefecture = text;
                }
                else
                {
                    if (firstPref.PrefName.EndsWith("県") || firstPref.PrefName.EndsWith("府") || firstPref.PrefName.EndsWith("道") || firstPref.PrefName.EndsWith("都"))
                        MainWindow.DataContext.Kyoshin.Prefecture = firstPref.PrefName;
                    else
                        MainWindow.DataContext.Kyoshin.Prefecture = "";
                }
            }
            else
            {
                MainWindow.DataContext.Kyoshin.Prefecture = "";
            }
        }

        /// <summary>
        /// スコアを計算する観測点の範囲を設定します。
        /// </summary>
        /// <param name="pref"></param>
        /// <returns></returns>
        private static int SetComputeRange(string pref)
        {
            switch (pref)
            {
                case "茨城県":
                case "栃木県":
                case "埼玉県":
                case "千葉県":
                case "東京都":
                case "神奈川県":
                    return 5;   // 観測点同士の間隔が狭い地域では範囲を狭く設定
                case "鹿児島県":
                case "沖縄県":
                    return 10;  // 観測点同士の間隔が広い地域では範囲を広く設定
                default:
                    return 8;
            }
        }

        /// <summary>
        /// 地震判定用スコアを計算します。
        /// </summary>
        /// <param name="stations"></param>
        /// <returns></returns>
        private static int ComputeScore(IReadOnlyList<Intensity.Station> stations)
        {
            var score = stations.Count(station => station.Int >= 1.5) * 59;
            score += stations.Count(station => station.Int < 1.5 && station.Int >= 1.0) * 48;
            score += stations.Count(station => station.Int < 1.0 && station.Int >= 0.5) * 37;
            score += stations.Count(station => station.Int < 0.5 && station.Int >= 0.0) * 26;
            score += stations.Count(station => station.Int < 0.0 && station.Int >= -0.5) * 15;
            score += stations.Count(station => station.Int < -0.5 && station.Int >= -1.0) * 4;
            return score;
        }

        /// <summary>
        /// 指定した色の計測震度を取得します。
        /// </summary>
        /// <param name="x">取得するピクセルの x 座標</param>
        /// <param name="y">取得するピクセルの y 座標</param>
        /// <returns></returns>
        private static double GetInstIntensity(int x, int y) => ColorMap.TryGetValue(GetColorHash(x, y), out var value) ? value : -3.0;

        /// <summary>
        /// 指定したピクセルの色のハッシュを取得します。
        /// </summary>
        /// <param name="x">取得するピクセルの x 座標</param>
        /// <param name="y">取得するピクセルの y 座標</param>
        /// <returns></returns>
        private static unsafe int GetColorHash(int x, int y)
        {
            var index = (byte*)_readTimeBitmap.BackBuffer + y * _readTimeBitmap.BackBufferStride + x * 4;
            return index[2] * index[2] * index[2] + index[1] * index[1] + index[0];
        }

        /// <summary>
        /// 指定したピクセルの色を取得します。
        /// </summary>
        /// <param name="x">取得するピクセルの x 座標</param>
        /// <param name="y">取得するピクセルの y 座標</param>
        /// <returns></returns>
        private static unsafe Color GetColor(int x, int y)
        {
            var index = (byte*)_readTimeBitmap.BackBuffer + y * _readTimeBitmap.BackBufferStride + x * 4;
            return Color.FromRgb(index[2], index[1], index[0]);
        }

        /// <summary>
        /// 計測震度を気象庁震度階級の整数に変換します。
        /// </summary>
        /// <param name="seismicInt">変換する計測震度</param>
        /// <returns></returns>
        private static int ToIntensityInt(double seismicInt)
        {
            if (seismicInt < 0.5)
                return 0;
            if (seismicInt < 1.5)
                return 1;
            if (seismicInt < 2.5)
                return 2;
            if (seismicInt < 3.5)
                return 3;
            if (seismicInt < 4.5)
                return 4;
            if (seismicInt < 5.0)
                return 5;
            if (seismicInt < 5.5)
                return 6;
            if (seismicInt < 6.0)
                return 7;
            return seismicInt < 6.5 ? 8 : 9;
        }

        /// <summary>
        /// 計測震度を気象庁震度階級の文字列に変換します。
        /// </summary>
        /// <param name="seismicInt">変換する計測震度</param>
        /// <returns></returns>
        private static string ToIntensityString(double seismicInt)
        {
            if (seismicInt < 0.5)
                return "0";
            if (seismicInt < 1.5)
                return "1";
            if (seismicInt < 2.5)
                return "2";
            if (seismicInt < 3.5)
                return "3";
            if (seismicInt < 4.5)
                return "4";
            if (seismicInt < 5.0)
                return "5弱";
            if (seismicInt < 5.5)
                return "5強";
            if (seismicInt < 6.0)
                return "6弱";
            return seismicInt < 6.5 ? "6強" : "7";
        }

        public static void UpdateState()
        {
            if (IsTriggerOn && !_isTriggerWait && !Eew.IsTriggerOn)
            {
                if (_maxInt >= 2 && (_prevTriggerWait != _isTriggerWait || _prevMaxInt < _maxInt) ||
                    _maxInt >= 1 && (!_prevTriggerOn || _prevMaxInt < _maxInt))
                {
                    UpdateLevel();
                    Sound.PlayKyoshin(_maxInt);
                    SetActive();
                }
                else if (_prevMaxInt > _maxInt)
                {
                    UpdateLevel();
                }
            }
            else if (_prevMaxInt != _maxInt || _prevTriggerWait != _isTriggerWait)
            {
                UpdateLevel();
            }

            _prevTriggerOn = IsTriggerOn;
            _prevTriggerWait = _isTriggerWait;
            _prevMaxInt = _maxInt;
        }

        private static void UpdateLevel()
        {
            if (MainWindow.DataContext.Kyoshin.MaxIntString.Contains('弱') || MainWindow.DataContext.Kyoshin.MaxIntString.Contains('強') || MainWindow.DataContext.Kyoshin.MaxIntString.Contains("7（"))
                MainWindow.DataContext.Kyoshin.Level = Level.Red;
            else if (MainWindow.DataContext.Kyoshin.MaxIntString.Contains("3（") || MainWindow.DataContext.Kyoshin.MaxIntString.Contains("4（"))
                MainWindow.DataContext.Kyoshin.Level = Level.Yellow;
            else
                MainWindow.DataContext.Kyoshin.Level = Level.White;
        }

        public class Station
        {
            /// <summary>
            /// 観測点が有効であるか
            /// </summary>
            public bool IsEnabled { get; set; }

            /// <summary>
            /// 観測点名
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 都道府県名
            /// </summary>
            public string PrefName { get; set; }

            /// <summary>
            /// X座標
            /// </summary>
            public int X { get; set; }

            /// <summary>
            /// Y座標
            /// </summary>
            public int Y { get; set; }
        }

        public class Intensity
        {
            /// <summary>
            /// 最大リアルタイム震度
            /// </summary>
            public double MaxInt { get; set; }

            /// <summary>
            /// 都道府県
            /// </summary>
            public List<Pref> Prefs { get; set; }

            /// <summary>
            /// 観測点
            /// </summary>
            public List<Station> Stations { get; set; }

            public class Pref
            {
                /// <summary>
                /// 都道府県名
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 最大リアルタイム震度
                /// </summary>
                public double MaxInt { get; set; }

                /// <summary>
                /// リアルタイム震度1以上の観測点数
                /// </summary>
                public int Number { get;set; }
            }

            public class Station
            {
                /// <summary>
                /// 観測点名
                /// </summary>
                public string Name { get; set; }

                /// <summary>
                /// 都道府県名
                /// </summary>
                public string PrefName { get; set; }

                /// <summary>
                /// リアルタイム震度
                /// </summary>
                public double Int { get; set; }

                /// <summary>
                /// 座標
                /// </summary>
                public Point Point { get; set; }
            }
        }

        public class DataContext : INotifyPropertyChanged
        {
            private static readonly PropertyChangedEventArgs LevelPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Level));
            private static readonly PropertyChangedEventArgs ImageSourcePropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(ImageSource));
            private static readonly PropertyChangedEventArgs MaxIntStringPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(MaxIntString));
            private static readonly PropertyChangedEventArgs MaxIntDetailPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(MaxIntDetail));
            private static readonly PropertyChangedEventArgs PrefecturePropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Prefecture));
            private static readonly PropertyChangedEventArgs ComboBoxSelectedIndexPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(ComboBoxSelectedIndex));
            private static readonly PropertyChangedEventArgs RadioButtonPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(RadioButton));
            private static readonly PropertyChangedEventArgs SliderValuePropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(SliderValue));
            private Level _level;
            private ImageSource _imageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/BaseMap.png", UriKind.Absolute));
            private string _maxIntString;
            private string _maxIntDetail;
            private string _prefecture;
            private int _comboBoxSelectedIndex;
            private RadioButtonEnum _radioButton;
            private double _sliderValue;

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

            public ImageSource ImageSource
            {
                get => _imageSource;
                set
                {
                    _imageSource = value;
                    PropertyChanged?.Invoke(this, ImageSourcePropertyChangedEventArgs);
                }
            }

            public string MaxIntString
            {
                get => _maxIntString;
                set
                {
                    _maxIntString = value;
                    PropertyChanged?.Invoke(this, MaxIntStringPropertyChangedEventArgs);
                }
            }

            public string MaxIntDetail
            {
                get => _maxIntDetail;
                set
                {
                    _maxIntDetail = value;
                    PropertyChanged?.Invoke(this, MaxIntDetailPropertyChangedEventArgs);
                }
            }

            public string Prefecture
            {
                get => _prefecture;
                set
                {
                    _prefecture = value;
                    PropertyChanged?.Invoke(this, PrefecturePropertyChangedEventArgs);
                }
            }

            public int ComboBoxSelectedIndex
            {
                get => _comboBoxSelectedIndex;
                set
                {
                    _comboBoxSelectedIndex = value;
                    PropertyChanged?.Invoke(this, ComboBoxSelectedIndexPropertyChangedEventArgs);
                }
            }

            public enum RadioButtonEnum
            {
                Surface,
                Borehole
            }

            public RadioButtonEnum RadioButton
            {
                get => _radioButton;
                set
                {
                    _radioButton = value;
                    PropertyChanged?.Invoke(this, RadioButtonPropertyChangedEventArgs);
                }
            }

            public double SliderValue
            {
                get => _sliderValue;
                set
                {
                    _sliderValue = value;
                    PropertyChanged?.Invoke(this, SliderValuePropertyChangedEventArgs);
                }
            }
        }
    }
}
