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

        public record Json(Appearance Appearance, Behavior Behavior, Sounds Sounds);

        public record Appearance(
            [property: JsonPropertyName("useJMASeismicIntensityScale")]
            bool UseJmaSeismicIntensityScale,
            bool ShowIntensityStation
        );

        public record Behavior(bool ActivateWindow);

        public record Sounds(Kyoshin Kyoshin, Eew Eew, EqInfo EqInfo);

        public record Kyoshin(
            string Intensity1,
            string Intensity2,
            string Intensity3,
            string Intensity4,
            string Intensity5,
            string Intensity6,
            string Intensity7,
            string Intensity8,
            string Intensity9
        );

        public record Eew(FirstReport FirstReport, MaxIntChange MaxIntChange);

        public record FirstReport(
            string Intensity1,
            string Intensity2,
            string Intensity3,
            string Intensity4,
            string Intensity5,
            string Intensity6,
            string Intensity7,
            string Intensity8,
            string Intensity9,
            string Unknown
        );

        public record MaxIntChange(
            string Cancel,
            string Intensity1,
            string Intensity2,
            string Intensity3,
            string Intensity4,
            string Intensity5,
            string Intensity6,
            string Intensity7,
            string Intensity8,
            string Intensity9,
            string Unknown
        );

        public record EqInfo(
            string Distant,
            string Intensity1,
            string Intensity2,
            string Intensity3,
            string Intensity4,
            string Intensity5,
            string Intensity6,
            string Intensity7,
            string Intensity8,
            string Intensity9
        );
    }
}
