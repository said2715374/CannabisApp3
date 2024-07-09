using System;
using System.Windows;
using System.Windows.Controls;

namespace CannabisApp
{
    public partial class Archive : Page
    {
        private readonly AppDbContext _context;

        public Archive()
        {
            InitializeComponent();
            _context = new AppDbContext();
            LoadArchiveData();
        }

        private void LoadArchiveData()
        {
            
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ArchiveDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Gestionnaire de sélection (si nécessaire)
        }
    }
}
