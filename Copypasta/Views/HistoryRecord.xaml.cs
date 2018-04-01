using Copypasta.ViewModels.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Copypasta.Views
{
    /// <summary>
    /// Interaction logic for HistoryRecord.xaml
    /// </summary>
    public partial class HistoryRecord : UserControl
    {
        private IHistoryRecordViewModel _historyRecordViewModel;

        public HistoryRecord()
        {
            DataContextChanged += OnDataContextChanged;
            InitializeComponent();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _historyRecordViewModel = e.NewValue as IHistoryRecordViewModel ?? throw new InvalidOperationException("DataContext must be of type: IHistoryRecordViewModel");
        }
    }
}
