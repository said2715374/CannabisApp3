using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CannabisApp
{
    public partial class HisPlantes : Page
    {
        private readonly AppDbContext _context;

        public HisPlantes()
        {
            InitializeComponent();
            _context = new AppDbContext();
            LoadHistorique();
        }

        private void LoadHistorique()
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
                            roles r ON u.id_role = r.id_role;
                    ";
                    SqlCommand command = new SqlCommand(query, connection);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    HistoriqueDataGrid.ItemsSource = dataTable.DefaultView;
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
           // NavigationService.Navigate(new TableauDebordUser());
        }

        private void HistoriqueDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
