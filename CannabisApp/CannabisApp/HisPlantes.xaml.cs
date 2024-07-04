using System.Collections.ObjectModel;
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
                    string query = "SELECT \r\n    h.id_historique, \r\n    h.id_plante, \r\n    p.description AS plante_description, \r\n    p.stade, \r\n    h.action, \r\n    h.timestamp, \r\n    h.id_utilisateur, \r\n    u.nom_utilisateur, \r\n    r.nom_role\r\nFROM historique_plantes h\r\nINNER JOIN plantes p ON h.id_plante = p.id_plante\r\nINNER JOIN utilisateurs u ON h.id_utilisateur = u.id_utilisateur\r\nINNER JOIN roles r ON u.id_role = r.id_role;";
                    SqlCommand command = new SqlCommand(query, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        ObservableCollection<Historique_Plantes> historiquePlantesList = new ObservableCollection<Historique_Plantes>();
                        while (reader.Read())
                        {
                            Historique_Plantes historiquePlante = new Historique_Plantes
                            {
                                IdHistorique = reader.GetInt32(reader.GetOrdinal("id_historique")),
                                IdPlante = reader.GetInt32(reader.GetOrdinal("id_plante")),
                                Action = reader.GetString(reader.GetOrdinal("action")),
                                Timestamp = reader.GetDateTime(reader.GetOrdinal("timestamp")),
                                IdUtilisateur = reader.GetInt32(reader.GetOrdinal("id_utilisateur"))
                            };
                            historiquePlantesList.Add(historiquePlante);

                            
                        }

                        
                        HistoriqueDataGrid.ItemsSource = historiquePlantesList;
                    }
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
