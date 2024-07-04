using System;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Controls;

namespace CannabisApp
{
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void Connexion_Click(object sender, RoutedEventArgs e)
        {
            string username = nomDutilisateur.Text;
            string password = motDePasse.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Veuillez entrer un nom d'utilisateur et un mot de passe.");
                return;
            }

            // Connexion à la base de données
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;User Id=LAPTOP-K1T841TP\\user;Trusted_Connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM utilisateurs WHERE nom_utilisateur = @username AND mot_de_passe = @password";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    int count = (int)command.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Connexion réussie !");
                        string query1 = "SELECT id_role FROM utilisateurs WHERE nom_utilisateur = @username";
                        SqlCommand command1 = new SqlCommand(query1, connection);
                        command1.Parameters.AddWithValue("@username", username);
                        int roleID = (int)command1.ExecuteScalar();

                        if (roleID == 1) {
                            if (Application.Current.MainWindow is MainWindow mainWindow)
                            {
                                mainWindow.MainFrame.Navigate(new TableauDeBord(username) );
                            }
                        }
                        else if (roleID == 3)
                        {
                            if (Application.Current.MainWindow is MainWindow mainWindow)
                            {
                                mainWindow.MainFrame.Navigate(new TableauDebordUser(username));
                            }
                        }


                    }
                    else
                    {
                        MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur : " + ex.Message);
                }
            }
        }
    

        private void Quitter_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
