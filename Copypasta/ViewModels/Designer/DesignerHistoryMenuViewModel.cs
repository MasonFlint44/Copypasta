using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Copypasta.Annotations;
using Copypasta.ViewModels.Interfaces;

namespace Copypasta.ViewModels.Designer
{
    public class DesignerHistoryMenuViewModel: IHistoryMenuViewModel
    {
        public List<IHistoryRecordViewModel> HistoryList { get; } = new List<IHistoryRecordViewModel>();

        public event PropertyChangedEventHandler PropertyChanged;

        public DesignerHistoryMenuViewModel()
        {
            HistoryList.Add(new DesignerHistoryRecordViewModel());
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
