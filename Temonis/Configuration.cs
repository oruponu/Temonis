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

        [XmlRoot("Configuration")]
        public class Root
        {
            [XmlElement]
            public BehaviorClass Behavior { get; set; }

            [XmlElement]
            public SoundsClass Sounds { get; set; }

            [XmlRoot]
            public class BehaviorClass
            {
                [XmlElement]
                public bool ForceActive { get; set; }
            }

            [XmlRoot]
            public class SoundsClass
            {
                [XmlElement]
                public Kyoshin Kyoshin { get; set; }

                [XmlElement("EEW")]
                public Eew Eew { get; set; }

                [XmlElement]
                public EqInfo EqInfo { get; set; }
            }

            [XmlRoot]
            public class Kyoshin
            {
                [XmlElement]
                public string Intensity1 { get; set; }

                [XmlElement]
                public string Intensity2 { get; set; }

                [XmlElement]
                public string Intensity3 { get; set; }

                [XmlElement]
                public string Intensity4 { get; set; }

                [XmlElement]
                public string Intensity5 { get; set; }

                [XmlElement]
                public string Intensity6 { get; set; }

                [XmlElement]
                public string Intensity7 { get; set; }

                [XmlElement]
                public string Intensity8 { get; set; }

                [XmlElement]
                public string Intensity9 { get; set; }
            }

            [XmlRoot]
            public class Eew
            {
                [XmlElement]
                public FirstReport FirstReport { get; set; }

                [XmlElement]
                public MaxIntChange MaxIntChange { get; set; }
            }

            [XmlRoot]
            public class FirstReport
            {
                [XmlElement]
                public string Intensity1 { get; set; }

                [XmlElement]
                public string Intensity2 { get; set; }

                [XmlElement]
                public string Intensity3 { get; set; }

                [XmlElement]
                public string Intensity4 { get; set; }

                [XmlElement]
                public string Intensity5 { get; set; }

                [XmlElement]
                public string Intensity6 { get; set; }

                [XmlElement]
                public string Intensity7 { get; set; }

                [XmlElement]
                public string Intensity8 { get; set; }

                [XmlElement]
                public string Intensity9 { get; set; }

                [XmlElement]
                public string Unknown { get; set; }
            }

            [XmlRoot]
            public class MaxIntChange
            {
                [XmlElement]
                public string Cancel { get; set; }

                [XmlElement]
                public string Intensity1 { get; set; }

                [XmlElement]
                public string Intensity2 { get; set; }

                [XmlElement]
                public string Intensity3 { get; set; }

                [XmlElement]
                public string Intensity4 { get; set; }

                [XmlElement]
                public string Intensity5 { get; set; }

                [XmlElement]
                public string Intensity6 { get; set; }

                [XmlElement]
                public string Intensity7 { get; set; }

                [XmlElement]
                public string Intensity8 { get; set; }

                [XmlElement]
                public string Intensity9 { get; set; }

                [XmlElement]
                public string Unknown { get; set; }
            }

            [XmlRoot]
            public class EqInfo
            {
                [XmlElement]
                public string Distant { get; set; }

                [XmlElement]
                public string Intensity1 { get; set; }

                [XmlElement]
                public string Intensity2 { get; set; }

                [XmlElement]
                public string Intensity3 { get; set; }

                [XmlElement]
                public string Intensity4 { get; set; }

                [XmlElement]
                public string Intensity5 { get; set; }

                [XmlElement]
                public string Intensity6 { get; set; }

                [XmlElement]
                public string Intensity7 { get; set; }

                [XmlElement]
                public string Intensity8 { get; set; }

                [XmlElement]
                public string Intensity9 { get; set; }
            }
        }
    }
}
