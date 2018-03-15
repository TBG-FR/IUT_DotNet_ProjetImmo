using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commandes.ViewModels
{
    public class MainViewModel : BaseNotifyPropertyChanged
    {

        public string TexteAAffecter
        {
            get { return (string)GetProperty(); }
            set { SetProperty(value); }
        }
        public string TexteAffecte
        {
            get { return (string)GetProperty(); }
            set { SetProperty(value); }
        }
        public Commands.BaseCommand CommandeAffecterTexte
        {
            get
            {
                return new Commands.BaseCommand(AffecterTexte);
            }
        }

        private void AffecterTexte()
        {
            this.TexteAffecte = this.TexteAAffecter;
        }


    }
}
