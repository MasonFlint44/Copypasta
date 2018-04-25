using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using Copypasta.Annotations;
using Copypasta.Models.Interfaces;
using Copypasta.ViewModels.Interfaces;
using PaperClip.Collections.Interfaces;

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

        public HistoryRecordViewModel(IClipboardItemModel clipboardItem)
        {
            Key = clipboardItem.Key.ToString();
            ClipboardText = GetText(clipboardItem.ClipboardData);

            clipboardItem.KeyUpdated += (sender, args) => Key = clipboardItem.Key.ToString();
            clipboardItem.ClipboardDataUpdated += (sender, args) => ClipboardText = GetText(args.NewValue);
        }

        public static string GetText(IOrderedDictionary<string, MemoryStream> clipboardData)
        {
            if (!clipboardData.TryGetValue(DataFormats.UnicodeText.ToLower(), out var stream)) { return string.Empty; }
            return Encoding.Unicode.GetString(stream.ToArray()).TrimEnd('\0');
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
