using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Temonis.MainWindow;
using static Temonis.NativeMethods;

namespace Temonis
{
    internal class Sound
    {
        private static readonly Dictionary<int, string> PlayMap = new Dictionary<int, string>();

        public Sound()
        {
            new Window().AssignHandle(Instance.Handle);
        }

        public async Task PlayKyoshinAsync()
        {
            switch (Kyoshin.MaxIntensity)
            {
                case 1:
                    await PlayAsync(Configuration.RootClass.Sounds.Kyoshin.Intensity1);
                    break;
                case 2:
                    await PlayAsync(Configuration.RootClass.Sounds.Kyoshin.Intensity2);
                    break;
                case 3:
                    await PlayAsync(Configuration.RootClass.Sounds.Kyoshin.Intensity3);
                    break;
                case 4:
                    await PlayAsync(Configuration.RootClass.Sounds.Kyoshin.Intensity4);
                    break;
                case 5:
                    await PlayAsync(Configuration.RootClass.Sounds.Kyoshin.Intensity5);
                    break;
                case 6:
                    await PlayAsync(Configuration.RootClass.Sounds.Kyoshin.Intensity6);
                    break;
                case 7:
                    await PlayAsync(Configuration.RootClass.Sounds.Kyoshin.Intensity7);
                    break;
                case 8:
                    await PlayAsync(Configuration.RootClass.Sounds.Kyoshin.Intensity8);
                    break;
                case 9:
                    await PlayAsync(Configuration.RootClass.Sounds.Kyoshin.Intensity9);
                    break;
            }
        }

        public async Task PlayFirstReportAsync(string key)
        {
            switch (EEW.Info[key])
            {
                case "1":
                    await PlayAsync(Configuration.RootClass.Sounds.EEW.FirstReport.Intensity1);
                    break;
                case "2":
                    await PlayAsync(Configuration.RootClass.Sounds.EEW.FirstReport.Intensity2);
                    break;
                case "3":
                    await PlayAsync(Configuration.RootClass.Sounds.EEW.FirstReport.Intensity3);
                    break;
                case "4":
                    await PlayAsync(Configuration.RootClass.Sounds.EEW.FirstReport.Intensity4);
                    break;
                case "5弱":
                    await PlayAsync(Configuration.RootClass.Sounds.EEW.FirstReport.Intensity5);
                    break;
                case "5強":
                    await PlayAsync(Configuration.RootClass.Sounds.EEW.FirstReport.Intensity6);
                    break;
                case "6弱":
                    await PlayAsync(Configuration.RootClass.Sounds.EEW.FirstReport.Intensity7);
                    break;
                case "6強":
                    await PlayAsync(Configuration.RootClass.Sounds.EEW.FirstReport.Intensity8);
                    break;
                case "7":
                    await PlayAsync(Configuration.RootClass.Sounds.EEW.FirstReport.Intensity9);
                    break;
                case "不明":
                    await PlayAsync(Configuration.RootClass.Sounds.EEW.FirstReport.Unknown);
                    break;
            }
        }

        public async Task PlayMaxIntChangeAsync(string key)
        {
            switch (EEW.Info[key])
            {
                case "1":
                    await PlayAsync(Configuration.RootClass.Sounds.EEW.MaxIntChange.Intensity1);
                    break;
                case "2":
                    await PlayAsync(Configuration.RootClass.Sounds.EEW.MaxIntChange.Intensity2);
                    break;
                case "3":
                    await PlayAsync(Configuration.RootClass.Sounds.EEW.MaxIntChange.Intensity3);
                    break;
                case "4":
                    await PlayAsync(Configuration.RootClass.Sounds.EEW.MaxIntChange.Intensity4);
                    break;
                case "5弱":
                    await PlayAsync(Configuration.RootClass.Sounds.EEW.MaxIntChange.Intensity5);
                    break;
                case "5強":
                    await PlayAsync(Configuration.RootClass.Sounds.EEW.MaxIntChange.Intensity6);
                    break;
                case "6弱":
                    await PlayAsync(Configuration.RootClass.Sounds.EEW.MaxIntChange.Intensity7);
                    break;
                case "6強":
                    await PlayAsync(Configuration.RootClass.Sounds.EEW.MaxIntChange.Intensity8);
                    break;
                case "7":
                    await PlayAsync(Configuration.RootClass.Sounds.EEW.MaxIntChange.Intensity9);
                    break;
                case "不明":
                    await PlayAsync(Configuration.RootClass.Sounds.EEW.MaxIntChange.Unknown);
                    break;
                default:
                    await PlayAsync(Configuration.RootClass.Sounds.EEW.MaxIntChange.Cancel);
                    break;
            }
        }

        public async Task PlayEqInfoAsync()
        {
            switch (EqInfo.MaxInt)
            {
                case "7":
                    await PlayAsync(Configuration.RootClass.Sounds.EqInfo.Intensity9);
                    break;
                case "6強":
                    await PlayAsync(Configuration.RootClass.Sounds.EqInfo.Intensity8);
                    break;
                case "6弱":
                    await PlayAsync(Configuration.RootClass.Sounds.EqInfo.Intensity7);
                    break;
                case "5強":
                    await PlayAsync(Configuration.RootClass.Sounds.EqInfo.Intensity6);
                    break;
                case "5弱":
                    await PlayAsync(Configuration.RootClass.Sounds.EqInfo.Intensity5);
                    break;
                case "4":
                    await PlayAsync(Configuration.RootClass.Sounds.EqInfo.Intensity4);
                    break;
                case "3":
                    await PlayAsync(Configuration.RootClass.Sounds.EqInfo.Intensity3);
                    break;
                case "2":
                    await PlayAsync(Configuration.RootClass.Sounds.EqInfo.Intensity2);
                    break;
                case "1":
                    await PlayAsync(Configuration.RootClass.Sounds.EqInfo.Intensity1);
                    break;
                default:
                    await PlayAsync(Configuration.RootClass.Sounds.EqInfo.Distant);
                    break;
            }
        }
        
        private static async Task PlayAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;
            if (PlayMap.ContainsValue(filePath)) return;
            await Task.Run(() =>
            {
                Instance.Invoke((MethodInvoker)(() =>
                {
                    if (mciSendString($"open \"{filePath}\" alias {filePath}", null, 0, Instance.Handle) != 0) return;
                    mciSendString($"play {filePath} notify", null, 0, Instance.Handle);
                    PlayMap.Add((int)mciGetDeviceID(filePath), filePath);
                }));
            });
        }

        protected class Window : NativeWindow
        {
            protected override void WndProc(ref Message m)
            {
                if (m.Msg == 953 && (int)m.WParam == 1)
                {
                    mciSendString($"close {PlayMap[(int)m.LParam]}", null, 0, Instance.Handle);
                    PlayMap.Remove((int)m.LParam);
                }
                base.WndProc(ref m);
            }
        }
    }
}
