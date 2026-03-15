using System.Windows;
using System.Windows.Controls;
using W2G_desktop.Models;
using W2G_desktop.Services;

namespace W2G_desktop.Pages
{
    public partial class OffresPage : Page
    {
        private User currentUser;
        private OfferService offerService = new OfferService();

        public OffresPage(User user)
        {
            InitializeComponent();
            currentUser = user;

            LoadOffers();
            UpdateUI();
        }

        private void LoadOffers()
        {
            // ⚡ Appel correct de la méthode du service
            OffersGrid.ItemsSource = offerService.GetAllOffers();
        }

        private void UpdateUI()
        {
            // Seul l'admin peut créer, modifier ou supprimer
            bool isAdmin = currentUser != null && currentUser.Role.Contains("ROLE_ADMIN");

            CreateOfferButton.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
            EditOfferButton.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
            DeleteOfferButton.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
        }

        private void OffersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool hasSelection = OffersGrid.SelectedItem != null;
            bool isAdmin = currentUser != null && currentUser.Role.Contains("ROLE_ADMIN");

            // Activer uniquement si admin et qu'une offre est sélectionnée
            EditOfferButton.IsEnabled = isAdmin && hasSelection;
            DeleteOfferButton.IsEnabled = isAdmin && hasSelection;
        }

        private void CreateOfferButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser != null)
                NavigationService?.Navigate(new CreateOffrePage(currentUser));
        }

        private void EditOfferButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedOffer = OffersGrid.SelectedItem as Offer;
            if (selectedOffer != null)
                NavigationService?.Navigate(new EditOfferPage(selectedOffer, currentUser));
        }

        private void DeleteOfferButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedOffer = OffersGrid.SelectedItem as Offer;
            if (selectedOffer != null)
            {
                if (MessageBox.Show("Voulez-vous vraiment supprimer cette offre ?",
                        "Confirmer", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    offerService.DeleteOffer(selectedOffer.Id);
                    LoadOffers();
                }
            }
        }
    }
}