using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;

namespace Temonis
{
    internal static class Settings
    {
        private const string FileName = "Settings.json";

        public static Json JsonClass { get; private set; }

        public static void Load()
        {
            if (!File.Exists(FileName))
            {
                using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Temonis.Resources.Settings.json");
                using var memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
                File.WriteAllBytes(FileName, memoryStream.ToArray());
            }

            try
            {
                JsonClass = JsonSerializer.Deserialize<Json>(File.ReadAllBytes(FileName));
            }
            catch
            {
                MessageBox.Show("設定ファイルを開けませんでした。\n音声は再生されません。", "Temonis", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        public class Json
        {
            [JsonPropertyName("appearance")]
            public AppearanceClass Appearance { get; set; }

            [JsonPropertyName("behavior")]
            public BehaviorClass Behavior { get; set; }

            [JsonPropertyName("sounds")]
            public SoundsClass Sounds { get; set; }

            public class AppearanceClass
            {
                [JsonPropertyName("useJMASeismicIntensityScale")]
                public bool UseJmaSeismicIntensityScale { get; set; }
            }

            public class BehaviorClass
            {
                [JsonPropertyName("forceActive")]
                public bool ForceActive { get; set; }
            }

            public class SoundsClass
            {
                [JsonPropertyName("kyoshin")]
                public Kyoshin Kyoshin { get; set; }

                [JsonPropertyName("eew")]
                public Eew Eew { get; set; }

                [JsonPropertyName("eqInfo")]
                public EqInfo EqInfo { get; set; }
            }

            public class Kyoshin
            {
                [JsonPropertyName("intensity1")]
                public string Intensity1 { get; set; }

                [JsonPropertyName("intensity2")]
                public string Intensity2 { get; set; }

                [JsonPropertyName("intensity3")]
                public string Intensity3 { get; set; }

                [JsonPropertyName("intensity4")]
                public string Intensity4 { get; set; }

                [JsonPropertyName("intensity5")]
                public string Intensity5 { get; set; }

                [JsonPropertyName("intensity6")]
                public string Intensity6 { get; set; }

                [JsonPropertyName("intensity7")]
                public string Intensity7 { get; set; }

                [JsonPropertyName("intensity8")]
                public string Intensity8 { get; set; }

                [JsonPropertyName("intensity9")]
                public string Intensity9 { get; set; }
            }

            public class Eew
            {
                [JsonPropertyName("firstReport")]
                public FirstReport FirstReport { get; set; }

                [JsonPropertyName("maxIntChange")]
                public MaxIntChange MaxIntChange { get; set; }
            }

            public class FirstReport
            {
                [JsonPropertyName("intensity1")]
                public string Intensity1 { get; set; }

                [JsonPropertyName("intensity2")]
                public string Intensity2 { get; set; }

                [JsonPropertyName("intensity3")]
                public string Intensity3 { get; set; }

                [JsonPropertyName("intensity4")]
                public string Intensity4 { get; set; }

                [JsonPropertyName("intensity5")]
                public string Intensity5 { get; set; }

                [JsonPropertyName("intensity6")]
                public string Intensity6 { get; set; }

                [JsonPropertyName("intensity7")]
                public string Intensity7 { get; set; }

                [JsonPropertyName("intensity8")]
                public string Intensity8 { get; set; }

                [JsonPropertyName("intensity9")]
                public string Intensity9 { get; set; }

                [JsonPropertyName("unknown")]
                public string Unknown { get; set; }
            }

            public class MaxIntChange
            {
                [JsonPropertyName("cancel")]
                public string Cancel { get; set; }

                [JsonPropertyName("intensity1")]
                public string Intensity1 { get; set; }

                [JsonPropertyName("intensity2")]
                public string Intensity2 { get; set; }

                [JsonPropertyName("intensity3")]
                public string Intensity3 { get; set; }

                [JsonPropertyName("intensity4")]
                public string Intensity4 { get; set; }

                [JsonPropertyName("intensity5")]
                public string Intensity5 { get; set; }

                [JsonPropertyName("intensity6")]
                public string Intensity6 { get; set; }

                [JsonPropertyName("intensity7")]
                public string Intensity7 { get; set; }

                [JsonPropertyName("intensity8")]
                public string Intensity8 { get; set; }

                [JsonPropertyName("intensity9")]
                public string Intensity9 { get; set; }

                [JsonPropertyName("unknown")]
                public string Unknown { get; set; }
            }

            public class EqInfo
            {
                [JsonPropertyName("distant")]
                public string Distant { get; set; }

                [JsonPropertyName("intensity1")]
                public string Intensity1 { get; set; }

                [JsonPropertyName("intensity2")]
                public string Intensity2 { get; set; }

                [JsonPropertyName("intensity3")]
                public string Intensity3 { get; set; }

                [JsonPropertyName("intensity4")]
                public string Intensity4 { get; set; }

                [JsonPropertyName("intensity5")]
                public string Intensity5 { get; set; }

                [JsonPropertyName("intensity6")]
                public string Intensity6 { get; set; }

                [JsonPropertyName("intensity7")]
                public string Intensity7 { get; set; }

                [JsonPropertyName("intensity8")]
                public string Intensity8 { get; set; }

                [JsonPropertyName("intensity9")]
                public string Intensity9 { get; set; }
            }
        }
    }
}
