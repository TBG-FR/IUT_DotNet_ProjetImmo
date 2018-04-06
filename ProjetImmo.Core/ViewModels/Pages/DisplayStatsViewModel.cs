using Microsoft.EntityFrameworkCore;
using ProjetImmo.Core.Models;
using ProjetImmo.Core.Models.Enums;
using ProjetImmo.Core.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using LiveCharts;
using LiveCharts.Wpf;
using System.Globalization;
using System.Reflection;

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

        public Dictionary<PeriodType, SeriesCollection> SalesChartValues
        {
            get { return GetProperty<Dictionary<PeriodType, SeriesCollection>>(); }
            set { SetProperty<Dictionary<PeriodType, SeriesCollection>>(value); }
        }
        public Dictionary<PeriodType, SeriesCollection> RentalsChartValues
        {
            get { return GetProperty<Dictionary<PeriodType, SeriesCollection>>(); }
            set { SetProperty<Dictionary<PeriodType, SeriesCollection>>(value); }
        }

        public SeriesCollection TEST
        {
            get { return GetProperty<SeriesCollection>(); }
            set { SetProperty<SeriesCollection>(value); }
        }

        public DisplayStatsViewModel()
        {
            //Assembly.LoadWithPartialName("PresentationFramework");
            //Assembly.Load(new AssemblyName("PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"));

            CurrentSales = new ObservableCollection<Estate>();
            CurrentRentals = new ObservableCollection<Estate>();

            CurrentSalesDetails = new Dictionary<EstateType, int>
            {
                { EstateType.HOUSE, 0 },
                { EstateType.FLAT, 0 },
                { EstateType.COMMERCIAL, 0 },
                { EstateType.GARAGE, 0 },
                { EstateType.FIELD, 0 }
            };
            CurrentRentalsDetails = new Dictionary<EstateType, int>
            {
                { EstateType.HOUSE, 0 },
                { EstateType.FLAT, 0 },
                { EstateType.COMMERCIAL, 0 },
                { EstateType.GARAGE, 0 },
                { EstateType.FIELD, 0 }
            };

            SalesChartValues = new Dictionary<PeriodType, SeriesCollection>
            {
                { PeriodType.ALL, new SeriesCollection() },
                { PeriodType.YEAR, new SeriesCollection() },
                { PeriodType.MONTH, new SeriesCollection() },
                { PeriodType.WEEK, new SeriesCollection() },
                { PeriodType.DAY, new SeriesCollection() }
            };

            RentalsChartValues = new Dictionary<PeriodType, SeriesCollection>
            {
                { PeriodType.ALL, new SeriesCollection() },
                { PeriodType.YEAR, new SeriesCollection() },
                { PeriodType.MONTH, new SeriesCollection() },
                { PeriodType.WEEK, new SeriesCollection() },
                { PeriodType.DAY, new SeriesCollection() }
            };

            refreshStats();
            //refreshGraphs();

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

            //CurrentSales.Where((e) => e.GetType().Equals(EstateType.HOUSE)).Count();

        }

        public void refreshStats()
        {

            #region CurrentEstates : Get all Estates from the Database

            ObservableCollection<Estate> CurrentEstates = 
                new ObservableCollection<Estate>(DataAccess.AgencyDbContext.Current.Estate
                    .Where((e) => e.Transactions.Count() != 0)/*
                    .Where((e) => e.Transactions.Last().TransactionDate.Equals())*/
                    .Include((e) => e.Transactions).ToArray());

            //Estates = new ObservableCollection<Estate>(DataAccess.AgencyDbContext.Current.Estate.Include(e => e.Address).ToArray());

            #endregion

            #region CurrentRentals/CurrentSales : Extract from CurrentEstates

            foreach (Estate e in CurrentEstates) {

                if(e.Transactions.Last().GetType() == typeof(RentalTransaction)) { CurrentRentals.Add(e);  }
                else if(e.Transactions.Last().GetType() == typeof(SaleTransaction)) { CurrentSales.Add(e);  }
                else { /* TODO : Ajouter dans l'un des deux ? Ajouter un message log ? */ }

            }

            #endregion

            #region CurrentRentalsDetails : Reset values and re-calculate them

            CurrentRentalsDetails.ResetValues();

            foreach (Estate e in CurrentRentals)
            {
                switch(e.Type)
                {
                    case EstateType.HOUSE:
                        //CurrentRentalsDetails[EstateType.HOUSE] += 1; 
                        CurrentRentalsDetails[EstateType.HOUSE] = 120; // TEMP
                        break;

                    case EstateType.FLAT:
                        //CurrentRentalsDetails[EstateType.FLAT] += 1;
                        CurrentRentalsDetails[EstateType.FLAT] = 21; // TEMP
                        break;

                    case EstateType.COMMERCIAL:
                        CurrentRentalsDetails[EstateType.COMMERCIAL] += 1;
                        break;

                    case EstateType.GARAGE:
                        CurrentRentalsDetails[EstateType.GARAGE] += 1;
                        break;

                    case EstateType.FIELD:
                        CurrentRentalsDetails[EstateType.FIELD] += 1;
                        break;
                }
            }

            #endregion

            #region CurrentSalesDetails : Reset values and re-calculate them

            CurrentSalesDetails.ResetValues();

            foreach (Estate e in CurrentSales)
            {
                switch (e.Type)
                {
                    case EstateType.HOUSE:
                        //CurrentSalesDetails[EstateType.HOUSE] += 1;
                        CurrentSalesDetails[EstateType.HOUSE] = 170; // TEMP
                        break;

                    case EstateType.FLAT:
                        //CurrentSalesDetails[EstateType.FLAT] += 1;
                        CurrentSalesDetails[EstateType.FLAT] = 71; // TEMP
                        break;

                    case EstateType.COMMERCIAL:
                        CurrentSalesDetails[EstateType.COMMERCIAL] += 1;
                        break;

                    case EstateType.GARAGE:
                        CurrentSalesDetails[EstateType.GARAGE] += 1;
                        break;

                    case EstateType.FIELD:
                        CurrentSalesDetails[EstateType.FIELD] += 1;
                        break;
                }
            }

            #endregion
            /*
            SalesChartValues.ResetValues();
            SalesChartValues[PeriodType.ALL] = new SeriesCollection(generateChartValues(PeriodType.ALL, typeof(SaleTransaction)));
            SalesChartValues[PeriodType.YEAR] = new SeriesCollection(generateChartValues(PeriodType.YEAR, typeof(SaleTransaction)));
            SalesChartValues[PeriodType.MONTH] = new SeriesCollection(generateChartValues(PeriodType.MONTH, typeof(SaleTransaction)));
            SalesChartValues[PeriodType.WEEK] = new SeriesCollection(generateChartValues(PeriodType.WEEK, typeof(SaleTransaction)));
            SalesChartValues[PeriodType.DAY] = new SeriesCollection(generateChartValues(PeriodType.DAY, typeof(SaleTransaction)));

            RentalsChartValues.ResetValues();
            RentalsChartValues[PeriodType.ALL] = new SeriesCollection(generateChartValues(PeriodType.ALL, typeof(RentalTransaction)));
            RentalsChartValues[PeriodType.YEAR] = new SeriesCollection(generateChartValues(PeriodType.YEAR, typeof(RentalTransaction)));
            RentalsChartValues[PeriodType.MONTH] = new SeriesCollection(generateChartValues(PeriodType.MONTH, typeof(RentalTransaction)));
            RentalsChartValues[PeriodType.WEEK] = new SeriesCollection(generateChartValues(PeriodType.WEEK, typeof(RentalTransaction)));
            RentalsChartValues[PeriodType.DAY] = new SeriesCollection(generateChartValues(PeriodType.DAY, typeof(RentalTransaction)));
            */
            CurrentSalesDetails.Count();

        }

        public SeriesCollection generateChartValues(PeriodType period, Type transactionType)
        {

            // Data initialization
            LineSeries ligValues = new LineSeries();
            ColumnSeries colValues = new ColumnSeries();

            // Data generation
            if(transactionType == typeof(SaleTransaction))
            {

                switch (period)
                {

                    case PeriodType.ALL:
                        // TODO
                        return new SeriesCollection();
                        break;

                    case PeriodType.YEAR:

                        // Chart Columns : Months of a Year
                        //colValues.Values = new ChartValues<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                        colValues.Values = new ChartValues<string> { CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(1),
                                                                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(2),
                                                                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(3),
                                                                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(4),
                                                                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(5),
                                                                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(6),
                                                                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(7),
                                                                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(8),
                                                                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(9),
                                                                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(10),
                                                                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(11),
                                                                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(12)};

                        // Data : SaleTransactions of the 
                        var TEMP = DataAccess.AgencyDbContext.Current.SaleTransaction.ToArray();

                        var a = DataAccess.AgencyDbContext.Current.SaleTransaction.Where((t) => t.TransactionDate.HasValue && t.TransactionDate.Value.Year == DateTime.Now.Year).ToArray();
                        ObservableCollection<SaleTransaction> matchingTransactions = new ObservableCollection<SaleTransaction>(a);
                        

                        // Data : Sales per Month
                        int[] values = new int[12];
                        foreach(SaleTransaction t in matchingTransactions)
                        {
                            int m = t.TransactionDate.Value.Month;
                            values[m-1]++;
                        }

                        // Chart Lines : Sales per Month
                        //ligValues.Values = new ChartValues<> { 0, 0, 0 };
                        ligValues.Values = new ChartValues<int> { values[0], values[1], values[2],
                                                                    values[3], values[4], values[5],
                                                                    values[6], values[7], values[8],
                                                                    values[9], values[10], values[11]};

                        // Return the complete Dataset
                        return new SeriesCollection { ligValues, colValues };
                        break;

                    case PeriodType.MONTH:
                        // TODO
                        return new SeriesCollection();
                        break;

                    case PeriodType.WEEK:
                        // TODO
                        return new SeriesCollection();
                        break;

                    case PeriodType.DAY:
                        // TODO
                        return new SeriesCollection();
                        break;

                    default:
                        // TODO
                        return null;
                        break;

                }

            }
            else if (transactionType == typeof(RentalTransaction))
            {

                switch (period)
                {

                    case PeriodType.ALL:
                        // TODO
                        return new SeriesCollection();
                        break;

                    case PeriodType.YEAR:

                        // Chart Columns : Months of a Year
                        //colValues.Values = new ChartValues<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                        colValues.Values = new ChartValues<string> { CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(1),
                                                                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(2),
                                                                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(3),
                                                                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(4),
                                                                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(5),
                                                                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(6),
                                                                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(7),
                                                                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(8),
                                                                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(9),
                                                                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(10),
                                                                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(11),
                                                                    CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(12)};

                        // Data : SaleTransactions of the Year
                        var a = DataAccess.AgencyDbContext.Current.RentalTransaction.Where((t) => t.TransactionDate.HasValue && t.TransactionDate.Value.Year == DateTime.Now.Year).ToArray();
                        ObservableCollection<RentalTransaction> matchingTransactions = new ObservableCollection<RentalTransaction>(a);


                        // Data : Sales per Month
                        int[] values = new int[12];
                        foreach (RentalTransaction t in matchingTransactions)
                        {
                            int m = t.TransactionDate.Value.Month;
                            values[m - 1]++;
                        }

                        // Chart Lines : Sales per Month
                        //ligValues.Values = new ChartValues<> { 0, 0, 0 };
                        ligValues.Values = new ChartValues<int> { values[0], values[1], values[2],
                                                                    values[3], values[4], values[5],
                                                                    values[6], values[7], values[8],
                                                                    values[9], values[10], values[11]};

                        // Return the complete Dataset

                        var tobby = new SeriesCollection { ligValues, colValues };
                        return tobby;

                        break;

                    case PeriodType.MONTH:
                        // TODO
                        return new SeriesCollection();
                        break;

                    case PeriodType.WEEK:
                        // TODO
                        return new SeriesCollection();
                        break;

                    case PeriodType.DAY:
                        // TODO
                        return new SeriesCollection();
                        break;

                    default:
                        // TODO
                        return null;
                        break;

                }

            }
            else { return null; /* TODO : return special values ? Add error message/log ? */ }

        }

    }
}
