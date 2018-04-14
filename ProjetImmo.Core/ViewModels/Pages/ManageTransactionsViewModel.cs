using Microsoft.EntityFrameworkCore;
using ProjetImmo.Core.Commandes;
using ProjetImmo.Core.Models;
using ProjetImmo.Core.Tools;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ProjetImmo.Core.ViewModels
{
    public class ManageTransactionsViewModel : BaseNotifyPropertyChanged
    {
        public ObservableCollection<Estate> Estates
        {
            get { return GetProperty<ObservableCollection<Estate>>(); }
            set { if (SetProperty(value)) { SetProperty(value); } }
        }

        public ManageTransactionsViewModel() {
            Estates = new ObservableCollection<Estate>(DataAccess.AgencyDbContext.Current.Estate.Include(e => e.Address).ToArray());
        }

        public Estate SelectedItem
        {
            get { return GetProperty<Estate>(); }
            set { if (SetProperty(value)) { SetProperty(value); } }
        }

        public String selectedFilter
        {
            get { return GetProperty<String>(); }
            set { if (SetProperty(value)) { SetProperty(value); } }
        }

        public BaseCommand<Type> OpenEditEstateWindowCommand
        {

            get => new BaseCommand<Type>((type) =>
            {
                NavigationService.ShowDialog<UpsertEstateViewModel>(type);
                Estates = new ObservableCollection<Estate>(DataAccess.AgencyDbContext.Current.Estate.Include(e => e.Address).ToArray());
            });

        }

        public BaseCommand<Type> DeleteSelectedEstateCommand
        {

            get => new BaseCommand<Type>(async (type) =>
            {
                if (SelectedItem != null)
                {
                    DataAccess.AgencyDbContext.Current.Remove(SelectedItem);
                    DataAccess.AgencyDbContext.Current.SaveChangesAsync();
                    Estates = new ObservableCollection<Estate>(DataAccess.AgencyDbContext.Current.Estate.Include(e => e.Address).ToArray());
                }
                else
                {
                    MessageBox.Show("Veuillez selectionner un item", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            });

        }

        public BaseCommand<Type> ModifySelectedEstateCommand
        {

            get => new BaseCommand<Type>((type) =>
            {
                if (SelectedItem != null)
                {
                    NavigationService.ShowDialog<UpsertEstateViewModel>(type, SelectedItem);

                    //executé au retour sur la fentre ManageEstate
                    Estates = new ObservableCollection<Estate>(DataAccess.AgencyDbContext.Current.Estate.Include(e => e.Address).ToArray());
                }
                else
                {
                    MessageBox.Show("Veuillez selectionner un item", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            });

        }

        public BaseCommand<Type> OpenSearchFilterWindowCommand
        {

            get => new BaseCommand<Type>(/*async*/(type) => { NavigationService.ShowDialog<SearchFilterViewModel>(type); });
            
        }

    }
}
