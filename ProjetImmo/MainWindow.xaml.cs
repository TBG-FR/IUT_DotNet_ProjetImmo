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
using System.Windows.Shapes;
using ProjetImmo.Core.Models;
using ProjetImmo.WPF;

namespace ProjetImmo
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {

            InitializeComponent();

            /*
            MainWindow = NavigationService.GetWindow<MainWindow, MainViewModel>();
            MainWindow.Show();

            this.DataContext = NavigationService.GetViewModelInstance()*/

            /* Pas de code ici, tout dans le ViewModel 'MainViewModel' */

        }

    }
}
