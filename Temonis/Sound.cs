using System;
using System.Collections.Generic;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Temonis.NativeMethods;

namespace Temonis
{
    internal class Sound
    {
        private static readonly Dictionary<int, string> PlayList = new Dictionary<int, string>();
        private static MainWindow _instance;
        private static bool _kyoshinOn;
        private static int _lastKyoshinIntensity;
        private static Dictionary<string, string> _lastEEWInfo = new Dictionary<string, string>();
        private static DateTime _lastEqInfoTime;
        private static string _lastEqInfoIntensity;

        public Sound(MainWindow instance)
        {
            _instance = instance;
            new Window().AssignHandle(_instance.Handle);

            // 音量ミキサーに表示されるようにする
            using (var player = new SoundPlayer(Properties.Resources.Dummy))
            {
                player.Play();
                player.Stop();
            }
        }

        // 音を再生
        public async Task PlaySound()
        {
            // 強震モニタ
            if (Kyoshin.OnTrigger && !EEW.OnTrigger)
            {
                if (!_kyoshinOn)
                {
                    await PlayKyoshin();
                    _kyoshinOn = true;
                }
                if (_lastKyoshinIntensity < Kyoshin.Intensity)
                {
                    await PlayKyoshin();
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
                        await PlayFirstReport(key);
                        _lastEEWInfo.Add(key, EEW.Info[key]);
                    }
                    else
                    {
                        if (_lastEEWInfo[key] == EEW.Info[key]) continue;
                        await PlayMaxIntChange(key);
                        _lastEEWInfo[key] = EEW.Info[key];
                    }
                }
            }
            else _lastEEWInfo = new Dictionary<string, string>();
            // 地震情報
            if (_lastEqInfoTime == EqInfo.ArrivalTime && _lastEqInfoIntensity == EqInfo.Intensity) return;
            if (_lastEqInfoIntensity == null)
            {
                _lastEqInfoTime = EqInfo.ArrivalTime;
                _lastEqInfoIntensity = EqInfo.Intensity;
                return;
            }
            _lastEqInfoTime = EqInfo.ArrivalTime;
            _lastEqInfoIntensity = EqInfo.Intensity;
            await PlayEqInfo();
        }

        private static async Task PlayKyoshin()
        {
            switch (Kyoshin.Intensity)
            {
                case 1:
                    await Play(Settings.Configuration.Sounds.Kyoshin.Intensity1);
                    break;
                case 2:
                    await Play(Settings.Configuration.Sounds.Kyoshin.Intensity2);
                    break;
                case 3:
                    await Play(Settings.Configuration.Sounds.Kyoshin.Intensity3);
                    break;
                case 4:
                    await Play(Settings.Configuration.Sounds.Kyoshin.Intensity4);
                    break;
                case 5:
                    await Play(Settings.Configuration.Sounds.Kyoshin.Intensity5);
                    break;
                case 6:
                    await Play(Settings.Configuration.Sounds.Kyoshin.Intensity6);
                    break;
                case 7:
                    await Play(Settings.Configuration.Sounds.Kyoshin.Intensity7);
                    break;
                case 8:
                    await Play(Settings.Configuration.Sounds.Kyoshin.Intensity8);
                    break;
                case 9:
                    await Play(Settings.Configuration.Sounds.Kyoshin.Intensity9);
                    break;
            }
        }

        private static async Task PlayFirstReport(string key)
        {
            switch (EEW.Info[key])
            {
                case "1":
                    await Play(Settings.Configuration.Sounds.EEW.FirstReport.Intensity1);
                    break;
                case "2":
                    await Play(Settings.Configuration.Sounds.EEW.FirstReport.Intensity2);
                    break;
                case "3":
                    await Play(Settings.Configuration.Sounds.EEW.FirstReport.Intensity3);
                    break;
                case "4":
                    await Play(Settings.Configuration.Sounds.EEW.FirstReport.Intensity4);
                    break;
                case "5弱":
                    await Play(Settings.Configuration.Sounds.EEW.FirstReport.Intensity5);
                    break;
                case "5強":
                    await Play(Settings.Configuration.Sounds.EEW.FirstReport.Intensity6);
                    break;
                case "6弱":
                    await Play(Settings.Configuration.Sounds.EEW.FirstReport.Intensity7);
                    break;
                case "6強":
                    await Play(Settings.Configuration.Sounds.EEW.FirstReport.Intensity8);
                    break;
                case "7":
                    await Play(Settings.Configuration.Sounds.EEW.FirstReport.Intensity9);
                    break;
                case "不明":
                    await Play(Settings.Configuration.Sounds.EEW.FirstReport.Unknown);
                    break;
            }
        }

        private static async Task PlayMaxIntChange(string key)
        {
            switch (EEW.Info[key])
            {
                case "1":
                    await Play(Settings.Configuration.Sounds.EEW.MaxIntChange.Intensity1);
                    break;
                case "2":
                    await Play(Settings.Configuration.Sounds.EEW.MaxIntChange.Intensity2);
                    break;
                case "3":
                    await Play(Settings.Configuration.Sounds.EEW.MaxIntChange.Intensity3);
                    break;
                case "4":
                    await Play(Settings.Configuration.Sounds.EEW.MaxIntChange.Intensity4);
                    break;
                case "5弱":
                    await Play(Settings.Configuration.Sounds.EEW.MaxIntChange.Intensity5);
                    break;
                case "5強":
                    await Play(Settings.Configuration.Sounds.EEW.MaxIntChange.Intensity6);
                    break;
                case "6弱":
                    await Play(Settings.Configuration.Sounds.EEW.MaxIntChange.Intensity7);
                    break;
                case "6強":
                    await Play(Settings.Configuration.Sounds.EEW.MaxIntChange.Intensity8);
                    break;
                case "7":
                    await Play(Settings.Configuration.Sounds.EEW.MaxIntChange.Intensity9);
                    break;
                case "不明":
                    await Play(Settings.Configuration.Sounds.EEW.MaxIntChange.Unknown);
                    break;
                default:
                    await Play(Settings.Configuration.Sounds.EEW.MaxIntChange.Cancel);
                    break;
            }
        }

        private static async Task PlayEqInfo()
        {
            if (EqInfo.Intensity.Contains("7"))
            {
                await Play(Settings.Configuration.Sounds.EqInfo.Intensity9);
            }
            else if (EqInfo.Intensity.Contains("6強"))
            {
                await Play(Settings.Configuration.Sounds.EqInfo.Intensity8);
            }
            else if (EqInfo.Intensity.Contains("6弱"))
            {
                await Play(Settings.Configuration.Sounds.EqInfo.Intensity7);
            }
            else if (EqInfo.Intensity.Contains("5強"))
            {
                await Play(Settings.Configuration.Sounds.EqInfo.Intensity6);
            }
            else if (EqInfo.Intensity.Contains("5弱"))
            {
                await Play(Settings.Configuration.Sounds.EqInfo.Intensity5);
            }
            else if (EqInfo.Intensity.Contains("4"))
            {
                await Play(Settings.Configuration.Sounds.EqInfo.Intensity4);
            }
            else if (EqInfo.Intensity.Contains("3"))
            {
                await Play(Settings.Configuration.Sounds.EqInfo.Intensity3);
            }
            else if (EqInfo.Intensity.Contains("2"))
            {
                await Play(Settings.Configuration.Sounds.EqInfo.Intensity2);
            }
            else if (EqInfo.Intensity.Contains("1"))
            {
                await Play(Settings.Configuration.Sounds.EqInfo.Intensity1);
            }
            else if (EqInfo.Intensity == "")
            {
                await Play(Settings.Configuration.Sounds.EqInfo.Distant);
            }
        }

        private static async Task Play(string filePath)
        {
            if (PlayList.ContainsValue(filePath)) return;
            await Task.Run(() =>
            {
                _instance.Invoke((MethodInvoker)(() =>
                {
                    if (mciSendString($"open \"{filePath}\" alias {filePath}", null, 0, _instance.Handle) != 0) return;
                    mciSendString($"play {filePath} notify", null, 0, _instance.Handle);
                    PlayList.Add((int)mciGetDeviceID(filePath), filePath);
                }));
            });
        }

        protected class Window : NativeWindow
        {
            protected override void WndProc(ref Message m)
            {
                if (m.Msg == 953 && (int)m.WParam == 1)
                {
                    mciSendString($"close {PlayList[(int)m.LParam]}", null, 0, _instance.Handle);
                    PlayList.Remove((int)m.LParam);
                }
                base.WndProc(ref m);
            }
        }
    }
}
