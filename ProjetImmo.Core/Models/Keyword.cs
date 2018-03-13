using ProjetImmo.Core.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjetImmo.Core.Models
{
    public class Keyword : ViewModels.BaseNotifyPropertyChanged
    {

        #region Model.Keyword - ID

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        #endregion

        #region Model.Keyword - Foreign Keys & Links

        // Foreign Key links with [ForeignKey], [InverseForeignKey], [NameOf], etc.
        //      are replaced with the code located into AgencyDbContext

        public ObservableCollection<EstateKeyword> DescribedEstates
        {
            get { return GetProperty<ObservableCollection<EstateKeyword>>(); }
            set { SetProperty(value); }
        }

        #endregion

        #region Model.Keyword - Attributes

        public string Name
        {
            get { return GetProperty<String>(); }
            set { SetProperty(value); }
        }

        #endregion

    }
}