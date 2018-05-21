using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Copypasta.Annotations;
using Copypasta.Domain.Interfaces;
using Copypasta.Models.Interfaces;
using Copypasta.ViewModels.Interfaces;

namespace Copypasta.ViewModels
{
    public class HistoryMenuViewModel: IHistoryMenuViewModel
    {
        private readonly IClipboardHistoryManager _clipboardHistoryManager;
        private readonly Dictionary<IHistoryRecordModel, IHistoryRecordViewModel> _modelToViewModelMappings;

        private readonly List<IHistoryRecordViewModel> _historyList;
        public ObservableCollection<IHistoryRecordViewModel> HistoryList => 
            new ObservableCollection<IHistoryRecordViewModel>(_historyList.AsEnumerable().Reverse());

        public event PropertyChangedEventHandler PropertyChanged;

        public HistoryMenuViewModel(IClipboardHistoryManager clipboardHistoryManager)
        {
            _clipboardHistoryManager = clipboardHistoryManager;
            _modelToViewModelMappings = new Dictionary<IHistoryRecordModel, IHistoryRecordViewModel>(_clipboardHistoryManager.RecordCount);
            _historyList = new List<IHistoryRecordViewModel>(_clipboardHistoryManager.RecordCount);

            _clipboardHistoryManager.Subscribe(notification =>
            {
                if (notification.WasRecordRemoved)
                {
                    var removedRecord = _modelToViewModelMappings[notification.RemovedRecord];
                    _modelToViewModelMappings.Remove(notification.RemovedRecord);
                    _historyList.Remove(removedRecord);
                }

                var addedRecord = new HistoryRecordViewModel(notification.AddedRecord);
                _modelToViewModelMappings[notification.AddedRecord] = addedRecord;
                _historyList.Add(addedRecord);

                OnPropertyChanged(nameof(HistoryList));
            });
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
