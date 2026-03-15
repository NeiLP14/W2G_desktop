using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using W2G_desktop.Models;
using W2G_desktop.Services;

namespace W2G_desktop.Pages
{
    public partial class OffresPage : Page
    {
        private OfferService offerService = new OfferService();
        private User? currentUser;

        public OffresPage(User? user = null)
        {
            InitializeComponent();
            currentUser = user;
            LoadOffers();
            UpdateCreateButton();
        }

        private void LoadOffers()
        {
            List<Offer> offers = offerService.GetAllOffers();
            OffersGrid.ItemsSource = offers;
        }

        private void UpdateCreateButton()
        {
            if (currentUser == null || !currentUser.Role.Contains("ROLE_ADMIN"))
            {
                CreateOfferButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                CreateOfferButton.Visibility = Visibility.Visible;
            }
        }

        private void CreateOfferButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigue vers la page de création d'offre
            NavigationService?.Navigate(new CreateOffrePage());
        }

        private void OffersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool selected = OffersGrid.SelectedItem != null;

            EditOfferButton.IsEnabled = selected;
            DeleteOfferButton.IsEnabled = selected;
        }

        private void EditOfferButton_Click(object sender, RoutedEventArgs e)
        {
            Offer selectedOffer = OffersGrid.SelectedItem as Offer;

            if (selectedOffer != null)
            {
                NavigationService?.Navigate(new EditOfferPage(selectedOffer, currentUser));
            }
        }

        private void DeleteOfferButton_Click(object sender, RoutedEventArgs e)
        {
            Offer selectedOffer = OffersGrid.SelectedItem as Offer;

            if (selectedOffer == null)
                return;

            // Vérifier si l'offre a des réservations
            if (offerService.OfferHasReservations(selectedOffer.Id))
            {
                MessageBox.Show("Impossible de supprimer cette offre car elle possède des réservations.");
                return;
            }

            // Confirmation
            var result = MessageBox.Show(
                "Voulez-vous vraiment supprimer cette offre ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                offerService.DeleteOffer(selectedOffer.Id);

                MessageBox.Show("Offre supprimée.");

                LoadOffers(); // recharge le tableau
            }
        }
    }
}