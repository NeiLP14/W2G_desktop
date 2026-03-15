using System.Windows;
using System.Windows.Controls;
using W2G_desktop.Models;
using W2G_desktop.Services;

namespace W2G_desktop.Pages
{
    public partial class EditUserPage : Page
    {
        private User user;
        private UserService userService = new UserService();

        public EditUserPage(User selectedUser)
        {
            InitializeComponent();
            user = selectedUser;

            EmailTextBox.Text = user.Email;
            UsernameTextBox.Text = user.Username;

            foreach (ComboBoxItem item in RoleComboBox.Items)
            {
                if ((string)item.Content == user.Role)
                {
                    RoleComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string username = UsernameTextBox.Text;
            string role = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(role))
            {
                ErrorTextBlock.Text = "Tous les champs doivent être remplis.";
                return;
            }

            // Recalculer discr si nécessaire
            string discr = role switch
            {
                "ROLE_ADMIN" => "admin",
                "ROLE_TECHNICIAN" => "technician",
                "ROLE_ACCOUNTANT" => "accountant",
                _ => "staff"
            };

            user.Email = email;
            user.Username = username;
            user.Role = role;
            user.Discr = discr;

            bool success = userService.UpdateUser(user);

            if (success)
            {
                MessageBox.Show("Utilisateur mis à jour !");
                NavigationService?.GoBack();
            }
            else
            {
                ErrorTextBlock.Text = "Erreur lors de la mise à jour.";
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}