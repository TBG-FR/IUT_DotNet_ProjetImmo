using ProjetImmo.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EstateAgencyManager.Wpf
{
    public static class NavigationService
    {
        private static Dictionary<Type, Window> _windowsCache = new Dictionary<Type, Window>();
        private static Dictionary<Type, BaseNotifyPropertyChanged> _viewModelCache = new Dictionary<Type, BaseNotifyPropertyChanged>();

        private static TViewModel GetViewModelInstance<TViewModel>(params object[] viewModelParameters)
            where TViewModel : BaseNotifyPropertyChanged
        {
            TViewModel vm = null;
            Type vmType = typeof(TViewModel);
            if (_viewModelCache.ContainsKey(vmType))
                vm = (TViewModel)_viewModelCache[vmType];
            else
            {
                vm = (TViewModel)Activator.CreateInstance(vmType, viewModelParameters);
                _viewModelCache[vmType] = vm;
            }
            return vm;
        }
        private static TWindow GetWindowInstance<TWindow>(object viewModel)
            where TWindow : Window
        {
            TWindow win = null;
            Type winType = typeof(TWindow);
            if (_windowsCache.ContainsKey(winType))
                win = (TWindow)_windowsCache[winType];
            else
            {
                win = (TWindow)Activator.CreateInstance(winType);
                win.DataContext = viewModel;
                _windowsCache[winType] = win;
            }
            return win;
        }

        public static void Show<TWindow, TViewModel>(params object[] viewModelParameters)
            where TWindow : Window
            where TViewModel : BaseNotifyPropertyChanged
        {
            var win = GetWindowInstance<TWindow>(GetViewModelInstance<TViewModel>(viewModelParameters));
            win.Show();
        }

        public static bool? ShowDialog<TWindow, TViewModel>(params object[] viewModelParameters)
            where TWindow : Window
            where TViewModel : BaseNotifyPropertyChanged
        {
            var win = GetWindowInstance<TWindow>(GetViewModelInstance<TViewModel>(viewModelParameters));
            return win.ShowDialog();
        }

    }
}
