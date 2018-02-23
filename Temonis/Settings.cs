using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Temonis
{
    internal class Settings
    {
        private const string FileName = "Settings.xml";

        public static Configuration Configuration { get; private set; }

        public static void LoadSettings()
        {
            var serializer = new XmlSerializer(typeof(Configuration));
            if (File.Exists(FileName))
            {
                try
                {
                    using (var stream = new FileStream(FileName, FileMode.Open))
                    {
                        Configuration = (Configuration)serializer.Deserialize(stream);
                    }
                }
                catch
                {
                    MessageBox.Show("設定ファイルを開けませんでした。\n音声は再生されません。", "Temonis", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("設定ファイルが見つかりませんでした。\n音声は再生されません。", "Temonis", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }

    /// <summary>
    /// XMLクラス
    /// </summary>
    [XmlRoot("configuration")]
    public class Configuration
    {
        [XmlElement("sounds")]
        public Sounds Sounds { get; set; }
    }

    [XmlRoot("sounds")]
    public class Sounds
    {
        [XmlElement("kyoshin")]
        public KyoshinSound Kyoshin { get; set; }

        [XmlElement("eew")]
        public EEWSound EEW { get; set; }

        [XmlElement("eqInfo")]
        public EqInfoSound EqInfo { get; set; }
    }

    [XmlRoot("kyoshinSound")]
    public class KyoshinSound
    {
        [XmlElement("intensity1")]
        public string Intensity1 { get; set; }

        [XmlElement("intensity2")]
        public string Intensity2 { get; set; }

        [XmlElement("intensity3")]
        public string Intensity3 { get; set; }

        [XmlElement("intensity4")]
        public string Intensity4 { get; set; }

        [XmlElement("intensity5")]
        public string Intensity5 { get; set; }

        [XmlElement("intensity6")]
        public string Intensity6 { get; set; }

        [XmlElement("intensity7")]
        public string Intensity7 { get; set; }

        [XmlElement("intensity8")]
        public string Intensity8 { get; set; }

        [XmlElement("intensity9")]
        public string Intensity9 { get; set; }
    }

    [XmlRoot("eewSound")]
    public class EEWSound
    {
        [XmlElement("firstReport")]
        public FirstReport FirstReport { get; set; }

        [XmlElement("maxIntChange")]
        public MaxIntChange MaxIntChange { get; set; }
    }

    [XmlRoot("firstReport")]
    public class FirstReport
    {
        [XmlElement("unknown")]
        public string Unknown { get; set; }

        [XmlElement("intensity1")]
        public string Intensity1 { get; set; }

        [XmlElement("intensity2")]
        public string Intensity2 { get; set; }

        [XmlElement("intensity3")]
        public string Intensity3 { get; set; }

        [XmlElement("intensity4")]
        public string Intensity4 { get; set; }

        [XmlElement("intensity5")]
        public string Intensity5 { get; set; }

        [XmlElement("intensity6")]
        public string Intensity6 { get; set; }

        [XmlElement("intensity7")]
        public string Intensity7 { get; set; }

        [XmlElement("intensity8")]
        public string Intensity8 { get; set; }

        [XmlElement("intensity9")]
        public string Intensity9 { get; set; }
    }

    [XmlRoot("maxIntChange")]
    public class MaxIntChange
    {
        [XmlElement("cancel")]
        public string Cancel { get; set; }

        [XmlElement("unknown")]
        public string Unknown { get; set; }

        [XmlElement("intensity1")]
        public string Intensity1 { get; set; }

        [XmlElement("intensity2")]
        public string Intensity2 { get; set; }

        [XmlElement("intensity3")]
        public string Intensity3 { get; set; }

        [XmlElement("intensity4")]
        public string Intensity4 { get; set; }

        [XmlElement("intensity5")]
        public string Intensity5 { get; set; }

        [XmlElement("intensity6")]
        public string Intensity6 { get; set; }

        [XmlElement("intensity7")]
        public string Intensity7 { get; set; }

        [XmlElement("intensity8")]
        public string Intensity8 { get; set; }

        [XmlElement("intensity9")]
        public string Intensity9 { get; set; }
    }

    [XmlRoot("eqInfoSound")]
    public class EqInfoSound
    {
        [XmlElement("intensity1")]
        public string Intensity1 { get; set; }

        [XmlElement("intensity2")]
        public string Intensity2 { get; set; }

        [XmlElement("intensity3")]
        public string Intensity3 { get; set; }

        [XmlElement("intensity4")]
        public string Intensity4 { get; set; }

        [XmlElement("intensity5")]
        public string Intensity5 { get; set; }

        [XmlElement("intensity6")]
        public string Intensity6 { get; set; }

        [XmlElement("intensity7")]
        public string Intensity7 { get; set; }

        [XmlElement("intensity8")]
        public string Intensity8 { get; set; }

        [XmlElement("intensity9")]
        public string Intensity9 { get; set; }

        [XmlElement("distant")]
        public string Distant { get; set; }
    }
}
