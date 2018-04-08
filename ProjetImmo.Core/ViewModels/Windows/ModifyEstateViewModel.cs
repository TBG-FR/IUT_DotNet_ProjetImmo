using ProjetImmo.Core.Commandes;
using ProjetImmo.Core.Models;
using ProjetImmo.Core.Tools;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Windows;

namespace ProjetImmo.Core.ViewModels
{
    public class ModifyEstateViewModel : BaseNotifyPropertyChanged
    {

        public ModifyEstateViewModel()
        {
            //Chargement des données
            loadData();

            ObservableCollection<Models.Enums.EstateType> tmp = new ObservableCollection<Models.Enums.EstateType>();
            foreach (var value in Enum.GetValues(typeof(Models.Enums.EstateType)))
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

        public double Charges
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

            get => new BaseCommand<object>(/*async*/(view) =>
            {
                List<string> errors = new List<string>();

                if (Surface <= 0)
                {
                    errors.Add("Surface négative ou nulle");
                }
                if (SelectedType == null)
                {
                    errors.Add("Type de bien null");
                }
                if (RoomsCount < 1)
                {
                    errors.Add("Nombre de chambres négatif ou null");
                }
                if (Charges <= 0)
                {
                    errors.Add("Charges négatives ou nulles");
                }
                if (Taxe <= 0)
                {
                    errors.Add("Taxe négative ou nulle");
                }
                if (FloorNum < -100)
                {
                    errors.Add("Numéro de l'étage trop petit");
                }
                if (FloorNum > 100)
                {
                    errors.Add("Numéro de l'étage trop grand");
                }
                if (FloorNum < -100)
                {
                    errors.Add("Numéro de l'étage trop petit");
                }
                if (selectedPerson == null)
                {
                    errors.Add("Le propriétaire est null");
                }
                if (Address == null)
                {
                    errors.Add("L'addresse est nulle");
                }
                if (ZIP == null)
                {
                    errors.Add("Le Code Postal est null");
                }
                if (City == null)
                {
                    errors.Add("La ville est nulle");
                }
                if (ConcatKeyWords == null)
                {
                    errors.Add("Les mots clés sont nulls");
                }

                if (errors.Count == 0)
                {

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
                    foreach (string keyStr in listKeyWordsStr)
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
                    est1.AnnualCharges = Charges;
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
                }

                //affichage des erreurs
                else
                {
                    string text = "";
                    foreach (string error in errors)
                    {
                        text += "- " + error + "\n";
                    }
                    MessageBox.Show(text, "Estate Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }

            });

        }

        private void loadData()
        {
            //On charge la liste de personne
            Persons = new ObservableCollection<Person>(DataAccess.AgencyDbContext.Current.Person.ToArray());
        }

    }
}

