using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using static Temonis.MainWindow;

namespace Temonis
{
    public static class Eew
    {
        private static Dictionary<string, string> _info = new Dictionary<string, string>();
        private static Dictionary<string, string> _prevInfo = new Dictionary<string, string>();

        public static bool IsTriggerOn { get; private set; }

        /// <summary>
        /// 緊急地震速報を取得します。
        /// </summary>
        /// <returns></returns>
        public static async Task UpdateAsync()
        {
            var json = default(Root);
            try
            {
                using (var stream = await MainWindow.HttpClient.GetStreamAsync(Properties.Resources.EewUri + LatestTime.ToString("yyyyMMddHHmmss") + ".json"))
                    json = (Root)new DataContractJsonSerializer(typeof(Root)).ReadObject(stream);
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException || ex is SerializationException)
            {
                InternalLog(ex);
            }

            if (json == default(Root))
                return;

            if (json.Result.Message != "データがありません")
            {
                if ((bool)json.IsCancel)
                {
                    IsTriggerOn = false;
                    MainWindow.DataContext.Eew.Message = "緊急地震速報は取り消されました。";
                    MainWindow.DataContext.Eew.Visible = false;
                    _info[json.ReportId] = "0";
                }
                else if (EqInfo.Id == json.ReportId && !Kyoshin.IsTriggerOn && (bool)json.IsFinal)
                {
                    IsTriggerOn = false;
                    _info[json.ReportId] = "-1";
                }
                else if (!_info.ContainsKey(json.ReportId) || _info.TryGetValue(json.ReportId, out var intensity) && intensity != "-1")
                {
                    IsTriggerOn = true;
                    var serial = '第' + json.ReportNum + '報';
                    if ((bool)json.IsFinal)
                        serial += " 最終";
                    MainWindow.DataContext.Eew.Message = $"緊急地震速報（{json.Alertflg}）{serial}";
                    MainWindow.DataContext.Eew.Visible = true;
                    MainWindow.DataContext.Eew.DateTime = DateTime.ParseExact(json.OriginTime, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日 HH時mm分ss秒");
                    MainWindow.DataContext.Eew.Epicenter = json.RegionName;
                    MainWindow.DataContext.Eew.Depth = json.Depth;
                    MainWindow.DataContext.Eew.Magnitude = json.Magunitude;
                    MainWindow.DataContext.Eew.Intensity = json.Calcintensity;

                    if (_info.TryGetValue(json.ReportId, out var value))
                    {
                        if (value != json.Calcintensity)
                            _info[json.ReportId] = json.Calcintensity;
                    }
                    else
                    {
                        _info.Add(json.ReportId, json.Calcintensity);
                    }
                }
            }
            else
            {
                IsTriggerOn = false;
                _info = new Dictionary<string, string>();
            }

            UpdateState();
        }

        private static void UpdateState()
        {
            if (IsTriggerOn)
            {
                if (MainWindow.DataContext.Eew.Message.Contains("警報"))
                    MainWindow.DataContext.Eew.Level = Level.Red;
                else if (MainWindow.DataContext.Eew.Message.Contains("予報"))
                    MainWindow.DataContext.Eew.Level = Level.Yellow;

                foreach (var info in _info)
                {
                    if (_prevInfo.TryGetValue(info.Key, out var value))
                    {
                        if (value == info.Value)
                            continue;
                        _prevInfo[info.Key] = info.Value;
                        Sound.PlayMaxIntChangeAsync(info.Value);
                        SetActive();
                    }
                    else
                    {
                        _prevInfo.Add(info.Key, info.Value);
                        Sound.PlayFirstReportAsync(info.Value);
                        SetActive();
                    }
                }
            }
            else
            {
                MainWindow.DataContext.Eew.Level = Level.White;
                _prevInfo = new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// JSONクラス
        /// </summary>
        [DataContract]
        public class Root
        {
            [DataMember(Name = "result")]
            public ResultClass Result { get; set; }

            [DataMember(Name = "report_time")]
            public string ReportTime { get; set; }

            [DataMember(Name = "region_code")]
            public string RegionCode { get; set; }

            [DataMember(Name = "request_time")]
            public string RequestTime { get; set; }

            [DataMember(Name = "region_name")]
            public string RegionName { get; set; }

            [DataMember(Name = "longitude")]
            public string Longitude { get; set; }

            [DataMember(Name = "is_cancel")]
            public object IsCancel { get; set; }

            [DataMember(Name = "depth")]
            public string Depth { get; set; }

            [DataMember(Name = "calcintensity")]
            public string Calcintensity { get; set; }

            [DataMember(Name = "is_final")]
            public object IsFinal { get; set; }

            [DataMember(Name = "is_training")]
            public object IsTraining { get; set; }

            [DataMember(Name = "latitude")]
            public string Latitude { get; set; }

            [DataMember(Name = "origin_time")]
            public string OriginTime { get; set; }

            [DataMember(Name = "security")]
            public SecurityClass Security { get; set; }

            [DataMember(Name = "magunitude")]
            public string Magunitude { get; set; }

            [DataMember(Name = "report_num")]
            public string ReportNum { get; set; }

            [DataMember(Name = "request_hypo_type")]
            public string RequestHypoType { get; set; }

            [DataMember(Name = "report_id")]
            public string ReportId { get; set; }

            [DataMember(Name = "alertflg")]
            public string Alertflg { get; set; }

            [DataContract]
            public class ResultClass
            {
                [DataMember(Name = "status")]
                public string Status { get; set; }

                [DataMember(Name = "message")]
                public string Message { get; set; }

                [DataMember(Name = "is_auth")]
                public bool IsAuth { get; set; }
            }

            [DataContract]
            public class SecurityClass
            {
                [DataMember(Name = "realm")]
                public string Realm { get; set; }

                [DataMember(Name = "hash")]
                public string Hash { get; set; }
            }
        }

        public class DataContext : INotifyPropertyChanged
        {
            private static readonly PropertyChangedEventArgs LevelPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Level));
            private static readonly PropertyChangedEventArgs MessagePropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Message));
            private static readonly PropertyChangedEventArgs VisiblePropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Visible));
            private static readonly PropertyChangedEventArgs DateTimePropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(DateTime));
            private static readonly PropertyChangedEventArgs EpicenterPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Epicenter));
            private static readonly PropertyChangedEventArgs DepthPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Depth));
            private static readonly PropertyChangedEventArgs MagnitudePropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Magnitude));
            private static readonly PropertyChangedEventArgs IntensityPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Intensity));
            private Level _level;
            private string _message = "緊急地震速報は発表されていません。";
            private bool _visible;
            private string _dateTime;
            private string _epicenter;
            private string _depth;
            private string _magnitude;
            private string _intensity;

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

            public string Message
            {
                get => _message;
                set
                {
                    _message = value;
                    PropertyChanged?.Invoke(this, MessagePropertyChangedEventArgs);
                }
            }

            public bool Visible
            {
                get => _visible;
                set
                {
                    _visible = value;
                    PropertyChanged?.Invoke(this, VisiblePropertyChangedEventArgs);
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

            public string Intensity
            {
                get => _intensity;
                set
                {
                    _intensity = value;
                    PropertyChanged?.Invoke(this, IntensityPropertyChangedEventArgs);
                }
            }
        }
    }
}
