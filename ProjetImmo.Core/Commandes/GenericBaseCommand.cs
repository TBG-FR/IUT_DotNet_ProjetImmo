using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjetImmo.Core.Commandes
{
    public class BaseCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public BaseCommand(Action<T> execute) : this(execute, null) { }

        public BaseCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            _execute = execute;
            _canExecute = canExecute;
        }


        public void OnCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter) && _execute != null)
            {
                T param = default(T);
                try
                {
                    param = (T)parameter;
                }
                catch { }

                _execute(param);
            }
        }
    }

    public class BaseCommand<T1, T2> : ICommand
    {
        private readonly Action<T1, T2> _execute;
        private readonly Func<T1, T2, bool> _canExecute;

        public event EventHandler CanExecuteChanged;

        public BaseCommand(Action<T1, T2> execute) : this(execute, null) { }

        public BaseCommand(Action<T1, T2> execute, Func<T1, T2, bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            _execute = execute;
            _canExecute = canExecute;
        }


        public void OnCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public bool CanExecute(object parameter)
        {
            T1 param1;
            T2 param2;
            GetParameters(parameter, out param1, out param2);

            return _canExecute == null || _canExecute(param1, param2);
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter) && _execute != null)
            {
                T1 param1;
                T2 param2;
                GetParameters(parameter, out param1, out param2);
                _execute(param1, param2);
            }
        }

        private void GetParameters(object parameter, out T1 param1, out T2 param2)
        {
            param1 = default(T1);
            param2 = default(T2);

            if (parameter == null || parameter.GetType() != typeof(object[]) || ((object[])parameter).Length != 2) return;

            param1 = (T1)((object[])parameter)[0];
            param2 = (T2)((object[])parameter)[1];
        }
    }
}
