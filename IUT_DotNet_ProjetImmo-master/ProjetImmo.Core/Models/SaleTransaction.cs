using ProjetImmo.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjetImmo.Core.Models
{
    public class SaleTransaction : Transaction
    {

        #region Model.Transaction - ID

        /* See Superclass 'Transaction' */

        #endregion

        #region Model.SaleTransaction - Foreign Keys & Links

        // Foreign Key links with [ForeignKey], [InverseForeignKey], [NameOf], etc.
        //      are replaced with the code located into AgencyDbContext

        /* See Superclass 'Transaction' */

        #endregion

        #region Model.SaleTransaction - Attributes

        /* Nothing here */

        #endregion

    }
}