using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CannabisApp
{
    public partial class TableauDebordUser : Page
    {
        private const int CapacityMax = 2000; // Capacité maximale de l'entrepôt
        private int TotalPlantes = 500; // Nombre total de plantes, initialisé à 500
        private int PlantesBonneSante = 80; // Initialisé à 80
        private int PlantesNecessitantAttention = 20; // Initialisé à 20
        private string Username;

        public TableauDebordUser(string username)
        {
            InitializeComponent();
            Username = username;
            UsernameTextBlock.Text = Username;
            InitializeDashboard();
            UpdateDashboard();
        }

        private void InitializeDashboard()
        {
            // Initialiser les valeurs par défaut
            PlantesBonneSanteTextBlock.Text = "80";
            PlantesNecessitantAttentionTextBlock.Text = "20";

            // Afficher le pourcentage initial de 25%
            double initialPercentage = 25;
            UpdateProgressArc(initialPercentage);
        }

        private void UpdateDashboard()
        {
            // Mettre à jour les valeurs réelles
            // Vous pouvez obtenir ces valeurs à partir de votre base de données ou autre source de données
            // Ici, elles sont initialisées à des valeurs de démonstration

            // Mettre à jour les statistiques
            PlantesBonneSanteTextBlock.Text = PlantesBonneSante.ToString();
            PlantesNecessitantAttentionTextBlock.Text = PlantesNecessitantAttention.ToString();

            // Calculer et mettre à jour le pourcentage de la capacité
            double percentage = (double)TotalPlantes / CapacityMax * 100;
            UpdateProgressArc(percentage);
        }

        private void UpdateProgressArc(double percentage)
        {
            PercentageText.Text = $"{percentage:F1}%";
            TotalPlantesText.Text = $"{TotalPlantes}/{CapacityMax}";

            // Définir l'angle pour le segment de l'arc de progression
            double angle = 180 * (percentage / 100);

            // Calculer la nouvelle position de l'arc
            double radians = angle * Math.PI / 180;
            double x = 200 + 190 * Math.Cos(radians);
            double y = 190 - 190 * Math.Sin(radians);

            ProgressSegment.Point = new Point(x, y);
            ProgressFigure.Segments.Clear();
            ProgressFigure.Segments.Add(ProgressSegment);
        }

        private void AjouterPlante_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AjouterPlante(Username, 2));
        }

        private void AccederInventaire_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PageInventaire(Username, 2));
        }

        private void VoirHistorique_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new HisPlantes());
        }

        private void Deconnecter_Click(object sender, RoutedEventArgs e)
        {
            // Redirigez vers la page de connexion
            NavigationService.Navigate(new Page1());
        }
    }
}
