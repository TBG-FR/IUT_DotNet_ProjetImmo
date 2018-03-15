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
    public class Transaction : BaseNotifyPropertyChanged
    {

        #region Model.Transaction - ID

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        #endregion

        #region Model.Transaction - Foreign Keys & Links

        // Foreign Key links with [ForeignKey], [InverseForeignKey], [NameOf], etc.
        //      are replaced with the code located into AgencyDbContext

        public Estate RelatedEstate
        {
            get { return GetProperty<Estate>(); }
            set { SetProperty(value); }
        }
        

        public Person RelatedCustomer // Buyer, Occupant, Occupier, Leaseholder
        {
            get { return GetProperty<Person>(); }
            set { SetProperty(value); }
        }

        #endregion

        #region Model.Transaction - Attributes

        public string Title
        {
            get { return GetProperty<String>(); }
            set { SetProperty(value); }
        }

        public string Description
        {
            get { return GetProperty<String>(); }
            set { SetProperty(value); }
        }

        public double Price
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        public double Fees
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        public DateTime CreationDate
        {
            get { return GetProperty<DateTime>(); }
            set { SetProperty(value); }
        }

        public DateTime? TransactionDate
        {
            get { return GetProperty<DateTime?>(); }
            set { SetProperty(value); }
        }

        #endregion

    }
}