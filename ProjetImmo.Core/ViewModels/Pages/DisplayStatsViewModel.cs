using Microsoft.EntityFrameworkCore;
using ProjetImmo.Core.Models;
using ProjetImmo.Core.Models.Enums;
using ProjetImmo.Core.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ProjetImmo.Core.ViewModels
{

    public class DisplayStatsViewModel : BaseNotifyPropertyChanged
    {

        public ObservableCollection<Estate> CurrentSales
        {
            get { return GetProperty<ObservableCollection<Estate>>(); }
            set { SetProperty<ObservableCollection<Estate>>(value); }
        }

        public ObservableCollection<Estate> CurrentRentals
        {
            get { return GetProperty<ObservableCollection<Estate>>(); }
            set { SetProperty<ObservableCollection<Estate>>(value); }
        }

        public Dictionary<EstateType, int> CurrentSalesDetails
        {
            get { return GetProperty<Dictionary<EstateType, int>>(); }
            set { SetProperty<Dictionary<EstateType, int>>(value); }
        }

        public Dictionary<EstateType, int> CurrentRentalsDetails
        {
            get { return GetProperty<Dictionary<EstateType, int>>(); }
            set { SetProperty<Dictionary<EstateType, int>>(value); }
        }

        public DisplayStatsViewModel()
        {
            CurrentSales = new ObservableCollection<Estate>();
            CurrentRentals = new ObservableCollection<Estate>();

            CurrentSalesDetails = new Dictionary<EstateType, int>
            {
                { EstateType.HOUSE, 150 },
                { EstateType.FLAT, 0 },
                { EstateType.COMMERCIAL, 0 },
                { EstateType.GARAGE, 0 },
                { EstateType.FIELD, 0 }
            };

            CurrentRentalsDetails = new Dictionary<EstateType, int>
            {
                { EstateType.HOUSE, 150 },
                { EstateType.FLAT, 0 },
                { EstateType.COMMERCIAL, 0 },
                { EstateType.GARAGE, 0 },
                { EstateType.FIELD, 0 }
            };

            refreshStats();

            //CurrentSalesNumbers.(EstateType.HOUSE, 15);
            //CurrentSalesNumbers.ElementAt(Convert.ChangeType(EstateType.HOUSE, EstateType.HOUSE.GetTypeCode());

            /*
            foreach (Estate e in CurrentSales)
            {

                DateTime? D1 = e.Transactions.Last().TransactionDate;

                if(D1 == null) {

                    String S1 = D1.ToString();
                    Boolean B1 = D1.HasValue;

                    S1.ToUpper();
                }

            }
            */

            /*
            CurrentSales.Add(DataAccess.AgencyDbContext.Current.Estate
                .Where((e) => e.Transactions.Count() != 0)
                .Where((e) => e.Transactions.Last().TransactionDate.Equals(null))
                .Where((e) => e.Transactions.Last().GetType() == typeof(SaleTransaction)).ToArray());

            CurrentRentals.Add(DataAccess.AgencyDbContext.Current.Estate
                .Where((e) => e.Transactions.Count() != 0)
                .Where((e) => e.Transactions.Last().TransactionDate.Equals(null))
                .Where((e) => e.Transactions.Last().GetType() == typeof(RentalTransaction)).ToArray());
            */
            //CurrentSales.Count();
            //CurrentRentals.Count();

            CurrentSales.Where((e) => e.GetType().Equals(EstateType.HOUSE)).Count();

        }

        public void refreshStats()
        {
            ObservableCollection<Estate> CurrentEstates = 
                new ObservableCollection<Estate>(DataAccess.AgencyDbContext.Current.Estate
                    .Where((e) => e.Transactions.Count() != 0)/*
                    .Where((e) => e.Transactions.Last().TransactionDate.Equals())*/
                    .Include((e) => e.Transactions).ToArray());

            //Estates = new ObservableCollection<Estate>(DataAccess.AgencyDbContext.Current.Estate.Include(e => e.Address).ToArray());

            foreach(Estate e in CurrentEstates) {

                if(e.Transactions.Last().GetType() == typeof(RentalTransaction)) { CurrentRentals.Add(e);  }
                else if(e.Transactions.Last().GetType() == typeof(SaleTransaction)) { CurrentSales.Add(e);  }
                else { /* TODO : Ajouter dans l'un des deux ? Ajouter un message log ? */ }

            }
        }

    }
}
