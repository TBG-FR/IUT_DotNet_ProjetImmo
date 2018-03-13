using ProjetImmo.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjetImmo.Core.Models
{
    public class RentalTransaction : Transaction
    {

        #region Model.RentalTransaction - ID

        /* See Superclass 'Transaction' */

        #endregion

        #region Model.RentalTransaction - Foreign Keys & Links

        // Foreign Key links with [ForeignKey], [InverseForeignKey], [NameOf], etc.
        //      are replaced with the code located into AgencyDbContext

        /* See Superclass 'Transaction' */

        #endregion

        #region Model.RentalTransaction - Attributes

        public bool Furnished
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        #endregion

    }
}