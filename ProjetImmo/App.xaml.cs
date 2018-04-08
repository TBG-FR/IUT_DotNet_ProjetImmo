using ProjetImmo.WPF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ProjetImmo.Core.ViewModels;
using System.Windows.Controls;
using ProjetImmo.Core;
using ProjetImmo.Core.Tools;
using ProjetImmo.WPF.Pages;

namespace ProjetImmo.WPF
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected async override void OnStartup(StartupEventArgs e)
        {

            base.OnStartup(e);
            await Core.DataAccess.AgencyDbContext.Initialize();

            MainWindow = NavigationService.GetView<MainWindow, MainViewModel<Page>>(typeof(ManageEstatesPage));
            //MainWindow = NavigationService.GetView<MainWindow, MainViewModel<Page>>(typeof(DisplayStatsPage));
            MainWindow.Show();

        }



    }
    
}
