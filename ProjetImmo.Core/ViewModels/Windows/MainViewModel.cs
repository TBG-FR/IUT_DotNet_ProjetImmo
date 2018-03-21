using ProjetImmo.Core.Commandes;
using ProjetImmo.Core.Models;
using ProjetImmo.Core.Tools;
using System;
using System.Collections.Generic;
using System.Text;

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

            //génération d'estates 
            Estate est1 = new Estate();

            est1.Surface = 300;
            est1.Type = Models.Enums.EstateType.FLAT;
            est1.RoomsCount = 0;
            est1.AnnualCharges = 35000;
            est1.PropertyTax = 200;
            est1.FloorNumber = 0;
            est1.FloorCount = 0;

            Estate est2 = new Estate();

            est2.Surface = 300;
            est2.Type = Models.Enums.EstateType.HOUSE;
            est2.RoomsCount = 6;
            est2.AnnualCharges = 40000;
            est2.PropertyTax = 1500;
            est2.FloorNumber = 1;
            est2.FloorCount = 3;

            Core.DataAccess.AgencyDbContext.Current.Address.Add(BobAd);
            Core.DataAccess.AgencyDbContext.Current.Person.Add(Bob);
            Core.DataAccess.AgencyDbContext.Current.Estate.Add(est1);
            Core.DataAccess.AgencyDbContext.Current.Estate.Add(est2);
            Core.DataAccess.AgencyDbContext.Current.SaveChanges();

            /* -------------------------------------------------------- */

        }

        public BaseCommand<Type> OpenNewWindowCommand // Démo/Test
        {

            get => new BaseCommand<Type>((type) => { NavigationService.Show<MainViewModel<TPage>>(type); });

        }
        public BaseCommand<Type> OpenCreateEstatePageCommand
        {
            get => new BaseCommand<Type>((pageType) => { this.CurrentPage = (TPage) NavigationService.GetView<CreateEstatesViewModel>(pageType); });
        }

    }
}
