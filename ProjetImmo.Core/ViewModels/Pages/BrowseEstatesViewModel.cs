using Microsoft.EntityFrameworkCore;
using ProjetImmo.Core.Commandes;
using ProjetImmo.Core.Models;
using ProjetImmo.Core.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ProjetImmo.Core.ViewModels
{
    public class BrowseEstatesViewModel : BaseNotifyPropertyChanged
    {
        public ObservableCollection<Estate> Estates {
            get { return GetProperty<ObservableCollection<Estate>>(); }
            set { if (SetProperty(value)) { /**/ } }
        }

        public BrowseEstatesViewModel() { Estates = new ObservableCollection<Estate>(DataAccess.AgencyDbContext.Current.Estate.Include(e => e.Address).ToArray()); }

        public Estate SelectedItem
        {
            get { return GetProperty<Estate>(); }
            set { if (SetProperty(value)) { /**/ } }
        }

        public String SelectedFilter
        {
            get { return GetProperty<String>(); }
            set { if (SetProperty(value)) {
                    switch (SelectedFilter)
                    {
                        case "Date":
                            //Estates = new ObservableCollection<Estate>(Estates.OrderBy(e => e.Transactions)
                            break;
                        case "Prix":
                            break;
                        case "Superficie":
                            break;
                        default:
                            break;


                    }
            } }
        }

        public BaseCommand<Type> OpenSearchFilterWindowCommand
        {

            get => new BaseCommand<Type>(/*async*/(type) => { NavigationService.ShowDialog<SearchFilterViewModel>(type); });

        }

    }
}
