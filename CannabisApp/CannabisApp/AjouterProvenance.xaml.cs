using System;
using System.Collections.Generic;
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

namespace CannabisApp
{
    /// <summary>
    /// Logique d'interaction pour AjouterProvenance.xaml
    /// </summary>
    public partial class AjouterProvenance : Page
    {
        public AjouterProvenance()
        {
            InitializeComponent();
        }

        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer les valeurs des champs de texte
            string ville = VilleTextBox.Text;
            string province = ProvinceTextBox.Text;
            string pays = PaysTextBox.Text;

            // Vérifier si tous les champs obligatoires sont remplis
            if (string.IsNullOrEmpty(ville) || string.IsNullOrEmpty(province) || string.IsNullOrEmpty(pays))
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.");
                return;
            }

            // Créer une nouvelle instance de la classe provenances avec les données saisies
            provenances nouvelleProvenance = new provenances
            {
                ville = ville,
                province = province,
                pays = pays
            };

            // Chaîne de connexion (assurez-vous qu'elle est correcte pour votre base de données)
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;Trusted_Connection=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Requête INSERT pour ajouter une nouvelle provenance
                    string query = @"
            INSERT INTO provenances 
            (ville, province, pays)
            VALUES 
            (@ville, @province, @pays)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Ajouter les paramètres pour éviter les injections SQL
                        command.Parameters.AddWithValue("@ville", nouvelleProvenance.ville);
                        command.Parameters.AddWithValue("@province", nouvelleProvenance.province);
                        command.Parameters.AddWithValue("@pays", nouvelleProvenance.pays);

                        // Exécuter la requête
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Nouvelle provenance ajoutée avec succès !");
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de l'ajout de la provenance.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'ajout de la provenance : " + ex.Message);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
