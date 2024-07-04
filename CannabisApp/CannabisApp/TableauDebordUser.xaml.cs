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

namespace CannabisApp
{
    /// <summary>
    /// Logique d'interaction pour TableauDebordUser.xaml
    /// </summary>
    public partial class TableauDebordUser : Page
    {
        private string Username;
        public TableauDebordUser(string username)
        {
            InitializeComponent();
            Username = username;
            UsernameTextBlock.Text = Username;
           
        }


        private void AjouterPlante_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AjouterPlante());
        }

        private void AccederInventaire_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageInventaire());
        }

        private void VoirHistorique_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HisPlantes());
        }
        private void Deconnecter_Click(object sender, RoutedEventArgs e)
        {
            // Redirigez vers la page de connexion
            NavigationService.Navigate(new Page1());
        }
    }
}

