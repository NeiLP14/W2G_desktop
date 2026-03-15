using System.Windows;
using System.Windows.Controls;
using W2G_desktop.Models;
using W2G_desktop.Services;

namespace W2G_desktop.Pages
{
    public partial class EditOfferPage : Page
    {
        private Offer offer;
        private OfferService offerService = new OfferService();
        private User currentUser;

        public EditOfferPage(Offer selectedOffer, User user)
        {
            InitializeComponent();

            offer = selectedOffer;
            currentUser = user;

            LabelTextBox.Text = offer.Label;
            NbUnitTextBox.Text = offer.NbUnit.ToString();
            PriceTextBox.Text = offer.Price.ToString();
            ReductionTextBox.Text = offer.Reduction.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(NbUnitTextBox.Text, out int nbUnit) &&
                decimal.TryParse(PriceTextBox.Text, out decimal price) &&
                int.TryParse(ReductionTextBox.Text, out int reduction))
            {
                offer.Label = LabelTextBox.Text;
                offer.NbUnit = nbUnit;
                offer.Price = price;
                offer.Reduction = reduction;

                offerService.UpdateOffer(offer);

                MessageBox.Show("Offre mise à jour !");

                NavigationService?.Navigate(new OffresPage(currentUser));
            }
            else
            {
                MessageBox.Show("Valeurs invalides !");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}