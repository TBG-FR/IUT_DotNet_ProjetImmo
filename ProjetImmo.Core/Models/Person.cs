using ProjetImmo.Core.Models.Enums;
using ProjetImmo.Core.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjetImmo.Core.Models
{
    public class Person : BaseNotifyPropertyChanged
    {

        #region Model.Person - ID

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        #endregion

        #region Model.Person - Foreign Keys & Links

        // Foreign Key links with [ForeignKey], [InverseForeignKey], [NameOf], etc.
        //      are replaced with the code located into AgencyDbContext

        public ObservableCollection<Estate> OwnedEstates
        {
            get { return GetProperty<ObservableCollection<Estate>>(); }
            set { SetProperty(value); }
        }

        public ObservableCollection<Estate> ManagedEstates
        {
            get { return GetProperty<ObservableCollection<Estate>>(); }
            set { SetProperty(value); }
        }

        public Address Address
        {
            get { return GetProperty<Address>(); }
            set { SetProperty(value); }
        }

        #endregion

        #region Model.Person - Attributes

        public PersonType Type
        {
            get { return GetProperty<PersonType>(); }
            set { SetProperty(value); }
        }

        public String Firstname
        {
            get { return GetProperty<String>(); }
            set { SetProperty(value); }
        }

        public String Lastname
        {
            get { return GetProperty<String>(); }
            set { SetProperty(value); }
        }

        public String Password
        {
            get { return GetProperty<String>(); }
            set { SetProperty(value); }
        }

        public bool Admin
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        #endregion

        public override void refresh()
        {
            // Do nothing
        }

    }
}