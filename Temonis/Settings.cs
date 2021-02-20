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
            public AppearanceClass Appearance { get; init; }

            public BehaviorClass Behavior { get; init; }

            public SoundsClass Sounds { get; init; }

            public class AppearanceClass
            {
                [JsonPropertyName("useJMASeismicIntensityScale")]
                public bool UseJmaSeismicIntensityScale { get; init; }

                public bool ShowIntensityStation { get; init; }
            }

            public class BehaviorClass
            {
                public bool ActivateWindow { get; init; }
            }

            public class SoundsClass
            {
                public Kyoshin Kyoshin { get; init; }

                public Eew Eew { get; init; }

                public EqInfo EqInfo { get; init; }
            }

            public class Kyoshin
            {
                public string Intensity1 { get; init; }

                public string Intensity2 { get; init; }

                public string Intensity3 { get; init; }

                public string Intensity4 { get; init; }

                public string Intensity5 { get; init; }

                public string Intensity6 { get; init; }

                public string Intensity7 { get; init; }

                public string Intensity8 { get; init; }

                public string Intensity9 { get; init; }
            }

            public class Eew
            {
                public FirstReport FirstReport { get; init; }

                public MaxIntChange MaxIntChange { get; init; }
            }

            public class FirstReport
            {
                public string Intensity1 { get; init; }

                public string Intensity2 { get; init; }

                public string Intensity3 { get; init; }

                public string Intensity4 { get; init; }

                public string Intensity5 { get; init; }

                public string Intensity6 { get; init; }

                public string Intensity7 { get; init; }

                public string Intensity8 { get; init; }

                public string Intensity9 { get; init; }

                public string Unknown { get; init; }
            }

            public class MaxIntChange
            {
                public string Cancel { get; init; }

                public string Intensity1 { get; init; }

                public string Intensity2 { get; init; }

                public string Intensity3 { get; init; }

                public string Intensity4 { get; init; }

                public string Intensity5 { get; init; }

                public string Intensity6 { get; init; }

                public string Intensity7 { get; init; }

                public string Intensity8 { get; init; }

                public string Intensity9 { get; init; }

                public string Unknown { get; init; }
            }

            public class EqInfo
            {
                public string Distant { get; init; }

                public string Intensity1 { get; init; }

                public string Intensity2 { get; init; }

                public string Intensity3 { get; init; }

                public string Intensity4 { get; init; }

                public string Intensity5 { get; init; }

                public string Intensity6 { get; init; }

                public string Intensity7 { get; init; }

                public string Intensity8 { get; init; }

                public string Intensity9 { get; init; }
            }
        }
    }
}
