﻿using Microsoft.EntityFrameworkCore;
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
    public class ManageEstatesViewModel : BaseNotifyPropertyChanged
    {

        public override void refresh() {

            // This instruction is used to initialize the list (data), and also to refresh it (after an action in another window)
            Estates = new ObservableCollection<Estate>(DataAccess.AgencyDbContext.Current.Estate.Include(e => e.Address).Include(e => e.Owner).Include(e => e.Keywords).ToArray());

            // Replacer le SelectedItem
            if (Estates != null && Estates.Count != 0) { SelectedItem = Estates.First(); }

        }

        // Constructeur
        public ManageEstatesViewModel()
        {
            refresh();
            SearchContent = "";
        }

        // Liste (éléments)
        public ObservableCollection<Estate> Estates
        {
            get { return GetProperty<ObservableCollection<Estate>>(); }
            set { if (SetProperty(value)) { SetProperty(value); } }
        }

        // Élément selectionné
        public Estate SelectedItem
        {
            get { return GetProperty<Estate>(); }
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

        public BaseCommand<Type> OpenEditEstateWindowCommand
        {

            get => new BaseCommand<Type>((type) =>
            {
                NavigationService.ShowDialog<UpsertEstateViewModel>(type);
                refresh();
            });

        }

        public BaseCommand<Type> ModifySelectedEstateCommand
        {

            get => new BaseCommand<Type>((type) =>
            {
                if (SelectedItem != null)
                {
                    NavigationService.ShowDialog<UpsertEstateViewModel>(type, SelectedItem);

                    //executé au retour sur la fentre ManageEstate
                    refresh();
                }
                else
                {
                    MessageBox.Show("Veuillez selectionner un item", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            });

        }

        public BaseCommand<Type> DeleteSelectedEstateCommand
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

        #region Commandes - Transactions-related

        public BaseCommand<Type> ValidateSaleCommand
        {

            get => new BaseCommand<Type>((type) => {
                if (SelectedItem != null)
                {
                    NavigationService.ShowDialog<UpsertTransactionViewModel>(type, SelectedItem.Transactions.Last(), true);

                    //executé au retour sur la fentre ManageEstate
                    refresh();
                }
                else
                {
                    MessageBox.Show("Veuillez selectionner un item", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            });

        }

        public BaseCommand<Type> ValidateRentalCommand
        {

            get => new BaseCommand<Type>((type) => {
                if (SelectedItem != null)
                {
                    NavigationService.ShowDialog<UpsertTransactionViewModel>(type, SelectedItem.Transactions.Last(), false);

                    //executé au retour sur la fentre ManageEstate
                    refresh();
                }
                else
                {
                    MessageBox.Show("Veuillez selectionner un item", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            });

        }

        public BaseCommand<Type> NewSaleCommand
        {

            get => new BaseCommand<Type>((type) => {
                if (SelectedItem != null)
                {
                    NavigationService.ShowDialog<UpsertTransactionViewModel>(type, SelectedItem, true);

                    //executé au retour sur la fentre ManageEstate
                    refresh();
                }
                else
                {
                    MessageBox.Show("Veuillez selectionner un item", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            });

        }

        public BaseCommand<Type> NewRentalCommand
        {

            get => new BaseCommand<Type>((type) =>
            {
                if (SelectedItem != null)
                {
                    NavigationService.ShowDialog<UpsertTransactionViewModel>(type, SelectedItem, false);

                    //executé au retour sur la fentre ManageEstate
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



            get => new BaseCommand<Type>(/*async*/(type) => {
                //On crée une liste de keyWords et une liste qui va stocker les poids d'occurences (avec les keyWords) pour chaque Estate
                List<string> motsCles = GenerateSlug(SearchContent);
                List<int> occurencesEstates = new List<int>();

                //On initialise la liste d'occurences à 0
                foreach (Estate est in Estates)
                {
                    occurencesEstates.Add(0);
                }

                //On parcours les Estates (toutes les variables contenant estate dans leurs noms sont parcourues avec les i)
                for (int i = 0; i < Estates.Count; i++)
                {
                    //On parcours les motsCles (toutes les variables contenant motCle dans leurs noms sont parcourues avec les j)
                    for (int j = 0; j < motsCles.Count; j++)
                    {
                        if (Estates[i].Owner.Firstname.Equals(motsCles[j]))
                        {
                            occurencesEstates[i] += 3;
                        }
                        if (Estates[i].Owner.Lastname.Equals(motsCles[j]))
                        {
                            occurencesEstates[i] += 3;
                        }
                        //On parcours tous les mots clés de l'Estate courant
                        foreach (EstateKeyword estKeyw in Estates[i].Keywords)
                        {
                            List<Keyword> keyword = new List<Keyword>(DataAccess.AgencyDbContext.Current.Keyword.Where((e) => e.ID == estKeyw.KeywordID).ToList());
                            if (motsCles[j].Equals(keyword[0].Name))
                            {
                                occurencesEstates[i] += 2;
                            }
                        }
                    }
                }

                //On crée une nouvelle ObservableCollection d'Estates qui trira la liste courante
                ObservableCollection<Estate> tmpSortEst = new ObservableCollection<Estate>();

                bool toChange = false;
                foreach (int occ in occurencesEstates)
                {
                    if (occ != 0)
                        toChange = true;
                }

                if (toChange)
                {
                    //On parcours les valeurs de Min a Max de la Liste d'occurences
                    for (int i = occurencesEstates.Max(); i >= occurencesEstates.Min(); i--)
                    {
                        //On parcours la Liste d'occurences
                        for (int j = 0; j < occurencesEstates.Count; j++)
                        {
                            //Si l'occurence courrante est égale à i (i allant de min à max occurences)
                            if (occurencesEstates[j].Equals(i))
                            {
                                //On ajoute l'Estate correspondant au numéro de l'occurence courante
                                tmpSortEst.Add(Estates[j]);
                            }
                        }
                    }

                    //On copie tmpSortEst (la liste triée) dans Estates
                    Estates = tmpSortEst;
                }

                // --> !!! CRITICAL WARNING END !!! <-- //

            });

        }

        #endregion

    }
}
