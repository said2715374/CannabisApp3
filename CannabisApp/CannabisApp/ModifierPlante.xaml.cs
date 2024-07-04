using CannabisApp.Converters;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CannabisApp
{
    public partial class ModifierPlante : Page
    {
        private readonly AppDbContext _context;
        private readonly int _planteId;

        public ModifierPlante(int planteId)
        {
            InitializeComponent();
            _context = new AppDbContext();
            _planteId = planteId;
            LoadProvenances();
            LoadEmplacements();
            ChargerPlante(planteId);
            
            
        }

        private void ChargerPlante(int planteId)
        {
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;User Id=LAPTOP-K1T841TP\\user;Trusted_Connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Requête combinée pour récupérer les informations de la plante et de sa provenance
                    string query = @"
                SELECT p.id_plante, p.code_qr, p.id_provenance, p.etat_sante, p.date_expiration, 
                       p.cree_le, p.stade, p.Note, p.identification, p.id_Entreposage, p.Quentité, p.nombre_plantes_actives,p.Identification,
                       pr.ville, pr.province, pr.pays, en.emplacement
                FROM plantes p
                INNER JOIN provenances pr ON p.id_provenance = pr.id_provenance
                INNER JOIN Enterposage en ON p.id_Entreposage = en.id
                WHERE p.id_plante = @planteId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@planteId", planteId);
                   
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        // Mettez à jour les contrôles avec les informations récupérées
                        Description.Text = reader.GetString(reader.GetOrdinal("Identification"));
                        Identification.Text = reader.GetString(reader.GetOrdinal("Identification"));

                        int etatSanteValue = reader.GetInt32(reader.GetOrdinal("etat_sante"));

                        // Sélectionner l'élément ComboBox basé sur le Tag correspondant à l'état de santé
                        EtatSanteComboBox.SelectedItem = EtatSanteComboBox.Items
                        .OfType<ComboBoxItem>()
                        .FirstOrDefault(item => item.Tag?.ToString() == etatSanteValue.ToString());


                        Quantite.Text = reader.GetString(reader.GetOrdinal("Quentité"));
                        DateExpiration.Text = reader.GetDateTime(reader.GetOrdinal("date_expiration")).ToString("yyyy-MM-dd");
                        var stadeValue = reader.GetString(reader.GetOrdinal("stade"));
                        StadeComboBox.SelectedItem = StadeComboBox.Items.OfType<ComboBoxItem>()
                            .FirstOrDefault(item => item.Content.ToString() == stadeValue);


                        string ville = reader.GetString(reader.GetOrdinal("ville"));
                        ProvenanceComboBox.SelectedItem = ProvenanceComboBox.Items.Cast<provenances>()
                            .FirstOrDefault(item => item.ville == ville);

                        string entroposage = reader.GetString(reader.GetOrdinal("emplacement"));
                        EmplacementComboBox.SelectedItem = EmplacementComboBox.Items.Cast<Enterposage>()
                            .FirstOrDefault(item => item.emplacement == entroposage);



                        NoteTextBox.Text = reader.GetString(reader.GetOrdinal("Note"));

                        
                        
                    }

                    reader.Close(); // Fermez le DataReader après utilisation
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur : " + ex.Message);
                }
            }
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
                            ville = reader.GetString(reader.GetOrdinal("ville")),
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

        private void Modifier_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;User Id=LAPTOP-K1T841TP\\user;Trusted_Connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"
                UPDATE plantes
                SET stade = @stade,
                    identification = @identification,
                    id_provenance = @id_provenance,
                    date_expiration = @date_expiration,
                    etat_sante = @etat_sante,
                    Note = @note,
                    Quentité = @quentite
                WHERE id_plante = @planteId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@planteId", _planteId);
                    command.Parameters.AddWithValue("@stade", ((ComboBoxItem)StadeComboBox.SelectedItem)?.Content.ToString() ?? string.Empty);
                    command.Parameters.AddWithValue("@identification", Identification.Text);
                    command.Parameters.AddWithValue("@id_provenance", (int)ProvenanceComboBox.SelectedValue);
                    command.Parameters.AddWithValue("@date_expiration", DateTime.Parse(DateExpiration.Text));
                    command.Parameters.AddWithValue("@etat_sante", int.Parse((EtatSanteComboBox.SelectedItem as ComboBoxItem)?.Tag.ToString()));
                    command.Parameters.AddWithValue("@note", NoteTextBox.Text);
                    command.Parameters.AddWithValue("@quentite", Quantite.Text);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Plante modifiée avec succès !");
                    }
                    else
                    {
                        MessageBox.Show("Aucune plante modifiée.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la modification de la plante : " + ex.Message);
                }
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

        private void AjouterEmplacement_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AjouterEmplacement());
        }
    }
}
