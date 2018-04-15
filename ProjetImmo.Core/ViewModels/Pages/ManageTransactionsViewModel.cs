using Microsoft.EntityFrameworkCore;
using ProjetImmo.Core.Commandes;
using ProjetImmo.Core.Models;
using ProjetImmo.Core.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace ProjetImmo.Core.ViewModels
{
    public class ManageTransactionsViewModel : BaseNotifyPropertyChanged
    {

        public override void refresh()
        {

            // This instruction is used to initialize the list (data), and also to refresh it (after an action in another window)
            Transactions = new ObservableCollection<Transaction>();

            List<RentalTransaction> rentalTransactions = new List<RentalTransaction>(DataAccess.AgencyDbContext.Current.RentalTransaction/*.Include(e => e.Address).Include(e => e.Owner).Include(e => e.Keywords).*/.ToArray());
            foreach (RentalTransaction rt in rentalTransactions) { Transactions.Add(rt); }

            List<SaleTransaction> saleTransactions = new List<SaleTransaction>(DataAccess.AgencyDbContext.Current.SaleTransaction/*.Include(e => e.Address).Include(e => e.Owner).Include(e => e.Keywords).*/.ToArray());
            foreach (SaleTransaction st in saleTransactions) { Transactions.Add(st); }

            // Replacer le SelectedItem
            if (Transactions != null && Transactions.Count != 0) { SelectedItem = Transactions.First(); }

        }

        // Constructeur
        public ManageTransactionsViewModel()
        {
            refresh();
            SearchContent = "";
        }

        // Liste (éléments)
        public ObservableCollection<Transaction> Transactions
        {
            get { return GetProperty<ObservableCollection<Transaction>>(); }
            set { if (SetProperty(value)) { SetProperty(value); } }
        }

        // Élément selectionné
        public Transaction SelectedItem
        {
            get { return GetProperty<Transaction>(); }
            set { if (SetProperty(value)) { SetProperty(value); } }
        }

        #region Propriétés & Fonctions - Search-related

        public String selectedFilter
        {
            get { return GetProperty<String>(); }
            set { if (SetProperty(value)) { SetProperty(value); } }
        }

        public String SearchContent
        {
            get { return GetProperty<String>(); }
            set
            {
                if (SetProperty(value))
                {
                }
            }
        }

        public List<string> GenerateSlug(string phrase)
        {
            string str = RemoveAccent(phrase).ToLower();
            // suppressions de caratères spéciaux          
            str = Regex.Replace(str, @"[^a-z0-9\,\;\s-]", "");
            // convertie les espace, virgules et points virgules en espace simple  
            str = Regex.Replace(str, @"[\s\,\;]+", " ");
            //on explose le chaine à chaque espace et on retourne la liste de string résultant   
            return str.Split(' ').ToList<string>();
        }

        private string RemoveAccent(string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        #endregion

        #region Commandes - Ajout/Modification/Suppression d'élément

        /*
        public BaseCommand<Type> OpenEditTransactionWindowCommand
        {

            get => new BaseCommand<Type>((type) =>
            {
                NavigationService.ShowDialog<UpsertTransactionViewModel>(type);
                reInitList();
            });

        }
        */

        public BaseCommand<Type> ModifySelectedTransactionCommand
        {

            get => new BaseCommand<Type>((type) =>
            {
                if (SelectedItem != null)
                {
                    NavigationService.ShowDialog<UpsertTransactionViewModel>(type, SelectedItem);

                    //executé au retour sur la fentre ManageTransaction
                    refresh();
                }
                else
                {
                    MessageBox.Show("Veuillez selectionner un item", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            });

        }

        public BaseCommand<Type> DeleteSelectedTransactionCommand
        {

            get => new BaseCommand<Type>(async (type) =>
            {
                if (SelectedItem != null)
                {
                    DataAccess.AgencyDbContext.Current.Remove(SelectedItem);
                    DataAccess.AgencyDbContext.Current.SaveChangesAsync();
                    refresh();
                }
                else
                {
                    MessageBox.Show("Veuillez selectionner un item", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            });

        }

        #endregion

        #region Commandes - Search/Sort-related

        public BaseCommand<Type> OpenSearchFilterWindowCommand
        {

            get => new BaseCommand<Type>(/*async*/(type) => { NavigationService.ShowDialog<SearchFilterViewModel>(type); });

        }

        public BaseCommand<Type> SortListBySearchCommand
        {

            // --> !!! CRITICAL WARNING BEGIN !!! <-- //

            //L'alogorithme qui va suivre peut entrainer des crises d'épilepsies ou de comas de durés indéteminées
            // Nous nous dérésponsabilison de toute plaintes ou réclamations, suite à des dommages cérébraux ou perte de capacité motrices

            //Bonne Chance à vous...


            get => new BaseCommand<Type>((type) => {

                /* MODIFY FROM HERE------------------------------------------------ -

               //On crée une liste de keyWords et une liste qui va stocker les poids d'occurences (avec les keyWords) pour chaque Transaction
               List<string> motsCles = GenerateSlug(SearchContent);
                List<int> occurencesTransactions = new List<int>();

                //On initialise la liste d'occurences à 0
                foreach (Transaction est in Transactions)
                {
                    occurencesTransactions.Add(0);
                }

                //On parcours les Transactions (toutes les variables contenant Transaction dans leurs noms sont parcourues avec les i)
                for (int i = 0; i < Transactions.Count; i++)
                {
                    //On parcours les motsCles (toutes les variables contenant motCle dans leurs noms sont parcourues avec les j)
                    for (int j = 0; j < motsCles.Count; j++)
                    {
                        if (Transactions[i].Owner.Firstname.Equals(motsCles[j]))
                        {
                            occurencesTransactions[i] += 3;
                        }
                        if (Transactions[i].Owner.Lastname.Equals(motsCles[j]))
                        {
                            occurencesTransactions[i] += 3;
                        }
                        //On parcours tous les mots clés de l'Transaction courant
                        foreach (TransactionKeyword estKeyw in Transactions[i].Keywords)
                        {
                            List<Keyword> keyword = new List<Keyword>(DataAccess.AgencyDbContext.Current.Keyword.Where((e) => e.ID == estKeyw.KeywordID).ToList());
                            if (motsCles[j].Equals(keyword[0].Name))
                            {
                                occurencesTransactions[i] += 2;
                            }
                        }
                    }
                }

                //On crée une nouvelle ObservableCollection d'Transactions qui trira la liste courante
                ObservableCollection<Transaction> tmpSortEst = new ObservableCollection<Transaction>();

                bool toChange = false;
                foreach (int occ in occurencesTransactions)
                {
                    if (occ != 0)
                        toChange = true;
                }

                if (toChange)
                {
                    //On parcours les valeurs de Min a Max de la Liste d'occurences
                    for (int i = occurencesTransactions.Max(); i >= occurencesTransactions.Min(); i--)
                    {
                        //On parcours la Liste d'occurences
                        for (int j = 0; j < occurencesTransactions.Count; j++)
                        {
                            //Si l'occurence courrante est égale à i (i allant de min à max occurences)
                            if (occurencesTransactions[j].Equals(i))
                            {
                                //On ajoute l'Transaction correspondant au numéro de l'occurence courante
                                tmpSortEst.Add(Transactions[j]);
                            }
                        }
                    }

                    //On copie tmpSortEst (la liste triée) dans Transactions
                    Transactions = tmpSortEst;
                }

                // --> !!! CRITICAL WARNING END !!! <-- //

            */});

        }

        #endregion

    }
}
