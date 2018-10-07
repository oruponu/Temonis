using System.Threading.Tasks;
using System.Windows.Forms;
using static Temonis.MainWindow;
using static Temonis.NativeMethods;

namespace Temonis
{
    internal class Sound
    {
        public static void OpenFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;
            mciSendString($"open \"{filePath}\" alias {filePath}", null, 0, Instance.Handle);
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
            switch (Eew.Info[key])
            {
                case "不明":
                    await PlayAsync(Configuration.RootClass.Sounds.Eew.FirstReport.Unknown);
                    break;
                case "1":
                    await PlayAsync(Configuration.RootClass.Sounds.Eew.FirstReport.Intensity1);
                    break;
                case "2":
                    await PlayAsync(Configuration.RootClass.Sounds.Eew.FirstReport.Intensity2);
                    break;
                case "3":
                    await PlayAsync(Configuration.RootClass.Sounds.Eew.FirstReport.Intensity3);
                    break;
                case "4":
                    await PlayAsync(Configuration.RootClass.Sounds.Eew.FirstReport.Intensity4);
                    break;
                case "5弱":
                    await PlayAsync(Configuration.RootClass.Sounds.Eew.FirstReport.Intensity5);
                    break;
                case "5強":
                    await PlayAsync(Configuration.RootClass.Sounds.Eew.FirstReport.Intensity6);
                    break;
                case "6弱":
                    await PlayAsync(Configuration.RootClass.Sounds.Eew.FirstReport.Intensity7);
                    break;
                case "6強":
                    await PlayAsync(Configuration.RootClass.Sounds.Eew.FirstReport.Intensity8);
                    break;
                case "7":
                    await PlayAsync(Configuration.RootClass.Sounds.Eew.FirstReport.Intensity9);
                    break;
            }
        }

        public async Task PlayMaxIntChangeAsync(string key)
        {
            switch (Eew.Info[key])
            {
                case "不明":
                    await PlayAsync(Configuration.RootClass.Sounds.Eew.MaxIntChange.Unknown);
                    break;
                case "1":
                    await PlayAsync(Configuration.RootClass.Sounds.Eew.MaxIntChange.Intensity1);
                    break;
                case "2":
                    await PlayAsync(Configuration.RootClass.Sounds.Eew.MaxIntChange.Intensity2);
                    break;
                case "3":
                    await PlayAsync(Configuration.RootClass.Sounds.Eew.MaxIntChange.Intensity3);
                    break;
                case "4":
                    await PlayAsync(Configuration.RootClass.Sounds.Eew.MaxIntChange.Intensity4);
                    break;
                case "5弱":
                    await PlayAsync(Configuration.RootClass.Sounds.Eew.MaxIntChange.Intensity5);
                    break;
                case "5強":
                    await PlayAsync(Configuration.RootClass.Sounds.Eew.MaxIntChange.Intensity6);
                    break;
                case "6弱":
                    await PlayAsync(Configuration.RootClass.Sounds.Eew.MaxIntChange.Intensity7);
                    break;
                case "6強":
                    await PlayAsync(Configuration.RootClass.Sounds.Eew.MaxIntChange.Intensity8);
                    break;
                case "7":
                    await PlayAsync(Configuration.RootClass.Sounds.Eew.MaxIntChange.Intensity9);
                    break;
                default:
                    await PlayAsync(Configuration.RootClass.Sounds.Eew.MaxIntChange.Cancel);
                    break;
            }
        }

        public async Task PlayEqInfoAsync()
        {
            switch (EqInfo.MaxInt)
            {
                case "1":
                    await PlayAsync(Configuration.RootClass.Sounds.EqInfo.Intensity1);
                    break;
                case "2":
                    await PlayAsync(Configuration.RootClass.Sounds.EqInfo.Intensity2);
                    break;
                case "3":
                    await PlayAsync(Configuration.RootClass.Sounds.EqInfo.Intensity3);
                    break;
                case "4":
                    await PlayAsync(Configuration.RootClass.Sounds.EqInfo.Intensity4);
                    break;
                case "5弱":
                    await PlayAsync(Configuration.RootClass.Sounds.EqInfo.Intensity5);
                    break;
                case "5強":
                    await PlayAsync(Configuration.RootClass.Sounds.EqInfo.Intensity6);
                    break;
                case "6弱":
                    await PlayAsync(Configuration.RootClass.Sounds.EqInfo.Intensity7);
                    break;
                case "6強":
                    await PlayAsync(Configuration.RootClass.Sounds.EqInfo.Intensity8);
                    break;
                case "7":
                    await PlayAsync(Configuration.RootClass.Sounds.EqInfo.Intensity9);
                    break;
                default:
                    await PlayAsync(Configuration.RootClass.Sounds.EqInfo.Distant);
                    break;
            }
        }
        
        private static async Task PlayAsync(string filePath)
        {
            await Task.Run(() =>
            {
                Instance.Invoke((MethodInvoker)(() =>
                {
                    mciSendString($"play {filePath} from 0", null, 0, Instance.Handle);
                }));
            });
        }
    }
}
