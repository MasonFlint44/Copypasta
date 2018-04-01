using Copypasta.ViewModels.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Copypasta.Views
{
    /// <summary>
    /// Interaction logic for HistoryMenu.xaml
    /// </summary>
    public partial class HistoryMenu : UserControl
    {
        private IHistoryMenuViewModel _historyMenuViewModel;

        public HistoryMenu()
        {
            DataContextChanged += OnDataContextChanged;
            // Set DataContext to prevent inheriting parent's DataContext
            DataContext = null;
            InitializeComponent();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _historyMenuViewModel = e.NewValue as IHistoryMenuViewModel ?? throw new InvalidOperationException("DataContext must be of type: IHistoryMenuViewModel");
        }
    }
}
