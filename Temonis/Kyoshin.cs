using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Temonis
{
    internal class Kyoshin
    {
        // マップの種類
        private static readonly string[] MapType =
        {
            "jma", "acmap", "vcmap", "dcmap", "rsp0125", "rsp0250", "rsp0500", "rsp1000", "rsp2000", "rsp4000"
        };
        
        private static readonly Dictionary<Color, float> ColorTable = new Dictionary<Color, float>
        {
            {Color.FromArgb(0, 0, 205), -3.0f},
            {Color.FromArgb(0, 7, 209), -2.9f},
            {Color.FromArgb(0, 14, 214), -2.8f},
            {Color.FromArgb(0, 21, 218), -2.7f},
            {Color.FromArgb(0, 28, 223), -2.6f},
            {Color.FromArgb(0, 36, 227), -2.5f},
            {Color.FromArgb(0, 43, 231), -2.4f},
            {Color.FromArgb(0, 50, 236), -2.3f},
            {Color.FromArgb(0, 57, 240), -2.2f},
            {Color.FromArgb(0, 64, 245), -2.1f},
            {Color.FromArgb(0, 72, 250), -2.0f},
            {Color.FromArgb(0, 85, 238), -1.9f},
            {Color.FromArgb(0, 99, 227), -1.8f},
            {Color.FromArgb(0, 112, 216), -1.7f},
            {Color.FromArgb(0, 126, 205), -1.6f},
            {Color.FromArgb(0, 140, 194), -1.5f},
            {Color.FromArgb(0, 153, 183), -1.4f},
            {Color.FromArgb(0, 167, 172), -1.3f},
            {Color.FromArgb(0, 180, 161), -1.2f},
            {Color.FromArgb(0, 194, 150), -1.1f},
            {Color.FromArgb(0, 208, 139), -1.0f},
            {Color.FromArgb(6, 212, 130), -0.9f},
            {Color.FromArgb(12, 216, 121), -0.8f},
            {Color.FromArgb(18, 220, 113), -0.7f},
            {Color.FromArgb(25, 224, 104), -0.6f},
            {Color.FromArgb(31, 228, 96), -0.5f},
            {Color.FromArgb(37, 233, 88), -0.4f},
            {Color.FromArgb(44, 237, 79), -0.3f},
            {Color.FromArgb(50, 241, 71), -0.2f},
            {Color.FromArgb(56, 245, 62), -0.1f},
            {Color.FromArgb(63, 250, 54), 0.0f},
            {Color.FromArgb(75, 250, 49), 0.1f},
            {Color.FromArgb(88, 250, 45), 0.2f},
            {Color.FromArgb(100, 251, 41), 0.3f},
            {Color.FromArgb(113, 251, 37), 0.4f},
            {Color.FromArgb(125, 252, 33), 0.5f},
            {Color.FromArgb(138, 252, 28), 0.6f},
            {Color.FromArgb(151, 253, 24), 0.7f},
            {Color.FromArgb(163, 253, 20), 0.8f},
            {Color.FromArgb(176, 254, 16), 0.9f},
            {Color.FromArgb(189, 255, 12), 1.0f},
            {Color.FromArgb(195, 254, 10), 1.1f},
            {Color.FromArgb(202, 254, 9), 1.2f},
            {Color.FromArgb(208, 254, 8), 1.3f},
            {Color.FromArgb(215, 254, 7), 1.4f},
            {Color.FromArgb(222, 255, 5), 1.5f},
            {Color.FromArgb(228, 254, 4), 1.6f},
            {Color.FromArgb(235, 255, 3), 1.7f},
            {Color.FromArgb(241, 254, 2), 1.8f},
            {Color.FromArgb(248, 255, 1), 1.9f},
            {Color.FromArgb(255, 255, 0), 2.0f},
            {Color.FromArgb(254, 251, 0), 2.1f},
            {Color.FromArgb(254, 248, 0), 2.2f},
            {Color.FromArgb(254, 244, 0), 2.3f},
            {Color.FromArgb(254, 241, 0), 2.4f},
            {Color.FromArgb(255, 238, 0), 2.5f},
            {Color.FromArgb(254, 234, 0), 2.6f},
            {Color.FromArgb(255, 231, 0), 2.7f},
            {Color.FromArgb(254, 227, 0), 2.8f},
            {Color.FromArgb(255, 224, 0), 2.9f},
            {Color.FromArgb(255, 221, 0), 3.0f},
            {Color.FromArgb(254, 213, 0), 3.1f},
            {Color.FromArgb(254, 205, 0), 3.2f},
            {Color.FromArgb(254, 197, 0), 3.3f},
            {Color.FromArgb(254, 190, 0), 3.4f},
            {Color.FromArgb(255, 182, 0), 3.5f},
            {Color.FromArgb(254, 174, 0), 3.6f},
            {Color.FromArgb(255, 167, 0), 3.7f},
            {Color.FromArgb(254, 159, 0), 3.8f},
            {Color.FromArgb(255, 151, 0), 3.9f},
            {Color.FromArgb(255, 144, 0), 4.0f},
            {Color.FromArgb(254, 136, 0), 4.1f},
            {Color.FromArgb(254, 128, 0), 4.2f},
            {Color.FromArgb(254, 121, 0), 4.3f},
            {Color.FromArgb(254, 113, 0), 4.4f},
            {Color.FromArgb(255, 106, 0), 4.5f},
            {Color.FromArgb(254, 98, 0), 4.6f},
            {Color.FromArgb(255, 90, 0), 4.7f},
            {Color.FromArgb(254, 83, 0), 4.8f},
            {Color.FromArgb(255, 75, 0), 4.9f},
            {Color.FromArgb(255, 68, 0), 5.0f},
            {Color.FromArgb(254, 61, 0), 5.1f},
            {Color.FromArgb(253, 54, 0), 5.2f},
            {Color.FromArgb(252, 47, 0), 5.3f},
            {Color.FromArgb(251, 40, 0), 5.4f},
            {Color.FromArgb(250, 33, 0), 5.5f},
            {Color.FromArgb(249, 27, 0), 5.6f},
            {Color.FromArgb(248, 20, 0), 5.7f},
            {Color.FromArgb(247, 13, 0), 5.8f},
            {Color.FromArgb(246, 6, 0), 5.9f},
            {Color.FromArgb(245, 0, 0), 6.0f},
            {Color.FromArgb(238, 0, 0), 6.1f},
            {Color.FromArgb(230, 0, 0), 6.2f},
            {Color.FromArgb(223, 0, 0), 6.3f},
            {Color.FromArgb(215, 0, 0), 6.4f},
            {Color.FromArgb(208, 0, 0), 6.5f},
            {Color.FromArgb(200, 0, 0), 6.6f},
            {Color.FromArgb(192, 0, 0), 6.7f},
            {Color.FromArgb(185, 0, 0), 6.8f},
            {Color.FromArgb(177, 0, 0), 6.9f},
        };
        private static readonly HttpClient HttpClient = new HttpClient();
        // 観測点リスト
        private readonly string[] _station = Properties.Resources.Station.Split('\n');
        // 強震モニタ
        private readonly ColorMap[] _mapRealTime =
        {
            new ColorMap
            {
                OldColor = Color.FromArgb(0, 0, 0),
                NewColor = MainWindow.Black
            }
        };
        // 緊急地震速報 P波・S波到達予想円
        private readonly ColorMap[] _mapPSWave = 
        {
            new ColorMap
            {
                OldColor = Color.FromArgb(0, 0, 255),
                NewColor = MainWindow.Blue
            },
            new ColorMap
            {
                OldColor = Color.FromArgb(255, 0, 0),
                NewColor = MainWindow.Red
            }
        };
        private const string Uri = "http://www.kmoni.bosai.go.jp/new/data/map_img/";
        private static MainWindow _instance;
        // 地表震度
        private static BitmapData _dataRealTime;

        public static bool OnTrigger { get; private set; }

        public static int Intensity { get; private set; }

        public Kyoshin(MainWindow instance)
        {
            _instance = instance;
            // インデックスを「リアルタイム震度」に設定
            _instance.comboBox_MapType.SelectedIndex = 0;
        }

        public async Task UpdateKyoshinAsync()
        {
            var time = $"{MainWindow.LatestTime:yyyyMMdd}/{MainWindow.LatestTime:yyyyMMddHHmmss}";
            using (var realTimeImg = await GetRealTimeImageAsync(time))
            {
                using (var bitmapRealTime = (Bitmap)await RequestImageAsync($"{Uri}RealTimeImg/jma_s/{time}.jma_s.gif"))
                {
                    _dataRealTime =
                        bitmapRealTime.LockBits(new Rectangle(0, 0, bitmapRealTime.Width, bitmapRealTime.Height),
                            ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                    GetRealtimeIntensity(realTimeImg);
                    bitmapRealTime.UnlockBits(_dataRealTime);
                }

                var bitmapPictureBox = new Bitmap(realTimeImg);
                using (var graphicsEpicenter = Graphics.FromImage(bitmapPictureBox))
                {
                    if (EEW.OnTrigger)
                    {
                        graphicsEpicenter.DrawImage(await GetPSWaveImageAsync(time), 0, 0, realTimeImg.Width,
                            realTimeImg.Height);
                    }
                    else
                    {
                        if (EqInfo.BitmapEpicenter != null)
                        {
                            graphicsEpicenter.DrawImage(EqInfo.BitmapEpicenter, 0, 0, realTimeImg.Width,
                                realTimeImg.Height);
                        }
                    }
                    graphicsEpicenter.DrawImage(Properties.Resources.BaseMapBorder, 0, 0, realTimeImg.Width,
                        realTimeImg.Height);
                }

                var oldImage = _instance.pictureBox_kyoshinMap.Image;
                _instance.pictureBox_kyoshinMap.Image = bitmapPictureBox;
                oldImage?.Dispose();
            }
        }

        // 画像をリクエスト
        private static async Task<Image> RequestImageAsync(string requestUri)
        {
            using (var stream = await HttpClient.GetStreamAsync(requestUri))
            {
                return Image.FromStream(stream, false, false);
            }
        }

        // 強震モニタ
        private async Task<Bitmap> GetRealTimeImageAsync(string time)
        {
            // コントロールから設定を取得
            var mapType = MapType[_instance.comboBox_MapType.SelectedIndex];
            var mapSb = _instance.radioButton_Surface.Checked ? "s" : "b";
            var bitmap = Properties.Resources.BaseMap;
            using (var graphics = Graphics.FromImage(bitmap))
            using (var imageAttrs = new ImageAttributes())
            {
                imageAttrs.SetRemapTable(_mapRealTime);
                graphics.DrawImage(
                    await RequestImageAsync($"{Uri}RealTimeImg/{mapType}_{mapSb}/{time}.{mapType}_{mapSb}.gif"),
                    new Rectangle(Point.Empty, bitmap.Size), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel,
                    imageAttrs);
            }
            return bitmap;
        }

        // 最大地表震度を取得
        private void GetRealtimeIntensity(Bitmap realTimeImg)
        {
            OnTrigger = false;
            var pointList = new Dictionary<Dictionary<string, Point>, float>(); // 観測点リスト
            var prefList = new Dictionary<string, int>(); // 地震を検知した都道府県リスト
            var prefName = "";  // 地震を検知した都道府県名
            var pointNum = 0;   // 地震を検知した都道府県別の地点数
            foreach (var line in _station)
            {
                var items = line.Split(',');
                var intensity = GetInstIntensity(int.Parse(items[3]), int.Parse(items[4]));
                if (items[0] == "0" || intensity == -3.0f) continue;
                var pointInfo = new Dictionary<string, Point>
                {
                    {$"{items[2]} {items[1]}", new Point(int.Parse(items[3]), int.Parse(items[4]))}
                };
                if (!pointList.ContainsKey(pointInfo))
                {
                    pointList.Add(pointInfo, intensity);
                }
                else if (pointList[pointInfo] < intensity)
                {
                    pointList[pointInfo] = intensity;
                }

                // 都道府県リストを作成
                if (intensity <= 0.0f) continue;
                if (prefName != items[2] || pointNum == 0)
                {
                    prefName = items[2];
                    pointNum = 1;
                }
                else
                {
                    pointNum++;
                }
                if (pointNum < 3) continue;
                if (!prefList.ContainsKey(items[2]))
                {
                    prefList.Add(prefName, pointNum);
                }
                else
                {
                    prefList[prefName] = pointNum;
                }
            }
            var maxInt = pointList.Values.Max();
            Intensity = ToInteger(maxInt);
            // トリガチェック（レガシー）
            if (prefList.Count != 0)
            {
                OnTrigger = true;
            }
            // 最大震度（気象庁震度階級）を検知した地点数
            var kvpd = pointList.OrderByDescending(x => x.Value);
            var maxIntNum = -1 + kvpd.Where(x => !(x.Value < 0.5))
                                .Count(kvp => ToJMAIntensity(kvp.Value) == ToJMAIntensity(maxInt));
            // トリガチェック（テリトリー）
            var trigger = -1;
            var maxIntPoint = kvpd.First().Key.Values.First();
            for (var y = maxIntPoint.Y - 2; y <= maxIntPoint.Y + 2; y += 2)
            {
                for (var x = maxIntPoint.X - 2; x <= maxIntPoint.X + 2; x += 2)
                {
                    if (GetInstIntensity(x, y) >= 1.5f)
                    {
                        trigger++;
                    }
                }
            }
            if (trigger > 0) OnTrigger = true;
            // 最大震度を検知した地点に円を描画
            using (var graphicsMaxInt = Graphics.FromImage(realTimeImg))
            {
                graphicsMaxInt.DrawEllipse(new Pen(GetColor(maxIntPoint.X, maxIntPoint.Y)),
                    maxIntPoint.X - 12, maxIntPoint.Y - 12, 24, 24);
            }
            // 最大震度を検知した地点をラベルに設定
            _instance.label_kyoshinMaxInt.Text = $"{ToJMAIntensity(maxInt)}（{kvpd.First().Key.Keys.First()}）";
            if (maxIntNum > 1)
            {
                _instance.label_kyoshinMaxIntDetail.Text = $"他 {maxIntNum - 1} 地点";
                _instance.label_kyoshinMaxIntDetail.Location =
                    new Point(_instance.label_kyoshinMaxInt.Location.X + _instance.label_kyoshinMaxInt.Size.Width,
                        _instance.label_kyoshinMaxIntDetail.Location.Y);
            }
            else
            {
                _instance.label_kyoshinMaxIntDetail.Text = "";
            }
            // 地表リアルアイム震度0.1以上を検知した都道府県をラベルに設定
            if (prefList.Count == 0)
            {
                _instance.label_kyoshinPrefecture.Text = "";
            }
            else
            {
                var kvpi = prefList.OrderByDescending(x => x.Value);
                prefName = "";
                var num = 0;
                foreach (var kvp in kvpi)
                {
                    prefName += kvp.Key + "　";
                    if (prefList.Count > 5 && prefList.Count < 10 && num == (prefList.Count - 1) >> 1 ||
                        prefList.Count > 9 && num == 4)
                    {
                        prefName = $"{prefName.TrimEnd('　')}\n";
                    }
                    if (num >= 8)
                    {
                        prefName += $"他 {prefList.Count - num} 都道府県";
                        break;
                    }
                    num++;
                }
                prefName = prefName.TrimEnd('　');
                _instance.label_kyoshinPrefecture.Text = prefName;
                _instance.label_kyoshinPrefecture.Location =
                    new Point(_instance.pictureBox_kyoshinMap.Width - _instance.label_kyoshinPrefecture.Size.Width,
                        _instance.pictureBox_kyoshinMap.Height - _instance.label_kyoshinPrefecture.Size.Height + 16);
            }
        }

        // 緊急地震速報 P波・S波到達予想円
        private async Task<Bitmap> GetPSWaveImageAsync(string time)
        {
            var bitmap = new Bitmap(_instance.pictureBox_kyoshinMap.Width, _instance.pictureBox_kyoshinMap.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            using (var imageAttrs = new ImageAttributes())
            {
                imageAttrs.SetRemapTable(_mapPSWave);
                graphics.DrawImage(await RequestImageAsync($"{Uri}PSWaveImg/eew/{time}.eew.gif"),
                    new Rectangle(Point.Empty, bitmap.Size), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel,
                    imageAttrs);
            }
            return bitmap;
        }

        private static unsafe Color GetColor(int x, int y)
        {
            var scan0 = (byte*)_dataRealTime.Scan0;
            var index = x * 3 + _dataRealTime.Stride * y;
            return Color.FromArgb(scan0[index + 2], scan0[index + 1], scan0[index]);
        }

        // 色から計測震度を取得
        private static float GetInstIntensity(int x, int y)
        {
            return !ColorTable.TryGetValue(GetColor(x, y), out var value) ? -3.0f : value;
        }

        // 計測震度を整数に変換
        private static int ToInteger(float seismicInt)
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

        // 計測震度を気象庁震度階級に変換
        private static string ToJMAIntensity(float seismicInt)
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
    }
}
