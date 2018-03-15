using ProjetImmo.Core.Commandes;
using ProjetImmo.Core.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetImmo.Core.ViewModels
{
    public class MainViewModel<TPage> : BaseNotifyPropertyChanged
    {

        public TPage CurrentPage { get; set; }

        public MainViewModel(Type defaultPageType)
        {

            this.CurrentPage = (TPage) NavigationService.GetView<DisplayStatsViewModel>(defaultPageType);

        }

        public BaseCommand<Type> OpenNewWindowCommand
        {

            get => new BaseCommand<Type>((type) => { NavigationService.Show<MainViewModel<TPage>>(type); });

        }

        /*
         *
         string Titre {get; set; }
         RECHERCHE BDD : [...].where(x => (titre != "" && x.Title == titre)&&(price != 0 && x.Price == price)
         *
         */

    }
}
