using System;
using System.Collections.Generic;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;

namespace Temonis
{
    internal class Sound
    {
        private const string Alias = "temonis";
        private static bool _kyoshinOn;
        private static int _lastKyoshinIntensity;
        private static Dictionary<string, string> _lastEEWInfo = new Dictionary<string, string>();
        private static DateTime _lastEqInfoTime;
        private static string _lastEqInfoIntensity;
        
        public Sound()
        {
            // 音量ミキサーに表示されるようにする
            using (var player = new SoundPlayer(Properties.Resources.Dummy))
            {
                player.Play();
                player.Stop();
            }
        }

        [DllImport("winmm.dll")]
        private static extern int mciSendString(string lpszCommand, StringBuilder lpszReturnString,
            int cchReturn, IntPtr hwndCallback);

        // 音を再生
        public void PlaySound()
        {
            // 強震モニタ
            if (Kyoshin.OnTrigger && !EEW.OnTrigger)
            {
                if (!_kyoshinOn)
                {
                    PlayKyoshin();
                    _kyoshinOn = true;
                }
                if (_lastKyoshinIntensity < Kyoshin.Intensity)
                {
                    PlayKyoshin();
                    _kyoshinOn = true;
                }
            }
            else _kyoshinOn = false;
            _lastKyoshinIntensity = Kyoshin.Intensity;
            // 緊急地震速報
            if (EEW.OnTrigger)
            {
                foreach (var key in EEW.Info.Keys)
                {
                    if (!_lastEEWInfo.ContainsKey(key))
                    {
                        PlayFirstReport(key);
                        _lastEEWInfo.Add(key, EEW.Info[key]);
                    }
                    else
                    {
                        if (_lastEEWInfo[key] == EEW.Info[key]) continue;
                        PlayMaxIntChange(key);
                        _lastEEWInfo[key] = EEW.Info[key];
                    }
                }
            }
            else _lastEEWInfo = new Dictionary<string, string>();
            // 地震情報
            if (_lastEqInfoTime == EqInfo.DateTime && _lastEqInfoIntensity == EqInfo.Intensity) return;
            if (_lastEqInfoIntensity == null)
            {
                _lastEqInfoTime = EqInfo.DateTime;
                _lastEqInfoIntensity = EqInfo.Intensity;
                return;
            }
            _lastEqInfoTime = EqInfo.DateTime;
            _lastEqInfoIntensity = EqInfo.Intensity;
            PlayEqInfo();
        }

        private static void PlayKyoshin()
        {
            switch (Kyoshin.Intensity)
            {
                case 1:
                    Play(Settings.Configuration.Sounds.Kyoshin.Intensity1);
                    break;
                case 2:
                    Play(Settings.Configuration.Sounds.Kyoshin.Intensity2);
                    break;
                case 3:
                    Play(Settings.Configuration.Sounds.Kyoshin.Intensity3);
                    break;
                case 4:
                    Play(Settings.Configuration.Sounds.Kyoshin.Intensity4);
                    break;
                case 5:
                    Play(Settings.Configuration.Sounds.Kyoshin.Intensity5);
                    break;
                case 6:
                    Play(Settings.Configuration.Sounds.Kyoshin.Intensity6);
                    break;
                case 7:
                    Play(Settings.Configuration.Sounds.Kyoshin.Intensity7);
                    break;
                case 8:
                    Play(Settings.Configuration.Sounds.Kyoshin.Intensity8);
                    break;
                case 9:
                    Play(Settings.Configuration.Sounds.Kyoshin.Intensity9);
                    break;
            }
        }

        private static void PlayFirstReport(string key)
        {
            switch (EEW.Info[key])
            {
                case "1":
                    Play(Settings.Configuration.Sounds.EEW.FirstReport.Intensity1);
                    break;
                case "2":
                    Play(Settings.Configuration.Sounds.EEW.FirstReport.Intensity2);
                    break;
                case "3":
                    Play(Settings.Configuration.Sounds.EEW.FirstReport.Intensity3);
                    break;
                case "4":
                    Play(Settings.Configuration.Sounds.EEW.FirstReport.Intensity4);
                    break;
                case "5弱":
                    Play(Settings.Configuration.Sounds.EEW.FirstReport.Intensity5);
                    break;
                case "5強":
                    Play(Settings.Configuration.Sounds.EEW.FirstReport.Intensity6);
                    break;
                case "6弱":
                    Play(Settings.Configuration.Sounds.EEW.FirstReport.Intensity7);
                    break;
                case "6強":
                    Play(Settings.Configuration.Sounds.EEW.FirstReport.Intensity8);
                    break;
                case "7":
                    Play(Settings.Configuration.Sounds.EEW.FirstReport.Intensity9);
                    break;
                case "不明":
                    Play(Settings.Configuration.Sounds.EEW.FirstReport.Unknown);
                    break;
            }
        }

        private static void PlayMaxIntChange(string key)
        {
            switch (EEW.Info[key])
            {
                case "1":
                    Play(Settings.Configuration.Sounds.EEW.MaxIntChange.Intensity1);
                    break;
                case "2":
                    Play(Settings.Configuration.Sounds.EEW.MaxIntChange.Intensity2);
                    break;
                case "3":
                    Play(Settings.Configuration.Sounds.EEW.MaxIntChange.Intensity3);
                    break;
                case "4":
                    Play(Settings.Configuration.Sounds.EEW.MaxIntChange.Intensity4);
                    break;
                case "5弱":
                    Play(Settings.Configuration.Sounds.EEW.MaxIntChange.Intensity5);
                    break;
                case "5強":
                    Play(Settings.Configuration.Sounds.EEW.MaxIntChange.Intensity6);
                    break;
                case "6弱":
                    Play(Settings.Configuration.Sounds.EEW.MaxIntChange.Intensity7);
                    break;
                case "6強":
                    Play(Settings.Configuration.Sounds.EEW.MaxIntChange.Intensity8);
                    break;
                case "7":
                    Play(Settings.Configuration.Sounds.EEW.MaxIntChange.Intensity9);
                    break;
                case "不明":
                    Play(Settings.Configuration.Sounds.EEW.MaxIntChange.Unknown);
                    break;
                default:
                    Play(Settings.Configuration.Sounds.EEW.MaxIntChange.Cancel);
                    break;
            }
        }

        private static void PlayEqInfo()
        {
            if (EqInfo.Intensity.Contains("7"))
            {
                Play(Settings.Configuration.Sounds.EqInfo.Intensity9);
            }
            else if (EqInfo.Intensity.Contains("6強"))
            {
                Play(Settings.Configuration.Sounds.EqInfo.Intensity8);
            }
            else if (EqInfo.Intensity.Contains("6弱"))
            {
                Play(Settings.Configuration.Sounds.EqInfo.Intensity7);
            }
            else if (EqInfo.Intensity.Contains("5強"))
            {
                Play(Settings.Configuration.Sounds.EqInfo.Intensity6);
            }
            else if (EqInfo.Intensity.Contains("5弱"))
            {
                Play(Settings.Configuration.Sounds.EqInfo.Intensity5);
            }
            else if (EqInfo.Intensity.Contains("4"))
            {
                Play(Settings.Configuration.Sounds.EqInfo.Intensity4);
            }
            else if (EqInfo.Intensity.Contains("3"))
            {
                Play(Settings.Configuration.Sounds.EqInfo.Intensity3);
            }
            else if (EqInfo.Intensity.Contains("2"))
            {
                Play(Settings.Configuration.Sounds.EqInfo.Intensity2);
            }
            else if (EqInfo.Intensity.Contains("1"))
            {
                Play(Settings.Configuration.Sounds.EqInfo.Intensity1);
            }
            else if (EqInfo.Intensity == "")
            {
                Play(Settings.Configuration.Sounds.EqInfo.Distant);
            }
        }

        private static void Play(string filePath)
        {
            Stop();
            if (mciSendString($"open \"{filePath}\" type mpegvideo alias {Alias}", null, 0, IntPtr.Zero) != 0) return;
            mciSendString($"play {Alias}", null, 0, IntPtr.Zero);
        }

        private static void Stop()
        {
            mciSendString($"stop {Alias}", null, 0, IntPtr.Zero);
            mciSendString($"close {Alias}", null, 0, IntPtr.Zero);
        }
    }
}
