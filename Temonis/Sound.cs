using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;

namespace Temonis
{
    internal static class Sound
    {
        private static readonly MediaPlayer Player = new();

        public static void PlayKyoshin(int maxInt)
        {
            switch (maxInt)
            {
                case 1:
                    Play(Settings.JsonClass.Sounds.Kyoshin.Intensity1);
                    break;
                case 2:
                    Play(Settings.JsonClass.Sounds.Kyoshin.Intensity2);
                    break;
                case 3:
                    Play(Settings.JsonClass.Sounds.Kyoshin.Intensity3);
                    break;
                case 4:
                    Play(Settings.JsonClass.Sounds.Kyoshin.Intensity4);
                    break;
                case 5:
                    Play(Settings.JsonClass.Sounds.Kyoshin.Intensity5);
                    break;
                case 6:
                    Play(Settings.JsonClass.Sounds.Kyoshin.Intensity6);
                    break;
                case 7:
                    Play(Settings.JsonClass.Sounds.Kyoshin.Intensity7);
                    break;
                case 8:
                    Play(Settings.JsonClass.Sounds.Kyoshin.Intensity8);
                    break;
                case 9:
                    Play(Settings.JsonClass.Sounds.Kyoshin.Intensity9);
                    break;
            }
        }

        public static void PlayFirstReport(string maxInt)
        {
            switch (maxInt)
            {
                case "1":
                    Play(Settings.JsonClass.Sounds.Eew.FirstReport.Intensity1);
                    break;
                case "2":
                    Play(Settings.JsonClass.Sounds.Eew.FirstReport.Intensity2);
                    break;
                case "3":
                    Play(Settings.JsonClass.Sounds.Eew.FirstReport.Intensity3);
                    break;
                case "4":
                    Play(Settings.JsonClass.Sounds.Eew.FirstReport.Intensity4);
                    break;
                case "5弱":
                    Play(Settings.JsonClass.Sounds.Eew.FirstReport.Intensity5);
                    break;
                case "5強":
                    Play(Settings.JsonClass.Sounds.Eew.FirstReport.Intensity6);
                    break;
                case "6弱":
                    Play(Settings.JsonClass.Sounds.Eew.FirstReport.Intensity7);
                    break;
                case "6強":
                    Play(Settings.JsonClass.Sounds.Eew.FirstReport.Intensity8);
                    break;
                case "7":
                    Play(Settings.JsonClass.Sounds.Eew.FirstReport.Intensity9);
                    break;
                default:
                    Play(Settings.JsonClass.Sounds.Eew.FirstReport.Unknown);
                    break;
            }
        }

        public static void PlayMaxIntChange(string maxInt)
        {
            switch (maxInt)
            {
                case "不明":
                    Play(Settings.JsonClass.Sounds.Eew.MaxIntChange.Unknown);
                    break;
                case "1":
                    Play(Settings.JsonClass.Sounds.Eew.MaxIntChange.Intensity1);
                    break;
                case "2":
                    Play(Settings.JsonClass.Sounds.Eew.MaxIntChange.Intensity2);
                    break;
                case "3":
                    Play(Settings.JsonClass.Sounds.Eew.MaxIntChange.Intensity3);
                    break;
                case "4":
                    Play(Settings.JsonClass.Sounds.Eew.MaxIntChange.Intensity4);
                    break;
                case "5弱":
                    Play(Settings.JsonClass.Sounds.Eew.MaxIntChange.Intensity5);
                    break;
                case "5強":
                    Play(Settings.JsonClass.Sounds.Eew.MaxIntChange.Intensity6);
                    break;
                case "6弱":
                    Play(Settings.JsonClass.Sounds.Eew.MaxIntChange.Intensity7);
                    break;
                case "6強":
                    Play(Settings.JsonClass.Sounds.Eew.MaxIntChange.Intensity8);
                    break;
                case "7":
                    Play(Settings.JsonClass.Sounds.Eew.MaxIntChange.Intensity9);
                    break;
                default:
                    Play(Settings.JsonClass.Sounds.Eew.MaxIntChange.Cancel);
                    break;
            }
        }

        public static void PlayEqInfo(string maxInt)
        {
            switch (maxInt)
            {
                case "1":
                    Play(Settings.JsonClass.Sounds.EqInfo.Intensity1);
                    break;
                case "2":
                    Play(Settings.JsonClass.Sounds.EqInfo.Intensity2);
                    break;
                case "3":
                    Play(Settings.JsonClass.Sounds.EqInfo.Intensity3);
                    break;
                case "4":
                    Play(Settings.JsonClass.Sounds.EqInfo.Intensity4);
                    break;
                case "5-":
                    Play(Settings.JsonClass.Sounds.EqInfo.Intensity5);
                    break;
                case "5+":
                    Play(Settings.JsonClass.Sounds.EqInfo.Intensity6);
                    break;
                case "6-":
                    Play(Settings.JsonClass.Sounds.EqInfo.Intensity7);
                    break;
                case "6+":
                    Play(Settings.JsonClass.Sounds.EqInfo.Intensity8);
                    break;
                case "7":
                    Play(Settings.JsonClass.Sounds.EqInfo.Intensity9);
                    break;
                case "D":
                    Play(Settings.JsonClass.Sounds.EqInfo.Distant);
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
            if (!Path.IsPathRooted(uriString))
                uriString = Path.Combine(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName), uriString);
            Player.Open(new Uri(uriString, UriKind.Absolute));
            Player.Play();
        }
    }
}
