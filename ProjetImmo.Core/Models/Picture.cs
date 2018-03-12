using ProjetImmo.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjetImmo.Core.Models
{
    public class Picture : ViewModels.BaseNotifyPropertyChanged
    {

        #region Model.Picture - ID

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        #endregion

        #region Model.Picture - Foreign Keys & Links

        // Foreign Key links with [ForeignKey], [InverseForeignKey], [NameOf], etc.
        //      are replaced with the code located into AgencyDbContext

        public Estate RelatedEstate
        {
            get { return GetProperty<Estate>(); }
            set { SetProperty(value); }
        }

        #endregion

        #region Model.Picture - Attributes

        public string Title
        {
            get { return GetProperty<String>(); }
            set { SetProperty(value); }
        }

        public DateTime Date
        {
            get { return GetProperty<DateTime>(); }
            set { SetProperty(value); }
        }

        public Byte Base64
        {
            get { return GetProperty<Byte>(); }
            set { SetProperty(value); }
        }

        #endregion

    }
}