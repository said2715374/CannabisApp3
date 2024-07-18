using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
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
using Microsoft.VisualBasic.ApplicationServices;

namespace CannabisApp
{
    /// <summary>
    /// Logique d'interaction pour RetirerPlante.xaml
    /// </summary>
    public partial class RetirerPlante : Page
    {
        int IdUser;
        private readonly AppDbContext _context;
         int _planteId;
        int Num;
        string Nom;
        public RetirerPlante(int IdPlante, string nom, int num)
        {
            Num = num;
            Nom = nom;
            _planteId = IdPlante;
            InitializeComponent();
            LoadResponsables();
        }
        private void LoadResponsables()
        {
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Requête SQL pour récupérer les noms des utilisateurs avec le rôle spécifié
                    string query = @"
                SELECT u.nom_utilisateur
                FROM Utilisateurs u
                INNER JOIN Roles r ON u.id_role = r.id_role
                WHERE r.id_role = @idUtilisateur";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@idUtilisateur", 4);

                    SqlDataReader reader = command.ExecuteReader();

                    // Liste pour stocker les noms des utilisateurs
                    List<string> responsables = new List<string>();

                    // Lire les données du lecteur
                    while (reader.Read())
                    {
                        string nomUtilisateur = reader["nom_utilisateur"].ToString();
                        responsables.Add(nomUtilisateur);
                    }

                    // Assigner la liste de noms d'utilisateurs au ComboBox
                    ResponsableComboBox.ItemsSource = responsables;

                    // Fermer le lecteur
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la récupération des détails de l'utilisateur : " + ex.Message);
                }
            }
        }

        private void Retirer_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer la raison du retrait sélectionnée
            var selectedRaisonItem = RaisonRetraitComboBox.SelectedItem as ComboBoxItem;
            string raison = selectedRaisonItem != null ? selectedRaisonItem.Content.ToString() : string.Empty;

            // Récupérer le responsable de la décontamination sélectionné
            string responsable = ResponsableComboBox.SelectedItem != null ? ResponsableComboBox.SelectedItem.ToString() : string.Empty;

            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;User Id=LAPTOP-K1T841TP\\user;Trusted_Connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"
                UPDATE plantes
                SET nombre_plantes_actives = 0,
                date_expiration = @date_expiration
                WHERE id_plante = @planteId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@planteId", _planteId);
                    command.Parameters.AddWithValue("@date_expiration", DateTime.Parse(DateExpiration.Text));
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        AddHistorique(_planteId, GetIdByName(responsable), raison, DateTime.Parse(DateExpiration.Text));
                        MessageBox.Show("Plantes retirer avec succès !");
                    }
                    else
                    {
                        MessageBox.Show("Aucune plante mise à jour.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la mise à jour du nombre de plantes actives : " + ex.Message);
                }
            }
        }

        private void AjouterResponsable_Click(object sender, RoutedEventArgs e)
        {
            // Naviguer vers la page AjouterResponsable
            NavigationService.Navigate(new AjouterUtilisateur(Num,Nom));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DetailsPlante(_planteId, Nom, Num));
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            // NavigationService.Navigate(new TableauDebordUser());
        }
        private void AddHistorique(int Idplante, int Iduser, string cause, DateTime date)
        {
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;Trusted_Connection=True;";
            bool exists = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Requête SQL pour vérifier si le code existe dans la base de données
                    string query = "INSERT INTO historique_plantes (id_plante, action, timestamp, id_utilisateur) VALUES (@IdPlante, @Action, @date, @IdUser);";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdPlante", Idplante);
                        command.Parameters.AddWithValue("@IdUser", Iduser);
                        command.Parameters.AddWithValue("@date", date);
                        command.Parameters.AddWithValue("@Action", $"Retirer, La cause est : {cause}");
                        command.ExecuteScalar();

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la vérification du code : " + ex.Message);
                // Gestion de l'erreur selon vos besoins
            }
        }
        private int GetIdByName(string Name)
        {
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;Trusted_Connection=True;";
            bool exists = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Requête SQL pour vérifier si le code existe dans la base de données
                    string query = "SELECT id_utilisateur from utilisateurs WHERE nom_utilisateur = @nom";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nom", Name);
                        int count = (int)command.ExecuteScalar();
                        IdUser = count;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la vérification du code : " + ex.Message);
                // Gestion de l'erreur selon vos besoins
            }

            return IdUser;
        }
    }
}
