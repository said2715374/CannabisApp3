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
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualBasic.ApplicationServices;

namespace CannabisApp
{
    public partial class AjouterPlante : Page
    {
        private readonly AppDbContext _context;
        int Num;
        string Nom;
        int IdUser;

        public AjouterPlante(string name, int num)
        {
            Num = num;
            Nom = name;
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
            var identification = CheckName(Identification.Text);
            var selectedProvenance = (int?)ProvenanceComboBox.SelectedValue;
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
                EtatSante = etatSante,
                CreeLe = DateTime.Now,
                IdProvenance = selectedProvenance.Value,
                Stade = stade,
                Identification = identification,
                Note = note,
                CodeQr = CodeQR,
                NombrePlantesActives = Nombre_plantes_actives


            };

            if (string.IsNullOrEmpty(description) || selectedProvenance == null || idEmplacement == null )
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
            (description, stade, Identification, id_provenance,  etat_sante, id_Entreposage, note, code_qr, nombre_plantes_actives)
            VALUES 
            (@description, @stade, @identification, @idProvenance,  @etatSante, @idEmplacement, @note, @CodeQr, @Nombre_plantes_actives)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Ajouter les paramètres pour éviter les injections SQL
                        command.Parameters.AddWithValue("@description", description);
                        command.Parameters.AddWithValue("@stade", stade);
                        command.Parameters.AddWithValue("@identification", identification);
                        command.Parameters.AddWithValue("@idProvenance", selectedProvenance);
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
                            AddHistorique(newPlanteId, GetIdByName(Nom));
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
                mainWindow.MainFrame.Navigate(new DetailsPlante(newPlanteId, Nom, Num ));
            }
        }

        private string CheckName(String name)
        {
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;Trusted_Connection=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Requête SQL pour sélectionner les noms de plantes commençant par @name
                    string query = "SELECT Identification FROM plantes WHERE Identification LIKE @name ;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", name + "%");

                        List<string> existingNames = new List<string>();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string existingName = reader.GetString(0);
                                existingNames.Add(existingName);
                            }
                        }

                        // Trouver le prochain numéro disponible
                        int nextNumber = 1;
                        while (existingNames.Contains(name + nextNumber))
                        {
                            nextNumber++;
                        }

                        // Nom final à retourner
                        name = name + nextNumber.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur dans le nom de plante : " + ex.Message);
                // Valeur par défaut ou indication d'erreur
            }

            return name;
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

            // Vérifier si le code généré existe déjà
            if (CodeExistsInDatabase(uniqueCode))
            {
                // Si le code existe, générer un nouveau code récursivement
                uniqueCode = GenerateUniqueCode(); // Appel récursif pour générer un nouveau code unique
            }

            return uniqueCode;
        }

        private bool CodeExistsInDatabase(string code)
        {
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;Trusted_Connection=True;";
            bool exists = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Requête SQL pour vérifier si le code existe dans la base de données
                    string query = "SELECT COUNT(*) FROM plantes WHERE code_qr = @code;";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@code", code);
                        int count = (int)command.ExecuteScalar();
                        exists = count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la vérification du code : " + ex.Message);
                // Gestion de l'erreur selon vos besoins
            }

            return exists;
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
            if (Num == 1)
            {
                NavigationService.Navigate(new TableauDeBord(Nom));
            }
            else
            {
                NavigationService.Navigate(new TableauDebordUser(Nom));
            }
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

        private void AjouterEmplacement_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AjouterEmplacement(Nom, Num));
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
                mainWindow.MainFrame.Navigate(new ImportPage(Nom, Num));
            }
        }

        private void AjouterProvenance_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new AjouterProvenance(Nom, Num));
            }
        }
        private void AddHistorique(int Idplante, int Iduser)
        {
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;Trusted_Connection=True;";
            bool exists = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Requête SQL pour vérifier si le code existe dans la base de données
                    string query = "INSERT INTO historique_plantes (id_plante, action, timestamp, id_utilisateur) VALUES (@IdPlante, 'Ajouté', GETDATE(), @IdUser);";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdPlante", Idplante);
                        command.Parameters.AddWithValue("@IdUser", Iduser);
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
