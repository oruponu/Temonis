using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Temonis.Resources;
using static Temonis.MainWindow;
using static Temonis.Resources.General.Kyoshin;

namespace Temonis
{
    internal class Kyoshin
    {
        private readonly int _pictureBoxWidth = Instance.PictureBox_KyoshinMap.Width;
        private readonly int _pictureBoxHeight = Instance.PictureBox_KyoshinMap.Height;
        private static BitmapData _dataRealTime;
        private static int _offTriggerTime;

        public static bool IsTriggerOn { get; private set; }

        public static bool IsTriggerWait { get; private set; }

        public static int MaxIntensity { get; private set; }

        public async Task<bool> UpdateAsync()
        {
            var isSucceeded = false;
            var time = $"{LatestTime:yyyyMMdd/yyyyMMddHHmmss}";

            var bitmap = Properties.Resources.BaseMap;
            using (var graphics = Graphics.FromImage(bitmap))
            {
                try
                {
                    await DrawRealTimeImageAsync(graphics, time);
                    isSucceeded = true;
                }
                catch (Exception ex) when (ex is HttpRequestException || ex is IOException || ex is TaskCanceledException || ex is ArgumentException)
                {
                    InternalLog(ex);
                }

                try
                {
                    using (var intensityBitmap = (Bitmap)await DownloadImageAsync($"{Properties.Resources.KyoshinUri}RealTimeImg/jma_s/{time}.jma_s.gif"))
                    {
                        var rect = new Rectangle(0, 0, _pictureBoxWidth, _pictureBoxHeight);
                        _dataRealTime = intensityBitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                        try
                        {
                            GetRealtimeIntensity(graphics);
                        }
                        finally
                        {
                            intensityBitmap.UnlockBits(_dataRealTime);
                        }
                    }

                    isSucceeded = true;
                }
                catch (Exception ex) when (ex is HttpRequestException || ex is IOException || ex is TaskCanceledException || ex is ArgumentException)
                {
                    InternalLog(ex);
                }

                if (EEW.IsTriggerOn)
                {
                    try
                    {
                        await DrawPSWaveImageAsync(graphics, time);
                        isSucceeded = true;
                    }
                    catch (Exception ex) when (ex is HttpRequestException || ex is IOException || ex is TaskCanceledException || ex is ArgumentException)
                    {
                        InternalLog(ex);
                    }
                }
                else
                {
                    if (EqInfo.Epicenter != null)
                    {
                        graphics.DrawImage(EqInfo.EpicenterBitmap, 0, 0, _pictureBoxWidth, _pictureBoxHeight);
                    }
                }

                using (var baseMapBorder = Properties.Resources.BaseMapBorder)
                {
                    graphics.DrawImage(baseMapBorder, 0, 0, _pictureBoxWidth, _pictureBoxHeight);
                }
            }

            var oldImage = Instance.PictureBox_KyoshinMap.Image;
            Instance.PictureBox_KyoshinMap.Image = bitmap;
            oldImage?.Dispose();

            return isSucceeded;
        }

        /// <summary>
        /// 画像をダウンロード
        /// </summary>
        /// <param name="requestUri">要求の送信先 URI</param>
        /// <returns></returns>
        private static async Task<Image> DownloadImageAsync(string requestUri)
        {
            using (var stream = await MainWindow.HttpClient.GetStreamAsync(requestUri).ConfigureAwait(false))
            {
                return Image.FromStream(stream, false, false);
            }
        }

        /// <summary>
        /// 強震モニタの画像を描画
        /// </summary>
        /// <param name="graphics">描画対象のオブジェクト</param>
        /// <param name="time">取得する時刻</param>
        /// <returns></returns>
        private async Task DrawRealTimeImageAsync(Graphics graphics, string time)
        {
            // コントロールから設定を取得
            var mapType = MapType[Instance.ComboBox_MapType.SelectedIndex];
            var mapSb = Instance.RadioButton_Surface.Checked ? "s" : "b";
            using (var imageAttrs = new ImageAttributes())
            {
                imageAttrs.SetRemapTable(MapRealTime);
                using (var image = await DownloadImageAsync($"{Properties.Resources.KyoshinUri}RealTimeImg/{mapType}_{mapSb}/{time}.{mapType}_{mapSb}.gif"))
                {
                    var destRect = new Rectangle(0, 0, _pictureBoxWidth, _pictureBoxHeight);
                    graphics.DrawImage(image, destRect, 0, 0, _pictureBoxWidth, _pictureBoxHeight, GraphicsUnit.Pixel, imageAttrs);
                }
            }
        }

        /// <summary>
        /// 緊急地震速報 P波・S波到達予想円を描画
        /// </summary>
        /// <param name="graphics">描画対象のオブジェクト</param>
        /// <param name="time">取得する時刻</param>
        /// <returns></returns>
        private async Task DrawPSWaveImageAsync(Graphics graphics, string time)
        {
            using (var imageAttrs = new ImageAttributes())
            {
                imageAttrs.SetRemapTable(MapPSWave);
                using (var image = await DownloadImageAsync($"{Properties.Resources.KyoshinUri}PSWaveImg/eew/{time}.eew.gif"))
                {
                    var destRect = new Rectangle(0, 0, _pictureBoxWidth, _pictureBoxHeight);
                    graphics.DrawImage(image, destRect, 0, 0, _pictureBoxWidth, _pictureBoxHeight, GraphicsUnit.Pixel, imageAttrs);
                }
            }
        }

        /// <summary>
        /// 画像から最大地表震度を取得
        /// </summary>
        /// <param name="graphics">描画対象のオブジェクト</param>
        private void GetRealtimeIntensity(Graphics graphics)
        {
            var intensity = new Intensity
            {
                Prefs = new List<Intensity.Pref>(47),
                Stations = new List<Intensity.Station>(Stations.Length)
            };
            foreach (var items in Stations)
            {
                var realtimeInt = GetInstIntensity(int.Parse(items[3]), int.Parse(items[4]));
                if (items[0] == "0" || realtimeInt <= -3.0f) continue;

                if (intensity.MaxInt < realtimeInt)
                {
                    intensity.MaxInt = realtimeInt;
                }

                var station = new Intensity.Station
                {
                    Name = items[1],
                    PrefName = items[2],
                    Int = realtimeInt,
                    Point = new Point(int.Parse(items[3]), int.Parse(items[4]))
                };
                intensity.Stations.Add(station);

                // 都道府県リストを作成
                if (realtimeInt < 0.5f) continue;
                if (!station.PrefName.Contains("県") && !station.PrefName.Contains("府") &&
                    !station.PrefName.Contains("道") && !station.PrefName.Contains("都")) continue;
                if (!intensity.Prefs.Select(x => x.Name).Contains(items[2]))
                {
                    var pref = new Intensity.Pref
                    {
                        Name = items[2],
                        MaxInt = realtimeInt,
                        Number = 1
                    };
                    intensity.Prefs.Add(pref);
                }
                else
                {
                    var index = intensity.Prefs.FindIndex(x => x.Name == items[2]);
                    if (intensity.Prefs[index].MaxInt < realtimeInt)
                    {
                        intensity.Prefs[index].MaxInt = realtimeInt;
                    }

                    intensity.Prefs[index].Number++;
                }
            }

            MaxIntensity = ToIntensityInt(intensity.MaxInt);
            intensity.Stations = intensity.Stations.OrderByDescending(x => x.Int).ToList();
            var firstIntStation = intensity.Stations.First();
            var secondIntStation = intensity.Stations.ElementAt(1);
            var thirdIntStation = intensity.Stations.ElementAt(2);

            // トリガチェック
            var nearest = intensity.Stations.OrderBy(x => Math.Sqrt(Math.Pow(x.Point.X - firstIntStation.Point.X, 2) + Math.Pow(x.Point.Y - firstIntStation.Point.Y, 2))).ElementAt(1);
            var range = Math.Sqrt(Math.Pow(nearest.Point.X - firstIntStation.Point.X, 2) + Math.Pow(nearest.Point.Y - firstIntStation.Point.Y, 2)) * 1.1;
            var minRange = SetComputeRange(firstIntStation.PrefName);
            if (range < minRange) range = minRange;
            var stations = intensity.Stations.Where(x => Math.Sqrt(Math.Pow(x.Point.X - firstIntStation.Point.X, 2) + Math.Pow(x.Point.Y - firstIntStation.Point.Y, 2)) <= range).ToArray();
            var firstScore = ComputeScore(stations);
            nearest = intensity.Stations.OrderBy(x => Math.Sqrt(Math.Pow(x.Point.X - secondIntStation.Point.X, 2) + Math.Pow(x.Point.Y - secondIntStation.Point.Y, 2))).ElementAt(1);
            range = Math.Sqrt(Math.Pow(nearest.Point.X - secondIntStation.Point.X, 2) + Math.Pow(nearest.Point.Y - secondIntStation.Point.Y, 2)) * 1.1;
            minRange = SetComputeRange(secondIntStation.PrefName);
            if (range < minRange) range = minRange;
            stations = intensity.Stations.Skip(1).Where(x => Math.Sqrt(Math.Pow(x.Point.X - secondIntStation.Point.X, 2) + Math.Pow(x.Point.Y - secondIntStation.Point.Y, 2)) <= range).ToArray();
            var secondScore = 0;
            if (secondIntStation.Int >= 0.5) secondScore = ComputeScore(stations);
            nearest = intensity.Stations.OrderBy(x => Math.Sqrt(Math.Pow(x.Point.X - thirdIntStation.Point.X, 2) + Math.Pow(x.Point.Y - thirdIntStation.Point.Y, 2))).ElementAt(1);
            range = Math.Sqrt(Math.Pow(nearest.Point.X - thirdIntStation.Point.X, 2) + Math.Pow(nearest.Point.Y - thirdIntStation.Point.Y, 2)) * 1.1;
            minRange = SetComputeRange(thirdIntStation.PrefName);
            if (range < minRange) range = minRange;
            stations = intensity.Stations.Skip(2).Where(x => Math.Sqrt(Math.Pow(x.Point.X - thirdIntStation.Point.X, 2) + Math.Pow(x.Point.Y - thirdIntStation.Point.Y, 2)) <= range).ToArray();
            var thirdScore = 0;
            if (thirdIntStation.Int >= 0.5) thirdScore = ComputeScore(stations);

            var firstPref = firstIntStation;
            var maxScore = firstScore;
            if (maxScore < secondScore)
            {
                firstPref = secondIntStation;
                maxScore = secondScore;
                if (firstIntStation.Int <= secondIntStation.Int)
                {
                    firstIntStation = secondIntStation;
                }

                if (maxScore < thirdScore)
                {
                    firstPref = thirdIntStation;
                    maxScore = thirdScore;
                    if (secondIntStation.Int <= thirdIntStation.Int)
                    {
                        firstIntStation = thirdIntStation;
                    }
                }
            }

            if (maxScore >= 100)
            {
                IsTriggerOn = true;
                IsTriggerWait = false;
            }
            else if (IsTriggerOn)
            {
                if (firstIntStation.Int < 0.5)
                {
                    if (++_offTriggerTime >= 10)
                    {
                        IsTriggerOn = false;
                        IsTriggerWait = false;
                        _offTriggerTime = 0;
                    }
                    else
                    {
                        IsTriggerWait = true;
                    }
                }
                else
                {
                    IsTriggerWait = true;
                    _offTriggerTime = 0;
                }
            }

            intensity.Prefs = intensity.Prefs.Where(x => x.Number > 2).ToList();

            // 最大震度を検知した地点に円を描画
            using (var pen = new Pen(GetColor(firstIntStation.Point.X, firstIntStation.Point.Y)))
            {
                graphics.DrawEllipse(pen, firstIntStation.Point.X - 12, firstIntStation.Point.Y - 12, 24, 24);
            }

            // 最大震度を検知した地点をラベルに設定
            Instance.Label_KyoshinMaxInt.Text = $"{ToIntensityString(intensity.MaxInt)}（{firstIntStation.PrefName} {firstIntStation.Name}）";

            // 最大震度（気象庁震度階級）を検知した地点数
            var maxIntNum = -1 + intensity.Stations.Where(x => x.Int >= 0.5).Count(x => ToIntensityInt(x.Int) == ToIntensityInt(intensity.MaxInt));
            if (maxIntNum > 1)
            {
                Instance.Label_KyoshinMaxIntDetail.Text = $"他 {maxIntNum - 1} 地点";
                Instance.Label_KyoshinMaxIntDetail.Location = new Point(Instance.Label_KyoshinMaxInt.Location.X + Instance.Label_KyoshinMaxInt.Size.Width, Instance.Label_KyoshinMaxIntDetail.Location.Y);
            }
            else
            {
                Instance.Label_KyoshinMaxIntDetail.Text = "";
            }

            // 地表リアルアイム震度0.5以上を検知した都道府県をラベルに設定
            if (IsTriggerOn && !IsTriggerWait && firstIntStation.Int >= 0.5)
            {
                if (intensity.Prefs.Any())
                {
                    var text = string.Join("　", intensity.Prefs.Take(9).Select(x => x.Name));
                    if (intensity.Prefs.Count >= 11)
                    {
                        text += $"　他 {intensity.Prefs.Count - 9} 都道府県";
                    }
                    else if (intensity.Prefs.Count >= 10)
                    {
                        text += "　" + intensity.Prefs.Last().Name;
                    }

                    var space = text.Length - text.Replace("　", "").Length;
                    var insert = (int)Math.Ceiling(space / 2.0);
                    if (insert >= 3)
                    {
                        var split = text.Split('　');
                        var builder = new StringBuilder();
                        for (var i = 0; i < split.Length; i++)
                        {
                            if (insert == i + 1)
                            {
                                builder.Append(split[i] + "\n");
                            }
                            else
                            {
                                builder.Append(split[i] + "　");
                            }
                        }

                        text = builder.ToString().TrimEnd();
                    }

                    Instance.Label_KyoshinPrefecture.Text = text;
                }
                else
                {
                    if (firstPref.PrefName.Contains("県") || firstPref.PrefName.Contains("府") ||
                        firstPref.PrefName.Contains("道") || firstPref.PrefName.Contains("都"))
                    {
                        Instance.Label_KyoshinPrefecture.Text = firstPref.PrefName;
                    }
                    else
                    {
                        Instance.Label_KyoshinPrefecture.Text = "";
                    }
                }

                Instance.Label_KyoshinPrefecture.Location = new Point(_pictureBoxWidth - Instance.Label_KyoshinPrefecture.Size.Width + 4, _pictureBoxHeight - Instance.Label_KyoshinPrefecture.Size.Height + 54);
            }
            else
            {
                Instance.Label_KyoshinPrefecture.Text = "";
            }
        }

        /// <summary>
        /// スコアを計算する観測点の範囲を設定
        /// </summary>
        /// <param name="pref"></param>
        /// <returns></returns>
        private static int SetComputeRange(string pref)
        {
            switch (pref)
            {
                case "茨城県":
                case "栃木県":
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
        /// 地震判定用スコアを計算
        /// </summary>
        /// <param name="stations"></param>
        /// <returns></returns>
        private static int ComputeScore(Intensity.Station[] stations)
        {
            var score = stations.Count(x => x.Int >= 1.5) * 59;
            score += stations.Count(x => x.Int < 1.5 && x.Int >= 1.0) * 48;
            score += stations.Count(x => x.Int < 1.0 && x.Int >= 0.5) * 37;
            score += stations.Count(x => x.Int < 0.5 && x.Int >= 0.0) * 26;
            score += stations.Count(x => x.Int < 0.0 && x.Int >= -0.5) * 15;
            score += stations.Count(x => x.Int < -0.5 && x.Int >= -1.0) * 4;
            return score;
        }

        /// <summary>
        /// 指定したピクセルの色を取得
        /// </summary>
        /// <param name="x">取得するピクセルの x 座標</param>
        /// <param name="y">取得するピクセルの y 座標</param>
        /// <returns></returns>
        private static unsafe Color GetColor(int x, int y)
        {
            var scan0 = (byte*)_dataRealTime.Scan0;
            var index = x * 3 + _dataRealTime.Stride * y;
            return Color.FromArgb(scan0[index + 2], scan0[index + 1], scan0[index]);
        }

        /// <summary>
        /// 指定した色の計測震度を取得
        /// </summary>
        /// <param name="x">取得するピクセルの x 座標</param>
        /// <param name="y">取得するピクセルの y 座標</param>
        /// <returns></returns>
        private static float GetInstIntensity(int x, int y) => !General.Kyoshin.ColorMap.TryGetValue(GetColor(x, y), out var value) ? -3.0f : value;

        /// <summary>
        /// 計測震度を気象庁震度階級の整数に変換
        /// </summary>
        /// <param name="seismicInt">変換する計測震度</param>
        /// <returns></returns>
        private static int ToIntensityInt(float seismicInt)
        {
            if (seismicInt < 0.5f) return 0;
            else if (seismicInt < 1.5f) return 1;
            else if (seismicInt < 2.5f) return 2;
            else if (seismicInt < 3.5f) return 3;
            else if (seismicInt < 4.5f) return 4;
            else if (seismicInt < 5.0f) return 5;
            else if (seismicInt < 5.5f) return 6;
            else if (seismicInt < 6.0f) return 7;
            else return seismicInt < 6.5f ? 8 : 9;
        }

        /// <summary>
        /// 計測震度を気象庁震度階級の文字列に変換
        /// </summary>
        /// <param name="seismicInt">変換する計測震度</param>
        /// <returns></returns>
        private static string ToIntensityString(float seismicInt)
        {
            if (seismicInt < 0.5f) return "0";
            else if (seismicInt < 1.5f) return "1";
            else if (seismicInt < 2.5f) return "2";
            else if (seismicInt < 3.5f) return "3";
            else if (seismicInt < 4.5f) return "4";
            else if (seismicInt < 5.0f) return "5弱";
            else if (seismicInt < 5.5f) return "5強";
            else if (seismicInt < 6.0f) return "6弱";
            else return seismicInt < 6.5f ? "6強" : "7";
        }

        public class Intensity
        {
            /// <summary>
            /// 最大リアルタイム震度
            /// </summary>
            public float MaxInt { get; set; }

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
                public float MaxInt { get; set; }

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
                public float Int { get; set; }

                /// <summary>
                /// 座標
                /// </summary>
                public Point Point { get; set; }
            }
        }
    }
}
