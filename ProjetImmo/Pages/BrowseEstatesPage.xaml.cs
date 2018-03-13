using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjetImmo.WPF
{
    /// <summary>
    /// Logique d'interaction pour SearchPage.xaml
    /// </summary>
    public partial class BrowseEstatePage : Page
    {
        public BrowseEstatePage()
        {
            InitializeComponent();

            /*
            Person Bob = new Person();
            Address BobAd = new Address();

            //BobAd.ID = 1;
            BobAd.City = "Bourg";
            BobAd.ZIP = "01000";
            BobAd.PostalAddress = "5 rue des champs";
            BobAd.Longitude = 39.45;
            BobAd.Latitude = 14.18;
            //BobAd.AddressedEstate = null;
            //BobAd.AddressedPerson = Bob;

            //Bob.ID = 1;
            Bob.Type = Core.Models.Enums.PersonType.NATURAL;
            Bob.Firstname = "Bob";
            Bob.Lastname = "Dylan";
            Bob.Address = BobAd;

            Core.DataAccess.AgencyDbContext.Current.Address.Add(BobAd);
            Core.DataAccess.AgencyDbContext.Current.Person.Add(Bob);
            Core.DataAccess.AgencyDbContext.Current.SaveChanges();
            */

            /*
             * APPLIQUER LES MODIFICATIONS SUR LA BASE DE DONNEES
             * => DataAccess.AgencyDbContext.Current.SaveChangesAsync
             */

        }
        /*
        private void ComboBox_OrderBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Filters_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Search_Click(object sender, RoutedEventArgs e)
        {

        }*/

    }

}
