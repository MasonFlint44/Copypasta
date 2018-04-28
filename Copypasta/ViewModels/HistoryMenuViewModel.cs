using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Copypasta.Annotations;
using Copypasta.Domain.Interfaces;
using Copypasta.Models;
using Copypasta.ViewModels.Interfaces;

namespace Copypasta.ViewModels
{
    public class HistoryMenuViewModel: IHistoryMenuViewModel
    {
        private readonly IClipboardHistoryManager _clipboardHistoryManager;
        private readonly Dictionary<HistoryRecordModel, HistoryRecordViewModel> _modelToViewModelMappings;

        public ObservableCollection<IHistoryRecordViewModel> HistoryList { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public HistoryMenuViewModel(IClipboardHistoryManager clipboardHistoryManager)
        {
            _clipboardHistoryManager = clipboardHistoryManager;
            _modelToViewModelMappings = new Dictionary<HistoryRecordModel, HistoryRecordViewModel>(_clipboardHistoryManager.RecordCount);
            HistoryList = new ObservableCollection<IHistoryRecordViewModel>();
            
            _clipboardHistoryManager.Subscribe(notification =>
            {
                if (notification.WasRecordRemoved)
                {
                    var removedRecord = _modelToViewModelMappings[notification.RemovedRecord];
                    _modelToViewModelMappings.Remove(notification.RemovedRecord);
                    HistoryList.Remove(removedRecord);
                }

                var addedRecord = new HistoryRecordViewModel(notification.AddedRecord);
                _modelToViewModelMappings[notification.AddedRecord] = addedRecord;
                HistoryList.Add(addedRecord);

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
