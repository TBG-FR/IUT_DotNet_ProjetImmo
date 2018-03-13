using ProjetImmo.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjetImmo.Core.Models
{
    public class Estate : ViewModels.BaseNotifyPropertyChanged
    {

        #region Model.Estate - ID

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        #endregion

        #region Model.Estate - Foreign Keys & Links

        // Foreign Key links with [ForeignKey], [InverseForeignKey], [NameOf], etc.
        //      are replaced with the code located into AgencyDbContext

        public Person Owner
        {
            get { return GetProperty<Person>(); }
            set { SetProperty(value); }
        }

        public Person Referent
        {
            get { return GetProperty<Person>(); }
            set { SetProperty(value); }
        }

        public Address Address
        {
            get { return GetProperty<Address>(); }
            set { SetProperty(value); }
        }

        public ObservableCollection<Picture> Pictures
        {
            get { return GetProperty<ObservableCollection<Picture>>(); }
            set { SetProperty(value); }
        }

        public ObservableCollection<EstateKeyword> Keywords
        {
            get { return GetProperty<ObservableCollection<EstateKeyword>>(); }
            set { SetProperty(value); }
        }

        public ObservableCollection<Transaction> Transactions
        {
            get { return GetProperty<ObservableCollection<Transaction>>(); }
            set { SetProperty(value); }
        }

        #endregion

        #region Model.Estate - Attributes

        public double Surface
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        public EstateType Type
        {
            get { return GetProperty<EstateType>(); }
            set { SetProperty(value); }
        }

        public int RoomsCount
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        public int FloorNumber
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        public int FloorCount
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        public double AnnualCharges
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        public double PropertyTax
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        #endregion

        /*public Estate()
        {
            this.ID = default(int);
            this.
        }*/
    }
}