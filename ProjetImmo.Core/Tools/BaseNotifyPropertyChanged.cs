using ProjetImmo.Core.Commandes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProjetImmo.Core.Tools
{
    public abstract class BaseNotifyPropertyChanged : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;

        private Dictionary<string, object> _propertyValues = new Dictionary<string, object>();

        protected T GetProperty<T>([CallerMemberName] string propertyName = null)
        {
            if (_propertyValues.ContainsKey(propertyName)) return (T)_propertyValues[propertyName];
            return default(T);
        }

        protected bool SetProperty<T>(T newValue, [CallerMemberName] string propertyName = null)
        {
            T current = GetProperty<T>(propertyName);
            if (!EqualityComparer<T>.Default.Equals(current, newValue))
            {
                _propertyValues[propertyName] = newValue;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
                return true;
            }
            return false;
        }
        
        
        public BaseCommand<Type, Type> ShowWindowCommand //OpenWindow
        {
            get => new BaseCommand<Type, Type>((tView, tViewModel) => { NavigationService.Show(tView, tViewModel);  });
        }

        public abstract void refresh();

    }
}
