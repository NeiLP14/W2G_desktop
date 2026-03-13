using System.Windows;
using System.Windows.Controls;
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

            if (!(user.Role.Contains("ROLE_ADMIN") || user.Role.Contains("ROLE_TECHNICIAN") || user.Role.Contains("ROLE_ACCOUNTANT")))
            {
                ErrorTextBlock.Text = "Vous n'êtes pas autorisé à vous connecter.";
                return;
            }

            // Connexion réussie, informer la MainWindow
            mainWindow.SetCurrentUser(user);
        }
    }
}