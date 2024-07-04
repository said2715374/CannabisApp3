using System;
using System.Linq;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CannabisApp
{
    public partial class AjouterPlante : Page
    {
        private readonly AppDbContext _context;

        public AjouterPlante()
        {
            InitializeComponent();
            _context = new AppDbContext();
            LoadProvenances();
            LoadEmplacements();
        }

        private void LoadProvenances()
        {
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;User Id=LAPTOP-K1T841TP\\user;Trusted_Connection=True;";
            List<provenances> provenances = new List<provenances>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM provenances";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        provenances provenance = new provenances
                        {
                            
                            id_provenance = reader.GetInt32(reader.GetOrdinal("id_provenance")), // Utilisez GetOrdinal pour obtenir les indices des colonnes
                            ville= reader.GetString(reader.GetOrdinal("ville")),
                            province = reader.GetString(reader.GetOrdinal("province")),
                            pays = reader.GetString(reader.GetOrdinal("pays"))
                        };
                        provenances.Add(provenance);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur : " + ex.Message);
                }
            }
            ProvenanceComboBox.ItemsSource = provenances;
            ProvenanceComboBox.DisplayMemberPath = "ville";
            ProvenanceComboBox.SelectedValuePath = "id_provenance";
        }

        private void LoadEmplacements()
        {
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;User Id=LAPTOP-K1T841TP\\user;Trusted_Connection=True;"; // Adaptez la chaîne de connexion si nécessaire
            List<Enterposage> enterposages = new List<Enterposage>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT* FROM Enterposage"; // Adaptez le nom de la table si nécessaire
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Enterposage enterposage = new Enterposage
                        {
                            id = reader.GetInt32(reader.GetOrdinal("id")), // Assurez-vous que les noms correspondent aux colonnes de la table
                            emplacement = reader.GetString(reader.GetOrdinal("emplacement"))
                        };
                        enterposages.Add(enterposage);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur : " + ex.Message);
                }
            }

            // Assigner les données au ComboBox
            EmplacementComboBox.ItemsSource = enterposages;
            EmplacementComboBox.DisplayMemberPath = "emplacement"; // Affiche "emplacement" dans le ComboBox
            EmplacementComboBox.SelectedValuePath = "id";
        }

        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            var description = Description.Text;
            var stade = ((ComboBoxItem)StadeComboBox.SelectedItem)?.Content.ToString() ?? string.Empty;
            var identification = Identification.Text;
            var selectedProvenance = (int?)ProvenanceComboBox.SelectedValue;
            var quantite = Quantite.Text;
            var dateExpiration = DateTime.Parse(DateExpiration.Text);
            var selectedEtatSanteItem = EtatSanteComboBox.SelectedItem as ComboBoxItem;
            int etatSante = selectedEtatSanteItem != null ? int.Parse(selectedEtatSanteItem.Tag.ToString()) : 0;
            var selectedEmplacement = (Enterposage)EmplacementComboBox.SelectedItem;
            int idEmplacement = selectedEmplacement.id;
            var note = NoteTextBox.Text;
            var CodeQR = GenerateUniqueCode();
            var Nombre_plantes_actives = true;
            int newPlanteId = 0;

            if (selectedProvenance == null || selectedEmplacement == null)
            {
                MessageBox.Show("Veuillez sélectionner une provenance et un emplacement.");
                return;
            }

            var nouvellePlante = new Plantes
            {
                
                id_Enterposage = idEmplacement,
                EtatSante = etatSante, // Exemple de valeur par défaut
                Quentite = quantite,
                DateExpiration = dateExpiration,
                CreeLe = DateTime.Now,
                IdProvenance = selectedProvenance.Value,
                Stade = stade,
                Identification = identification,
                Note = note,
                CodeQr = CodeQR,
                NombrePlantesActives = Nombre_plantes_actives


            };

            if (string.IsNullOrEmpty(description) || selectedProvenance == null || idEmplacement == null || dateExpiration == null)
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.");
                return;
            }

            // Chaîne de connexion (assurez-vous qu'elle est correcte pour votre base de données)
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;Trusted_Connection=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Requête INSERT pour ajouter une nouvelle plante
                    string query = @"
            
            INSERT INTO plantes 
            (description, stade, Identification, id_provenance, Quentité, date_expiration, etat_sante, id_Entreposage, note, code_qr, nombre_plantes_actives)
            VALUES 
            (@description, @stade, @identification, @idProvenance, @quantite, @dateExpiration, @etatSante, @idEmplacement, @note, @CodeQr, @Nombre_plantes_actives)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Ajouter les paramètres pour éviter les injections SQL
                        command.Parameters.AddWithValue("@description", description);
                        command.Parameters.AddWithValue("@stade", stade);
                        command.Parameters.AddWithValue("@identification", identification);
                        command.Parameters.AddWithValue("@idProvenance", selectedProvenance);
                        command.Parameters.AddWithValue("@quantite", quantite);
                        command.Parameters.AddWithValue("@dateExpiration", dateExpiration);
                        command.Parameters.AddWithValue("@etatSante", etatSante);
                        command.Parameters.AddWithValue("@idEmplacement", idEmplacement);
                        command.Parameters.AddWithValue("@note", note);
                        command.Parameters.AddWithValue("@CodeQr", CodeQR);
                        command.Parameters.AddWithValue("@Nombre_plantes_actives", Nombre_plantes_actives);
                        
                        

                        // Exécuter la requête
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Nouvelle plante ajoutée avec succès !");

                            // Récupérer l'ID de la plante nouvellement ajoutée
                            string queryGetId = "SELECT id_plante FROM plantes WHERE code_qr = @CodeQR";
                            using (SqlCommand getIdCommand = new SqlCommand(queryGetId, connection))
                            {
                                getIdCommand.Parameters.AddWithValue("@CodeQR", CodeQR);
                                newPlanteId = Convert.ToInt32(getIdCommand.ExecuteScalar());
                            }
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de l'ajout de la plante.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'ajout de la plante : " + ex.Message);
            }

            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new DetailsPlante(newPlanteId));
            }
        }

        private string GenerateUniqueCode()
        {
            Random random = new Random();

            // Générer 3 lettres aléatoires
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string randomLetters = new string(Enumerable.Repeat(letters, 3)
                                          .Select(s => s[random.Next(s.Length)]).ToArray());

            // Générer 3 chiffres aléatoires
            string numbers = "0123456789";
            string randomNumbers = new string(Enumerable.Repeat(numbers, 3)
                                          .Select(s => s[random.Next(s.Length)]).ToArray());

            // Concaténer lettres et chiffres
            string uniqueCode = randomLetters + randomNumbers;

            return uniqueCode;
        }


        private string ConvertImageToBase64(Bitmap image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            // NavigationService.Navigate(new TableauDebordUser());
        }

        private void AjouterEmplacement_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AjouterEmplacement());
        }

        private void EmplacementComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DateExpiration_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Importer_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new ImportPage());
            }
        }

        private void AjouterProvenance_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new AjouterProvenance());
            }
        }
    }
}
