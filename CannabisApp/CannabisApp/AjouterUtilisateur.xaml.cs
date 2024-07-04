using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CannabisApp
{
    public partial class AjouterUtilisateur : Page
    {
        private readonly AppDbContext _context;

        public AjouterUtilisateur()
        {
            InitializeComponent();
            _context = new AppDbContext();
            LoadRoles();
        }

        private void LoadRoles()
        {
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;Trusted_Connection=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT id_role, nom_role FROM roles";
                    SqlCommand command = new SqlCommand(query, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        ObservableCollection<Roles> roles = new ObservableCollection<Roles>();
                        while (reader.Read())
                        {
                            Roles role = new Roles
                            {
                                IdRole = reader.GetInt32(reader.GetOrdinal("id_role")),
                                NomRole = reader.GetString(reader.GetOrdinal("nom_role")).Trim()
                            };
                            roles.Add(role);
                        }

                        RoleComboBox.ItemsSource = roles;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement des rôles : " + ex.Message);
            }

        }

        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            var nomUtilisateur = NomUtilisateur.Text;
            var motDePasse = MotDePasse.Password;
            var selectedRole = (Roles)RoleComboBox.SelectedItem;

            if (string.IsNullOrEmpty(nomUtilisateur) || string.IsNullOrEmpty(motDePasse) || selectedRole == null)
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
                return;
            }

            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;Trusted_Connection=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO utilisateurs (nom_utilisateur, mot_de_passe, id_role) VALUES (@nomUtilisateur, @motDePasse, @selectedRoleId)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@nomUtilisateur", nomUtilisateur);
                    command.Parameters.AddWithValue("@motDePasse", motDePasse);
                    command.Parameters.AddWithValue("@selectedRoleId", selectedRole.IdRole); // Utilisation de l'ID du rôle sélectionné

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Nouvel utilisateur ajouté avec succès !");
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de l'ajout de l'utilisateur.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'ajout de l'utilisateur : " + ex.Message);
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
        
        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}