using ProjetImmo.Core.Tools;
using ProjetImmo.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetImmo.Core
{
    public static class NavigationService
    {
        // Nom de la propriété qui nous intéresse
        public static string ContextPropertyName = "DataContext";
        
        // "Cache" => Un seul objet (Window/Page/ViewModel/...) de chaque type à la fois (Dictionnaire)
        private static Dictionary<Type, object> _viewCache = new Dictionary<Type, object>();
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
        private static TView GetViewInstance<TView>(object viewModel)
            where TView : class
        {
            TView view = null;
            Type viewType = typeof(TView);
            if (_viewCache.ContainsKey(viewType))
                view = (TView)_viewCache[viewType];
            else
            {
                view = (TView)Activator.CreateInstance(viewType);

                // Récupérer la propriété recherchée (si inexistante, sera null)
                var prop = viewType.GetProperty(ContextPropertyName);

                // Si prop n'est pas null, on affecte le 'viewModel' sur la propriété choisie de 'view'
                prop?.SetValue(view, viewModel);
                _viewCache[viewType] = view;
            }
            return view;
        }

        // GetView => Renvoyer l'instance de la Page/Window, avec son ViewModel lié
        public static TView GetView<TView, TViewModel>(params object[] viewModelParameters)
            where TView : class
            where TViewModel : BaseNotifyPropertyChanged
        {
            return GetViewInstance<TView>(GetViewModelInstance<TViewModel>(viewModelParameters));
        }

        // Get<T> & Show/ShowDialog => Récupérer/Afficher une Fenêtre/Page
        public static void Show<TView, TViewModel>(params object[] viewModelParameters)
            where TView : class
            where TViewModel : BaseNotifyPropertyChanged
        {
            var win = GetViewInstance<TView>(GetViewModelInstance<TViewModel>(viewModelParameters));

            // Récuperer la méthode "Show" dans l'instance 'win', et l'appeler si elle existe
            var method = win.GetType().GetMethod("Show");
            method?.Invoke(win, null);
        }
        public static bool? ShowDialog<TView, TViewModel>(params object[] viewModelParameters)
            where TView : class
            where TViewModel : BaseNotifyPropertyChanged
        {
            var win = GetViewInstance<TView>(GetViewModelInstance<TViewModel>(viewModelParameters));

            // Récuperer le retour de la méthode "ShowDialog" dans l'instance 'win', et l'appeler si elle existe
            var method = win.GetType().GetMethod("ShowDialog");
            return (bool?)method?.Invoke(win, null);
        }


    }
}
