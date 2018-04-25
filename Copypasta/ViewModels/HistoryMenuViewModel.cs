using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Copypasta.Annotations;
using Copypasta.Domain.Interfaces;
using Copypasta.ViewModels.Interfaces;
using PaperClip.Collections;
using PaperClip.Collections.Interfaces;

namespace Copypasta.ViewModels
{
    public class HistoryMenuViewModel: IHistoryMenuViewModel
    {
        private readonly IClipboardHistoryManager _clipboardHistoryManager;

        private readonly ICircularList<IHistoryRecordViewModel> _historyList;
        public List<IHistoryRecordViewModel> HistoryList => _historyList.ToList();

        public event PropertyChangedEventHandler PropertyChanged;

        public HistoryMenuViewModel(IClipboardHistoryManager clipboardHistoryManager)
        {
            _clipboardHistoryManager = clipboardHistoryManager;

            _clipboardHistoryManager.History.ListUpdated += (sender, args) =>
            {
                _historyList.Add(new HistoryRecordViewModel(args.AddedElement));
                OnPropertyChanged(nameof(HistoryList));
            };

            _historyList = new CircularList<IHistoryRecordViewModel>(_clipboardHistoryManager.History.Size);
            foreach (var item in _clipboardHistoryManager.History)
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
