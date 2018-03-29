using ProjetImmo.Core.Commandes;
using ProjetImmo.Core.Models;
using ProjetImmo.Core.Tools;
using System;

namespace ProjetImmo.Core.ViewModels
{
    public class MainViewModel<TPage> : BaseNotifyPropertyChanged
    {

        public TPage CurrentPage
        {
            get { return GetProperty<TPage>(); }

            set { SetProperty<TPage>(value); }
        }

        public MainViewModel(Type defaultPageType)
        {

            this.CurrentPage = (TPage) NavigationService.GetView<BrowseEstatesViewModel>(defaultPageType);

            /* -------------------------------------------------------- */

            Person Bob = new Person();
            Address BobAd = new Address();

            //BobAd.ID = 1;
            BobAd.City = "Bourg";
            BobAd.ZIP = "01000";
            BobAd.PostalAddress = "5 rue des champs";
            BobAd.Longitude = 39.45;
            BobAd.Latitude = 14.18;
            //BobAd.AddressedEstate = null;
            //BobAd.AddressedPerson = Bob;

            //Bob.ID = 1;
            Bob.Type = Core.Models.Enums.PersonType.NATURAL;
            Bob.Firstname = "Bob";
            Bob.Lastname = "Dylan";
            Bob.Address = BobAd;

            Core.DataAccess.AgencyDbContext.Current.Address.Add(BobAd);
            Core.DataAccess.AgencyDbContext.Current.Person.Add(Bob);
            Core.DataAccess.AgencyDbContext.Current.SaveChanges();

            /* -------------------------------------------------------- */

        }

        public BaseCommand<Type> OpenNewWindowCommand
        {

            get => new BaseCommand<Type>((type) => { NavigationService.Show<MainViewModel<TPage>>(type); });

        }

    }
}
