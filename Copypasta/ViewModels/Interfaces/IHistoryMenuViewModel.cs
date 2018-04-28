using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Copypasta.ViewModels.Interfaces
{
    public interface IHistoryMenuViewModel: INotifyPropertyChanged
    {
        ObservableCollection<IHistoryRecordViewModel> HistoryList { get; }
    }
}
