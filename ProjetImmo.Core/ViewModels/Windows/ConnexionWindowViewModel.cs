using Microsoft.EntityFrameworkCore;
using ProjetImmo.Core.Commandes;
using ProjetImmo.Core.Models;
using ProjetImmo.Core.Tools;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ProjetImmo.Core.ViewModels
{
    public class ConnexionWindowViewModel : BaseNotifyPropertyChanged
    {

        public BaseCommand<Type/*, Type*/> OpenNewWindowCommand
        {

            //get => new BaseCommand<Type, Type>((tView, tViewModel) => { NavigationService.Show<MainViewModel<Page>>(tView, tViewModel); });
            get => new BaseCommand<Type>(/*async*/(type) => { NavigationService.Show<SearchFilterViewModel>(type); });

        }

        /*
        public BaseCommand<Type> OpenNewWindowCommand
        {

            get => new BaseCommand<Type>((type) => { NavigationService.Show<MainViewModel<TPage>>(type); });

        }*/
        /*
        public BaseCommand<Type> OpenMainWindowCommand
        {

            get => new BaseCommand<Type>((type) => { NavigationService.Show<MainViewModel<Page>>(type); });

        }
        */
        /*
        public BaseCommand<Type> OpenNewWindowCommand
        {

            get => new BaseCommand<Type>((type) => { NavigationService.Show<MainViewModel<TPage>>(type); });

        }
        */
        /*
        public BaseCommand<Type> OpenMainWindowCommand
        {

            get => new BaseCommand<Type>((tPage) => {  NavigationService.Show<MainViewModel<tPage>>(tPage); });

        }*/

        //MainWindow = NavigationService.GetView<MainWindow, MainViewModel<Page>>(typeof(DisplayStatsPage));
        /*
        public BaseCommand<Type> OpenNewWindowCommand
        {

            get => new BaseCommand<Type>((type) => { NavigationService.Show<MainViewModel<TPage>>(type); });

        }
        */

        // Ouvrir nouvelle fenetre
        /*
        public BaseCommand<Type> OpenNewWindowCommand
        {

            get => new BaseCommand<Type>((type) => { NavigationService.Show<MainWindow, MainViewModel<Page>>(typeof(DisplayStatsPage)); });

        }

        // Ouvrir la MainW

        MainWindow = NavigationService.GetView<MainWindow, MainViewModel<Page>>(typeof(DisplayStatsPage));
            */
    }

}
