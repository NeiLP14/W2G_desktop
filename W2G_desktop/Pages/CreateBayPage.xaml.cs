using System;
using System.Windows;
using System.Windows.Controls;
using W2G_desktop.Models;
using W2G_desktop.Services;

namespace W2G_desktop.Pages
{
    public partial class CreateBayPage : Page
    {
        private BayService bayService = new BayService();
        private User currentUser;

        public CreateBayPage(User user)
        {
            InitializeComponent();
            currentUser = user;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(SizeTextBox.Text, out int size))
            {
                ErrorTextBlock.Text = "Veuillez saisir une taille valide.";
                return;
            }

            try
            {
                bayService.CreateBay(size);
                MessageBox.Show("Baie créée avec succès !");
                NavigationService?.Navigate(new BaysPage(currentUser));
            }
            catch (Exception ex)
            {
                ErrorTextBlock.Text = "Erreur lors de la création : " + ex.Message;
            }
        }
    }
}