using System.Collections.ObjectModel;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Controls;
using Microsoft.VisualBasic.ApplicationServices;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CannabisApp
{
    public partial class DetailsUser : System.Windows.Controls.Page
    {
        string NomUtilisateur;
        string NomRole;
        int idUser;
        private readonly AppDbContext _context;
        public DetailsUser(int iduser)
        {
            InitializeComponent();
            LoadUtilisateurDetails(iduser);
            _context = new AppDbContext();
            idUser = iduser;
            
            // Set the details of the selected user
            //NomUtilisateurText.Text = selectedUser.nom_utilisateur;
            //RoleText.Text = selectedUser.id_role.ToString();
        }



        private void LoadUtilisateurDetails(int idUtilisateur)
        {
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;Trusted_Connection=True;";
            Utilisateur utilisateur = null;
            roles role = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Requête SQL pour récupérer les informations de l'utilisateur
                    string query = @"
                        SELECT u.id_utilisateur, u.nom_utilisateur, u.mot_de_passe, u.id_role,
                               r.nom_role
                        FROM Utilisateurs u
                        INNER JOIN Roles r ON u.id_role = r.id_role
                        WHERE u.id_utilisateur = @idUtilisateur"; 

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@idUtilisateur", idUtilisateur);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        utilisateur = new Utilisateur
                        {
                            IdUtilisateur = reader.GetInt32(reader.GetOrdinal("id_utilisateur")),
                            NomUtilisateur = reader.GetString(reader.GetOrdinal("nom_utilisateur")),
                            MotDePasse = reader.GetString(reader.GetOrdinal("mot_de_passe")),
                            IdRole = reader.GetInt32(reader.GetOrdinal("id_role"))
                        };
                        role = new roles
                        {
                            nom_role = reader.GetString(reader.GetOrdinal("nom_role"))
                        };

                        // Mettre à jour les contrôles TextBlock avec les informations récupérées
                        NomUtilisateurText.Text = utilisateur.NomUtilisateur.ToString();

                        RoleText.Text = role.nom_role.ToString();

                        NomUtilisateur = reader.GetString(reader.GetOrdinal("nom_utilisateur"));
                       NomRole = reader.GetString(reader.GetOrdinal("nom_role"));

                    }

                    reader.Close(); // Fermer le DataReader après utilisation
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la récupération des détails de l'utilisateur : " + ex.Message);
                }
            }
        }



        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new TableauDebordUser());
        }

        private void Modifier_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new ModifierUtilisateur(idUser));
            }


        }

        private void Retirer_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;User Id=LAPTOP-K1T841TP\\user;Trusted_Connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"
                DELETE utilisateurs where id_utilisateur = @Iduser";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Iduser", idUser);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Utilisateur retirer avec succès !");
                    }
                    else
                    {
                        MessageBox.Show("Aucune utilisateur mise à jour.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la mise à jour du nombre de plantes actives : " + ex.Message);
                }
                
            }
        }

        private void AjouterAutre_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new AjouterUtilisateur());
            }
        }

        private void VoirHistorique_Click(object sender, RoutedEventArgs e)
        {
            // Code pour voir l'historique
        }
    }
}
