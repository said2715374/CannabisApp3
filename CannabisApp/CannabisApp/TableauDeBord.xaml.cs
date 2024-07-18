using System.Windows;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.ComponentModel;
using System.Windows.Media;

namespace CannabisApp
{
    public partial class TableauDeBord : Page
    {
        int type = 1;
        int Count;
        int CountBonne;
        int CountPasBonne;
        int CountInactif;
        private  int TotalPlants;
        private int currentPlants;

        private string Username;
        public TableauDeBord(string username)
        {
            InitializeComponent();
            
            GetPlantesCount();
            GetPlantesCountBonne();
            GetPlantesCountPasBonne();
            GetPlantesCountInactif();
            GetCapacite();
            GetMaxCapacite();
            UpdateProgress(currentPlants);


            Username = username;
            UsernameTextBlock.Text = Username;
           
        }

        private void AjouterPlante_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AjouterPlante(Username, type));
        }

        private void AccederInventaire_Click(object sender, RoutedEventArgs e)
        {
           
            NavigationService.Navigate(new PageInventaire(Username, type));
        }

        private void VoirHistorique_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HisPlantes());
        }

        private void GererUser_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GererUser(type, Username));
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
                    string query = "SELECT COUNT(*) FROM plantes WHERE etat_sante = 1 AND nombre_plantes_actives = 1";

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
                    //totalPasBonne.Text = CountPasBonne.ToString();
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
                    string query = "SELECT COUNT(*) FROM plantes WHERE nombre_plantes_actives = 1";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Exécuter la requête et obtenir le nombre de lignes
                        int count = (int)command.ExecuteScalar();
                        currentPlants = count;
                    }
                    //totalInactif.Text = CountInactif.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération du nombre de plantes : " + ex.Message);
                // Valeur par défaut ou indication d'erreur
            }
        }

        private void UpdateProgress(int currentPlants)
        {
            double percentage = (TotalPlants-(double)currentPlants) / TotalPlants;
            double angle = percentage * 180;
            double radians = angle * (Math.PI / 180);
            double x = 200 + 190 * Math.Cos(radians);
            double y = 190 - 190 * Math.Sin(radians);

            ProgressSegment.Point = new Point(x, y);
            ProgressSegment.Size = new Size(190, 190);
            ProgressFigure.StartPoint = new Point(10, 190);
            ProgressSegment.IsLargeArc = angle > 180.0;

            TotalPlantesText.Text = $"{currentPlants} / {TotalPlants}";
            PercentageText.Text = $"{percentage * 100:F1}%";

            if (percentage < 0.15)
            {
                ProgressArc.Stroke = new SolidColorBrush(Colors.Red);
            }
            else if (percentage < 0.5)
            {
                ProgressArc.Stroke = new SolidColorBrush(Colors.Orange);
            }
            else
            {
                ProgressArc.Stroke = new SolidColorBrush(Colors.Green);
            }
        }

        private void GetCapacite()
        {
            // Chaîne de connexion à la base de données
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;Trusted_Connection=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Requête SQL pour compter le nombre de lignes dans la table 'plantes'
                    string query = "SELECT COUNT(*) FROM plantes WHERE nombre_plantes_actives = 1";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Exécuter la requête et obtenir le nombre de lignes
                        int count = (int)command.ExecuteScalar();
                        currentPlants = count;
                    }
                    //totalInactif.Text = CountInactif.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération du nombre de plantes : " + ex.Message);
                // Valeur par défaut ou indication d'erreur
            }
        }

        private void GetMaxCapacite()
        {
            // Chaîne de connexion à la base de données
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;Trusted_Connection=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Requête SQL pour compter le nombre de lignes dans la table 'plantes'
                    string query = "SELECT capacite from Capacit WHERE id = 1";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Exécuter la requête et obtenir le nombre de lignes
                        int count = (int)command.ExecuteScalar();
                        TotalPlants = count;
                    }
                    //totalInactif.Text = CountInactif.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la récupération du nombre de plantes : " + ex.Message);
                // Valeur par défaut ou indication d'erreur
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int NouvelleCapacite;

            if (int.TryParse(Ncapacite.Text, out NouvelleCapacite))
            {


                string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;Trusted_Connection=True;";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        // Requête SQL pour compter le nombre de lignes dans la table 'plantes'
                        string query = "UPDATE Capacit SET capacite = @capacite WHERE id = 1;";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            // Ajouter le paramètre @capacite à la commande SQL
                            command.Parameters.AddWithValue("@capacite", NouvelleCapacite);

                            // Exécuter la commande SQL
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show($"La capacité a été mise à jour avec succès : {NouvelleCapacite}");
                            }
                            else
                            {
                                MessageBox.Show("Aucune ligne mise à jour. Vérifiez l'ID spécifié.");
                            }
                        }
                        //totalInactif.Text = CountInactif.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la récupération du nombre de plantes : " + ex.Message);
                    // Valeur par défaut ou indication d'erreur
                }


                InitializeComponent();

                GetPlantesCount();
                GetPlantesCountBonne();
                GetPlantesCountPasBonne();
                GetPlantesCountInactif();
                GetCapacite();
                GetMaxCapacite();
                UpdateProgress(currentPlants);

                Ncapacite.Text = "";

            }
            else
            {
                MessageBox.Show("Veuillez saisir un nombre entier valide pour la capacité.");
            }
        }

        
    }
}
