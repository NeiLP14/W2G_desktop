using System.Windows.Controls;
using W2G_desktop.Models;
using W2G_desktop.Services;

namespace W2G_desktop.Pages
{
    public partial class InterventionsPage : Page
    {

        private InterventionService interventionService = new InterventionService();

        public InterventionsPage()
        {
            InitializeComponent();
            LoadInterventions();
        }

        private void LoadInterventions()
        {
            InterventionsGrid.ItemsSource = interventionService.GetAllInterventions();
        }

        private void InterventionsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Ici tu peux gérer ce qui se passe quand l'utilisateur sélectionne une ligne
            var selectedIntervention = InterventionsGrid.SelectedItem as Intervention;
            if (selectedIntervention != null)
            {
                // Par exemple afficher des détails ou activer un bouton
            }
        }
    }
}