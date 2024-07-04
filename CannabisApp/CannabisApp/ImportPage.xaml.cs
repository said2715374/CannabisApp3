using OfficeOpenXml;
using System;
using System.Data.SqlClient;
using Microsoft.Win32;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
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
    /// Logique d'interaction pour ImportPage.xaml
    /// </summary>
    public partial class ImportPage : Page
    {
        public ImportPage()
        {
            InitializeComponent();
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

                        for (int row = 2;  ; row++) // Commence à la deuxième ligne (suppose que la première ligne est l'en-tête)
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
                                identification = worksheet.Cells[row, 3].Value.ToString(),
                                id_provenance = Convert.ToInt32(worksheet.Cells[row, 4].Value),
                                description = Convert.ToString(worksheet.Cells[row, 5].Value),
                                code_qr = codeQR,
                                stade = worksheet.Cells[row, 6].Value.ToString(),
                                id_Enterposage = Convert.ToInt32(worksheet.Cells[row, 7].Value),
                                nombre_plantes_actives = etat, 
                                Note = worksheet.Cells[row, 9].Value.ToString(),
                                Quentite =Convert.ToString( worksheet.Cells[row, 10].Value),
                                date_expiration = Convert.ToDateTime(worksheet.Cells[row,11].Value)




                            };
                             

                            // Insérer les données dans la base de données avec une commande SQL
                            string insertQuery = "INSERT INTO plantes (Identification, code_qr, id_provenance, etat_sante, cree_le, Stade, Note, id_Entreposage, nombre_plantes_actives, Description, Quentité, date_expiration) " +
                         "VALUES (@Identification, @CodeQr, @id_provenance, @etat_sante, @cree_le,  @stade, @Note, @id_Enterposage, @Etat, @description, @quentite, @EXP)";

                            SqlCommand command = new SqlCommand(insertQuery, connection);
                            command.Parameters.AddWithValue("@Identification", plante.identification);
                            command.Parameters.AddWithValue("@CodeQr", plante.code_qr );
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
                            command.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Données importées avec succès !");
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

        private void Home_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ImportData_Click(object sender, RoutedEventArgs e)
        {
            ImportDataFromExcel(SelectedFilePathTextBlock.Text);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
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
    }
}
