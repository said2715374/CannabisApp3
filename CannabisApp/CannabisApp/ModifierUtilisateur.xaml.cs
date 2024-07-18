using System.Windows;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Controls;

namespace CannabisApp
{
    public partial class ModifierUtilisateur : Page
    {
        int Num;
        string Nom;
        int selectedUser;

        public ModifierUtilisateur(int idUser, int num, string nom) {
            Num = num;
            Nom = nom;
            selectedUser = idUser;
            InitializeComponent();
            ChargerUsers(selectedUser);
           
        }





        private void ChargerUsers(int userId)
        {
            List<Roles> roLes = new List<Roles>();
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;Trusted_Connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Requête pour récupérer les informations de l'utilisateur
                    string userQuery = @"
                SELECT u.nom_utilisateur, u.mot_de_passe, r.id_role, r.nom_role
                FROM Utilisateurs u
                INNER JOIN Roles r ON u.id_role = r.id_role
                WHERE u.id_utilisateur = @UserId";

                    SqlCommand userCommand = new SqlCommand(userQuery, connection);
                    userCommand.Parameters.AddWithValue("@UserId", userId);

                    SqlDataReader userReader = userCommand.ExecuteReader();

                    if (userReader.Read())
                    {
                        // Récupérer les données de l'utilisateur
                        string nomUtilisateur = userReader.GetString(userReader.GetOrdinal("nom_utilisateur"));
                        string nomRole = userReader.GetString(userReader.GetOrdinal("nom_role"));
                        string mdp = userReader.GetString(userReader.GetOrdinal("mot_de_passe"));
                        int idRole = userReader.GetInt32(userReader.GetOrdinal("id_role"));

                        // Assigner les valeurs aux contrôles appropriés
                        NomUtilisateur.Text = nomUtilisateur;
                        MotDePasse.Text = mdp;

                        // Ajouter le rôle actuel de l'utilisateur
                        Roles currentUserRole = new Roles
                        {
                            IdRole = idRole,
                            NomRole = nomRole
                        };
                        roLes.Add(currentUserRole);
                    }
                    else
                    {
                        MessageBox.Show("Aucun utilisateur trouvé avec l'ID spécifié.");
                        return; // Arrêter si l'utilisateur n'est pas trouvé
                    }
                    userReader.Close();

                    // Requête pour récupérer tous les rôles
                    string rolesQuery = "SELECT id_role, nom_role FROM Roles";
                    SqlCommand rolesCommand = new SqlCommand(rolesQuery, connection);

                    SqlDataReader rolesReader = rolesCommand.ExecuteReader();

                    while (rolesReader.Read())
                    {
                        int idRole = rolesReader.GetInt32(rolesReader.GetOrdinal("id_role"));
                        string nomRole = rolesReader.GetString(rolesReader.GetOrdinal("nom_role"));

                        // Ajouter les rôles à la liste
                        Roles role = new Roles
                        {
                            IdRole = idRole,
                            NomRole = nomRole
                        };
                        roLes.Add(role);
                    }
                    rolesReader.Close();

                    // Configurer le ComboBox
                    RoleComboBox.ItemsSource = roLes;
                    RoleComboBox.DisplayMemberPath = "NomRole";
                    RoleComboBox.SelectedValuePath = "IdRole";
                    RoleComboBox.SelectedValue = roLes[0].IdRole; // Sélectionner le rôle de l'utilisateur par défaut
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur : " + ex.Message);
                }
            }
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GererUser(Num, Nom));

        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            if (Num == 1)
            {
                NavigationService.Navigate(new TableauDeBord(Nom));
            }
            else
            {
                NavigationService.Navigate(new TableauDebordUser(Nom));
            }
        }

        private void Modifier_Click(object sender, RoutedEventArgs e)
        {
            string nomUtilisateur = NomUtilisateur.Text;
            string motDePasse = MotDePasse.Text;
            int idRole = (int)RoleComboBox.SelectedValue;

            if (string.IsNullOrEmpty(nomUtilisateur) || string.IsNullOrEmpty(motDePasse) || idRole == 0)
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
                return;
            }

            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;Trusted_Connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"
                UPDATE Utilisateurs
                SET nom_utilisateur = @NomUtilisateur, 
                    mot_de_passe = @MotDePasse, 
                    id_role = @IdRole
                WHERE id_utilisateur = @UserId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NomUtilisateur", nomUtilisateur);
                    command.Parameters.AddWithValue("@MotDePasse", motDePasse);
                    command.Parameters.AddWithValue("@IdRole", idRole);
                    command.Parameters.AddWithValue("@UserId", selectedUser);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("L'utilisateur a été mis à jour avec succès.");
                    }
                    else
                    {
                        MessageBox.Show("Aucun utilisateur trouvé avec l'ID spécifié.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la mise à jour de l'utilisateur : " + ex.Message);
                }
            }
        }
    }
}
