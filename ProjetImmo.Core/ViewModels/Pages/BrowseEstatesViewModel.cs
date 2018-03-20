using Microsoft.EntityFrameworkCore;
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
        public ObservableCollection<Estate> Estates { get; set; }

        public Estate SelectedItem
        {
            get { return GetProperty<Estate>(); }

            set {
                if (SetProperty(value))
                {
                    
                }
            }

        }

        public BrowseEstatesViewModel() { Estates = new ObservableCollection<Estate>(DataAccess.AgencyDbContext.Current.Estate.Include(e => e.Address).ToArray()); }

    }
}
