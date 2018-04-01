using System.ComponentModel;
using System.Runtime.CompilerServices;
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

        public HistoryRecordViewModel(IClipboardItemModel clipboardItem)
        {
            Key = clipboardItem.Key.ToString();
            ClipboardText = (string)clipboardItem.ClipboardData.GetData(typeof(string));

            clipboardItem.KeyUpdated += (sender, args) => Key = clipboardItem.Key.ToString();
            clipboardItem.ClipboardDataUpdated += (sender, args) => 
                ClipboardText = (string)clipboardItem.ClipboardData.GetData(typeof(string));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
