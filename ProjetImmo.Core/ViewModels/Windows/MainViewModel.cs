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

            this.CurrentPage = (TPage) NavigationService.GetView<ManageEstatesViewModel>(defaultPageType);
            //this.CurrentPage = (TPage) NavigationService.GetView<DisplayStatsViewModel>(defaultPageType);
            #region TestValues_DB

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
            Bob.Password = "passBOB";
            Bob.Admin = false;
            
            Core.DataAccess.AgencyDbContext.Current.Address.Add(BobAd);
            Core.DataAccess.AgencyDbContext.Current.Person.Add(Bob);
            Core.DataAccess.AgencyDbContext.Current.SaveChanges();
            
            //génération d'estates
            /*
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

            SaleTransaction T1 = new SaleTransaction();
            RentalTransaction T2 = new RentalTransaction();

            T1.Title = "VenteT1";
            T1.Description = "Ceci est une vente";
            T1.CreationDate = new DateTime();
            T1.TransactionDate = null;
            T1.Price = 0;
            T1.Fees = 0;

            T2.Title = "LocationT2";
            T2.Description = "Ceci est une location";
            T2.CreationDate = new DateTime();
            T2.TransactionDate = null;
            T2.Price = 0;
            T2.Fees = 0;
            T2.Furnished = false;

            Core.DataAccess.AgencyDbContext.Current.Address.Add(BobAd);
            Core.DataAccess.AgencyDbContext.Current.Person.Add(Bob);
            Core.DataAccess.AgencyDbContext.Current.Transaction.Add(T1);
            Core.DataAccess.AgencyDbContext.Current.Transaction.Add(T2);
            Core.DataAccess.AgencyDbContext.Current.Estate.Add(est1);
            Core.DataAccess.AgencyDbContext.Current.Estate.Add(est2);

            //est1.Transactions.Add(T1);
            //est2.Transactions.Add(T2);
            Core.DataAccess.AgencyDbContext.Current.SaveChanges();
            */
            #endregion

        }

        public BaseCommand<Type> OpenNewWindowCommand
        {

            get => new BaseCommand<Type>(/*async*/(type) => { NavigationService.Show<MainViewModel<TPage>>(type); });

        }

        public BaseCommand<object> CloseViewCommand //CloseWindowCommand
        {

            get => new BaseCommand<object>(/*async*/(view) => { NavigationService.Close(view); });

        }

        public BaseCommand<Type> ChangeViewCommand //ChangeWindowCommand
        {

            get => new BaseCommand<Type>(/*async*/(type) => { this.CurrentPage = (TPage) NavigationService.GetView<MainViewModel<TPage>>(type); });
        }

    }
}