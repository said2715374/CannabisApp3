using CannabisApp.Converters;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CannabisApp
{
    public partial class ModifierPlante : Page
    {
        int Num;
        string Nom;
        private readonly AppDbContext _context;
        private readonly int _planteId;
        int IdUser;
        string stade;
        string stade1;
        string Ident;
        string Ident1;
        string etat;
        string etat1;
        string prov;
        string prov1;
        string entropo;
        string entropo1;



        public ModifierPlante(int planteId, string nom, int type)
        {
            Num = type;
            Nom = nom;
            _planteId = planteId;

            InitializeComponent();
            _context = new AppDbContext();
            
            LoadProvenances();
            LoadEmplacements();
            ChargerPlante(_planteId);
            
            
        }

        private void ChargerPlante(int planteId)
        {
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;Trusted_Connection=True;";
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
                        Ident = reader.GetString(reader.GetOrdinal("Identification"));

                        int etatSanteValue = reader.GetInt32(reader.GetOrdinal("etat_sante"));

                        // Sélectionner l'élément ComboBox basé sur le Tag correspondant à l'état de santé
                        EtatSanteComboBox.SelectedItem = EtatSanteComboBox.Items
                        .OfType<ComboBoxItem>()
                        .FirstOrDefault(item => item.Tag?.ToString() == etatSanteValue.ToString());
                        etat = (EtatSanteComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                        stade = reader.GetString(reader.GetOrdinal("stade"));
                        StadeComboBox.SelectedItem = StadeComboBox.Items.Cast<ComboBoxItem>()
                            .FirstOrDefault(item => string.Equals(item.Content.ToString(), stade, StringComparison.OrdinalIgnoreCase));


                        string ville = reader.GetString(reader.GetOrdinal("ville"));
                        prov = reader.GetString(reader.GetOrdinal("ville"));
                        ProvenanceComboBox.SelectedItem = ProvenanceComboBox.Items.Cast<provenances>()
                            .FirstOrDefault(item => item.ville == ville);

                        string entroposage = reader.GetString(reader.GetOrdinal("emplacement"));
                        entropo = reader.GetString(reader.GetOrdinal("emplacement"));
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
                    etat_sante = @etat_sante,
                    Note = @note
                WHERE id_plante = @planteId;";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@planteId", _planteId);
                    command.Parameters.AddWithValue("@stade", ((ComboBoxItem)StadeComboBox.SelectedItem)?.Content.ToString() ?? string.Empty);
                    stade1 = ((ComboBoxItem)StadeComboBox.SelectedItem)?.Content.ToString() ?? string.Empty;
                    command.Parameters.AddWithValue("@identification", Identification.Text);
                    Ident1 = Identification.Text;
                    command.Parameters.AddWithValue("@id_provenance", (int)ProvenanceComboBox.SelectedValue);
                    prov1 = ProvenanceComboBox.Text;
                    command.Parameters.AddWithValue("@etat_sante", int.Parse((EtatSanteComboBox.SelectedItem as ComboBoxItem)?.Tag.ToString()));
                    etat1 = (EtatSanteComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                    command.Parameters.AddWithValue("@note", NoteTextBox.Text);

                    command.ExecuteNonQuery();

                    AddHistorique(_planteId, GetIdByName(Nom));

                    MessageBox.Show("Plante modifiée avec succès !");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la modification de la plante : " + ex.Message);
                }
               
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DetailsPlante(_planteId, Nom, Num));
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
        private void AddHistorique(int Idplante, int Iduser)
        {

            int columnWidth = 20;

            // Formater les chaînes pour l'alignement
            string initialHeader = "Données initiales";
            string finalHeader = "Données finales";
            string initialStade = stade.PadRight(columnWidth);
            string finalStade = stade1.PadRight(columnWidth);
            string initialIdent = Ident.PadRight(columnWidth);
            string finalIdent = Ident1.PadRight(columnWidth);
            string initialEtat = etat.PadRight(columnWidth);
            string finalEtat = etat1.PadRight(columnWidth);

            // Créer le texte pour @action
            string actionText = string.Format(
                "Modifier :\n" +
                "{0}\t{1}\n" +
                "{2}\t{3}\n" +
                "{4}\t{5}\n" +
                "{6}\t{7}\n",
                initialHeader.PadRight(columnWidth), finalHeader.PadRight(columnWidth),
                initialStade, finalStade,
                initialIdent, finalIdent,
                initialEtat, finalEtat
            );


            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;Trusted_Connection=True;";
            bool exists = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Requête SQL pour vérifier si le code existe dans la base de données
                    string query = "INSERT INTO historique_plantes (id_plante, action, timestamp, id_utilisateur, initial, final) VALUES (@IdPlante, @action, GETDATE(), @IdUser, @initial, @final);";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@IdPlante", Idplante);
                        command.Parameters.AddWithValue("@IdUser", Iduser);
                        command.Parameters.AddWithValue("@action", "Modifié");
                        command.Parameters.AddWithValue("@initial", "stade : \t \t"+ stade +"\n"+ "Identifient : \t"+ Ident  + "\n"+"Etat de santé : \t"+ etat +"\n"+ "Provenance \t"+ prov );
                        command.Parameters.AddWithValue("@final", "stade : \t \t"+ stade1 + "\n" + "Identifient : \t" + Ident1 + "\n" + "Etat de santé : \t" + etat1 +"\n"+ "Provenance \t" + prov1);
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
