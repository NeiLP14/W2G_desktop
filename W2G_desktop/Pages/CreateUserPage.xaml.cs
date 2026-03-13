using System.Windows;
using System.Windows.Controls;
using W2G_desktop.Services;

namespace W2G_desktop.Pages
{
    public partial class CreateUserPage : Page
    {
        private UserService userService = new UserService();

        public CreateUserPage()
        {
            InitializeComponent();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            string role = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
            {
                ErrorTextBlock.Text = "Tous les champs doivent être remplis.";
                return;
            }

            bool success = userService.CreateUser(email, username, password, role);

            if (success)
            {
                MessageBox.Show("Utilisateur créé avec succès !");
                // Retour à la page clients après création
                NavigationService?.Navigate(new CustomersPage());
            }
            else
            {
                ErrorTextBlock.Text = "Erreur lors de la création de l'utilisateur. Email peut-être déjà utilisé.";
            }
        }
    }
}