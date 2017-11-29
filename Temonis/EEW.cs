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

        public async Task UpdateEEW()
        {
            using (var stream = await HttpClient.GetStreamAsync($"{Uri}{MainWindow.LatestTime:yyyyMMddHHmmss}.json"))
            {
                var eew = (EEWJson)new DataContractJsonSerializer(typeof(EEWJson)).ReadObject(stream);
                if (eew.Result.Message != "データがありません")
                {
                    if ((bool)eew.IsCancel)
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
                        Info[eew.ReportId] = "0";
                    }
                    else if (EqInfo.Id == eew.ReportId)
                    {
                        OnTrigger = false;
                    }
                    else
                    {
                        OnTrigger = true;
                        var serial = "第" + eew.ReportNum + "報";
                        if ((bool)eew.IsFinal) serial += " 最終";
                        _instance.label_eewMessage.Text = $"緊急地震速報（{eew.Alertflg}）{serial}";
                        _instance.label_eewTimeHeader.Visible = true;
                        _instance.label_eewEpicenterHeader.Visible = true;
                        _instance.label_eewDepthHeader.Visible = true;
                        _instance.label_eewMagnitudeHeader.Visible = true;
                        _instance.label_eewIntensityHeader.Visible = true;
                        _instance.label_eewTime.Text = DateTime
                            .ParseExact(eew.OriginTime, "yyyyMMddHHmmss", CultureInfo.InvariantCulture)
                            .ToString("yyyy年MM月dd日 HH時mm分ss秒");
                        _instance.label_eewEpicenter.Text = eew.RegionName;
                        _instance.label_eewDepth.Text = eew.Depth;
                        _instance.label_eewMagnitude.Text = eew.Magunitude;
                        _instance.label_eewIntensity.Text = eew.Calcintensity;
                        if (!Info.ContainsKey(eew.ReportId))
                        {
                            Info.Add(eew.ReportId, eew.Calcintensity);
                        }
                        else if (Info[eew.ReportId] != eew.Calcintensity)
                        {
                            Info[eew.ReportId] = eew.Calcintensity;
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

    // JSONクラス
    [DataContract]
    public class EEWJson
    {
        [DataMember(Name = "result")]
        public ResultEEW Result { get; set; }

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
        public SecurityEEW Security { get; set; }

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
    }

    [DataContract]
    public class ResultEEW
    {
        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "is_auth")]
        public bool IsAuth { get; set; }
    }

    [DataContract]
    public class SecurityEEW
    {
        [DataMember(Name = "realm")]
        public string Realm { get; set; }

        [DataMember(Name = "hash")]
        public string Hash { get; set; }
    }
}
