using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Hotkeys
{
    public class KeyCombo
    {
        private List<List<Key>> _Key = new List<List<Key>>();
        private Dictionary<string, Key[]> _keyMappings = new Dictionary<string, Key[]>
        {
                { "ctrl", new[] { Key.LeftCtrl, Key.RightCtrl } },
                { "control", new[] { Key.LeftCtrl, Key.RightCtrl } },
                { "alt", new[] { Key.LeftAlt, Key.RightAlt } },
                { "win", new[] { Key.LWin, Key.RWin } },
                { "shift", new[] { Key.LeftShift, Key.RightShift } },
                { "del", new[] { Key.Delete } },
                { "esc", new[] { Key.Escape } },
                { "space", new[] { Key.Space } },
                { "capslock", new[] { Key.CapsLock, Key.Capital } },
                { "tab", new[] { Key.Tab } },
                { "enter", new[] { Key.Return, Key.Enter } },
                { "f1", new[] { Key.F1 } },
                { "f2", new[] { Key.F2 } },
                { "f3", new[] { Key.F3 } },
                { "f4", new[] { Key.F4 } },
                { "f5", new[] { Key.F5 } },
                { "f6", new[] { Key.F6 } },
                { "f7", new[] { Key.F7 } },
                { "f8", new[] { Key.F8 } },
                { "f9", new[] { Key.F9 } },
                { "f10", new[] { Key.F10 } },
                { "f11", new[] { Key.F11 } },
                { "f12", new[] { Key.F12 } },
                { "f13", new[] { Key.F13 } },
                { "f14", new[] { Key.F14 } },
                { "f15", new[] { Key.F15 } },
                { "f16", new[] { Key.F16 } },
                { "f17", new[] { Key.F17 } },
                { "f18", new[] { Key.F18 } },
                { "f19", new[] { Key.F19 } },
                { "f20", new[] { Key.F20 } },
                { "f21", new[] { Key.F21 } },
                { "f22", new[] { Key.F22 } },
                { "f23", new[] { Key.F23 } },
                { "f24", new[] { Key.F24 } },
                { "1", new[] { Key.D1 } },
                { "2", new[] { Key.D2 } },
                { "3", new[] { Key.D3 } },
                { "4", new[] { Key.D4 } },
                { "5", new[] { Key.D5 } },
                { "6", new[] { Key.D6 } },
                { "7", new[] { Key.D7 } },
                { "8", new[] { Key.D8 } },
                { "9", new[] { Key.D9 } },
                { "0", new[] { Key.D0 } },
                { "num1", new[] { Key.NumPad1 } },
                { "num2", new[] { Key.NumPad2 } },
                { "num3", new[] { Key.NumPad3 } },
                { "num4", new[] { Key.NumPad4 } },
                { "num5", new[] { Key.NumPad5 } },
                { "num6", new[] { Key.NumPad6 } },
                { "num7", new[] { Key.NumPad7 } },
                { "num8", new[] { Key.NumPad8 } },
                { "num9", new[] { Key.NumPad9 } },
                { "num0", new[] { Key.NumPad0 } },
                { "numlock", new[] { Key.NumLock } },
                { "left", new[] { Key.Left } },
                { "right", new[] { Key.Right } },
                { "up", new[] { Key.Up } },
                { "down", new[] { Key.Down } },
                { "backspace", new[] { Key.Back } },
                { "home", new[] { Key.Home } },
                { "end", new[] { Key.End } },
                { "pageup", new[] { Key.PageUp, Key.Prior } },
                { "pagedown", new[] { Key.PageDown, Key.Next } },
                { "insert", new[] { Key.Insert } },
                { "printscreen", new[] { Key.PrintScreen, Key.Snapshot } },
                { "scrolllock", new[] { Key.Scroll } },
                { "pause", new[] { Key.Pause } },
                { "a", new[] { Key.A } },
                { "b", new[] { Key.B } },
                { "c", new[] { Key.C } },
                { "d", new[] { Key.D } },
                { "e", new[] { Key.E } },
                { "f", new[] { Key.F } },
                { "g", new[] { Key.G } },
                { "h", new[] { Key.H } },
                { "i", new[] { Key.I } },
                { "j", new[] { Key.J } },
                { "k", new[] { Key.K } },
                { "l", new[] { Key.L } },
                { "m", new[] { Key.M } },
                { "n", new[] { Key.N } },
                { "o", new[] { Key.O } },
                { "p", new[] { Key.P } },
                { "q", new[] { Key.Q } },
                { "r", new[] { Key.R } },
                { "s", new[] { Key.S } },
                { "t", new[] { Key.T } },
                { "u", new[] { Key.U } },
                { "v", new[] { Key.V } },
                { "w", new[] { Key.W } },
                { "x", new[] { Key.X } },
                { "y", new[] { Key.Y } },
                { "z", new[] { Key.Z } },
                { "_", new[] { Key.OemMinus } },
                { "=", new[] { Key.OemPlus } },
                { "|", new[] { Key.OemPipe } },
                { "]", new[] { Key.Oem6 } },
                { "[", new[] { Key.OemOpenBrackets } },
                { "'", new[] { Key.Oem7 } },
                { ";", new[] { Key.Oem1 } },
                { "?", new[] { Key.OemQuestion } },
                { ">", new[] { Key.OemPeriod } },
                { "<", new[] { Key.OemComma } },
                { "~", new[] { Key.OemTilde } },
                { "/", new[] { Key.Divide } },
                { "*", new[] { Key.Multiply } },
                { "-", new[] { Key.Subtract } },
                { "plus", new[] { Key.Add } },
                { ".", new[] { Key.Decimal } },
                { "fn", new[] { Key.None } },
                { "break", new[] { Key.Cancel } },
                { "apps", new[] { Key.Apps } },
                { "attn", new[] { Key.Attn } },
                { "browserback", new[] { Key.BrowserBack } },
                { "browserfavorites", new[] { Key.BrowserFavorites } },
                { "browserforward", new[] { Key.BrowserForward } },
                { "browserhome", new[] { Key.BrowserHome } },
                { "browserrefresh", new[] { Key.BrowserRefresh } },
                { "browsersearch", new[] { Key.BrowserSearch } },
                { "browserstop", new[] { Key.BrowserStop } },
                { "clear", new[] { Key.Clear, Key.OemClear } },
                { "crsel", new[] { Key.CrSel } },
                { "eraseeof", new[] { Key.EraseEof } },
                { "execute", new[] { Key.Execute } },
                { "exsel", new[] { Key.ExSel } },
                { "finalmode", new[] { Key.FinalMode } },
                { "hangulmode", new[] { Key.HangulMode } },
                { "hanjamode", new[] { Key.HanjaMode } },
                { "help", new[] { Key.Help } },
                { "imeconvert", new[] { Key.ImeConvert } },
                { "imemodechange", new[] { Key.ImeModeChange } },
                { "imenonconvert", new[] { Key.ImeNonConvert } },
                { "junjamode", new[] { Key.JunjaMode } },
                { "kanamode", new[] { Key.KanaMode } },
                { "kanjimode", new[] { Key.KanjiMode } },
                { "app1", new[] { Key.LaunchApplication1 } },
                { "app2", new[] { Key.LaunchApplication2 } },
                { "mail", new[] { Key.LaunchMail } },
                { "lctrl", new[] { Key.LeftCtrl} },
                { "lcontrol", new[] { Key.LeftCtrl} },
                { "leftctrl", new[] { Key.LeftCtrl } },
                { "leftcontrol", new[] { Key.LeftCtrl} },
                { "linefeed", new[] { Key.LineFeed } },
                { "lalt", new[] { Key.LeftAlt } },
                { "leftalt", new[] { Key.LeftAlt } },
                { "lshift", new[] { Key.LeftShift } },
                { "leftshift", new[] { Key.LeftShift } },
                { "lwin", new[] { Key.LWin } },
                { "medianext", new[] { Key.MediaNextTrack } },
                { "mediaplay", new[] { Key.MediaPlayPause } },
                { "mediaprev", new[] { Key.MediaPreviousTrack } },
                { "mediastop", new[] { Key.MediaStop } },
                { "noname", new[] { Key.NoName } },
                { "none", new[] { Key.None } },
                { "oem1", new[] { Key.Oem1 } },
                { "oem102", new[] { Key.Oem102 } },
                { "oem2", new[] { Key.Oem2 } },
                { "oem3", new[] { Key.Oem3 } },
                { "oem4", new[] { Key.Oem4 } },
                { "oem5", new[] { Key.Oem5 } },
                { "oem6", new[] { Key.Oem6 } },
                { "oem7", new[] { Key.Oem7 } },
                { "oem8", new[] { Key.Oem8 } },
                { "oembackslash", new[] { Key.OemBackslash } },
                { "oemclear", new[] { Key.OemClear } },
                { "oemclosebrackets", new[] { Key.OemCloseBrackets } },
                { "oemcomma", new[] { Key.OemComma } },
                { "oemminus", new[] { Key.OemMinus } },
                { "oemopenbrackets", new[] { Key.OemOpenBrackets } },
                { "oemperiod", new[] { Key.OemPeriod } },
                { "oempipe", new[] { Key.OemPipe } },
                { "oemplus", new[] { Key.OemPlus } },
                { "oemquestion", new[] { Key.OemQuestion } },
                { "oemquotes", new[] { Key.OemQuotes } },
                { "oemsemicolon", new[] { Key.OemSemicolon } },
                { "oemtilde", new[] { Key.OemTilde } },
                { "pa1", new[] { Key.Pa1 } },
                { "play", new[] { Key.Play } },
                { "print", new[] { Key.Print } },
                { "prior", new[] { Key.Prior } },
                { "next", new[] { Key.Next } },
                { "imeprocessed", new[] { Key.ImeProcessed } },
                { "rcontrol", new[] { Key.RightCtrl} },
                { "rightcontrol", new[] { Key.RightCtrl} },
                { "rctrl", new[] { Key.RightCtrl} },
                { "rightctrl", new[] { Key.RightCtrl} },
                { "return", new[] { Key.Return } },
                { "ralt", new[] { Key.RightAlt } },
                { "rightalt", new[] { Key.RightAlt } },
                { "rshift", new[] { Key.RightShift } },
                { "rightshift", new[] { Key.RightShift } },
                { "rwin", new[] { Key.RWin } },
                { "select", new[] { Key.Select } },
                { "selectmedia", new[] { Key.SelectMedia } },
                { "separator", new[] { Key.Separator } },
                { "sleep", new[] { Key.Sleep } },
                { "voldown", new[] { Key.VolumeDown } },
                { "volumedown", new[] { Key.VolumeDown } },
                { "volmute", new[] { Key.VolumeMute } },
                { "volumemute", new[] { Key.VolumeMute } },
                { "volup", new[] { Key.VolumeUp } },
                { "volumeup", new[] { Key.VolumeUp } },
                { "zoom", new[] { Key.Zoom } },
                { "sys", new[] { Key.System } },
                { "system", new[] { Key.System } },
                { "dbealphanumeric", new[] { Key.DbeAlphanumeric } },
                { "dbecodeinput", new[] { Key.DbeCodeInput } },
                { "dbedbcschar", new[] { Key.DbeDbcsChar } },
                { "dbedeterminestring", new[] { Key.DbeDetermineString } },
                { "dbeenterdialogconversionmode", new[] { Key.DbeEnterDialogConversionMode } },
                { "dbeenterimeconfiguremode", new[] { Key.DbeEnterImeConfigureMode } },
                { "dbeenterwordregistermode", new[] { Key.DbeEnterWordRegisterMode } },
                { "dbeflushstring", new[] { Key.DbeFlushString } },
                { "dbehiragana", new[] { Key.DbeHiragana } },
                { "dbekatakana", new[] { Key.DbeKatakana } },
                { "dbenocodeinput", new[] { Key.DbeNoCodeInput } },
                { "dbenoroman", new[] { Key.DbeNoRoman } },
                { "dberoman", new[] { Key.DbeRoman } },
                { "dbesbcschar", new[] { Key.DbeSbcsChar } },
                { "deadcharprocessed", new[] { Key.DeadCharProcessed } }
        };
        private Dictionary<Key, bool> _usedKey = new Dictionary<Key, bool>();

        public int Count => _Key.Count;

        public KeyCombo(Key key)
        {
            And(key);
        }

        public KeyCombo(string key)
        {
            And(key);   
        }

        public KeyCombo And(string key)
        {
            if (!_keyMappings.TryGetValue(key, out var keyValues)) { throw new InvalidKeyComboException($"Invalid key: \"{key}\""); }

            And(keyValues[0]);
            for (int i = 1; i < keyValues.Length; i++)
            {
                Or(keyValues[i]);
            }

            return this;
        }

        public KeyCombo And(Key key)
        {
            if(_usedKey.ContainsKey(key)) { throw new InvalidKeyComboException($"Key \"{key}\" cannot be used more than once"); }
            _usedKey.Add(key, true);

            List<Key> Key = new List<Key> { key };
            _Key.Add(Key);

            return this;
        }

        public KeyCombo Or(string key)
        {
            if (!_keyMappings.TryGetValue(key, out var keyValues)) { throw new InvalidKeyComboException($"Invalid key: \"{key}\""); }

            foreach(var keyValue in keyValues)
            {
                Or(keyValue);
            }

            return this;
        }
        
        public KeyCombo Or(Key key)
        {
            if (_usedKey.ContainsKey(key)) { throw new InvalidKeyComboException($"Key \"{key}\" cannot be used more than once"); }
            _usedKey.Add(key, true);

            _Key.Last().Add(key);
            
            return this;
        }

        /// <summary>
        /// Evaluates whether the provided key satisfies the index of the KeyCombo.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool IsValid(int index, Key key)
        {
            return _Key[index].Contains(key);
        }
    }
}
