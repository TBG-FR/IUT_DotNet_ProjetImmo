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

        public BaseCommand<Type> OpenMainWindowCommand
        {

            get => new BaseCommand<Type>(/*async*/(type) => {
                NavigationService.Show<ConnexionWindowViewModel, MainViewModel<Page>>(type);
            });

        }
    }

}
