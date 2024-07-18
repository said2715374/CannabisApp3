using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
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
    /// Logique d'interaction pour HistoriqueUtilisateur.xaml
    /// </summary>
    public partial class HistoriqueUtilisateur : Page
    {
        string Nom;
        int Num;
        private readonly AppDbContext _context;
        int IdPlante;
        public HistoriqueUtilisateur(int UserId, int num, string nom)
        {
            Nom = nom;
            Num = num;
            InitializeComponent();
            IdPlante = UserId;
            InitializeComponent();
            _context = new AppDbContext();
            LoadHistorique(UserId);
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
                            u.id_utilisateur = @iduser;
                            
                    ";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@iduser", IdPlante);
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
            NavigationService.Navigate(new GererUser(Num, Nom));
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
    }
}
