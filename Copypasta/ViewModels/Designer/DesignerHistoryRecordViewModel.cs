using System.ComponentModel;
using System.Runtime.CompilerServices;
using Copypasta.Annotations;
using Copypasta.ViewModels.Interfaces;

namespace Copypasta.ViewModels.Designer
{
    internal class DesignerHistoryRecordViewModel: IHistoryRecordViewModel
    {
        public string ClipboardText { get; set; } = "This is some test text to test the text";
        public string Key { get; set; } = System.Windows.Input.Key.A.ToString();
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
