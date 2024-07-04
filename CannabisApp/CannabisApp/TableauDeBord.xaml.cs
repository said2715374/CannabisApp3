using System.Windows;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.ComponentModel;

namespace CannabisApp
{
    public partial class TableauDeBord : Page
    {
        int Count;
        int CountBonne;
        int CountPasBonne;
        int CountInactif;

        private string Username;
        public TableauDeBord(string username)
        {
            InitializeComponent();
            GetPlantesCount();
            GetPlantesCountBonne();
            GetPlantesCountPasBonne();
            GetPlantesCountInactif();


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

        private void GererUser_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GererUser());
        }

        private void Deconnecter_Click(object sender, RoutedEventArgs e)
        {
            // Redirigez vers la page de connexion
            NavigationService.Navigate(new Page1());
        }
        public void GetPlantesCount()
        {
            
            // Chaîne de connexion à la base de données
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;Trusted_Connection=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Requête SQL pour compter le nombre de lignes dans la table 'plantes'
                    string query = "SELECT COUNT(*) FROM plantes";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Exécuter la requête et obtenir le nombre de lignes
                         int count = (int)command.ExecuteScalar();
                        Count = count;
                    }
                    total.Text = Count.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération du nombre de plantes : " + ex.Message);
                 // Valeur par défaut ou indication d'erreur
            }
        }

        public void GetPlantesCountBonne()
        {

            // Chaîne de connexion à la base de données
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;Trusted_Connection=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Requête SQL pour compter le nombre de lignes dans la table 'plantes'
                    string query = "SELECT COUNT(*) FROM plantes WHERE etat_sante = 4";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Exécuter la requête et obtenir le nombre de lignes
                        int count = (int)command.ExecuteScalar();
                        CountBonne = count;
                    }
                    totalBonne.Text = CountBonne.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération du nombre de plantes : " + ex.Message);
                // Valeur par défaut ou indication d'erreur
            }
        }

        public void GetPlantesCountPasBonne()
        {

            // Chaîne de connexion à la base de données
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;Trusted_Connection=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Requête SQL pour compter le nombre de lignes dans la table 'plantes'
                    string query = "SELECT COUNT(*) FROM plantes WHERE etat_sante = 1";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Exécuter la requête et obtenir le nombre de lignes
                        int count = (int)command.ExecuteScalar();
                        CountPasBonne = count;
                    }
                    totalPasBonne.Text = CountPasBonne.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération du nombre de plantes : " + ex.Message);
                // Valeur par défaut ou indication d'erreur
            }
        }
        public void GetPlantesCountInactif()
        {

            // Chaîne de connexion à la base de données
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;Trusted_Connection=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Requête SQL pour compter le nombre de lignes dans la table 'plantes'
                    string query = "SELECT COUNT(*) FROM plantes WHERE nombre_plantes_actives = 0";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Exécuter la requête et obtenir le nombre de lignes
                        int count = (int)command.ExecuteScalar();
                        CountInactif = count;
                    }
                    totalInactif.Text = CountInactif.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération du nombre de plantes : " + ex.Message);
                // Valeur par défaut ou indication d'erreur
            }
        }

    }
}
