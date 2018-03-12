using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetImmo.Core.Models
{
    public class EstateKeyword : ViewModels.BaseNotifyPropertyChanged
    {

        #region Model.EstateKeyword - ID

        public int EstateID
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        public int KeywordID
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        #endregion

        #region Model.EstateKeyword - Foreign Keys & Links

        // Foreign Key links with [ForeignKey], [InverseForeignKey], [NameOf], etc.
        //      are replaced with the code located into AgencyDbContext

        public Estate Estate
        {
            get { return GetProperty<Estate>(); }
            set { SetProperty(value); }
        }

        public Keyword Keyword
        {
            get { return GetProperty<Keyword>(); }
            set { SetProperty(value); }
        }

        #endregion

        #region Model.EstateKeyword - Attributes

        // Nothing here

        #endregion

    }
}