using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
namespace CannabisApp
{
    public partial class AjouterEmplacement : Page
    {
        private readonly AppDbContext _context;
        int Num;
        string Nom;

        public AjouterEmplacement(string name, int num)
        {
            Num = num;
            Nom = name;
            InitializeComponent();
            _context = new AppDbContext();

            // Gérer le texte de l'espace réservé
            NomEmplacement.GotFocus += RemovePlaceholderText;
            NomEmplacement.LostFocus += ShowPlaceholderText;
        }

        private void RemovePlaceholderText(object sender, RoutedEventArgs e)
        {
            PlaceholderTextBlock.Visibility = Visibility.Collapsed;
        }

        private void ShowPlaceholderText(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NomEmplacement.Text))
            {
                PlaceholderTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            string nomEmplacement = NomEmplacement.Text.Trim();


            if (string.IsNullOrEmpty(nomEmplacement))
            {
                MessageBox.Show("Veuillez entrer un nom d'emplacement.");
                return;
            }

            // Chaîne de connexion à la base de données
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;Trusted_Connection=True;";

            try
            {
                int newPlanteId = 0;
                int newEnterposageId = 0;

                // Étape 1 : Récupérer l'ID de la dernière plante insérée
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Requête pour récupérer l'ID de la dernière plante insérée
                    string queryGetLastPlanteId = "SELECT MAX(id) FROM Enterposage";

                    using (SqlCommand command = new SqlCommand(queryGetLastPlanteId, connection))
                    {
                        object result = command.ExecuteScalar();
                        newPlanteId = result != DBNull.Value ? Convert.ToInt32(result) + 1 : 1;
                    }
                }

                // Étape 2 : Insérer un nouvel emplacement dans la table Enterposage
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Requête INSERT pour ajouter un nouvel emplacement
                    string insertQuery = "INSERT INTO Enterposage (id, emplacement) VALUES (@newPlanteId, @nomEmplacement);";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@nomEmplacement", nomEmplacement);
                        command.Parameters.AddWithValue("@newPlanteId", newPlanteId);

                        // Exécuter la requête et récupérer l'ID inséré
                        newEnterposageId = Convert.ToInt32(command.ExecuteScalar());
                    }
                }

                NavigationService.Navigate(new AjouterPlante(Nom, Num ));

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'ajout de la plante : " + ex.Message);
            }
        }
    }
}
