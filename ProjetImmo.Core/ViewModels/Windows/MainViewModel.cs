using ProjetImmo.Core.Commandes;
using ProjetImmo.Core.Models;
using ProjetImmo.Core.Tools;
using System;

namespace ProjetImmo.Core.ViewModels
{
    public class MainViewModel<TPage> : BaseNotifyPropertyChanged
    {

        public override void refresh()
        {
            // Do nothing
        }

        public object CurrentPage
        {
            get { return GetProperty<object>(); }
            set { SetProperty<object>(value); }
        }

        public MainViewModel(Type defaultPageType)
        {

            this.CurrentPage = (TPage) NavigationService.GetView<DisplayStatsViewModel>(defaultPageType);

        }

        public BaseCommand<Type> OpenNewWindowCommand
        {

            get => new BaseCommand<Type>(/*async*/(type) => { NavigationService.Show<SearchFilterViewModel>(type); });

        }

        public BaseCommand<object> CloseViewCommand //CloseWindowCommand
        {

            get => new BaseCommand<object>(/*async*/(view) => { NavigationService.Close(view); });

        }

        public BaseCommand<Type, Type> ChangeViewCommand //ChangeWindowCommand
        {

            get => new BaseCommand<Type, Type>(/*async*/(tView, tViewModel) => { this.CurrentPage = NavigationService.GetView(tView, tViewModel); });

        }

    }
}