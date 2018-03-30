using Microsoft.EntityFrameworkCore;
using ProjetImmo.Core.Commandes;
using ProjetImmo.Core.Models;
using ProjetImmo.Core.Tools;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;


namespace ProjetImmo.Core.ViewModels
{
    public class EditEstateViewModel : BaseNotifyPropertyChanged
    {
        
        public EditEstateViewModel()
        {
            //Chargement des données
            loadData();

            ObservableCollection<Models.Enums.EstateType> tmp = new ObservableCollection<Models.Enums.EstateType>();
            foreach(var value in Enum.GetValues(typeof(Models.Enums.EstateType)))
            {
                tmp.Add((Models.Enums.EstateType)value);
            }
            Type = tmp;
        }

        public double Surface
        {
            get { return GetProperty<double>(); }
            set { if (SetProperty(value)) { } }
        }

        public ObservableCollection<Models.Enums.EstateType> Type
        {
            get { return GetProperty<ObservableCollection<Models.Enums.EstateType>>(); }
            set { if (SetProperty(value)) { } }
        }

        public Models.Enums.EstateType SelectedType
        {
            get { return GetProperty<Models.Enums.EstateType>(); }
            set { if (SetProperty(value)) { } }
        }

        public int RoomsCount
        {
            get { return GetProperty<int>(); }
            set { if (SetProperty(value)) { } }
        }

        public double Chrages
        {
            get { return GetProperty<double>(); }
            set { if (SetProperty(value)) { } }

        }

        public double Taxe
        {
            get { return GetProperty<double>(); }
            set { if (SetProperty(value)) { } }
        }
        public int FloorNum
        {
            get { return GetProperty<int>(); }
            set { if (SetProperty(value)) { } }
        }

        public int FloorCount
        {
            get { return GetProperty<int>(); }
            set { if (SetProperty(value)) { } }
        }

        public ObservableCollection<Person> Persons
        {
            get { return GetProperty<ObservableCollection<Person>>(); }
            set { if (SetProperty(value)) { } }
        }

        public Person selectedPerson
        {
            get { return GetProperty<Person>(); }
            set { if (SetProperty(value)) { } }
        }

        public string Address
        {
            get { return GetProperty<string>(); }
            set { if (SetProperty(value)) { } }
        }

        public string ZIP
        {
            get { return GetProperty<string>(); }
            set { if (SetProperty(value)) { } }
        }

        public string City
        {
            get { return GetProperty<string>(); }
            set { if (SetProperty(value)) { } }
        }

        public string ConcatKeyWords
        {
            get { return GetProperty<string>(); }
            set { if (SetProperty(value)) { } }
        }

        public BaseCommand<object> InsertIntoBD //Insérer dans la BD et fermer la fenêtre
        {    

            get => new BaseCommand<object>(/*async*/(view) => {

                //On retires les espaces après les séparetreurs
                ConcatKeyWords = ConcatKeyWords.Replace(", ", ",");
                ConcatKeyWords = ConcatKeyWords.Replace(" ,", ",");
                ConcatKeyWords = ConcatKeyWords.Replace("; ", ";");
                ConcatKeyWords = ConcatKeyWords.Replace(" ;", ";");

                //On remplace les espaces simples par des virgules
                ConcatKeyWords = ConcatKeyWords.Replace(" ", ",");

                //On crée une observable collection des mots clés
                string[] separatingChars = { ",", ";" };
                ObservableCollection<string> listKeyWordsStr = new ObservableCollection<string>(ConcatKeyWords.Split(separatingChars, StringSplitOptions.RemoveEmptyEntries).ToArray());

                //On crée une observable collection de EstateKeywords à partir des strings dans listKeyWordsStr
                ObservableCollection<EstateKeyword> listKeys = new ObservableCollection<EstateKeyword>();
                foreach(string keyStr in listKeyWordsStr)
                {
                    if (!keyStr.Equals(""))
                    {
                        listKeys.Add(new EstateKeyword() { Keyword = new Keyword() { Name = keyStr } });
                    }
                }


                //On crée la nouvelle addresse
                Address addr = new Address();
                addr.PostalAddress = Address;
                addr.ZIP = ZIP;
                addr.City = City;

                //On crée le nouvel Estate
                Estate est1 = new Estate();
                est1.Surface = Surface;
                est1.Type = SelectedType;
                est1.RoomsCount = RoomsCount;
                est1.AnnualCharges = Chrages;
                est1.PropertyTax = Taxe;
                est1.FloorNumber = FloorNum;
                est1.FloorCount = FloorCount;
                est1.Owner = selectedPerson;
                est1.Address = addr;
                est1.Keywords = listKeys;


                //On evoies crée les nouvelles tables dans la DB
                Core.DataAccess.AgencyDbContext.Current.Estate.Add(est1);
                Core.DataAccess.AgencyDbContext.Current.SaveChanges();

                NavigationService.Close(view);
            });

        }

        private void loadData()
        {
            //On charge la liste de personne
            Persons = new ObservableCollection<Person>(DataAccess.AgencyDbContext.Current.Person.ToArray());
        }

    }
}
