using System.Windows;
using System.Windows.Controls;
using W2G_desktop.Models;
using W2G_desktop.Services;

namespace W2G_desktop.Pages
{
    public partial class EditBayPage : Page
    {
        private Bay bay;
        private BayService bayService = new BayService();

        public EditBayPage(Bay selectedBay)
        {
            InitializeComponent();
            bay = selectedBay;

            // Remplir les champs avec les données existantes
            LabelTextBox.Text = bay.Label;
            SizeTextBox.Text = bay.Size.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Mettre à jour la baie
            if (int.TryParse(SizeTextBox.Text, out int size))
            {
                bay.Label = LabelTextBox.Text;
                bay.Size = size;

                bayService.UpdateBay(bay); // il faut ajouter cette méthode dans BayService
                MessageBox.Show("Baie mise à jour !");
                NavigationService?.GoBack();
            }
            else
            {
                MessageBox.Show("Taille invalide !");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}