using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ProjetImmo.Core.ViewModels
{
    public class BaseCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;
        public event EventHandler CanExecuteChanged;

        public BaseCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null) { throw new ArgumentNullException("execute"); }
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public virtual void Execute(object parameter)
        {
            if (CanExecute(parameter) && _execute != null)
            {
                _execute();
            }
        }

    }

}