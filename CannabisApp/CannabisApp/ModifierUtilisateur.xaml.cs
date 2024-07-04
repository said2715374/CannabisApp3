using System.Windows;
using System.Windows.Controls;

namespace CannabisApp
{
    public partial class ModifierUtilisateur : Page
    {
        private utilisateur selectedUser;

        public ModifierUtilisateur(int idUser) { 
            InitializeComponent();

            //selectedUser = user;

            // Remplir les champs avec les informations de l'utilisateur sélectionné
            NomUtilisateur.Text = selectedUser.nom_utilisateur;
            // Définir le mot de passe si nécessaire
            RoleComboBox.SelectedValue = selectedUser.id_role;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();

        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            // Code pour aller à la page d'accueil
        }

        private void Modifier_Click(object sender, RoutedEventArgs e)
        {
            // Code pour modifier l'utilisateur
            selectedUser.nom_utilisateur = NomUtilisateur.Text;
            selectedUser.mot_de_passe = MotDePasse.Password;
            selectedUser.id_role = (int)RoleComboBox.SelectedValue;

            // Enregistrer les modifications dans la base de données ou autre stockage
        }
    }
}
