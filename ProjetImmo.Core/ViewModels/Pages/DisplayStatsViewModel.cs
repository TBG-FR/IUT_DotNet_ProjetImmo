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

        public override void refresh()
        {
            // NE FONCTIONNE PAS (Toutes les données reviennent à 0 sur l'affichage... Regarder du côté WPF)
            //refreshStats();
        }

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

        #region Properties - Calculated data to display

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

        public Dictionary<PeriodType, Pair<List<string>, SeriesCollection>> SalesChartValues
        {
            get { return GetProperty<Dictionary<PeriodType, Pair<List<string>, SeriesCollection>>>(); }
            set { SetProperty<Dictionary<PeriodType, Pair<List<string>, SeriesCollection>>>(value); }
        }
        public Dictionary<PeriodType, Pair<List<string>, SeriesCollection>> RentalsChartValues
        {
            get { return GetProperty<Dictionary<PeriodType, Pair<List<string>, SeriesCollection>>>(); }
            set { SetProperty<Dictionary<PeriodType, Pair<List<string>, SeriesCollection>>>(value); }
        }

        public Dictionary<PeriodType, Pair<int, int>> GridValues
        {
            get { return GetProperty<Dictionary<PeriodType, Pair<int, int>>>(); }
            set { SetProperty<Dictionary<PeriodType, Pair<int, int>>>(value); }
        }

        #endregion

        public DisplayStatsViewModel()
        {

            // Première Initilialisation des données
            CurrentSales = new ObservableCollection<Estate>();
            CurrentRentals = new ObservableCollection<Estate>();

            #region Constructeur - Initialisation des données

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

            SalesChartValues = new Dictionary<PeriodType, Pair<List<string>, SeriesCollection>>
            {
                { PeriodType.ALL, new Pair<List<string>, SeriesCollection>(new List<string>(), new SeriesCollection()) },
                { PeriodType.YEAR, new Pair<List<string>, SeriesCollection>(new List<string>(), new SeriesCollection()) },
                { PeriodType.MONTH, new Pair<List<string>, SeriesCollection>(new List<string>(), new SeriesCollection()) },
                { PeriodType.WEEK, new Pair<List<string>, SeriesCollection>(new List<string>(), new SeriesCollection()) },
                { PeriodType.DAY, new Pair<List<string>, SeriesCollection>(new List<string>(), new SeriesCollection()) }
            };
            RentalsChartValues = new Dictionary<PeriodType, Pair<List<string>, SeriesCollection>>
            {
                { PeriodType.ALL, new Pair<List<string>, SeriesCollection>(new List<string>(), new SeriesCollection()) },
                { PeriodType.YEAR, new Pair<List<string>, SeriesCollection>(new List<string>(), new SeriesCollection()) },
                { PeriodType.MONTH, new Pair<List<string>, SeriesCollection>(new List<string>(), new SeriesCollection()) },
                { PeriodType.WEEK, new Pair<List<string>, SeriesCollection>(new List<string>(), new SeriesCollection()) },
                { PeriodType.DAY, new Pair<List<string>, SeriesCollection>(new List<string>(), new SeriesCollection()) }
            };

            GridValues = new Dictionary<PeriodType, Pair<int, int>>
            {
                // Period   -       Sales   -   Rentals
                { PeriodType.YEAR, new Pair<int, int>(0,0) },
                { PeriodType.MONTH, new Pair<int, int>(0,0) },
                { PeriodType.DAY, new Pair<int, int>(0,0) }
            };

            #endregion

            // Premier rafraichissement des données
            refreshStats();

        }

        public void refreshStats()
        {

            #region CurrentEstats : Get all Estates from the Database

            ObservableCollection<Estate> CurrentEstatesTemp = 
                new ObservableCollection<Estate>(DataAccess.AgencyDbContext.Current.Estate
                    .Where((e) => e.Transactions.Count() != 0)
                    //.Where((e) => e.Transactions.Last().Equals(null))
                    //.Where((e) => e.Transactions.Last().HasValue.Equals(false))
                    //...
                    .Include((e) => e.Transactions).ToArray());

            // Replaces the "Where" instruction
            ObservableCollection<Estate> CurrentEstates = new ObservableCollection<Estate>();
            foreach (Estate e in CurrentEstatesTemp) { if(!(e.Transactions.Last().TransactionDate.HasValue)) { CurrentEstates.Add(e); } }

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
                        CurrentRentalsDetails[EstateType.HOUSE] += 1;
                        break;

                    case EstateType.FLAT:
                        CurrentRentalsDetails[EstateType.FLAT] += 1;
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
                        CurrentSalesDetails[EstateType.HOUSE] += 1;
                        break;

                    case EstateType.FLAT:
                        CurrentSalesDetails[EstateType.FLAT] += 1;
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
            
            #region GridValues : Reset values

            // Reset Numeric Values (they will be updated with SalesChartValues & RentalsChartValues)
            //GridValues.ResetValues();
            GridValues = new Dictionary<PeriodType, Pair<int, int>>
            {
                // Period   -       Sales   -   Rentals
                { PeriodType.YEAR, new Pair<int, int>(0,0) },
                { PeriodType.MONTH, new Pair<int, int>(0,0) },
                { PeriodType.DAY, new Pair<int, int>(0,0) }
            };

            #endregion

            #region SalesChartValues : Reset values and re-calculate them

            SalesChartValues.ResetValues();
            SalesChartValues[PeriodType.ALL] = generateChartValues(PeriodType.ALL, typeof(SaleTransaction));
            SalesChartValues[PeriodType.YEAR] = generateChartValues(PeriodType.YEAR, typeof(SaleTransaction));
            SalesChartValues[PeriodType.MONTH] = generateChartValues(PeriodType.MONTH, typeof(SaleTransaction));
            SalesChartValues[PeriodType.WEEK] = generateChartValues(PeriodType.WEEK, typeof(SaleTransaction));
            SalesChartValues[PeriodType.DAY] = generateChartValues(PeriodType.DAY, typeof(SaleTransaction));

            #endregion

            #region RentalsChartValues : Reset values and re-calculate them

            RentalsChartValues.ResetValues();
            RentalsChartValues[PeriodType.ALL] = generateChartValues(PeriodType.ALL, typeof(RentalTransaction));
            RentalsChartValues[PeriodType.YEAR] = generateChartValues(PeriodType.YEAR, typeof(RentalTransaction));
            RentalsChartValues[PeriodType.MONTH] = generateChartValues(PeriodType.MONTH, typeof(RentalTransaction));
            RentalsChartValues[PeriodType.WEEK] = generateChartValues(PeriodType.WEEK, typeof(RentalTransaction));
            RentalsChartValues[PeriodType.DAY] = generateChartValues(PeriodType.DAY, typeof(RentalTransaction));

            #endregion

            // At this point, all data should be loaded into the different properties

        }

        public Pair<List<string>, SeriesCollection> generateChartValues(PeriodType period, Type transactionType)
        {

            // Data initialization
            LineSeries allvalues = new LineSeries(); List<string> labels = new List<string>();
            ObservableCollection<SaleTransaction> matchingSaleTransactions = new ObservableCollection<SaleTransaction>();
            ObservableCollection<RentalTransaction> matchingRentalTransactions = new ObservableCollection<RentalTransaction>();
            int[] values; ChartValues<int> chartvalues;

            // Data generation
            if (transactionType == typeof(SaleTransaction))
            {

                /*
                ===== Actions on  =====
                GridValues[PeriodType.YEAR].First
                GridValues[PeriodType.MONTH].First
                GridValues[PeriodType.DAY].First
                */

                switch (period)
                {
                    #region case PeriodType.ALL:
                    case PeriodType.ALL:

                        int years = DateTime.Now.Year - 2015;

                        // Chart Columns : Years since 2015
                        for (int i = years; i >= 0; i--) { labels.Add((DateTime.Now.Year - i).ToString()); }

                        // Data : SaleTransactions of all time (since 2015), per Year
                        matchingSaleTransactions = new ObservableCollection<SaleTransaction>(DataAccess.AgencyDbContext.Current.SaleTransaction.Where((t) => t.TransactionDate.HasValue).ToArray());

                        // Data : Sales per Year
                        values = new int[years + 1];
                        for (int i = years; i >= 0; i--) { values[i] = 0; }
                        foreach (SaleTransaction t in matchingSaleTransactions) { int m = t.TransactionDate.Value.Year; values[m - 2015]++; }

                        // Chart Lines : Sales per Year
                        chartvalues = new ChartValues<int>();
                        for (int i = 0; i <= years; i++) { chartvalues.Add(values[i]); }

                        allvalues.Values = chartvalues;
                        allvalues.Name = "Ventes";

                        // Update the corresponding GridValues field
                        GridValues[PeriodType.YEAR].First = values[years];

                        // Return the complete Dataset
                        // PeriodType.ALL, new Pair<List<string>, SeriesCollection>(new List<string>(), new SeriesCollection()) },
                        return new Pair<List<string>, SeriesCollection>(labels, new SeriesCollection { allvalues });
                        break;
                    #endregion

                    #region case PeriodType.YEAR:
                    case PeriodType.YEAR:

                        // Chart Columns : Months of a Year
                        //colValues.Values = new ChartValues<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                        
                        //colValues.Values = new ChartValues<string> { CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(1), ... };
                        for (int i = 0; i < 12; i++) { labels.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i + 1)); }

                        // Data : SaleTransactions of the Year, per Month
                        matchingSaleTransactions = new ObservableCollection<SaleTransaction>(DataAccess.AgencyDbContext.Current.SaleTransaction.Where((t) => t.TransactionDate.HasValue && t.TransactionDate.Value.Year == DateTime.Now.Year).ToArray());

                        // Data : Sales per Month
                        values = new int[12];
                        foreach (SaleTransaction t in matchingSaleTransactions) { int m = t.TransactionDate.Value.Month; values[m - 1]++; }

                        // Chart Lines : Sales per Month
                        //ligValues.Values = new ChartValues<> { 0, 0, 0 };
                        chartvalues = new ChartValues<int>();
                        for (int i = 0; i < 12; i++) { chartvalues.Add(values[i]); }

                        allvalues.Values = chartvalues;
                        allvalues.Name = "Ventes";

                        // Update the corresponding GridValues field
                        GridValues[PeriodType.MONTH].First = values[DateTime.Now.Month - 1];

                        // Return the complete Dataset
                        // PeriodType.ALL, new Pair<List<string>, SeriesCollection>(new List<string>(), new SeriesCollection()) },
                        return new Pair<List<string>, SeriesCollection>(labels, new SeriesCollection { allvalues });
                        break;
                    #endregion

                    #region case PeriodType.MONTH:
                    case PeriodType.MONTH:

                        // Chart Columns : Days of the Month
                        int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                        for (int i = 0; i < days; i++) { labels.Add(i.ToString()); }

                        // Data : SaleTransactions of the Month, per Day
                        matchingSaleTransactions = new ObservableCollection<SaleTransaction>(DataAccess.AgencyDbContext.Current.SaleTransaction.Where((t) => t.TransactionDate.HasValue && t.TransactionDate.Value.Month == DateTime.Now.Month).ToArray());

                        // Data : Sales per Month
                        values = new int[days];
                        foreach (SaleTransaction t in matchingSaleTransactions) { int m = t.TransactionDate.Value.Day; values[m - 1]++; }

                        // Chart Lines : Sales per Day
                        chartvalues = new ChartValues<int>();
                        for (int i = 0; i < days; i++) { chartvalues.Add(values[i]); }

                        allvalues.Values = chartvalues;
                        allvalues.Name = "Ventes";

                        // Return the complete Dataset
                        // PeriodType.ALL, new Pair<List<string>, SeriesCollection>(new List<string>(), new SeriesCollection()) },
                        return new Pair<List<string>, SeriesCollection>(labels, new SeriesCollection { allvalues });
                        break;
                    #endregion

                    #region case PeriodType.WEEK:
                    case PeriodType.WEEK:

                        // Chart Columns : Days of the Week
                        for (int i = 0; i < 7; i++) { labels.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetDayName((DayOfWeek)i)); }

                        // Data : SaleTransactions of the Month, per Day
                        matchingSaleTransactions = new ObservableCollection<SaleTransaction>(DataAccess.AgencyDbContext.Current.SaleTransaction.Where((t) => t.TransactionDate.HasValue && (t.TransactionDate.Value.DayOfYear == DateTime.Now.DayOfYear || t.TransactionDate.Value.DayOfYear <= DateTime.Now.DayOfYear - 6)).ToArray());

                        // Data : Sales per Month
                        values = new int[7];
                        foreach (SaleTransaction t in matchingSaleTransactions) { int m = (int)t.TransactionDate.Value.DayOfWeek; values[m]++; }

                        // Chart Lines : Sales per Month
                        chartvalues = new ChartValues<int>();
                        for (int i = 0; i < 7; i++) { chartvalues.Add(values[i]); }

                        allvalues.Values = chartvalues;
                        allvalues.Name = "Ventes";

                        // Update the corresponding GridValues field
                        GridValues[PeriodType.DAY].First = values[(int) DateTime.Now.DayOfWeek];

                        // Return the complete Dataset
                        // PeriodType.ALL, new Pair<List<string>, SeriesCollection>(new List<string>(), new SeriesCollection()) },
                        return new Pair<List<string>, SeriesCollection>(labels, new SeriesCollection { allvalues });
                        break;
                    #endregion

                    #region case PeriodType.DAY:
                    case PeriodType.DAY:

                        // Chart Columns : Hours of the Day
                        for (int i = 0; i < 24; i++) { labels.Add(i + "H"); }

                        // Data : SaleTransactions of the Month, per Day
                        matchingSaleTransactions = new ObservableCollection<SaleTransaction>(DataAccess.AgencyDbContext.Current.SaleTransaction.Where((t) => t.TransactionDate.HasValue && t.TransactionDate.Value.Day == DateTime.Now.Day).ToArray());

                        // Data : Sales per Month
                        values = new int[24];
                        foreach (SaleTransaction t in matchingSaleTransactions) { int m = t.TransactionDate.Value.Hour; values[m - 1]++; }

                        // Chart Lines : Sales per Month
                        chartvalues = new ChartValues<int>();
                        for (int i = 0; i < 24; i++) { chartvalues.Add(values[i]); }

                        allvalues.Values = chartvalues;
                        allvalues.Name = "Ventes";

                        // Return the complete Dataset
                        // PeriodType.ALL, new Pair<List<string>, SeriesCollection>(new List<string>(), new SeriesCollection()) },
                        return new Pair<List<string>, SeriesCollection>(labels, new SeriesCollection { allvalues });
                        break;
                    #endregion

                    default:
                        // TODO ?
                        return null;
                        break;

                }

            }
            else if (transactionType == typeof(RentalTransaction))
            {

                /*
                ===== Actions on =====
                GridValues[PeriodType.YEAR].Second
                GridValues[PeriodType.MONTH].Second
                GridValues[PeriodType.DAY].Second
                */

                switch (period)
                {
                    #region case PeriodType.ALL:
                    case PeriodType.ALL:

                        int years = DateTime.Now.Year - 2015;

                        // Chart Columns : Years since 2015
                        for (int i = years; i >= 0; i--) { labels.Add((DateTime.Now.Year - i).ToString()); }

                        // Data : RentalTransactions of all time (since 2015), per Year
                        matchingRentalTransactions = new ObservableCollection<RentalTransaction>(DataAccess.AgencyDbContext.Current.RentalTransaction.Where((t) => t.TransactionDate.HasValue).ToArray());

                        // Data : Sales per Year
                        values = new int[years + 1];
                        for (int i = years; i >= 0; i--) { values[i] = 0; }
                        foreach (RentalTransaction t in matchingRentalTransactions) { int m = t.TransactionDate.Value.Year; values[m - 2015]++; }

                        // Chart Lines : Sales per Year
                        chartvalues = new ChartValues<int>();
                        for (int i = 0; i <= years; i++) { chartvalues.Add(values[i]); }

                        allvalues.Values = chartvalues;
                        allvalues.Name = "Ventes";

                        // Update the corresponding GridValues field
                        GridValues[PeriodType.YEAR].First = values[years];

                        // Return the complete Dataset
                        // PeriodType.ALL, new Pair<List<string>, SeriesCollection>(new List<string>(), new SeriesCollection()) },
                        return new Pair<List<string>, SeriesCollection>(labels, new SeriesCollection { allvalues });
                        break;
                    #endregion

                    #region case PeriodType.YEAR:
                    case PeriodType.YEAR:

                        // Chart Columns : Months of a Year
                        //colValues.Values = new ChartValues<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
                        //colValues.Values = new ChartValues<string> { CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(1), ... };
                        for (int i = 0; i < 12; i++) { labels.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i+1)); }

                        // Data : SaleTransactions of the Year, per Month
                        ObservableCollection<RentalTransaction> matchingTransactions = new ObservableCollection<RentalTransaction>(DataAccess.AgencyDbContext.Current.RentalTransaction.Where((t) => t.TransactionDate.HasValue && t.TransactionDate.Value.Year == DateTime.Now.Year).ToArray());

                        // Data : Sales per Month
                        values = new int[12];
                        foreach (RentalTransaction t in matchingRentalTransactions) { int m = t.TransactionDate.Value.Month; values[m - 1]++; }

                        // Chart Lines : Sales per Month
                        //ligValues.Values = new ChartValues<> { 0, 0, 0 };
                        chartvalues = new ChartValues<int>();
                        for (int i = 0; i < 12; i++) { chartvalues.Add(values[i]); }

                        allvalues.Values = chartvalues;
                        allvalues.Name = "Ventes";

                        // Update the corresponding GridValues field
                        GridValues[PeriodType.MONTH].First = values[DateTime.Now.Month - 1];

                        // Return the complete Dataset
                        // PeriodType.ALL, new Pair<List<string>, SeriesCollection>(new List<string>(), new SeriesCollection()) },
                        return new Pair<List<string>, SeriesCollection>(labels, new SeriesCollection { allvalues });
                        break;
                    #endregion

                    #region case PeriodType.MONTH:
                    case PeriodType.MONTH:

                        // Chart Columns : Days of the Month
                        int days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                        for(int i=0; i<days; i++) { labels.Add(i.ToString()); }

                        // Data : SaleTransactions of the Month, per Day
                        matchingRentalTransactions = new ObservableCollection<RentalTransaction>(DataAccess.AgencyDbContext.Current.RentalTransaction.Where((t) => t.TransactionDate.HasValue && t.TransactionDate.Value.Month == DateTime.Now.Month).ToArray());

                        // Data : Sales per Month
                        values = new int[days];
                        foreach (RentalTransaction t in matchingRentalTransactions) { int m = t.TransactionDate.Value.Day; values[m - 1]++; }

                        // Chart Lines : Sales per Day
                        chartvalues = new ChartValues<int>();
                        for (int i = 0; i < days; i++) { chartvalues.Add(values[i]); }

                        allvalues.Values = chartvalues;
                        allvalues.Name = "Ventes";

                        // Return the complete Dataset
                        // PeriodType.ALL, new Pair<List<string>, SeriesCollection>(new List<string>(), new SeriesCollection()) },
                        return new Pair<List<string>, SeriesCollection>(labels, new SeriesCollection { allvalues });
                        break;
                    #endregion

                    #region case PeriodType.WEEK:
                    case PeriodType.WEEK:

                        // Chart Columns : Days of the Week
                        for (int i = 0; i < 7; i++) { labels.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetDayName((DayOfWeek) i)); }

                        // Data : SaleTransactions of the Month, per Day
                        matchingRentalTransactions = new ObservableCollection<RentalTransaction>(DataAccess.AgencyDbContext.Current.RentalTransaction.Where((t) => t.TransactionDate.HasValue && (t.TransactionDate.Value.DayOfYear == DateTime.Now.DayOfYear || t.TransactionDate.Value.DayOfYear <= DateTime.Now.DayOfYear - 6)).ToArray());

                        // Data : Sales per Month
                        values = new int[7];
                        foreach (RentalTransaction t in matchingRentalTransactions) { int m = (int) t.TransactionDate.Value.DayOfWeek; values[m]++; }

                        // Chart Lines : Sales per Month
                        chartvalues = new ChartValues<int>();
                        for (int i = 0; i < 7; i++) { chartvalues.Add(values[i]); }

                        allvalues.Values = chartvalues;
                        allvalues.Name = "Ventes";

                        // Update the corresponding GridValues field
                        GridValues[PeriodType.DAY].Second = values[(int)DateTime.Now.DayOfWeek];

                        // Return the complete Dataset
                        // PeriodType.ALL, new Pair<List<string>, SeriesCollection>(new List<string>(), new SeriesCollection()) },
                        return new Pair<List<string>, SeriesCollection>(labels, new SeriesCollection { allvalues });
                        break;
                    #endregion

                    #region case PeriodType.DAY:
                    case PeriodType.DAY:

                        // Chart Columns : Hours of the Day
                        for (int i = 0; i < 24; i++) { labels.Add(i + "H"); }

                        // Data : SaleTransactions of the Month, per Day
                        matchingRentalTransactions = new ObservableCollection<RentalTransaction>(DataAccess.AgencyDbContext.Current.RentalTransaction.Where((t) => t.TransactionDate.HasValue && t.TransactionDate.Value.Day == DateTime.Now.Day).ToArray());

                        // Data : Sales per Month
                        values = new int[24];
                        foreach (RentalTransaction t in matchingRentalTransactions) { int m = t.TransactionDate.Value.Hour; values[m - 1]++; }

                        // Chart Lines : Sales per Month
                        chartvalues = new ChartValues<int>();
                        for (int i = 0; i < 24; i++) { chartvalues.Add(values[i]); }

                        allvalues.Values = chartvalues;
                        allvalues.Name = "Ventes";

                        // Return the complete Dataset
                        // PeriodType.ALL, new Pair<List<string>, SeriesCollection>(new List<string>(), new SeriesCollection()) },
                        return new Pair<List<string>, SeriesCollection>(labels, new SeriesCollection { allvalues });
                        break;
                    #endregion

                    default:
                        // TODO ?
                        return null;
                        break;

                }

            }
            else { return null; /* TODO : return special values ? Add error message/log ? */ }

        }

    }
}
