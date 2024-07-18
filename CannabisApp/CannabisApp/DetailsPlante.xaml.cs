using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using CannabisApp.Converters;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CannabisApp
{
    public partial class DetailsPlante : Page 
    {
        int Num;
        string Nom;
        string NomPlantee;
        private readonly AppDbContext _context;
        private readonly int _planteId;

        public DetailsPlante(int planteId, string nom, int type)
        {
            Nom = nom;
            Num = type;
            InitializeComponent();
            _context = new AppDbContext();
            _planteId = planteId;
            LoadPlanteDetails(planteId);
        }

        private void LoadPlanteDetails(int planteId)
        {
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;User Id=LAPTOP-K1T841TP\\user;Trusted_Connection=True;";
            List<plantes> Plante = new List<plantes>();
           

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Requête combinée pour récupérer les informations de la plante et de sa provenance
                    string query = @"
                 SELECT p.id_plante, p.code_qr, p.id_provenance, p.etat_sante, 
                 p.cree_le, p.stade, p.Note, p.identification, p.id_Entreposage,p.nombre_plantes_actives,
                 pr.ville, pr.province, pr.pays,en.emplacement
                 FROM plantes p
                 INNER JOIN provenances pr ON p.id_provenance = pr.id_provenance
                 INNER JOIN Enterposage en ON p.id_Entreposage = en.id
                 WHERE p.id_plante = @planteId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@planteId", planteId);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        plantes plante = new plantes
                        {
                            id_plante = reader.GetInt32(reader.GetOrdinal("id_plante")),
                            code_qr = reader.GetString(reader.GetOrdinal("code_qr")),
                            id_provenance = reader.GetInt32(reader.GetOrdinal("id_provenance")),
                            etat_sante = reader.GetInt32(reader.GetOrdinal("etat_sante")),
                            cree_le = reader.GetDateTime(reader.GetOrdinal("cree_le")),
                            stade = reader.GetString(reader.GetOrdinal("stade")),
                            Note = reader.GetString(reader.GetOrdinal("Note")),
                            identification = reader.GetString(reader.GetOrdinal("identification")),
                            id_Enterposage = reader.GetInt32(reader.GetOrdinal("id_Entreposage")),
                        };
                        Plante.Add(plante);

                        NomPlantee = plante.identification;

                        // Mettez à jour les contrôles TextBlock avec les informations récupérées
                        NomPlante.Text = plante.identification;
                        EmplacementText.Text = reader.GetString(reader.GetOrdinal("emplacement"));
                        IdProvenanceText.Text = reader.GetString(reader.GetOrdinal("ville")); // Utilisation de la colonne 'ville' de la provenance
                        EtatSanteText.Text = plante.etat_sante.ToString();
                        CreeLeText.Text = plante.cree_le.ToShortDateString();
                        StadeText.Text = plante.stade;
                        bool etat = reader.GetBoolean(reader.GetOrdinal("nombre_plantes_actives"));
                        if (etat == true) {
                            IdentificationText.Text = "Actif";
                        }
                        else if (etat == false)
                        {
                            IdentificationText.Text = "Inactif";
                        }
                        QRCodeImageConverter converter = new QRCodeImageConverter();

                        try
                        {
                            // Récupérer le code QR en tant que chaîne
                            string codeQr = reader.GetString(reader.GetOrdinal("code_qr"));

                            // Utiliser le convertisseur pour générer l'image
                            BitmapImage qrCodeImage = converter.Convert(codeQr, typeof(BitmapImage), null, null) as BitmapImage;

                            // Assigner l'image générée à l'objet Image
                            QRCodeImage.Source = qrCodeImage;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Erreur lors de l'affectation de l'image QR: " + ex.Message);
                        }




                    }

                    reader.Close(); // Fermez le DataReader après utilisation
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur : " + ex.Message);
                }
            }

        }

        private void Modifier_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new ModifierPlante(_planteId, Nom, Num));
            }
        }

        private void Retirer_Click(object sender, RoutedEventArgs e)
        {
           
                if (Application.Current.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.MainFrame.Navigate(new RetirerPlante(_planteId, Nom, Num) );
                }
            
        }

        private void AjouterAutre_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new AjouterPlante(Nom, Num));
            }
        }

        private void VoirHistorique_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Navigate(new HistoriquePlantePage(_planteId));
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageInventaire(Nom, Num));
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

        private void Imprimer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    // Créer une bitmap à partir de l'image QR
                    BitmapImage bitmap = QRCodeImage.Source as BitmapImage;
                    if (bitmap != null)
                    {
                        // Créer un StackPanel pour contenir l'image QR et le texte
                        StackPanel printPanel = new StackPanel();
                        printPanel.Orientation = Orientation.Vertical;
                        printPanel.HorizontalAlignment = HorizontalAlignment.Center;
                        printPanel.VerticalAlignment = VerticalAlignment.Center;

                        // Créer une nouvelle image à imprimer
                        Image printImage = new Image();
                        printImage.Source = bitmap;
                        printImage.Margin = new Thickness(0, 0, 0, 20); // Ajouter une marge pour espacer le texte

                        // Créer un TextBlock pour le texte "said"
                        TextBlock textBlock = new TextBlock();
                        textBlock.Text = NomPlantee;
                        textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                        textBlock.FontSize = 16; // Taille de la police
                        textBlock.FontWeight = FontWeights.Bold; // Style de la police

                        // Ajouter l'image et le texte au StackPanel
                        printPanel.Children.Add(printImage);
                        printPanel.Children.Add(textBlock);

                        // Imprimer le StackPanel
                        printDialog.PrintVisual(printPanel, "Impression de l'image QR Code avec texte");
                    }
                    else
                    {
                        MessageBox.Show("Aucune image QR trouvée à imprimer.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'impression : " + ex.Message);
            }
        }

    }
}
