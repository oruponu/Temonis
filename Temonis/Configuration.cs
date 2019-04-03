using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace Temonis
{
    public static class Configuration
    {
        private const string FileName = "Temonis.xml";

        internal static Root RootClass { get; private set; }

        internal static void Load()
        {
            var serializer = new XmlSerializer(typeof(Root));
            if (File.Exists(FileName))
            {
                try
                {
                    using (var stream = File.OpenRead(FileName))
                        RootClass = (Root)serializer.Deserialize(stream);
                }
                catch
                {
                    MessageBox.Show("設定ファイルを開けませんでした。\n音声は再生されません。", "Temonis", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("設定ファイルが見つかりませんでした。\n音声は再生されません。", "Temonis", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        [XmlRoot("configuration")]
        public class Root
        {
            [XmlElement("appearance")]
            public AppearanceClass Appearance { get; set; }

            [XmlElement("behavior")]
            public BehaviorClass Behavior { get; set; }

            [XmlElement("sounds")]
            public SoundsClass Sounds { get; set; }

            [XmlRoot]
            public class AppearanceClass
            {
                [XmlElement("useJMASeismicIntensityScale")]
                public bool UseJmaSeismicIntensityScale { get; set; }
            }

            [XmlRoot]
            public class BehaviorClass
            {
                [XmlElement("forceActive")]
                public bool ForceActive { get; set; }
            }

            [XmlRoot]
            public class SoundsClass
            {
                [XmlElement("kyoshin")]
                public Kyoshin Kyoshin { get; set; }

                [XmlElement("eew")]
                public Eew Eew { get; set; }

                [XmlElement("eqInfo")]
                public EqInfo EqInfo { get; set; }
            }

            [XmlRoot]
            public class Kyoshin
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

            [XmlRoot]
            public class Eew
            {
                [XmlElement("firstReport")]
                public FirstReport FirstReport { get; set; }

                [XmlElement("maxIntChange")]
                public MaxIntChange MaxIntChange { get; set; }
            }

            [XmlRoot]
            public class FirstReport
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

                [XmlElement("unknown")]
                public string Unknown { get; set; }
            }

            [XmlRoot]
            public class MaxIntChange
            {
                [XmlElement("cancel")]
                public string Cancel { get; set; }

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

                [XmlElement("unknown")]
                public string Unknown { get; set; }
            }

            [XmlRoot]
            public class EqInfo
            {
                [XmlElement("distant")]
                public string Distant { get; set; }

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
        }
    }
}
