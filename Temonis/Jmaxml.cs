using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using static Temonis.MainWindow;

namespace Temonis
{
    internal class Jmaxml
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private static readonly Dictionary<string, DateTimeOffset?> LastModified = new Dictionary<string, DateTimeOffset?>();

        private static string _prevRef;

        public static async Task RequestFeedAsync(string fileName = "eqvol")
        {
            var request = new HttpRequestMessage(HttpMethod.Get, Properties.Resources.JmxUri + fileName + ".xml");
            if (LastModified.TryGetValue(fileName, out var lastModified))
                request.Headers.IfModifiedSince = lastModified;

            HttpResponseMessage response;
            try
            {
                response = await HttpClient.SendAsync(request);
            }
            catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
            {
                WriteLog(ex);
                return;
            }

            if (!response.IsSuccessStatusCode)
                return;
            if (response.Content.Headers.LastModified != null)
            {
                if (lastModified != null)
                    LastModified[fileName] = response.Content.Headers.LastModified;
                else
                    LastModified.Add(fileName, response.Content.Headers.LastModified);
            }

            await using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = XmlReader.Create(stream);
            var feed = SyndicationFeed.Load(reader);
            var xmls = feed.Items.Select(item => new Feed
            {
                Title = item.Title.Text,
                Updated = item.LastUpdatedTime.LocalDateTime,
                Content = ((TextSyndicationContent)item.Content).Text,
                Uri = item.Links[0].Uri.OriginalString
            }).Where(item => item.Title == "震度速報" || item.Title == "震源に関する情報" || item.Title == "震源・震度に関する情報").ToArray();
            if (fileName == "eqvol" && !xmls.Any())
                await RequestFeedAsync("eqvol_l");
            if (!xmls.Any())
                return;

            var prevEventId = "";
            var prevInfoKind = "";
            foreach (var xml in xmls)
            {
                var report = await EqInfo.RequestAsync(xml.Uri);
                if (report == null)
                    continue;

                if (prevEventId.Length != 0 && report.Head.EventId != prevEventId)
                    return;
                if (prevEventId == report.Head.EventId && prevInfoKind == report.Control.Title)
                    continue;

                if (_prevRef == xml.Uri)
                    return;
                _prevRef = xml.Uri;

                if (xml.Title == "震源・震度に関する情報")
                {
                    EqInfo.Update(report);
                    return;
                }

                EqInfo.Update(report, prevEventId);
                if (prevInfoKind.Length != 0 && prevInfoKind != report.Control.Title)
                    return;
                prevEventId = report.Head.EventId;
                prevInfoKind = report.Control.Title;
            }
        }

        private class Feed
        {
            public string Title { get; set; }

            public DateTime Updated { get; set; }

            public string Content { get; set; }

            public string Uri { get; set; }
        }
    }
}
