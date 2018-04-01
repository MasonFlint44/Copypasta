using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Copypasta.Annotations;
using Copypasta.Models.Interfaces;
using Copypasta.ViewModels.Interfaces;
using PaperClip.Collections;
using PaperClip.Collections.Interfaces;

namespace Copypasta.ViewModels
{
    public class HistoryMenuViewModel: IHistoryMenuViewModel
    {
        private readonly IClipboardHistoryModel _clipboardHistoryModel;

        private readonly ICircularList<IHistoryRecordViewModel> _historyList;
        public List<IHistoryRecordViewModel> HistoryList => _historyList.ToList();

        public event PropertyChangedEventHandler PropertyChanged;

        public HistoryMenuViewModel(IClipboardHistoryModel clipboardHistoryModel)
        {
            _clipboardHistoryModel = clipboardHistoryModel;

            _clipboardHistoryModel.History.ListUpdated += (sender, args) =>
            {
                _historyList.Add(new HistoryRecordViewModel(args.AddedElement));
                OnPropertyChanged(nameof(HistoryList));
            };

            _historyList = new CircularList<IHistoryRecordViewModel>(_clipboardHistoryModel.History.Size);
            foreach (var item in _clipboardHistoryModel.History)
            {
                _historyList.Add(new HistoryRecordViewModel(item));
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
