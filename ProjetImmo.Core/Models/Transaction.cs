using ProjetImmo.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjetImmo.Core.Models
{
    public class Transaction : ViewModels.BaseNotifyPropertyChanged
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



        #endregion

        #region Model.Transaction - Attributes



        #endregion

    }
}