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
    public class UpsertEstateViewModel : BaseNotifyPropertyChanged
    {

        public bool NewItem
        {
            get;
            set;
        }

        public UpsertEstateViewModel(Estate SelectedItem)
        {
            // « This will be an Update »
            this.NewItem = false;

            // Attribution de la donnée
            this.SelectedItem = SelectedItem;
            if(this.SelectedItem.Address == null) { this.SelectedItem.Address = new Address(); }

            //On charge les Keywords
            ObservableCollection<EstateKeyword> keywords = SelectedItem.Keywords;
            string buff = "";
            foreach (EstateKeyword keyword in keywords)
            {
                List<Keyword> key = DataAccess.AgencyDbContext.Current.Keyword.Where(k => k.ID == keyword.KeywordID).ToList();
                buff += key[0].Name;
                if (keyword != keywords.Last())
                    buff += ",";
            }
            ConcatKeyWords = buff;

            // Chargement des données utiles
            loadData();
        }

        public UpsertEstateViewModel()
        {
            // « This will be an Insert »
            this.NewItem = true;

            // Attribution de la donnée
            this.SelectedItem = new Estate();
            this.SelectedItem.Address = new Address();

            // Chargement des données utiles
            loadData();
        }

        public Estate SelectedItem
        {
            get { return GetProperty<Estate>(); }
            set { if (SetProperty(value)) { } }
        }

        public ObservableCollection<Models.Enums.EstateType> Types
        {
            get { return GetProperty<ObservableCollection<Models.Enums.EstateType>>(); }
            set { if (SetProperty(value)) { } }
        }

        public ObservableCollection<Person> Persons
        {
            get { return GetProperty<ObservableCollection<Person>>(); }
            set { if (SetProperty(value)) { } }
        }

        public string ConcatKeyWords
        {
            get { return GetProperty<string>(); }
            set { if (SetProperty(value)) { } }
        }

        public BaseCommand<object> UpsertSelected //Insérer dans la BD et fermer la fenêtre
        {

            get => new BaseCommand<object>((view) =>
            {
                List<string> errors = new List<string>();

                if (SelectedItem.Surface <= 0)
                {
                    errors.Add("Surface négative ou nulle");
                }
                if (SelectedItem.Type == null)
                {
                    errors.Add("Type de bien null");
                }
                if (SelectedItem.RoomsCount < 1)
                {
                    errors.Add("Nombre de chambres négatif ou null");
                }
                if (SelectedItem.AnnualCharges <= 0)
                {
                    errors.Add("Charges négatives ou nulles");
                }
                if (SelectedItem.PropertyTax <= 0)
                {
                    errors.Add("Taxe négative ou nulle");
                }
                if (SelectedItem.FloorNumber < -100)
                {
                    errors.Add("Numéro de l'étage trop petit");
                }
                if (SelectedItem.FloorNumber > 100)
                {
                    errors.Add("Numéro de l'étage trop grand");
                }
                if (SelectedItem.FloorNumber < -100)
                {
                    errors.Add("Numéro de l'étage trop petit");
                }
                if (SelectedItem.Owner == null)
                {
                    errors.Add("Le propriétaire est null");
                }
                /*
                if (SelectedItem.Address == null)
                {
                    errors.Add("L'addresse est nulle");
                }
                */
                if (SelectedItem.Address.PostalAddress == null)
                {
                    errors.Add("L'addresse est nulle");
                }
                if (SelectedItem.Address.ZIP == null)
                {
                    errors.Add("Le Code Postal est null");
                }
                if (SelectedItem.Address.City == null)
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

                    // On crée la nouvelle addresse
                    // TODO : Améliorer cette partie ? Créer une nouvelle Adresse seulement si il y a eu modification ? Checker les existantes ?
                    Address addr = new Address();
                    addr.PostalAddress = SelectedItem.Address.PostalAddress;
                    addr.ZIP = SelectedItem.Address.ZIP;
                    addr.City = SelectedItem.Address.City;

                    switch(NewItem)
                    {
                        case false:

                            // On selectionne l'Estate à modifier
                            Estate target = (Estate)Core.DataAccess.AgencyDbContext.Current.Estate.Where(e => e.ID == SelectedItem.ID).ToArray().GetValue(0);

                            // On modifie les champs de l'Estate en question
                            target.Surface = SelectedItem.Surface;
                            target.Type = SelectedItem.Type;
                            target.RoomsCount = SelectedItem.RoomsCount;
                            target.AnnualCharges = SelectedItem.AnnualCharges;
                            target.PropertyTax = SelectedItem.PropertyTax;
                            target.FloorNumber = SelectedItem.FloorNumber;
                            target.FloorCount = SelectedItem.FloorCount;
                            target.Owner = SelectedItem.Owner;
                            target.Address = addr;
                            target.Keywords = listKeys;

                            // On update l'Estate modifié
                            Core.DataAccess.AgencyDbContext.Current.Estate.Update(target);

                            break;

                        case true:
                        //continue to default

                        default:
                            
                            //On crée le nouvel Estate et on remplit ses champs
                            Estate newEstate = new Estate();
                            newEstate.Surface = SelectedItem.Surface;
                            newEstate.Type = SelectedItem.Type;
                            newEstate.RoomsCount = SelectedItem.RoomsCount;
                            newEstate.AnnualCharges = SelectedItem.AnnualCharges;
                            newEstate.PropertyTax = SelectedItem.PropertyTax;
                            newEstate.FloorNumber = SelectedItem.FloorNumber;
                            newEstate.FloorCount = SelectedItem.FloorCount;
                            newEstate.Owner = SelectedItem.Owner;
                            newEstate.Address = addr;
                            newEstate.Keywords = listKeys;

                            //On crée les nouvelles tables dans la DB
                            Core.DataAccess.AgencyDbContext.Current.Estate.Add(newEstate);

                            break;
                    }

                    // On applique les modifications dans la Base de Données et on ferme la fenêtre
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

            // On charge la liste des Personnes
            Persons = new ObservableCollection<Person>(DataAccess.AgencyDbContext.Current.Person.ToArray());

            // On établi la liste des Types
            Types = new ObservableCollection<Models.Enums.EstateType>();
            foreach (var value in Enum.GetValues(typeof(Models.Enums.EstateType))) { Types.Add((Models.Enums.EstateType)value); }

        }

    }
}

