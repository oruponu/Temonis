﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using static Temonis.MainWindow;

namespace Temonis
{
    internal class Eew
    {
        public static Dictionary<string, string> Info { get; private set; } = new Dictionary<string, string>();

        public static bool IsTriggerOn { get; private set; }

        /// <summary>
        /// 緊急地震速報を取得します。
        /// </summary>
        /// <returns></returns>
        public async Task UpdateAsync()
        {
            var json = default(Root);
            try
            {
                using (var stream = await MainWindow.HttpClient.GetStreamAsync($"{Properties.Resources.EewUri}{LatestTime:yyyyMMddHHmmss}.json"))
                {
                    var serializer = new DataContractJsonSerializer(typeof(Root));
                    json = (Root)serializer.ReadObject(stream);
                }
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException || ex is SerializationException)
            {
                InternalLog(ex);
            }

            if (json == default(Root)) return;

            if (json.Result.Message != "データがありません")
            {
                if ((bool)json.IsCancel)
                {
                    IsTriggerOn = false;
                    Instance.Label_EewMessage.Text = "緊急地震速報は取り消されました。";
                    Instance.Label_EewDateTimeHeader.Visible = false;
                    Instance.Label_EewEpicenterHeader.Visible = false;
                    Instance.Label_EewDepthHeader.Visible = false;
                    Instance.Label_EewMagnitudeHeader.Visible = false;
                    Instance.Label_EewIntensityHeader.Visible = false;
                    Instance.Label_EewDateTime.Text = "";
                    Instance.Label_EewEpicenter.Text = "";
                    Instance.Label_EewDepth.Text = "";
                    Instance.Label_EewMagnitude.Text = "";
                    Instance.Label_EewIntensity.Text = "";
                    Info[json.ReportId] = "0";
                }
                else if (EqInfo.Id == json.ReportId && !Kyoshin.IsTriggerOn && (bool)json.IsFinal)
                {
                    IsTriggerOn = false;
                    Info[json.ReportId] = "-1";
                }
                else if (!Info.ContainsKey(json.ReportId) || Info.ContainsKey(json.ReportId) && Info[json.ReportId] != "-1")
                {
                    IsTriggerOn = true;
                    var serial = "第" + json.ReportNum + "報";
                    if ((bool)json.IsFinal) serial += " 最終";
                    Instance.Label_EewMessage.Text = $"緊急地震速報（{json.Alertflg}）{serial}";
                    Instance.Label_EewDateTimeHeader.Visible = true;
                    Instance.Label_EewEpicenterHeader.Visible = true;
                    Instance.Label_EewDepthHeader.Visible = true;
                    Instance.Label_EewMagnitudeHeader.Visible = true;
                    Instance.Label_EewIntensityHeader.Visible = true;
                    Instance.Label_EewDateTime.Text = DateTime.ParseExact(json.OriginTime, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("yyyy年MM月dd日 HH時mm分ss秒");
                    Instance.Label_EewEpicenter.Text = json.RegionName;
                    Instance.Label_EewDepth.Text = json.Depth;
                    Instance.Label_EewMagnitude.Text = json.Magunitude;
                    Instance.Label_EewIntensity.Text = json.Calcintensity;
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
                IsTriggerOn = false;
                Info = new Dictionary<string, string>();
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
    }
}
