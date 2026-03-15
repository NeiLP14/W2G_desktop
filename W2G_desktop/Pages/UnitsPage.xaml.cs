using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using W2G_desktop.Models;
using W2G_desktop.Services;

namespace W2G_desktop.Pages
{
    public partial class UnitsPage : Page
    {
        private UnitService unitService = new UnitService();
        private Bay bay;
        private User currentUser;

        public UnitsPage(Bay bay, User user)
        {
            InitializeComponent();

            this.bay = bay;
            this.currentUser = user;

            BayTitle.Text = "Unités de la baie : " + bay.Label;

            LoadUnits();
        }

        private void LoadUnits()
        {
            List<Unit> units = unitService.GetUnitsByBay(bay.Id);

            // Création d'une vue pour le DataGrid avec le Label U01, U02 etc et le nom de l'utilisateur
            var displayUnits = units.Select(u => new
            {
                u.Position,
                Label = "U" + u.Position.ToString("D2"), // U01, U02...
                u.IsOccupied,
                OccupantName = u.ReservationId.HasValue
                               ? unitService.GetOccupantName(u.ReservationId.Value)
                               : ""
            }).ToList();

            UnitsGrid.ItemsSource = displayUnits;
        }

        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}