using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjetImmo.Core.ViewModels
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
        
    }
}
