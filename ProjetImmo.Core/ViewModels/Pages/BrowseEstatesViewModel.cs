using Microsoft.EntityFrameworkCore;
using ProjetImmo.Core.Commandes;
using ProjetImmo.Core.Models;
using ProjetImmo.Core.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ProjetImmo.Core.ViewModels
{
    public class BrowseEstatesViewModel : BaseNotifyPropertyChanged
    {
        public ObservableCollection<Estate> Estates {
            get { return GetProperty<ObservableCollection<Estate>>(); }
            set { if (SetProperty(value)) { /**/ } }
        }

        public Estate SelectedItem
        {
            get { return GetProperty<Estate>(); }
            set { if (SetProperty(value)) { /**/ } }
        }

        public String SelectedFilter
        {
            get { return GetProperty<String>(); }
            set { if (SetProperty(value)) {
                    switch (SelectedFilter)
                    {
                        case "Date":
                            //Estates = new ObservableCollection<Estate>(Estates.OrderBy(e => e.Transactions)
                            break;
                        case "Prix":
                            break;
                        case "Superficie":
                            break;
                        default:
                            break;


                    }
            } }
        }

        public String SearchContent
        {
            get { return GetProperty<String>(); }
            set { if (SetProperty(value)) { /**/ } }
        }

        public BrowseEstatesViewModel() {
            Estates = new ObservableCollection<Estate>(DataAccess.AgencyDbContext.Current.Estate.Include(e => e.Address).ToArray());
        }

        public List<string> GenerateSlug(string phrase)
        {
            string str = RemoveAccent(phrase).ToLower();
            // suppressions de caratères spéciaux          
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convertie les espace, virgules et points virgules en espace simple  
            str = Regex.Replace(str, @"[\s,\,,;]+", " ").Trim();
            //on explose le chaine à chaque espace et on retourne la liste de string résultant   
            return str.Split(' ').ToList<string>();
        }

        private string RemoveAccent(string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        public BaseCommand<Type> OpenSearchFilterWindowCommand
        {

            get => new BaseCommand<Type>(/*async*/(type) => { NavigationService.ShowDialog<SearchFilterViewModel>(type); });

        }

    }
}
