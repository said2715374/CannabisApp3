using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace CannabisApp
{
    public partial class HistoriquePlantePage : Page
    {
        private readonly AppDbContext _context;
        int IdPlante;
        public HistoriquePlantePage(int planteId)
        {
           IdPlante = planteId;
            InitializeComponent();
            _context = new AppDbContext();
            LoadHistorique(planteId);
        }

        private void LoadHistorique(int planteId)
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
                            h.initial, 
                            h.final, 
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
                            p.id_plante = @idPlante;
                            
                    ";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@idPlante", IdPlante);
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
            //NavigationService.Navigate(new TableauDebordUser());
        }
    }
}
