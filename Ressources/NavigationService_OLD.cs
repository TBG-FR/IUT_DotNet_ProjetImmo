﻿using ProjetImmo.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjetImmo.WPF
{
   public static class OLD_NavigationService
    {

        // Cache static & co => Une seule fenêtre de chaque type à la fois
        private static Dictionary<Type, Window> windowsCache = new Dictionary<Type, Window>();
        private static Dictionary<Type, BaseNotifyPropertyChanged> viewModelCache = new Dictionary<Type, BaseNotifyPropertyChanged>();

        public static void Show<TWindow, TViewModel>(BaseNotifyPropertyChanged viewModel, params object[] viewModelParameters)
            where TWindow : Window
            where TViewModel : BaseNotifyPropertyChanged
        {
            BaseNotifyPropertyChanged vm = null; //viewModel
            Type vmType = typeof(TViewModel);

            if (viewModelCache.ContainsKey(vmType)) { vm = viewModelCache[vmType]; } // Si l'instance du ViewModel existe, on le récupère dans le cache
            else
            {
                vm = (TViewModel)Activator.CreateInstance(vmType, viewModelParameters); // Création
                viewModelCache[vmType] = vm; // Ajout dans le Cache
            }

            Window window = null;
            Type windowType = typeof(TWindow);

            if (windowsCache.ContainsKey(windowType)) { window = windowsCache[windowType]; }
            else
            {
                window = (TWindow)Activator.CreateInstance(windowType);
                window.DataContext = viewModel;
            }

            window.Show();
        }
        public static void Show(Window window, Core.ViewModels.BaseNotifyPropertyChanged viewModel)
        {
            window.DataContext = viewModel;
            window.Show();
        }

        public static bool? ShowDialog(Window window, Core.ViewModels.BaseNotifyPropertyChanged viewModel)
        {
            window.DataContext = viewModel;
            return window.ShowDialog();
        }

        /*------------------------*/

        public static void test()
        {
            //Show<MainWindow, TestViewModel>
        }

    }
}
