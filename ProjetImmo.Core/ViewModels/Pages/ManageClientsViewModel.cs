using Microsoft.EntityFrameworkCore;
using ProjetImmo.Core.Commandes;
using ProjetImmo.Core.Models;
using ProjetImmo.Core.Tools;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
/*
namespace ProjetImmo.Core.ViewModels
{
    public class ManageClientsViewModel : BaseNotifyPropertyChanged
    {
        public ObservableCollection<Person> Clients
        {
            get { return GetProperty<ObservableCollection<Person>>(); }
            set { if (SetProperty(value)) { } }
        }

        public ManageClientsViewModel() {
            Clients = new ObservableCollection<Person>(DataAccess.AgencyDbContext.Current.Person.Where(p => p.Admin == false).ToArray());
        }

        public Person SelectedItem
        {
            get { return GetProperty<Person>(); }
            set { if (SetProperty(value)) { } }
        }

        public BaseCommand<Type> OpenEditClientWindowCommand
        {

            get => new BaseCommand<Type>((type) =>
            {
                NavigationService.ShowDialog<EditClientViewModel>(type);
                Clients = new ObservableCollection<Person>(DataAccess.AgencyDbContext.Current.Person.Where(p => p.Admin == false).ToArray());
            });

        }

        public BaseCommand<Type> DeleteSelectedClientCommand
        {

            get => new BaseCommand<Type>((type) =>
            {
                if (SelectedItem != null)
                {
                    DataAccess.AgencyDbContext.Current.Remove(SelectedItem);
                    DataAccess.AgencyDbContext.Current.SaveChangesAsync();
                    Clients = new ObservableCollection<Person>(DataAccess.AgencyDbContext.Current.Person.Where(p => p.Admin == false).ToArray());
                }
                else
                {
                    MessageBox.Show("Veuillez selectionner un item", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            });

        }

        public BaseCommand<Type> ModifySelectedClientCommand
        {

            get => new BaseCommand<Type>((type) =>
            {
                if (SelectedItem != null)
                {
                    NavigationService.ShowDialog<ModifyClientViewModel>(type, SelectedItem);

                    //executé au retour sur la fentre ManageEstate
                    Clients = new ObservableCollection<Person>(DataAccess.AgencyDbContext.Current.Person.Where(p => p.Admin == false).ToArray());
                }
                else
                {
                    MessageBox.Show("Veuillez selectionner un item", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            });

        }

    }
}
*/