using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace CannabisApp
{
    public partial class Archive : Page
    {
        int Num;
        string Nom;
        private readonly AppDbContext _context;

        public Archive(string nom, int type)
        {
            this.Nom = nom;
            Num = type;
            InitializeComponent();
            _context = new AppDbContext();
            LoadArchiveData();
        }

        private void LoadArchiveData()
        {
            string connectionString = "Server=LAPTOP-K1T841TP\\SQLEXPRESS;Database=NomDeLaBaseDeDonnées;Trusted_Connection=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            h.id_historique, 
                            h.id_plante, 
                            p.description AS plante_description, 
                            p.stade, 
                            p.Identification, 
                            h.action, 
                            h.timestamp, 
                            h.id_utilisateur, 
                            u.nom_utilisateur, 
                            r.nom_role 
                        FROM 
                            historique_plantes h
                        INNER JOIN 
                            plantes p ON h.id_plante = p.id_plante
                        INNER JOIN 
                            utilisateurs u ON h.id_utilisateur = u.id_utilisateur
                        INNER JOIN 
                            roles r ON u.id_role = r.id_role
                         WHERE
                            p.nombre_plantes_actives = 0
                            
                    ";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    ArchiveDataGrid.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement de l'historique des plantes : " + ex.Message);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
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

        private void ArchiveDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Gestionnaire de sélection (si nécessaire)
        }
    }
}
