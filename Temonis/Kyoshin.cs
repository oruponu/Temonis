using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static Temonis.MainWindow;

namespace Temonis;

public static class Kyoshin
{
    public static readonly double ImageWidth = (double)Instance.FindResource("ImageWidth");
    public static readonly double ImageHeight = (double)Instance.FindResource("ImageHeight");

    private static readonly BitmapImage BaseMap = new(new Uri("pack://application:,,,/Resources/BaseMap.png", UriKind.Absolute));
    private static readonly BitmapImage BaseMapBorder = new(new Uri("pack://application:,,,/Resources/BaseMapBorder.png", UriKind.Absolute));
    private static readonly BitmapImage Epicenter = new(new Uri("pack://application:,,,/Resources/Epicenter.png", UriKind.Absolute));

    /// <summary>
    /// 観測点リスト
    /// </summary>
    private static readonly IReadOnlyList<Station> Stations = Encoding.UTF8.GetString(DecompressResource(Assembly.GetExecutingAssembly().GetManifestResourceStream("Temonis.Resources.Stations.utd"), 48141)).Split('\n').Select(line => new Station
    {
        IsEnabled = Convert.ToBoolean(int.Parse(line.Split(',')[0])),
        Name = string.Intern(line.Split(',')[1]),
        PrefName = string.Intern(line.Split(',')[2]),
        X = int.Parse(line.Split(',')[3]),
        Y = int.Parse(line.Split(',')[4])
    }).ToArray();

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
    private static readonly Intensity Observation = new()
    {
        Prefs = new List<Intensity.Pref>(47),
        Stations = new List<Intensity.Station>(Stations.Count)
    };
    private static bool _isSucceeded;
    private static bool _isTriggerWait;
    private static int _offTriggerTime;
    private static int _maxInt;
    private static bool _prevTriggerOn;
    private static int _triggerMaxInt;
    private static WriteableBitmap _readTimeBitmap;

    public static bool IsTriggerOn { get; private set; }

    public static async Task<bool> UpdateAsync()
    {
        _isSucceeded = false;
        var time = LatestTime.ToString("yyyyMMdd/yyyyMMddHHmmss");
        var visual = new DrawingVisual();
        using (var context = visual.RenderOpen())
        {
            var rect = new Rect(.0, .0, ImageWidth, ImageHeight);
            context.DrawImage(BaseMap, rect);

            try
            {
                await DrawRealTimeImageAsync(context, time);
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or IOException)
            {
                WriteLog(ex);
            }

            try
            {
                var requestUri = new Uri($"{Properties.Resources.KyoshinUri}RealTimeImg/jma_s/{time}.jma_s.gif");
                var source = await DownloadImageAsync(requestUri);
                if (source is not null)
                {
                    source.Freeze();
                    var converted = new FormatConvertedBitmap(source, PixelFormats.Bgr32, null, .0);
                    converted.Freeze();
                    _readTimeBitmap = new WriteableBitmap(converted);
                    _readTimeBitmap.Lock();
                    GetRealtimeIntensity(context);
                    _readTimeBitmap.Unlock();
                    _readTimeBitmap.Freeze();
                    _isSucceeded = true;
                }
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or IOException or ArgumentOutOfRangeException)
            {
                WriteLog(ex);
            }

            if (Eew.IsTriggerOn)
            {
                try
                {
                    await DrawPsWaveImageAsync(context, time);
                }
                catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or IOException)
                {
                    WriteLog(ex);
                }
            }
            else if (EqInfo.EpicenterX != default || EqInfo.EpicenterY != default)
            {
                context.DrawImage(Epicenter, new Rect((int)(EqInfo.EpicenterX - Epicenter.Width * .5), (int)(EqInfo.EpicenterY - Epicenter.Height * .5), Epicenter.Width, Epicenter.Height));
            }

            context.DrawImage(BaseMapBorder, rect);
        }

        var target = new RenderTargetBitmap((int)ImageWidth, (int)ImageHeight, 96, 96, PixelFormats.Pbgra32);
        target.Render(visual);
        target.Freeze();
        MainWindow.DataContext.Kyoshin.ImageSource = target;

        UpdateState();

        return _isSucceeded;
    }

    /// <summary>
    /// 画像をダウンロードします。
    /// </summary>
    /// <param name="requestUri">要求の送信先 URI</param>
    /// <returns></returns>
    private static async Task<BitmapSource> DownloadImageAsync(Uri requestUri)
    {
        using var response = await MainWindow.HttpClient.GetAsync(requestUri);
        if (!response.IsSuccessStatusCode)
            return null;
        var image = new BitmapImage();
        image.BeginInit();
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
        await using (var stream = await response.Content.ReadAsStreamAsync())
        {
            image.StreamSource = stream;
            image.EndInit();
        }

        image.Freeze();
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
        var mapSb = MainWindow.DataContext.Kyoshin.RadioButton == DataContext.MapSb.Surface ? "s" : "b";
        var requestUri = new Uri($"{Properties.Resources.KyoshinUri}RealTimeImg/{mapType}_{mapSb}/{time}.{mapType}_{mapSb}.gif");
        var image = await DownloadImageAsync(requestUri);
        if (image is null)
            return;
        image.Freeze();
        var rect = new Rect(.0, .0, ImageWidth, ImageHeight);
        context.DrawImage(ReplaceColorPalette(image, in RealTimeReplaceColor), rect);
        _isSucceeded = true;
    }

    /// <summary>
    /// 緊急地震速報 P波・S波到達予想円を描画します。
    /// </summary>
    /// <param name="context">描画対象のオブジェクト</param>
    /// <param name="time">取得する時刻</param>
    /// <returns></returns>
    private static async Task DrawPsWaveImageAsync(DrawingContext context, string time)
    {
        var requestUri = new Uri($"{Properties.Resources.KyoshinUri}PSWaveImg/eew/{time}.eew.gif");
        var image = await DownloadImageAsync(requestUri);
        if (image is null)
            return;
        image.Freeze();
        var rect = new Rect(.0, .0, ImageWidth, ImageHeight);
        context.DrawImage(ReplaceColorPalette(image, in PsWaveReplaceColor), rect);
        _isSucceeded = true;
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
                Index = i,
                Int = realtimeInt
            };
            Observation.Stations.Add(station);

            // 都道府県リストを作成
            if (realtimeInt < .5)
                continue;
            if (!Stations[station.Index].PrefName.Contains('県') && !Stations[station.Index].PrefName.Contains('府') &&
                !Stations[station.Index].PrefName.Contains('道') && !Stations[station.Index].PrefName.Contains('都'))
                continue;
            if (!Observation.Prefs.Select(pref => pref.Name).Contains(Stations[station.Index].PrefName))
            {
                var pref = new Intensity.Pref
                {
                    Name = Stations[station.Index].PrefName,
                    MaxInt = realtimeInt,
                    Number = 1
                };
                Observation.Prefs.Add(pref);
            }
            else
            {
                var index = Observation.Prefs.FindIndex(pref => pref.Name == Stations[station.Index].PrefName);
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
        var nearest = Observation.Stations.OrderBy(station => Math.Sqrt((Stations[station.Index].X - Stations[firstIntStation.Index].X) * (Stations[station.Index].X - Stations[firstIntStation.Index].X) + (Stations[station.Index].Y - Stations[firstIntStation.Index].Y) * (Stations[station.Index].Y - Stations[firstIntStation.Index].Y))).ElementAt(1);
        var range = Math.Sqrt((Stations[nearest.Index].X - Stations[firstIntStation.Index].X) * (Stations[nearest.Index].X - Stations[firstIntStation.Index].X) + (Stations[nearest.Index].Y - Stations[firstIntStation.Index].Y) * (Stations[nearest.Index].Y - Stations[firstIntStation.Index].Y)) * 1.1;
        var minRange = SetComputeRange(Stations[firstIntStation.Index].PrefName);
        if (range < minRange)
            range = minRange;
        var stations = Observation.Stations.Where(station => Math.Sqrt((Stations[station.Index].X - Stations[firstIntStation.Index].X) * (Stations[station.Index].X - Stations[firstIntStation.Index].X) + (Stations[station.Index].Y - Stations[firstIntStation.Index].Y) * (Stations[station.Index].Y - Stations[firstIntStation.Index].Y)) <= range).ToArray();
        var firstScore = ComputeScore(stations);
        nearest = Observation.Stations.OrderBy(station => Math.Sqrt((Stations[station.Index].X - Stations[secondIntStation.Index].X) * (Stations[station.Index].X - Stations[secondIntStation.Index].X) + (Stations[station.Index].Y - Stations[secondIntStation.Index].Y) * (Stations[station.Index].Y - Stations[secondIntStation.Index].Y))).ElementAt(1);
        range = Math.Sqrt((Stations[nearest.Index].X - Stations[secondIntStation.Index].X) * (Stations[nearest.Index].X - Stations[secondIntStation.Index].X) + (Stations[nearest.Index].Y - Stations[secondIntStation.Index].Y) * (Stations[nearest.Index].Y - Stations[secondIntStation.Index].Y)) * 1.1;
        minRange = SetComputeRange(Stations[secondIntStation.Index].PrefName);
        if (range < minRange)
            range = minRange;
        stations = Observation.Stations.Skip(1).Where(station => Math.Sqrt((Stations[station.Index].X - Stations[secondIntStation.Index].X) * (Stations[station.Index].X - Stations[secondIntStation.Index].X) + (Stations[station.Index].Y - Stations[secondIntStation.Index].Y) * (Stations[station.Index].Y - Stations[secondIntStation.Index].Y)) <= range).ToArray();
        var secondScore = 0;
        if (secondIntStation.Int >= .5)
            secondScore = ComputeScore(stations);
        nearest = Observation.Stations.OrderBy(station => Math.Sqrt((Stations[station.Index].X - Stations[thirdIntStation.Index].X) * (Stations[station.Index].X - Stations[thirdIntStation.Index].X) + (Stations[station.Index].Y - Stations[thirdIntStation.Index].Y) * (Stations[station.Index].Y - Stations[thirdIntStation.Index].Y))).ElementAt(1);
        range = Math.Sqrt((Stations[nearest.Index].X - Stations[thirdIntStation.Index].X) * (Stations[nearest.Index].X - Stations[thirdIntStation.Index].X) + (Stations[nearest.Index].Y - Stations[thirdIntStation.Index].Y) * (Stations[nearest.Index].Y - Stations[thirdIntStation.Index].Y)) * 1.1;
        minRange = SetComputeRange(Stations[thirdIntStation.Index].PrefName);
        if (range < minRange)
            range = minRange;
        stations = Observation.Stations.Skip(2).Where(station => Math.Sqrt((Stations[station.Index].X - Stations[thirdIntStation.Index].X) * (Stations[station.Index].X - Stations[thirdIntStation.Index].X) + (Stations[station.Index].Y - Stations[thirdIntStation.Index].Y) * (Stations[station.Index].Y - Stations[thirdIntStation.Index].Y)) <= range).ToArray();
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
        var color = GetColor(Stations[firstIntStation.Index].X, Stations[firstIntStation.Index].Y);
        var brush = new SolidColorBrush(color);
        var pen = new Pen(brush, 1.0);
        context.DrawEllipse(null, pen, new Point(Stations[firstIntStation.Index].X, Stations[firstIntStation.Index].Y), 12.0, 12.0);

        // 最大震度を検知した地点をラベルに設定
        MainWindow.DataContext.Kyoshin.MaxIntString = $"{(Settings.JsonClass.Appearance.UseJmaSeismicIntensityScale ? ToIntensityString(Observation.MaxInt) : Observation.MaxInt.ToString("F1"))}（{Stations[firstIntStation.Index].PrefName} {Stations[firstIntStation.Index].Name}）";

        // 最大震度（気象庁震度階級）を検知した地点数
        var maxIntNum = Observation.Stations.Where(station => station.Int >= .5).Count(station => Settings.JsonClass.Appearance.UseJmaSeismicIntensityScale ? ToIntensityInt(station.Int) == _maxInt : station.Int == Observation.MaxInt);
        MainWindow.DataContext.Kyoshin.MaxIntDetail = maxIntNum > 1 ? $"他 {maxIntNum - 1} 地点" : "";

        // 地表リアルアイム震度0.5以上を検知した都道府県をラベルに設定
        if (IsTriggerOn && !_isTriggerWait && firstIntStation.Int >= .5)
        {
            if (Observation.Prefs.Any())
            {
                var text = string.Join("　", Observation.Prefs.Take(9).Select(pref => pref.Name));
                if (Observation.Prefs.Count >= 11)
                    text += $"　他 {Observation.Prefs.Count - 9} 都道府県";
                else if (Observation.Prefs.Count >= 10)
                    text += "　" + Observation.Prefs.Last().Name;

                var space = text.Length - text.Replace("　", "").Length;
                var insert = (int)Math.Ceiling(space * .5);
                if (insert >= 3)
                {
                    var split = text.Split('　');
                    var builder = new StringBuilder();
                    for (var i = 0; i < split.Length; i++)
                    {
                        if (insert == i + 1)
                            builder.Append(split[i] + "\n");
                        else
                            builder.Append(split[i] + "　");
                    }

                    text = builder.ToString().TrimEnd();
                }

                MainWindow.DataContext.Kyoshin.Prefecture = text;
            }
            else
            {
                if (Stations[firstPref.Index].PrefName.EndsWith("県") || Stations[firstPref.Index].PrefName.EndsWith("府") || Stations[firstPref.Index].PrefName.EndsWith("道") || Stations[firstPref.Index].PrefName.EndsWith("都"))
                    MainWindow.DataContext.Kyoshin.Prefecture = Stations[firstPref.Index].PrefName;
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
    private static int SetComputeRange(string pref) => pref switch
    {
        "茨城県" or "栃木県" or "埼玉県" or "千葉県" or "東京都" or "神奈川県" => 5,   // 観測点同士の間隔が狭い地域では範囲を狭く設定
        "鹿児島県" or "沖縄県" => 10,  // 観測点同士の間隔が広い地域では範囲を広く設定
        _ => 8
    };

    /// <summary>
    /// 地震判定用スコアを計算します。
    /// </summary>
    /// <param name="stations"></param>
    /// <returns></returns>
    private static int ComputeScore(IReadOnlyList<Intensity.Station> stations)
    {
        var score = stations.Count(station => station.Int >= 1.5) * 59;
        score += stations.Count(station => station.Int is < 1.5 and >= 1.0) * 48;
        score += stations.Count(station => station.Int is < 1.0 and >= 0.5) * 37;
        score += stations.Count(station => station.Int is < 0.5 and >= 0.0) * 26;
        score += stations.Count(station => station.Int is < 0.0 and >= -0.5) * 15;
        score += stations.Count(station => station.Int is < -0.5 and >= -1.0) * 4;
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
    private static int ToIntensityInt(double seismicInt) => seismicInt switch
    {
        < 0.5 => 0,
        < 1.5 => 1,
        < 2.5 => 2,
        < 3.5 => 3,
        < 4.5 => 4,
        < 5.0 => 5,
        < 5.5 => 6,
        < 6.0 => 7,
        < 6.5 => 8,
        _ => 9
    };

    /// <summary>
    /// 計測震度を気象庁震度階級の文字列に変換します。
    /// </summary>
    /// <param name="seismicInt">変換する計測震度</param>
    /// <returns></returns>
    private static string ToIntensityString(double seismicInt) => seismicInt switch
    {
        < 0.5 => "0",
        < 1.5 => "1",
        < 2.5 => "2",
        < 3.5 => "3",
        < 4.5 => "4",
        < 5.0 => "5弱",
        < 5.5 => "5強",
        < 6.0 => "6弱",
        < 6.5 => "6強",
        _ => "7"
    };

    private static void UpdateState()
    {
        UpdateLevel();

        if (IsTriggerOn)
        {
            if (_triggerMaxInt < _maxInt)
            {
                _triggerMaxInt = _maxInt;
                if (!_isTriggerWait && !Eew.IsTriggerOn)
                {
                    Sound.PlayKyoshin(_maxInt);
                    SetActive();
                }
            }
        }
        else if (_prevTriggerOn && !IsTriggerOn)
        {
            _triggerMaxInt = _maxInt;
        }

        _prevTriggerOn = IsTriggerOn;
    }

    private static void UpdateLevel()
    {
        if (IsTriggerOn && !_isTriggerWait)
        {
            MainWindow.DataContext.Kyoshin.Level = _maxInt switch
            {
                >= 5 => Level.Red,
                >= 3 => Level.Yellow,
                _ => Level.White
            };
        }
        else
        {
            MainWindow.DataContext.Kyoshin.Level = Level.White;
        }
    }

    public class DataContext : BindableBase
    {
        private Level _level;
        private ImageSource _imageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/BaseMap.png", UriKind.Absolute));
        private string _maxIntString;
        private string _maxIntDetail;
        private string _prefecture;
        private int _comboBoxSelectedIndex;
        private MapSb _radioButton;
        private double _sliderValue;

        public Level Level
        {
            get => _level;
            set => SetProperty(ref _level, value);
        }

        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public string MaxIntString
        {
            get => _maxIntString;
            set => SetProperty(ref _maxIntString, value);
        }

        public string MaxIntDetail
        {
            get => _maxIntDetail;
            set => SetProperty(ref _maxIntDetail, value);
        }

        public string Prefecture
        {
            get => _prefecture;
            set => SetProperty(ref _prefecture, value);
        }

        public int ComboBoxSelectedIndex
        {
            get => _comboBoxSelectedIndex;
            set => SetProperty(ref _comboBoxSelectedIndex, value);
        }

        public enum MapSb
        {
            Surface,
            Borehole
        }

        public MapSb RadioButton
        {
            get => _radioButton;
            set => SetProperty(ref _radioButton, value);
        }

        public double SliderValue
        {
            get => _sliderValue;
            set => SetProperty(ref _sliderValue, value);
        }
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
            public string Name { get; init; }

            /// <summary>
            /// 最大リアルタイム震度
            /// </summary>
            public double MaxInt { get; set; }

            /// <summary>
            /// リアルタイム震度1以上の観測点数
            /// </summary>
            public int Number { get; set; }
        }

        public class Station
        {
            /// <summary>
            /// 観測点インデックス
            /// </summary>
            public int Index { get; init; }

            /// <summary>
            /// リアルタイム震度
            /// </summary>
            public double Int { get; init; }
        }
    }

    private class Station
    {
        /// <summary>
        /// 観測点が有効であるか
        /// </summary>
        public bool IsEnabled { get; init; }

        /// <summary>
        /// 観測点名
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// 都道府県名
        /// </summary>
        public string PrefName { get; init; }

        /// <summary>
        /// X座標
        /// </summary>
        public int X { get; init; }

        /// <summary>
        /// Y座標
        /// </summary>
        public int Y { get; init; }
    }
}
