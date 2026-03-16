using System.Windows.Controls;
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
    }
}