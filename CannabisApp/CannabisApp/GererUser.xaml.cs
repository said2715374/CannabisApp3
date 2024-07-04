using Microsoft.VisualBasic.ApplicationServices;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CannabisApp
{
    public partial class GererUser : Page
    {
        private readonly AppDbContext _context;
        public GererUser()
        {
            InitializeComponent();
            _context = new AppDbContext();
            LoadUsers();
        }

        private void LoadUsers()
        {
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;User Id=LAPTOP-K1T841TP\\user;Trusted_Connection=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM utilisateurs";
                    SqlCommand command = new SqlCommand(query, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        ObservableCollection<Utilisateur> utilisateurs = new ObservableCollection<Utilisateur>();
                        while (reader.Read())
                        {
                            Utilisateur utilisateur = new Utilisateur
                            {
                                IdUtilisateur = reader.GetInt32(reader.GetOrdinal("id_utilisateur")),      
                                NomUtilisateur = reader.GetString(reader.GetOrdinal("nom_utilisateur")),   
                                MotDePasse = reader.GetString(reader.GetOrdinal("mot_de_passe")),          
                                IdRole = reader.GetInt32(reader.GetOrdinal("id_role"))                     
                            };
                            utilisateurs.Add(utilisateur); 
                        }

                        
                       UsersListView.ItemsSource = utilisateurs;


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des utilisateurs : " + ex.Message);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            // Code pour aller à la page d'accueil
        }

        private void AjouterUtilisateur_Click(object sender, RoutedEventArgs e)
        {
            // Code pour aller à la page AjouterUtilisateur
            NavigationService.Navigate(new AjouterUtilisateur());
        }

        private void UsersListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if( UsersListView.SelectedItem != null)
            {
                dynamic selectedItem = UsersListView.SelectedItem;
                int userId = selectedItem.IdUtilisateur;

                // Naviguer vers la page des détails de la plante
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.MainFrame.Navigate(new DetailsUser(userId));
                }
            }
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Code pour gérer le focus de la boîte de recherche
            if (SearchTextBox.Text == "Rechercher un utilisateur")
            {
                SearchTextBox.Text = "";
                SearchTextBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Code pour gérer la perte de focus de la boîte de recherche
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchTextBox.Text = "Rechercher un utilisateur";
                SearchTextBox.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Code pour filtrer les utilisateurs dans la liste en fonction de la recherche
        }

        private void UsersListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
