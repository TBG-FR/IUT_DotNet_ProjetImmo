using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjetImmo.Core.Models
{
    public class Address : ViewModels.BaseNotifyPropertyChanged
    {

        #region Model.Address - ID

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        #endregion

        #region Model.Address - Foreign Keys & Links

        // Foreign Key links with [ForeignKey], [InverseForeignKey], [NameOf], etc.
        //      are replaced with the code located into AgencyDbContext

        public Estate AddressedEstate
        {
            get { return GetProperty<Estate>(); }
            set { SetProperty(value); }
        }

        public Person AddressedPerson
        {
            get { return GetProperty<Person>(); }
            set { SetProperty(value); }
        }

        #endregion

        #region Model.Address - Attributes

        public String PostalAddress
        {
            get { return GetProperty<String>(); }
            set { SetProperty(value); }
        }

        public String ZIP
        {
            get { return GetProperty<String>(); }
            set { SetProperty(value); }
        }

        public String City
        {
            get { return GetProperty<String>(); }
            set { SetProperty(value); }
        }

        public double Longitude
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        public double Latitude
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        #endregion

    }
}