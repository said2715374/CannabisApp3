using System;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using OfficeOpenXml;
using System.IO;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.EMMA;

namespace CannabisApp
{
    public partial class ImportPage : Page
    {
        int Num;
        string Nom;
        int IdUser;
        public ImportPage(string name, int type)
        {
            Num = type;
            Nom = name;
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AjouterPlante(Nom, Num));
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

        private void ImportDataFromExcel(string filePath)
        {

            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Pour une utilisation non commerciale

                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Première feuille de calcul

                    int rowCount = worksheet.Dimension.Rows;
                    List<Plantes> plantesToImport = new List<Plantes>();

                    // Connexion à la base de données avec ADO.NET
                    string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;User Id=LAPTOP-K1T841TP\\user;Trusted_Connection=True;";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        for (int row = 2; ; row++) // Commence à la deuxième ligne (suppose que la première ligne est l'en-tête)
                        {
                            if (string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Text))
                            {
                                break; // Sortir de la boucle si la cellule est vide
                            }
                            var codeQR = GenerateUniqueCode();
                            bool etat;
                            if (worksheet.Cells[row, 8].Value != null && worksheet.Cells[row, 8].Value.ToString() == "1")
                            {
                                etat = true;
                            }
                            else
                            {
                                etat = false;
                            }
                            var plante = new plantes


                            {
                                etat_sante = Convert.ToInt32(worksheet.Cells[row, 1].Value),
                                cree_le = Convert.ToDateTime(worksheet.Cells[row, 2].Value),
                                identification = CheckName(worksheet.Cells[row, 3].Value.ToString()),
                                id_provenance = Convert.ToInt32(worksheet.Cells[row, 4].Value),
                                description = Convert.ToString(worksheet.Cells[row, 5].Value),
                                code_qr = codeQR,
                                stade = worksheet.Cells[row, 6].Value.ToString(),
                                id_Enterposage = Convert.ToInt32(worksheet.Cells[row, 7].Value),
                                nombre_plantes_actives = etat,
                                Note = worksheet.Cells[row, 9].Value.ToString(),
                                Quentite = Convert.ToString(worksheet.Cells[row, 10].Value),
                                date_expiration = Convert.ToDateTime(worksheet.Cells[row, 11].Value)




                            };


                            // Insérer les données dans la base de données avec une commande SQL
                            string insertQuery = @"
                                            INSERT INTO plantes (Identification, code_qr, id_provenance, etat_sante, cree_le, Stade, Note, id_Entreposage, nombre_plantes_actives, Description, Quentité, date_expiration) 
                                            VALUES (@Identification, @CodeQr, @id_provenance, @etat_sante, @cree_le, @stade, @Note, @id_Enterposage, @Etat, @description, @quentite, @EXP);
                                            SELECT SCOPE_IDENTITY();";

                            SqlCommand command = new SqlCommand(insertQuery, connection);
                            command.Parameters.AddWithValue("@Identification", plante.identification);
                            command.Parameters.AddWithValue("@CodeQr", plante.code_qr);
                            command.Parameters.AddWithValue("@id_provenance", plante.id_provenance);
                            command.Parameters.AddWithValue("@etat_sante", plante.etat_sante);
                            command.Parameters.AddWithValue("@cree_le", plante.cree_le);
                            command.Parameters.AddWithValue("@stade", plante.stade);
                            command.Parameters.AddWithValue("@Note", plante.Note);
                            command.Parameters.AddWithValue("@id_Enterposage", plante.id_Enterposage);
                            command.Parameters.AddWithValue("@Etat", plante.nombre_plantes_actives);
                            command.Parameters.AddWithValue("@description", plante.description);
                            command.Parameters.AddWithValue("@quentite", plante.Quentite);
                            command.Parameters.AddWithValue("@EXP", plante.date_expiration);

                            // Exécuter la commande
                            //command.ExecuteNonQuery();

                            int id = Convert.ToInt32(command.ExecuteScalar());

                            AddHistorique(id, GetIdByName(Nom));
                        }
                    }

                    MessageBox.Show("Données importées avec succès !");
                    NavigationService.Navigate(new AjouterPlante(Nom, Num));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'importation des données : " + ex.Message);
            }
        }

        private void BrowseExcelFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fichiers Excel|*.xlsx;*.xls"; // Filtre pour les fichiers Excel

            if (openFileDialog.ShowDialog() == true)
            {
                // Obtenez le chemin complet du fichier sélectionné
                string filePath = openFileDialog.FileName;

                // Vous pouvez maintenant utiliser le chemin `filePath` pour votre logique d'importation
                // Par exemple, afficher le chemin dans un TextBox ou effectuer une autre action avec le fichier sélectionné
                SelectedFilePathTextBlock.Text = filePath; // 
            }
        }

        private void ImportData_Click(object sender, RoutedEventArgs e)
        {
            ImportDataFromExcel(SelectedFilePathTextBlock.Text);
        }

        private void Border_DragLeave(object sender, DragEventArgs e)
        {
            // Optional: Handle visual feedback when the drag leaves the border
        }

        private void Border_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    SelectedFilePathTextBlock.Text = files[0];
                }
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
                    string query = "SELECT Identification FROM plantes WHERE Identification LIKE @name;";

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
                    string query = "INSERT INTO historique_plantes (id_plante, action, timestamp, id_utilisateur) VALUES (@IdPlante, 'Importé', GETDATE(), @IdUser);";
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
