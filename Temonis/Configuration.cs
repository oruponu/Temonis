using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Temonis
{
    public class Configuration
    {
        private const string FileName = "Temonis.xml";

        internal static Root RootClass { get; private set; }

        internal static void LoadSettings()
        {
            var serializer = new XmlSerializer(typeof(Root));
            if (File.Exists(FileName))
            {
                try
                {
                    using (var stream = File.OpenRead(FileName))
                    {
                        RootClass = (Root)serializer.Deserialize(stream);
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

            Sound.OpenFile(RootClass.Sounds.Kyoshin.Intensity1);
            Sound.OpenFile(RootClass.Sounds.Kyoshin.Intensity2);
            Sound.OpenFile(RootClass.Sounds.Kyoshin.Intensity3);
            Sound.OpenFile(RootClass.Sounds.Kyoshin.Intensity4);
            Sound.OpenFile(RootClass.Sounds.Kyoshin.Intensity5);
            Sound.OpenFile(RootClass.Sounds.Kyoshin.Intensity6);
            Sound.OpenFile(RootClass.Sounds.Kyoshin.Intensity7);
            Sound.OpenFile(RootClass.Sounds.Kyoshin.Intensity8);
            Sound.OpenFile(RootClass.Sounds.Kyoshin.Intensity9);
            Sound.OpenFile(RootClass.Sounds.Eew.FirstReport.Unknown);
            Sound.OpenFile(RootClass.Sounds.Eew.FirstReport.Intensity1);
            Sound.OpenFile(RootClass.Sounds.Eew.FirstReport.Intensity2);
            Sound.OpenFile(RootClass.Sounds.Eew.FirstReport.Intensity3);
            Sound.OpenFile(RootClass.Sounds.Eew.FirstReport.Intensity4);
            Sound.OpenFile(RootClass.Sounds.Eew.FirstReport.Intensity5);
            Sound.OpenFile(RootClass.Sounds.Eew.FirstReport.Intensity6);
            Sound.OpenFile(RootClass.Sounds.Eew.FirstReport.Intensity7);
            Sound.OpenFile(RootClass.Sounds.Eew.FirstReport.Intensity8);
            Sound.OpenFile(RootClass.Sounds.Eew.FirstReport.Intensity9);
            Sound.OpenFile(RootClass.Sounds.Eew.MaxIntChange.Cancel);
            Sound.OpenFile(RootClass.Sounds.Eew.MaxIntChange.Unknown);
            Sound.OpenFile(RootClass.Sounds.Eew.MaxIntChange.Intensity1);
            Sound.OpenFile(RootClass.Sounds.Eew.MaxIntChange.Intensity2);
            Sound.OpenFile(RootClass.Sounds.Eew.MaxIntChange.Intensity3);
            Sound.OpenFile(RootClass.Sounds.Eew.MaxIntChange.Intensity4);
            Sound.OpenFile(RootClass.Sounds.Eew.MaxIntChange.Intensity5);
            Sound.OpenFile(RootClass.Sounds.Eew.MaxIntChange.Intensity6);
            Sound.OpenFile(RootClass.Sounds.Eew.MaxIntChange.Intensity7);
            Sound.OpenFile(RootClass.Sounds.Eew.MaxIntChange.Intensity8);
            Sound.OpenFile(RootClass.Sounds.Eew.MaxIntChange.Intensity9);
            Sound.OpenFile(RootClass.Sounds.EqInfo.Intensity1);
            Sound.OpenFile(RootClass.Sounds.EqInfo.Intensity2);
            Sound.OpenFile(RootClass.Sounds.EqInfo.Intensity3);
            Sound.OpenFile(RootClass.Sounds.EqInfo.Intensity4);
            Sound.OpenFile(RootClass.Sounds.EqInfo.Intensity5);
            Sound.OpenFile(RootClass.Sounds.EqInfo.Intensity6);
            Sound.OpenFile(RootClass.Sounds.EqInfo.Intensity7);
            Sound.OpenFile(RootClass.Sounds.EqInfo.Intensity8);
            Sound.OpenFile(RootClass.Sounds.EqInfo.Intensity9);
            Sound.OpenFile(RootClass.Sounds.EqInfo.Distant);
        }

        /// <summary>
        /// XMLクラス
        /// </summary>
        [XmlRoot("configuration")]
        public class Root
        {
            [XmlElement("behavior")]
            public BehaviorClass Behavior { get; set; }

            [XmlElement("sounds")]
            public SoundsClass Sounds { get; set; }

            [XmlRoot("behavior")]
            public class BehaviorClass
            {
                [XmlElement("forceActive")]
                public bool ForceActive { get; set; }
            }

            [XmlRoot("sounds")]
            public class SoundsClass
            {
                [XmlElement("kyoshin")]
                public Kyoshin Kyoshin { get; set; }

                [XmlElement("eew")]
                public Eew Eew { get; set; }

                [XmlElement("eqInfo")]
                public EqInfo EqInfo { get; set; }
            }

            [XmlRoot("kyoshinSound")]
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

            [XmlRoot("eewSound")]
            public class Eew
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
            public class EqInfo
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
    }
}
