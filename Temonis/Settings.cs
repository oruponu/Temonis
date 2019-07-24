using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Windows;

namespace Temonis
{
    public static class Settings
    {
        private const string FileName = "Settings.json";

        internal static Root RootClass { get; private set; }

        internal static void Load()
        {
            if (!File.Exists(FileName))
            {
                File.WriteAllBytes(FileName, Properties.Resources.Settings);
            }

            try
            {
                using (var stream = File.OpenRead(FileName))
                    RootClass = (Root)new DataContractJsonSerializer(typeof(Root)).ReadObject(stream);
            }
            catch
            {
                MessageBox.Show("設定ファイルを開けませんでした。\n音声は再生されません。", "Temonis", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        [DataContract]
        public class Root
        {
            [DataMember(Name = "appearance")]
            public AppearanceClass Appearance { get; set; }

            [DataMember(Name = "behavior")]
            public BehaviorClass Behavior { get; set; }

            [DataMember(Name = "sounds")]
            public SoundsClass Sounds { get; set; }

            [DataContract]
            public class AppearanceClass
            {
                [DataMember(Name = "useJMASeismicIntensityScale")]
                public bool UseJmaSeismicIntensityScale { get; set; }
            }

            [DataContract]
            public class BehaviorClass
            {
                [DataMember(Name = "forceActive")]
                public bool ForceActive { get; set; }
            }

            [DataContract]
            public class SoundsClass
            {
                [DataMember(Name = "kyoshin")]
                public Kyoshin Kyoshin { get; set; }

                [DataMember(Name = "eew")]
                public Eew Eew { get; set; }

                [DataMember(Name = "eqInfo")]
                public EqInfo EqInfo { get; set; }
            }

            [DataContract]
            public class Kyoshin
            {
                [DataMember(Name = "intensity1")]
                public string Intensity1 { get; set; }

                [DataMember(Name = "intensity2")]
                public string Intensity2 { get; set; }

                [DataMember(Name = "intensity3")]
                public string Intensity3 { get; set; }

                [DataMember(Name = "intensity4")]
                public string Intensity4 { get; set; }

                [DataMember(Name = "intensity5")]
                public string Intensity5 { get; set; }

                [DataMember(Name = "intensity6")]
                public string Intensity6 { get; set; }

                [DataMember(Name = "intensity7")]
                public string Intensity7 { get; set; }

                [DataMember(Name = "intensity8")]
                public string Intensity8 { get; set; }

                [DataMember(Name = "intensity9")]
                public string Intensity9 { get; set; }
            }

            [DataContract]
            public class Eew
            {
                [DataMember(Name = "firstReport")]
                public FirstReport FirstReport { get; set; }

                [DataMember(Name = "maxIntChange")]
                public MaxIntChange MaxIntChange { get; set; }
            }

            [DataContract]
            public class FirstReport
            {
                [DataMember(Name = "intensity1")]
                public string Intensity1 { get; set; }

                [DataMember(Name = "intensity2")]
                public string Intensity2 { get; set; }

                [DataMember(Name = "intensity3")]
                public string Intensity3 { get; set; }

                [DataMember(Name = "intensity4")]
                public string Intensity4 { get; set; }

                [DataMember(Name = "intensity5")]
                public string Intensity5 { get; set; }

                [DataMember(Name = "intensity6")]
                public string Intensity6 { get; set; }

                [DataMember(Name = "intensity7")]
                public string Intensity7 { get; set; }

                [DataMember(Name = "intensity8")]
                public string Intensity8 { get; set; }

                [DataMember(Name = "intensity9")]
                public string Intensity9 { get; set; }

                [DataMember(Name = "unknown")]
                public string Unknown { get; set; }
            }

            [DataContract]
            public class MaxIntChange
            {
                [DataMember(Name = "cancel")]
                public string Cancel { get; set; }

                [DataMember(Name = "intensity1")]
                public string Intensity1 { get; set; }

                [DataMember(Name = "intensity2")]
                public string Intensity2 { get; set; }

                [DataMember(Name = "intensity3")]
                public string Intensity3 { get; set; }

                [DataMember(Name = "intensity4")]
                public string Intensity4 { get; set; }

                [DataMember(Name = "intensity5")]
                public string Intensity5 { get; set; }

                [DataMember(Name = "intensity6")]
                public string Intensity6 { get; set; }

                [DataMember(Name = "intensity7")]
                public string Intensity7 { get; set; }

                [DataMember(Name = "intensity8")]
                public string Intensity8 { get; set; }

                [DataMember(Name = "intensity9")]
                public string Intensity9 { get; set; }

                [DataMember(Name = "unknown")]
                public string Unknown { get; set; }
            }

            [DataContract]
            public class EqInfo
            {
                [DataMember(Name = "distant")]
                public string Distant { get; set; }

                [DataMember(Name = "intensity1")]
                public string Intensity1 { get; set; }

                [DataMember(Name = "intensity2")]
                public string Intensity2 { get; set; }

                [DataMember(Name = "intensity3")]
                public string Intensity3 { get; set; }

                [DataMember(Name = "intensity4")]
                public string Intensity4 { get; set; }

                [DataMember(Name = "intensity5")]
                public string Intensity5 { get; set; }

                [DataMember(Name = "intensity6")]
                public string Intensity6 { get; set; }

                [DataMember(Name = "intensity7")]
                public string Intensity7 { get; set; }

                [DataMember(Name = "intensity8")]
                public string Intensity8 { get; set; }

                [DataMember(Name = "intensity9")]
                public string Intensity9 { get; set; }
            }
        }
    }
}
