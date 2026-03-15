using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            // Seul l'admin peut voir le bouton pour créer une baie
            CreateBayButton.Visibility = currentUser != null && currentUser.Role.Contains("ROLE_ADMIN")
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void CreateBayButton_Click(object sender, RoutedEventArgs e)
        {
            // Naviguer vers la page de création de baie
            NavigationService?.Navigate(new CreateBayPage(currentUser));
        }

        private void BaysGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EditBayButton.IsEnabled = BaysGrid.SelectedItem != null;
        }

        private void EditBayButton_Click(object sender, RoutedEventArgs e)
        {
            Bay selectedBay = BaysGrid.SelectedItem as Bay;

            if (selectedBay != null)
            {
                NavigationService?.Navigate(new EditBayPage(selectedBay));
            }
        }
    }
}