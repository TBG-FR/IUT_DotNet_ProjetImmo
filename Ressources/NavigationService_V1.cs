using ProjetImmo.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ProjetImmo.WPF
{
    public static class NavigationService
    {

        // "Cache" => Une seule Window/Page/ViewModel de chaque type à la fois (Dictionnaire)
        private static Dictionary<Type, Window> _windowsCache = new Dictionary<Type, Window>();
        private static Dictionary<Type, Page> _pagesCache = new Dictionary<Type, Page>();
        private static Dictionary<Type, BaseNotifyPropertyChanged> _viewModelCache = new Dictionary<Type, BaseNotifyPropertyChanged>();

        // Get<T>Instance => Récupérer l'instance existante ou la créer
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
        private static TPage GetPageInstance<TPage>(object viewModel)
            where TPage : Page
        {
            TPage page = null;
            Type pageType = typeof(TPage);
            if (_pagesCache.ContainsKey(pageType))
                page = (TPage)_pagesCache[pageType];
            else
            {
                page = (TPage)Activator.CreateInstance(pageType);
                page.DataContext = viewModel;
                _pagesCache[pageType] = page;
            }
            return page;
        }

        // Get<T> & Show/ShowDialog => Récupérer/Afficher une Fenêtre/Page
        public static TWindow GetWindow<TWindow, TViewModel>(params object[] viewModelParameters)
            where TWindow : Window
            where TViewModel : BaseNotifyPropertyChanged
        {
            return GetWindowInstance<TWindow>(GetViewModelInstance<TViewModel>(viewModelParameters));
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
        public static Page GetPage<TPage, TViewModel>(params object[] viewModelParameters)
            where TPage : Page
            where TViewModel : BaseNotifyPropertyChanged
        {
            return GetPageInstance<TPage>(GetViewModelInstance<TViewModel>(viewModelParameters));
        }

    }
}
