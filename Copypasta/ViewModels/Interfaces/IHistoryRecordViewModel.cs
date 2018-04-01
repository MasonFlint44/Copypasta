using System.ComponentModel;

namespace Copypasta.ViewModels.Interfaces
{
    public interface IHistoryRecordViewModel: INotifyPropertyChanged
    {
        string ClipboardText { get; set; }
        string Key { get; set; }
    }
}