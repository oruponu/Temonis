using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Temonis.MainWindow;

namespace Temonis
{
    public static class Eew
    {
        private static readonly Dictionary<string, string> Info = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> PrevInfo = new Dictionary<string, string>();

        public static bool IsTriggerOn { get; private set; }

        /// <summary>
        /// 緊急地震速報を取得します。
        /// </summary>
        /// <returns></returns>
        public static async Task UpdateAsync()
        {
            var json = default(Json);
            try
            {
                var response = await MainWindow.HttpClient.GetAsync(Properties.Resources.EewUri + LatestTime.ToString("yyyyMMddHHmmss") + ".json");
                if (!response.IsSuccessStatusCode)
                    return;
                await using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                json = await JsonSerializer.DeserializeAsync<Json>(stream).ConfigureAwait(false);
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                WriteLog(ex);
            }

            if (json == default(Json))
                return;

            if (json.Result.Message != "データがありません")
            {
                if (json.IsCancel.GetBoolean())
                {
                    MainWindow.DataContext.Eew.Message = "緊急地震速報は取り消されました。";
                }
                else if (EqInfo.Id == json.ReportId && !Kyoshin.IsTriggerOn && json.IsFinal.GetBoolean())
                {
                    IsTriggerOn = false;
                    Info[json.ReportId] = "-1";
                }
                else if (!Info.TryGetValue(json.ReportId, out var value) || value != null && value != "-1")
                {
                    IsTriggerOn = true;
                    var serial = '第' + json.ReportNum + '報';
                    if (json.IsFinal.GetBoolean())
                        serial += " 最終";
                    MainWindow.DataContext.Eew.Message = $"緊急地震速報（{json.Alertflg}）{serial}";
                    MainWindow.DataContext.Eew.Visible = true;
                    MainWindow.DataContext.Eew.DateTime = DateTime.ParseExact(json.OriginTime, "yyyyMMddHHmmss", new CultureInfo("ja-JP")).ToString("yyyy年MM月dd日 HH時mm分ss秒");
                    MainWindow.DataContext.Eew.Epicenter = json.RegionName;
                    MainWindow.DataContext.Eew.Depth = json.Depth;
                    MainWindow.DataContext.Eew.Magnitude = json.Magunitude;
                    MainWindow.DataContext.Eew.Intensity = json.Calcintensity;

                    if (value == null)
                        Info.Add(json.ReportId, json.Calcintensity);
                    else if (value != json.Calcintensity)
                        Info[json.ReportId] = json.Calcintensity;
                }
            }
            else
            {
                IsTriggerOn = false;
                Info.Clear();
            }

            UpdateState();
        }

        private static void UpdateState()
        {
            if (IsTriggerOn)
            {
                if (MainWindow.DataContext.Eew.Message.Contains("警報"))
                {
                    MainWindow.DataContext.Eew.Level = Level.Red;
                }
                else if (MainWindow.DataContext.Eew.Message.Contains("予報"))
                {
                    MainWindow.DataContext.Eew.Level = Level.Yellow;
                }
                else // キャンセル報
                {
                    IsTriggerOn = false;
                    MainWindow.DataContext.Eew.Level = Level.White;
                    PrevInfo.Clear();
                    Sound.PlayMaxIntChange("0");
                    SetActive();
                    return;
                }

                foreach (var info in Info)
                {
                    if (!PrevInfo.TryGetValue(info.Key, out var value))
                    {
                        PrevInfo.Add(info.Key, info.Value);
                        Sound.PlayFirstReport(info.Value);
                        SetActive();
                    }
                    else if (value != info.Value)
                    {
                        PrevInfo[info.Key] = info.Value;
                        Sound.PlayMaxIntChange(info.Value);
                        SetActive();
                    }
                }
            }
            else
            {
                MainWindow.DataContext.Eew.Level = Level.White;
                PrevInfo.Clear();
            }
        }

        public class DataContext : BindableBase
        {
            private Level _level;
            private string _message = "緊急地震速報は発表されていません。";
            private bool _visible;
            private string _dateTime;
            private string _epicenter;
            private string _depth;
            private string _magnitude;
            private string _intensity;

            public Level Level
            {
                get => _level;
                set => SetProperty(ref _level, value);
            }

            public string Message
            {
                get => _message;
                set => SetProperty(ref _message, value);
            }

            public bool Visible
            {
                get => _visible;
                set => SetProperty(ref _visible, value);
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

            public string Intensity
            {
                get => _intensity;
                set => SetProperty(ref _intensity, value);
            }
        }

        private class Json
        {
            [JsonPropertyName("result")]
            public ResultClass Result { get; set; }

            [JsonPropertyName("region_name")]
            public string RegionName { get; set; }

            [JsonPropertyName("is_cancel")]
            public JsonElement IsCancel { get; set; }

            [JsonPropertyName("depth")]
            public string Depth { get; set; }

            [JsonPropertyName("calcintensity")]
            public string Calcintensity { get; set; }

            [JsonPropertyName("is_final")]
            public JsonElement IsFinal { get; set; }

            [JsonPropertyName("origin_time")]
            public string OriginTime { get; set; }

            [JsonPropertyName("magunitude")]
            public string Magunitude { get; set; }

            [JsonPropertyName("report_num")]
            public string ReportNum { get; set; }

            [JsonPropertyName("report_id")]
            public string ReportId { get; set; }

            [JsonPropertyName("alertflg")]
            public string Alertflg { get; set; }

            public class ResultClass
            {
                [JsonPropertyName("message")]
                public string Message { get; set; }
            }
        }
    }
}
