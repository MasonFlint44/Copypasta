using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Copypasta.Annotations;
using Copypasta.Models.Interfaces;
using Copypasta.ViewModels.Interfaces;

namespace Copypasta.ViewModels
{
    public class HistoryRecordViewModel : IHistoryRecordViewModel
    {
        private string _key;
        public string Key
        {
            get => _key;
            set
            {
                _key = value;
                OnPropertyChanged(nameof(Key));
            }
        }

        private string _clipboardText;
        public string ClipboardText
        {
            get => _clipboardText;
            set
            {
                _clipboardText = value;
                OnPropertyChanged(nameof(ClipboardText));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public HistoryRecordViewModel(IHistoryRecordModel historyRecord)
        {
            historyRecord.Subscribe(notification =>
            {
                Key = GetText(notification.Key);
                ClipboardText = GetText(notification.ClipboardData);
            });
        }

        private static string GetText(Key key)
        {
            if(key == System.Windows.Input.Key.None) { return string.Empty; }
            return key.ToString();
        }

        private static string GetText(IClipboardDataModel clipboardData)
        {
            if (!clipboardData.ClipboardData.TryGetValue(DataFormats.UnicodeText.ToLower(), out var stream))
            {
                return string.Empty;
            }
            return Encoding.Unicode.GetString(stream.ToArray()).TrimEnd('\0');
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
