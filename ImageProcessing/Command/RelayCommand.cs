using System;
using System.Windows.Input;

namespace ImageProcessing.Command
{
    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private Action _execute;
        private Func<bool> _canExecuteEvaluator;

        public RelayCommand(Action execute, Func<bool> canExecuteEvaluator)
        {
            _execute = execute;
            _canExecuteEvaluator = canExecuteEvaluator;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecuteEvaluator == null)
            {
                return true;
            }
            else
            {
                return _canExecuteEvaluator.Invoke();
            }
        }

        public void Execute(object parameter)
        {
            _execute.Invoke();
        }
    }
}
