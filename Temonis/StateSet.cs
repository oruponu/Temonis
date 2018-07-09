using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Temonis.Resources;
using static Temonis.MainWindow;

namespace Temonis
{
    internal class StateSet
    {
        private static Dictionary<string, string> _lastEewInfo = new Dictionary<string, string>();
        private static Sound _sound;
        private static bool _isKyoshinOn;
        private static bool _isKyoshinWait = true;
        private static int _lastKyoshinIntensity;
        private static bool _isEewOn;
        private static string _lastEqInfoId;
        private static string _lastEqInfoEpicenter;
        private static string _lastEqInfoIntensity;

        public StateSet()
        {
            _sound = new Sound();
        }

        public async Task UpdateAsync()
        {
            var changeLevel = false;
            var setActive = false;

            // 強震モニタ
            if (Kyoshin.IsTriggerOn && !Kyoshin.IsTriggerWait && !Eew.IsTriggerOn)
            {
                if (Kyoshin.MaxIntensity >= 2 && (_isKyoshinWait != Kyoshin.IsTriggerWait || _lastKyoshinIntensity < Kyoshin.MaxIntensity))
                {
                    setActive = true;
                    await _sound.PlayKyoshinAsync();
                    InternalLog(Instance.Label_KyoshinMaxInt.Text);
                }
                else if (Kyoshin.MaxIntensity >= 1 && (!_isKyoshinOn || _lastKyoshinIntensity < Kyoshin.MaxIntensity))
                {
                    setActive = true;
                    await _sound.PlayKyoshinAsync();
                    InternalLog(Instance.Label_KyoshinMaxInt.Text);
                }
                else if (_lastKyoshinIntensity > Kyoshin.MaxIntensity)
                {
                    changeLevel = true;
                }
            }
            else if (_lastKyoshinIntensity != Kyoshin.MaxIntensity || _isKyoshinWait != Kyoshin.IsTriggerWait)
            {
                changeLevel = true;
            }
            
            _isKyoshinOn = Kyoshin.IsTriggerOn;
            _isKyoshinWait = Kyoshin.IsTriggerWait;
            _lastKyoshinIntensity = Kyoshin.MaxIntensity;

            // 緊急地震速報
            if (Eew.IsTriggerOn)
            {
                foreach (var key in Eew.Info.Keys)
                {
                    if (!_lastEewInfo.ContainsKey(key))
                    {
                        _lastEewInfo.Add(key, Eew.Info[key]);
                        setActive = true;
                        await _sound.PlayFirstReportAsync(key);
                    }
                    else
                    {
                        if (_lastEewInfo[key] == Eew.Info[key]) continue;
                        _lastEewInfo[key] = Eew.Info[key];
                        setActive = true;
                        await _sound.PlayMaxIntChangeAsync(key);
                    }
                }
            }
            else if (_isEewOn != Eew.IsTriggerOn)
            {
                changeLevel = true;
                _lastEewInfo = new Dictionary<string, string>();
            }

            _isEewOn = Eew.IsTriggerOn;

            // 地震情報
            if (_lastEqInfoId != EqInfo.Id || _lastEqInfoEpicenter != EqInfo.Epicenter || _lastEqInfoIntensity != EqInfo.Intensity)
            {
                if (_lastEqInfoId == null)
                {
                    _lastEqInfoId = EqInfo.Id;
                    _lastEqInfoEpicenter = EqInfo.Epicenter;
                    _lastEqInfoIntensity = EqInfo.Intensity;
                    changeLevel = true;
                }
                else
                {
                    _lastEqInfoId = EqInfo.Id;
                    _lastEqInfoEpicenter = EqInfo.Epicenter;
                    _lastEqInfoIntensity = EqInfo.Intensity;
                    setActive = true;
                    await _sound.PlayEqInfoAsync();
                }
            }

            if (changeLevel || setActive) ChangeLevel();
            if (Configuration.RootClass.Behavior.ForceActive && setActive) SetActive();
        }

        /// <summary>
        /// レベルを変更します。
        /// </summary>
        private static void ChangeLevel()
        {
            //強震モニタ
            if (Kyoshin.IsTriggerOn) //1点赤ではレベルを変更しない
            {
                if (Instance.Label_KyoshinMaxInt.Text.Contains("弱") || Instance.Label_KyoshinMaxInt.Text.Contains("強") || Instance.Label_KyoshinMaxInt.Text.Contains("7（"))
                {
                    Instance.GroupBox_Kyoshin.BorderColor = Utility.Red;
                }
                else if (Instance.Label_KyoshinMaxInt.Text.Contains("3（") || Instance.Label_KyoshinMaxInt.Text.Contains("4（"))
                {
                    Instance.GroupBox_Kyoshin.BorderColor = Utility.Yellow;
                }
                else
                {
                    Instance.GroupBox_Kyoshin.BorderColor = Utility.White;
                }
            }
            else
            {
                Instance.GroupBox_Kyoshin.BorderColor = Utility.White;
            }

            //緊急地震速報
            if (Eew.IsTriggerOn)
            {
                if (Instance.Label_EewMessage.Text.Contains("警報"))
                {
                    Instance.GroupBox_Eew.BorderColor = Utility.Red;
                }
                else if (Instance.Label_EewMessage.Text.Contains("予報"))
                {
                    Instance.GroupBox_Eew.BorderColor = Utility.Yellow;
                }
            }
            else
            {
                Instance.GroupBox_Eew.BorderColor = Utility.White;
            }

            //地震情報
            if (EqInfo.MaxInt.Contains("弱") || EqInfo.MaxInt.Contains("強") || EqInfo.MaxInt.Contains("7") || Instance.Label_EqInfoComment.Text.Contains("警報"))
            {
                Instance.GroupBox_EqInfo.BorderColor = Utility.Red;
            }
            else if (EqInfo.MaxInt.Contains("3") || EqInfo.MaxInt.Contains("4") || Instance.Label_EqInfoComment.Text.Contains("注意報"))
            {
                Instance.GroupBox_EqInfo.BorderColor = Utility.Yellow;
            }
            else
            {
                Instance.GroupBox_EqInfo.BorderColor = Utility.White;
            }
        }

        /// <summary>
        /// ウィンドウをアクティブにします。
        /// </summary>
        private static void SetActive()
        {
            if (Instance.WindowState == FormWindowState.Minimized)
            {
                Instance.WindowState = FormWindowState.Normal;
            }

            Instance.Refresh();
            Instance.Activate();
        }
    }
}
