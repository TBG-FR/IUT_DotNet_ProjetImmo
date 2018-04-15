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
    public class ManageClientsViewModel : BaseNotifyPropertyChanged
    {

        public override void refresh()
        {

            // This instruction is used to initialize the list (data), and also to refresh it (after an action in another window)
            Clients = new ObservableCollection<Person>(DataAccess.AgencyDbContext.Current.Person.Include(e => e.Address).ToArray());

            // Replacer le SelectedItem
            if (Clients != null && Clients.Count != 0) { SelectedItem = Clients.First(); }

        }

        // Constructeur
        public ManageClientsViewModel()
        {
            refresh();
            SearchContent = "";
        }

        // Liste (éléments)
        public ObservableCollection<Person> Clients
        {
            get { return GetProperty<ObservableCollection<Person>>(); }
            set { if (SetProperty(value)) { SetProperty(value); } }
        }

        // Élément selectionné
        public Person SelectedItem
        {
            get { return GetProperty<Person>(); }
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

        public BaseCommand<Type> OpenEditPersonWindowCommand
        {

            get => new BaseCommand<Type>((type) =>
            {
                NavigationService.ShowDialog<UpsertPersonViewModel>(type);
                refresh();
            });

        }

        public BaseCommand<Type> ModifySelectedPersonCommand
        {

            get => new BaseCommand<Type>((type) =>
            {
                if (SelectedItem != null)
                {
                    NavigationService.ShowDialog<UpsertPersonViewModel>(type, SelectedItem);

                    //executé au retour sur la fentre ManagePerson
                    refresh();
                }
                else
                {
                    MessageBox.Show("Veuillez selectionner un item", "Selection Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            });

        }

        public BaseCommand<Type> DeleteSelectedPersonCommand
        {

            get => new BaseCommand<Type>(async (type) =>
            {
                if (SelectedItem != null)
                {
                    DataAccess.AgencyDbContext.Current.Remove(SelectedItem);
                    await DataAccess.AgencyDbContext.Current.SaveChangesAsync();
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

                /* MODIFY FROM HERE -------------------------------------------------

                //On crée une liste de keyWords et une liste qui va stocker les poids d'occurences (avec les keyWords) pour chaque Person
                List<string> motsCles = GenerateSlug(SearchContent);
                List<int> occurencesClients = new List<int>();

                //On initialise la liste d'occurences à 0
                foreach (Person est in Clients)
                {
                    occurencesClients.Add(0);
                }

                //On parcours les Clients (toutes les variables contenant Person dans leurs noms sont parcourues avec les i)
                for (int i = 0; i < Clients.Count; i++)
                {
                    //On parcours les motsCles (toutes les variables contenant motCle dans leurs noms sont parcourues avec les j)
                    for (int j = 0; j < motsCles.Count; j++)
                    {
                        if (Clients[i].Owner.Firstname.Equals(motsCles[j]))
                        {
                            occurencesClients[i] += 3;
                        }
                        if (Clients[i].Owner.Lastname.Equals(motsCles[j]))
                        {
                            occurencesClients[i] += 3;
                        }
                        //On parcours tous les mots clés de l'Person courant
                        foreach (PersonKeyword estKeyw in Clients[i].Keywords)
                        {
                            List<Keyword> keyword = new List<Keyword>(DataAccess.AgencyDbContext.Current.Keyword.Where((e) => e.ID == estKeyw.KeywordID).ToList());
                            if (motsCles[j].Equals(keyword[0].Name))
                            {
                                occurencesClients[i] += 2;
                            }
                        }
                    }
                }

                //On crée une nouvelle ObservableCollection d'Clients qui trira la liste courante
                ObservableCollection<Person> tmpSortEst = new ObservableCollection<Person>();

                bool toChange = false;
                foreach (int occ in occurencesClients)
                {
                    if (occ != 0)
                        toChange = true;
                }

                if (toChange)
                {
                    //On parcours les valeurs de Min a Max de la Liste d'occurences
                    for (int i = occurencesClients.Max(); i >= occurencesClients.Min(); i--)
                    {
                        //On parcours la Liste d'occurences
                        for (int j = 0; j < occurencesClients.Count; j++)
                        {
                            //Si l'occurence courrante est égale à i (i allant de min à max occurences)
                            if (occurencesClients[j].Equals(i))
                            {
                                //On ajoute l'Person correspondant au numéro de l'occurence courante
                                tmpSortEst.Add(Clients[j]);
                            }
                        }
                    }

                    //On copie tmpSortEst (la liste triée) dans Clients
                    Clients = tmpSortEst;
                }

                // --> !!! CRITICAL WARNING END !!! <-- //

            */});

        }

        #endregion

    }
}
