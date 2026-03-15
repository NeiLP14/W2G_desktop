using System;
using System.Windows;
using System.Windows.Controls;
using W2G_desktop.Models;
using W2G_desktop.Services;
using W2G_desktop.Pages;

namespace W2G_desktop.Pages
{
    public partial class CreateOffrePage : Page
    {
        private OfferService offerService = new OfferService();
        private User currentUser; // stocker l'utilisateur

        public CreateOffrePage()
        {
            InitializeComponent();
        }

        // ⚡ Nouveau constructeur qui prend le user
        public CreateOffrePage(User user)
        {
            InitializeComponent();
            currentUser = user;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LabelTextBox.Text) ||
                !int.TryParse(NbUnitTextBox.Text, out int nbUnit) ||
                !decimal.TryParse(PriceTextBox.Text, out decimal price) ||
                !int.TryParse(ReductionTextBox.Text, out int reduction))
            {
                ErrorTextBlock.Text = "Veuillez remplir correctement tous les champs.";
                return;
            }

            Offer newOffer = new Offer
            {
                Label = LabelTextBox.Text.Trim(),
                NbUnit = nbUnit,
                Price = price,
                Reduction = reduction
            };

            try
            {
                offerService.CreateOffer(newOffer);
                MessageBox.Show("Offre créée avec succès !");
                NavigationService?.Navigate(new OffresPage(currentUser)); // passer le user aussi ici
            }
            catch (Exception ex)
            {
                ErrorTextBlock.Text = "Erreur lors de la création : " + ex.Message;
            }
        }
    }
}