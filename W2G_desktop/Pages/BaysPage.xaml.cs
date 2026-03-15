using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using W2G_desktop.Models;
using W2G_desktop.Services;

namespace W2G_desktop.Pages
{
    public partial class BaysPage : Page
    {
        private BayService bayService = new BayService();
        private User currentUser;

        public BaysPage(User user)
        {
            InitializeComponent();
            currentUser = user;
            LoadBays();
            UpdateUI();
        }

        private void LoadBays()
        {
            List<Bay> bays = bayService.GetBays();
            BaysGrid.ItemsSource = bays;
        }

        private void UpdateUI()
        {
            bool isAdmin = currentUser != null && currentUser.Role.Contains("ROLE_ADMIN");

            // Seul l'admin peut voir le bouton Créer
            CreateBayButton.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;

            // Seul l'admin peut voir et modifier
            EditBayButton.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;

            // Les boutons dépendent de la sélection
            bool hasSelection = BaysGrid.SelectedItem != null;
            EditBayButton.IsEnabled = isAdmin && hasSelection;
            ViewUnitsButton.IsEnabled = hasSelection;
        }

        private void CreateBayButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CreateBayPage(currentUser));
        }

        private void BaysGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUI(); // met à jour les boutons à chaque sélection
        }

        private void EditBayButton_Click(object sender, RoutedEventArgs e)
        {
            Bay selectedBay = BaysGrid.SelectedItem as Bay;
            if (selectedBay != null)
                NavigationService?.Navigate(new EditBayPage(selectedBay));
        }

        private void VoirUnits_Click(object sender, RoutedEventArgs e)
        {
            Bay selectedBay = BaysGrid.SelectedItem as Bay;
            if (selectedBay != null)
                NavigationService?.Navigate(new UnitsPage(selectedBay, currentUser));
        }
    }
}