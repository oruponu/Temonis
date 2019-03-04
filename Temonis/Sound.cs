using System;
using System.Windows.Media;

namespace Temonis
{
    internal static class Sound
    {
        private static readonly MediaPlayer Player = new MediaPlayer();

        public static void PlayKyoshin(int maxInt)
        {
            switch (maxInt)
            {
                case 1:
                    Play(Configuration.RootClass.Sounds.Kyoshin.Intensity1);
                    break;
                case 2:
                    Play(Configuration.RootClass.Sounds.Kyoshin.Intensity2);
                    break;
                case 3:
                    Play(Configuration.RootClass.Sounds.Kyoshin.Intensity3);
                    break;
                case 4:
                    Play(Configuration.RootClass.Sounds.Kyoshin.Intensity4);
                    break;
                case 5:
                    Play(Configuration.RootClass.Sounds.Kyoshin.Intensity5);
                    break;
                case 6:
                    Play(Configuration.RootClass.Sounds.Kyoshin.Intensity6);
                    break;
                case 7:
                    Play(Configuration.RootClass.Sounds.Kyoshin.Intensity7);
                    break;
                case 8:
                    Play(Configuration.RootClass.Sounds.Kyoshin.Intensity8);
                    break;
                case 9:
                    Play(Configuration.RootClass.Sounds.Kyoshin.Intensity9);
                    break;
            }
        }

        public static void PlayFirstReportAsync(string maxInt)
        {
            switch (maxInt)
            {
                case "1":
                    Play(Configuration.RootClass.Sounds.Eew.FirstReport.Intensity1);
                    break;
                case "2":
                    Play(Configuration.RootClass.Sounds.Eew.FirstReport.Intensity2);
                    break;
                case "3":
                    Play(Configuration.RootClass.Sounds.Eew.FirstReport.Intensity3);
                    break;
                case "4":
                    Play(Configuration.RootClass.Sounds.Eew.FirstReport.Intensity4);
                    break;
                case "5弱":
                    Play(Configuration.RootClass.Sounds.Eew.FirstReport.Intensity5);
                    break;
                case "5強":
                    Play(Configuration.RootClass.Sounds.Eew.FirstReport.Intensity6);
                    break;
                case "6弱":
                    Play(Configuration.RootClass.Sounds.Eew.FirstReport.Intensity7);
                    break;
                case "6強":
                    Play(Configuration.RootClass.Sounds.Eew.FirstReport.Intensity8);
                    break;
                case "7":
                    Play(Configuration.RootClass.Sounds.Eew.FirstReport.Intensity9);
                    break;
                default:
                    Play(Configuration.RootClass.Sounds.Eew.FirstReport.Unknown);
                    break;
            }
        }

        public static void PlayMaxIntChangeAsync(string maxInt)
        {
            switch (maxInt)
            {
                case "不明":
                    Play(Configuration.RootClass.Sounds.Eew.MaxIntChange.Unknown);
                    break;
                case "1":
                    Play(Configuration.RootClass.Sounds.Eew.MaxIntChange.Intensity1);
                    break;
                case "2":
                    Play(Configuration.RootClass.Sounds.Eew.MaxIntChange.Intensity2);
                    break;
                case "3":
                    Play(Configuration.RootClass.Sounds.Eew.MaxIntChange.Intensity3);
                    break;
                case "4":
                    Play(Configuration.RootClass.Sounds.Eew.MaxIntChange.Intensity4);
                    break;
                case "5弱":
                    Play(Configuration.RootClass.Sounds.Eew.MaxIntChange.Intensity5);
                    break;
                case "5強":
                    Play(Configuration.RootClass.Sounds.Eew.MaxIntChange.Intensity6);
                    break;
                case "6弱":
                    Play(Configuration.RootClass.Sounds.Eew.MaxIntChange.Intensity7);
                    break;
                case "6強":
                    Play(Configuration.RootClass.Sounds.Eew.MaxIntChange.Intensity8);
                    break;
                case "7":
                    Play(Configuration.RootClass.Sounds.Eew.MaxIntChange.Intensity9);
                    break;
                default:
                    Play(Configuration.RootClass.Sounds.Eew.MaxIntChange.Cancel);
                    break;
            }
        }

        public static void PlayEqInfoAsync(string maxInt)
        {
            switch (maxInt)
            {
                case "1":
                    Play(Configuration.RootClass.Sounds.EqInfo.Intensity1);
                    break;
                case "2":
                    Play(Configuration.RootClass.Sounds.EqInfo.Intensity2);
                    break;
                case "3":
                    Play(Configuration.RootClass.Sounds.EqInfo.Intensity3);
                    break;
                case "4":
                    Play(Configuration.RootClass.Sounds.EqInfo.Intensity4);
                    break;
                case "5弱":
                    Play(Configuration.RootClass.Sounds.EqInfo.Intensity5);
                    break;
                case "5強":
                    Play(Configuration.RootClass.Sounds.EqInfo.Intensity6);
                    break;
                case "6弱":
                    Play(Configuration.RootClass.Sounds.EqInfo.Intensity7);
                    break;
                case "6強":
                    Play(Configuration.RootClass.Sounds.EqInfo.Intensity8);
                    break;
                case "7":
                    Play(Configuration.RootClass.Sounds.EqInfo.Intensity9);
                    break;
                default:
                    Play(Configuration.RootClass.Sounds.EqInfo.Distant);
                    break;
            }
        }

        public static void PlayDummy()
        {
            if (Player.HasAudio)
                return;
            Player.Open(new Uri("pack://application:,,,/Resources/Dummy.wav"));
            Player.Play();
        }

        private static void Play(string uriString)
        {
            Player.Open(new Uri(uriString, UriKind.Relative));
            Player.Play();
        }
    }
}
