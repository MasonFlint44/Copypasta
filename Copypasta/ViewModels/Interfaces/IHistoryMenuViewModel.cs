using System.Collections.Generic;
using System.ComponentModel;

namespace Copypasta.ViewModels.Interfaces
{
    public interface IHistoryMenuViewModel: INotifyPropertyChanged
    {
        List<IHistoryRecordViewModel> HistoryList { get; }
    }
}
