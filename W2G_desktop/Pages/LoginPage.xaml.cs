using System.Text.Json;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using W2G_desktop.Models;
using W2G_desktop.Services;

namespace W2G_desktop.Pages
{
    public partial class LoginPage : Page
    {
        private MainWindow mainWindow;

        public LoginPage(MainWindow window)
        {
            InitializeComponent();
            mainWindow = window;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;

            UserService userService = new UserService();
            User user = userService.Authenticate(email, password);

            if (user == null)
            {
                ErrorTextBlock.Text = "Adresse mail ou mot de passe incorrecte.";
                return;
            }

            var roles = JsonSerializer.Deserialize<List<string>>(user.Role);

            if (!(roles.Contains("ROLE_ADMIN") ||
                  roles.Contains("ROLE_TECHNICIAN") ||
                  roles.Contains("ROLE_ACCOUNTANT")))
            {
                ErrorTextBlock.Text = "Vous n'êtes pas autorisé à vous connecter.";
                return;
            }

            mainWindow.SetCurrentUser(user);
        }

        private void InputChanged(object sender, RoutedEventArgs e)
        {
            LoginButton.IsEnabled =
                !string.IsNullOrWhiteSpace(EmailTextBox.Text) &&
                !string.IsNullOrWhiteSpace(PasswordBox.Password);
        }

        private void Page_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter && LoginButton.IsEnabled)
            {
                LoginButton_Click(null, null);
            }
        }
    }
}