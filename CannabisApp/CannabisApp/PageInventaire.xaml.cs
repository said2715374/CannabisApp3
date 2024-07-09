using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CannabisApp
{
    public partial class PageInventaire : Page
    {
        private readonly AppDbContext _context;
        private List<Plantes> _allPlantes;
        private ObservableCollection<plantes> _plantesCollection;
        private int _currentPage = 1;
        private const int PageSize = 4;

        public PageInventaire()
        {
            InitializeComponent();

            _context = new AppDbContext();
            LoadPlantes();


        }

        private void LoadPlantes()
        {
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;User Id=LAPTOP-K1T841TP\\user;Trusted_Connection=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM plantes";
                    SqlCommand command = new SqlCommand(query, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        ObservableCollection<plantes> Plantes = new ObservableCollection<plantes>();
                        while (reader.Read())
                        {
                            plantes plante = new plantes
                            {
                                id_plante = reader.GetInt32(reader.GetOrdinal("id_plante")),
                                identification = reader.GetString(reader.GetOrdinal("Identification")),
                                id_Enterposage = reader.GetInt32(reader.GetOrdinal("id_Entreposage")),
                                code_qr = reader.GetString(reader.GetOrdinal("code_qr")),
                                id_provenance = reader.GetInt32(reader.GetOrdinal("id_provenance")),
                                etat_sante = reader.GetInt32(reader.GetOrdinal("etat_sante")),
                                nombre_plantes_actives = reader.GetBoolean(reader.GetOrdinal("nombre_plantes_actives")),
                                date_expiration = reader.GetDateTime(reader.GetOrdinal("date_expiration")),
                                cree_le = reader.GetDateTime(reader.GetOrdinal("cree_le")),
                                stade = reader.GetString(reader.GetOrdinal("Stade")),
                                Quentite = reader.GetString(reader.GetOrdinal("Quentité")),
                                Note = reader.GetString(reader.GetOrdinal("Note")),
                                

                            };
                            Plantes.Add(plante);
                        }


                        PlantesListView.ItemsSource = Plantes;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des plantes : " + ex.Message);
            }
        }

        private void DisplayPage()
        {
            var plantesToShow = _allPlantes.Skip((_currentPage - 1) * PageSize).Take(PageSize).ToList();
            PlantesListView.ItemsSource = plantesToShow.Select(p => new
            {
                p.IdPlante,

                ProvenanceInfo = $"{p.Provenance.Ville}, {p.Provenance.Province}",
                p.Stade,
                QRCodeImage = p.CodeQr // Assurez-vous que cela convertit l'image correctement
            });
            UpdatePageNumber();
        }

        private void UpdatePageNumber()
        {

        }

        private void PagePrecedente_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                DisplayPage();
            }
        }

        private void PageSuivante_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < (_allPlantes.Count + PageSize - 1) / PageSize)
            {
                _currentPage++;
                DisplayPage();
            }
        }

        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AjouterPlante());
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchTextBox.Text = "Rechercher une plante";
                SearchTextBox.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }



        private void PlantesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (PlantesListView.SelectedItem != null)
            {
                dynamic selectedItem = PlantesListView.SelectedItem;
                int planteId = selectedItem.id_plante;

                // Naviguer vers la page des détails de la plante
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.MainFrame.Navigate(new DetailsPlante(planteId));
                }
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            
        }
        private void Archive_Click(object sender, RoutedEventArgs e)
        {
            // Navigation vers la page des archives
            NavigationService.Navigate(new Archive());
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //    string searchText = SearchTextBox.Text.Trim();
            //    Debug.WriteLine("Search Text: " + searchText); // Affiche le texte de recherche dans la fenêtre de sortie

            //    if (string.IsNullOrEmpty(searchText) || searchText == "Rechercher une plante")
            //    {
            //        LoadPlantes(); // Charger toutes les plantes si le texte est vide ou égal à "Rechercher une plante"
            //    }
            //    else
            //    {
            //        try
            //        {
            //            // Filtrer les plantes en fonction de l'identification contenant le texte de recherche
            //            var filteredPlantes = _plantesCollection
            //                .Where(p => p.identification.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            //                .ToList();

            //            if (filteredPlantes.Any())
            //            {
            //                PlantesListView.ItemsSource = new ObservableCollection<plantes>(filteredPlantes);
            //            }
            //            else
            //            {
            //                MessageBox.Show("Aucune plante trouvée pour la recherche : " + searchText);
            //                // Charger toutes les plantes si aucune plante ne correspond à la recherche
            //                LoadPlantes();
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show("Erreur lors de la recherche des plantes : " + ex.Message);
            //        }
            //    }
            //}
        }

        private void PlantesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
