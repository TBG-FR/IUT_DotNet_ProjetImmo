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
    public class UpsertTransactionViewModel : BaseNotifyPropertyChanged
    {

        public bool NewItem
        {
            get;
            set;
        }

        public bool Sale
        {
            get;
            set;
        }

        // Correspond à "Valider la vente" & "Valider la Location"
        public UpsertTransactionViewModel(Transaction SelectedItem, bool Sale)
        {
            // « This will be an Update »
            this.NewItem = false;
            this.Sale = Sale;

            // Attribution de la donnée
            if (this.Sale == true) { this.SelectedItem = (SaleTransaction)SelectedItem; }
            else { this.SelectedItem = (RentalTransaction)SelectedItem; }

            // Date de Transaction par défaut
            this.SelectedItem.TransactionDate = DateTime.Now;

            // Chargement des données utiles
            loadData();
        }

        // Correspond à "Remettre en Vente" & "Remettre à la Location"
        public UpsertTransactionViewModel(Estate RelatedEstate, bool Sale)
        {
            // « This will be an Insert »
            this.NewItem = true;
            this.Sale = Sale;

            // Attribution de la donnée
            if (this.Sale == true) {

                this.SelectedItem = new SaleTransaction();
                this.SelectedItem.RelatedEstate = RelatedEstate;

            }
            else {

                this.SelectedItem = new RentalTransaction();
                this.SelectedItem.RelatedEstate = RelatedEstate;
            }

            // Date de Création par défaut
            this.SelectedItem.CreationDate = DateTime.Now;

            // Chargement des données utiles
            loadData();
        }

        public Transaction SelectedItem
        {
            get { return GetProperty<Transaction>(); }
            set { if (SetProperty(value)) { } }
        }

        public ObservableCollection<Person> Persons
        {
            get { return GetProperty<ObservableCollection<Person>>(); }
            set { if (SetProperty(value)) { } }
        }

        public BaseCommand<object> UpsertSelected //Insérer dans la BD et fermer la fenêtre
        {

            get => new BaseCommand<object>((view) =>
            {
                List<string> errors = new List<string>();

                //if(NewItem == true)
                //{

                if (Sale == false)
                {
                    RentalTransaction rentalTransaction = (RentalTransaction) SelectedItem;
                    if (rentalTransaction.Furnished == null)
                    {
                        errors.Add("Veuillez indiquer si la location est meublée ou non");
                    }
                }

                if (SelectedItem.Title == null)
                {
                    errors.Add("Titre vide");
                }

                if (SelectedItem.Description == null)
                {
                    errors.Add("Description vide");
                }

                if (SelectedItem.Price < 0)
                {
                    errors.Add("Prix négatif");
                }

                if (SelectedItem.Fees < 0)
                {
                    errors.Add("Frais négatifs");
                }

                if (SelectedItem.CreationDate == null)
                {
                    errors.Add("Date de Création vide");
                }

                // Placer ici les vérifications additionnelles sur la date

                //}

                if (NewItem == false)
                {

                    if (SelectedItem.RelatedEstate == null)
                    {
                        errors.Add("Aucun bien immobilier relié à cette transaction");
                    }

                    if (SelectedItem.RelatedCustomer == null)
                    {
                        errors.Add("Aucun client lié à cet achat");
                    }

                    if (SelectedItem.TransactionDate == null)
                    {
                        errors.Add("Date de Transaction vide");
                    }

                    // Placer ici les vérifications additionnelles sur la date

                }

                if (errors.Count == 0)
                {

                    switch (NewItem)
                    {
                        case false:

                            if (Sale == true)
                            {

                                // On récupère la Transaction à modifier
                                SaleTransaction target = (SaleTransaction)Core.DataAccess.AgencyDbContext.Current.SaleTransaction.Where(t => t.ID == SelectedItem.ID).ToArray().GetValue(0);

                                // On modifie les champs de la Transaction en question
                                //target.Title = SelectedItem.Title;
                                //target.Description = SelectedItem.Description;
                                //target.CreationDate = SelectedItem.CreationDate;
                                target.TransactionDate = SelectedItem.TransactionDate;
                                target.RelatedCustomer = SelectedItem.RelatedCustomer;
                                //target.RelatedEstate = SelectedItem.RelatedEstate;

                                // On update la Transaction modifiée
                                Core.DataAccess.AgencyDbContext.Current.SaleTransaction.Update(target);

                            }
                            else
                            {

                                // On récupère la Transaction à modifier
                                RentalTransaction target = (RentalTransaction)Core.DataAccess.AgencyDbContext.Current.RentalTransaction.Where(t => t.ID == SelectedItem.ID).ToArray().GetValue(0);
                                //RentalTransaction rentalTransaction = (RentalTransaction)SelectedItem;

                                // On modifie les champs de la Transaction en question
                                //target.Title = SelectedItem.Title;
                                //target.Description = SelectedItem.Description;
                                //target.Furnished = rentalTransaction.Furnished;
                                //target.CreationDate = SelectedItem.CreationDate;
                                target.TransactionDate = SelectedItem.TransactionDate;
                                target.RelatedCustomer = SelectedItem.RelatedCustomer;
                                //target.RelatedEstate = SelectedItem.RelatedEstate;

                                // On update la Transaction modifiée
                                Core.DataAccess.AgencyDbContext.Current.RentalTransaction.Update(target);

                            }

                            break;

                        case true:
                        //continue to default

                        default:

                            if (Sale == true)
                            {

                                // On récupère la Transaction à modifier
                                SaleTransaction target = new SaleTransaction();

                                // On modifie les champs de la Transaction en question
                                target.Title = SelectedItem.Title;
                                target.Description = SelectedItem.Description;
                                target.CreationDate = SelectedItem.CreationDate;
                                //target.TransactionDate = SelectedItem.TransactionDate;
                                //target.RelatedCustomer = SelectedItem.RelatedCustomer;
                                target.RelatedEstate = SelectedItem.RelatedEstate;

                                // On update la Transaction modifiée
                                Core.DataAccess.AgencyDbContext.Current.SaleTransaction.Update(target);

                            }
                            else
                            {

                                // On récupère la Transaction à modifier
                                RentalTransaction target = new RentalTransaction();
                                RentalTransaction rentalTransaction = (RentalTransaction)SelectedItem;

                                // On modifie les champs de la Transaction en question
                                target.Title = SelectedItem.Title;
                                target.Description = SelectedItem.Description;
                                target.Furnished = rentalTransaction.Furnished;
                                target.CreationDate = SelectedItem.CreationDate;
                                //target.TransactionDate = SelectedItem.TransactionDate;
                                //target.RelatedCustomer = SelectedItem.RelatedCustomer;
                                target.RelatedEstate = SelectedItem.RelatedEstate;

                                // On update la Transaction modifiée
                                Core.DataAccess.AgencyDbContext.Current.RentalTransaction.Update(target);

                            }

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

        }

        public override void refresh()
        {
            // Do nothing
        }

    }
}

