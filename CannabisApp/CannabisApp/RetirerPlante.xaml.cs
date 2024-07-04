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
    /// Logique d'interaction pour RetirerPlante.xaml
    /// </summary>
    public partial class RetirerPlante : Page
    {
        private readonly AppDbContext _context;
        private readonly int _planteId;
        public RetirerPlante()
        {
            InitializeComponent();
        }
        private void LoadResponsables()
        {
            // Exemple de chargement des responsables depuis la base de données
            var responsables = new[] { "Kadija Houssein Youssouf", "Alexandre Tromas" }.ToList();
            ResponsableComboBox.ItemsSource = responsables;
        }

        private void Retirer_Click(object sender, RoutedEventArgs e)
        {
            var plante = _context.Plantes.FirstOrDefault(p => p.IdPlante == _planteId);
            if (plante != null)
            {
                _context.Plantes.Remove(plante);
                _context.SaveChanges();
                MessageBox.Show("Plante retirée avec succès.");
                NavigationService.GoBack();
            }
            else
            {
                MessageBox.Show("Plante non trouvée.");
            }
        }

        private void AjouterResponsable_Click(object sender, RoutedEventArgs e)
        {
            // Naviguer vers la page AjouterResponsable
            NavigationService.Navigate(new AjouterUtilisateur());
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            // NavigationService.Navigate(new TableauDebordUser());
        }
    }
}
