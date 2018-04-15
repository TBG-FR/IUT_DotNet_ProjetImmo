using System;
using System.Collections.Generic;
using System.Text;
using ProjetImmo.Core.Commandes;
using ProjetImmo.Core.Models;
using ProjetImmo.Core.Tools;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ProjetImmo.Core.ViewModels
{
    public class UpsertPersonViewModel : BaseNotifyPropertyChanged
    {

        public bool NewItem
        {
            get;
            set;
        }

        public UpsertPersonViewModel(Person SelectedItem)
        {
            // « This will be an Update »
            this.NewItem = false;

            // Attribution de la donnée
            this.SelectedItem = SelectedItem;
            if (this.SelectedItem.Address == null) { this.SelectedItem.Address = new Address(); }

            // Chargement des données utiles
            loadData();
        }

        public UpsertPersonViewModel()
        {
            // « This will be an Insert »
            this.NewItem = true;

            // Attribution de la donnée
            this.SelectedItem = new Person();
            this.SelectedItem.Address = new Address();

            // Chargement des données utiles
            loadData();
        }

        public Person SelectedItem
        {
            get { return GetProperty<Person>(); }
            set { if (SetProperty(value)) { } }
        }

        public ObservableCollection<Models.Enums.PersonType> Types
        {
            get { return GetProperty<ObservableCollection<Models.Enums.PersonType>>(); }
            set { if (SetProperty(value)) { } }
        }

        public BaseCommand<object> UpsertSelected //Insérer dans la BD et fermer la fenêtre
        {

            get => new BaseCommand<object>((view) =>
            {
                List<string> errors = new List<string>();

                if (SelectedItem.Firstname == null)
                {
                    errors.Add("Prénom vide");
                }
                if (SelectedItem.Lastname == null)
                {
                    errors.Add("Nom de famille vide");
                }
                if (SelectedItem.Type == null)
                {
                    errors.Add("Type de personne vide");
                }

                //Add : Password, Admin, etc ?
                
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

                if (errors.Count == 0)
                {

                    // On crée la nouvelle addresse
                    // TODO : Améliorer cette partie ? Créer une nouvelle Adresse seulement si il y a eu modification ? Checker les existantes ?
                    Address addr = new Address();
                    addr.PostalAddress = SelectedItem.Address.PostalAddress;
                    addr.ZIP = SelectedItem.Address.ZIP;
                    addr.City = SelectedItem.Address.City;

                    switch (NewItem)
                    {
                        case false:

                            // On selectionne l'Person à modifier
                            Person target = (Person)Core.DataAccess.AgencyDbContext.Current.Person.Where(e => e.ID == SelectedItem.ID).ToArray().GetValue(0);

                            // On modifie les champs de l'Person en question
                            target.Firstname = SelectedItem.Firstname;
                            target.Lastname = SelectedItem.Lastname;
                            target.Type = SelectedItem.Type;
                            //Add : Password, Admin, etc ?
                            target.Address = addr;

                            // On update l'Person modifié
                            Core.DataAccess.AgencyDbContext.Current.Person.Update(target);

                            break;

                        case true:
                        //continue to default

                        default:

                            //On crée le nouvel Person et on remplit ses champs
                            Person newPerson = new Person();

                            // On modifie les champs de l'Person en question
                            newPerson.Firstname = SelectedItem.Firstname;
                            newPerson.Lastname = SelectedItem.Lastname;
                            newPerson.Type = SelectedItem.Type;
                            //Add : Password, Admin, etc ?
                            newPerson.Address = addr;

                            //On crée les nouvelles tables dans la DB
                            Core.DataAccess.AgencyDbContext.Current.Person.Add(newPerson);

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
                    MessageBox.Show(text, "Person Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }

            });

        }

        private void loadData()
        {

            // On établi la liste des Types
            Types = new ObservableCollection<Models.Enums.PersonType>();
            foreach (var value in Enum.GetValues(typeof(Models.Enums.PersonType))) { Types.Add((Models.Enums.PersonType)value); }

        }

        public override void refresh()
        {
            // Do nothing
        }

    }
}


