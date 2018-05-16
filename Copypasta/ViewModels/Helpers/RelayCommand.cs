using System;
using System.Windows.Input;

namespace Copypasta.ViewModels.Helpers
{
    public class RelayCommand: ICommand
    {
        private readonly Action<object> _action;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public RelayCommand(Action<object> action, Func<object, bool> canExecute = null)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }
    }
}
