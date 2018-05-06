using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace Temonis
{
    internal class EEW
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private const string Uri = "http://www.kmoni.bosai.go.jp/new/webservice/hypo/eew/";
        private static MainWindow _instance;

        public static Dictionary<string, string> Info { get; private set; } = new Dictionary<string, string>();

        public static bool OnTrigger { get; private set; }
        
        public EEW(MainWindow instance)
        {
            _instance = instance;
        }

        /// <summary>
        /// 緊急地震速報を取得
        /// </summary>
        /// <returns></returns>
        public async Task UpdateEEWAsync()
        {
            using (var stream = await HttpClient.GetStreamAsync($"{Uri}{MainWindow.LatestTime:yyyyMMddHHmmss}.json"))
            {
                var serializer = new DataContractJsonSerializer(typeof(EEWJson));
                var json = (EEWJson)serializer.ReadObject(stream);
                if (json.Result.Message != "データがありません")
                {
                    if ((bool)json.IsCancel)
                    {
                        OnTrigger = false;
                        _instance.label_eewMessage.Text = "緊急地震速報は取り消されました。";
                        _instance.label_eewTimeHeader.Visible = false;
                        _instance.label_eewEpicenterHeader.Visible = false;
                        _instance.label_eewDepthHeader.Visible = false;
                        _instance.label_eewMagnitudeHeader.Visible = false;
                        _instance.label_eewIntensityHeader.Visible = false;
                        _instance.label_eewTime.Text = "";
                        _instance.label_eewEpicenter.Text = "";
                        _instance.label_eewDepth.Text = "";
                        _instance.label_eewMagnitude.Text = "";
                        _instance.label_eewIntensity.Text = "";
                        Info[json.ReportId] = "0";
                    }
                    else if (EqInfo.Id == json.ReportId)
                    {
                        OnTrigger = false;
                    }
                    else
                    {
                        OnTrigger = true;
                        var serial = "第" + json.ReportNum + "報";
                        if ((bool)json.IsFinal) serial += " 最終";
                        _instance.label_eewMessage.Text = $"緊急地震速報（{json.Alertflg}）{serial}";
                        _instance.label_eewTimeHeader.Visible = true;
                        _instance.label_eewEpicenterHeader.Visible = true;
                        _instance.label_eewDepthHeader.Visible = true;
                        _instance.label_eewMagnitudeHeader.Visible = true;
                        _instance.label_eewIntensityHeader.Visible = true;
                        _instance.label_eewTime.Text = DateTime
                            .ParseExact(json.OriginTime, "yyyyMMddHHmmss", CultureInfo.InvariantCulture)
                            .ToString("yyyy年MM月dd日 HH時mm分ss秒");
                        _instance.label_eewEpicenter.Text = json.RegionName;
                        _instance.label_eewDepth.Text = json.Depth;
                        _instance.label_eewMagnitude.Text = json.Magunitude;
                        _instance.label_eewIntensity.Text = json.Calcintensity;
                        if (!Info.ContainsKey(json.ReportId))
                        {
                            Info.Add(json.ReportId, json.Calcintensity);
                        }
                        else if (Info[json.ReportId] != json.Calcintensity)
                        {
                            Info[json.ReportId] = json.Calcintensity;
                        }
                    }
                }
                else
                {
                    OnTrigger = false;
                    Info = new Dictionary<string, string>();
                }
            }
        }
    }

    /// <summary>
    /// JSONクラス
    /// </summary>
    [DataContract]
    public class EEWJson
    {
        [DataMember(Name = "result")]
        public ResultJson Result { get; set; }

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
        public SecurityJson Security { get; set; }

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
        public class ResultJson
        {
            [DataMember(Name = "status")]
            public string Status { get; set; }

            [DataMember(Name = "message")]
            public string Message { get; set; }

            [DataMember(Name = "is_auth")]
            public bool IsAuth { get; set; }
        }

        [DataContract]
        public class SecurityJson
        {
            [DataMember(Name = "realm")]
            public string Realm { get; set; }

            [DataMember(Name = "hash")]
            public string Hash { get; set; }
        }
    }
}
