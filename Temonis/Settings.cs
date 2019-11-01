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
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                JsonClass = JsonSerializer.Deserialize<Json>(File.ReadAllBytes(FileName), options);
            }
            catch
            {
                MessageBox.Show("設定ファイルを開けませんでした。\n音声は再生されません。", "Temonis", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        public class Json
        {
            public AppearanceClass Appearance { get; set; }

            public BehaviorClass Behavior { get; set; }

            public SoundsClass Sounds { get; set; }

            public class AppearanceClass
            {
                [JsonPropertyName("useJMASeismicIntensityScale")]
                public bool UseJmaSeismicIntensityScale { get; set; }
            }

            public class BehaviorClass
            {
                public bool ForceActive { get; set; }
            }

            public class SoundsClass
            {
                public Kyoshin Kyoshin { get; set; }

                public Eew Eew { get; set; }

                public EqInfo EqInfo { get; set; }
            }

            public class Kyoshin
            {
                public string Intensity1 { get; set; }

                public string Intensity2 { get; set; }

                public string Intensity3 { get; set; }

                public string Intensity4 { get; set; }

                public string Intensity5 { get; set; }

                public string Intensity6 { get; set; }

                public string Intensity7 { get; set; }

                public string Intensity8 { get; set; }

                public string Intensity9 { get; set; }
            }

            public class Eew
            {
                public FirstReport FirstReport { get; set; }

                public MaxIntChange MaxIntChange { get; set; }
            }

            public class FirstReport
            {
                public string Intensity1 { get; set; }

                public string Intensity2 { get; set; }

                public string Intensity3 { get; set; }

                public string Intensity4 { get; set; }

                public string Intensity5 { get; set; }

                public string Intensity6 { get; set; }

                public string Intensity7 { get; set; }

                public string Intensity8 { get; set; }

                public string Intensity9 { get; set; }

                public string Unknown { get; set; }
            }

            public class MaxIntChange
            {
                public string Cancel { get; set; }

                public string Intensity1 { get; set; }

                public string Intensity2 { get; set; }

                public string Intensity3 { get; set; }

                public string Intensity4 { get; set; }

                public string Intensity5 { get; set; }

                public string Intensity6 { get; set; }

                public string Intensity7 { get; set; }

                public string Intensity8 { get; set; }

                public string Intensity9 { get; set; }

                public string Unknown { get; set; }
            }

            public class EqInfo
            {
                public string Distant { get; set; }

                public string Intensity1 { get; set; }

                public string Intensity2 { get; set; }

                public string Intensity3 { get; set; }

                public string Intensity4 { get; set; }

                public string Intensity5 { get; set; }

                public string Intensity6 { get; set; }

                public string Intensity7 { get; set; }

                public string Intensity8 { get; set; }

                public string Intensity9 { get; set; }
            }
        }
    }
}
