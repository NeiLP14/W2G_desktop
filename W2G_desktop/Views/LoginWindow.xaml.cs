using System.Windows;
using W2G_desktop.Services;

namespace W2G_desktop.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;

            UserService userService = new UserService();
            var user = userService.Authenticate(email, password);

            if (user == null)
            {
                ErrorTextBlock.Text = "Adresse mail ou mot de passe incorrecte.";
            }
            else if (!user.Role.Contains("ROLE_ADMIN") &&
                     !user.Role.Contains("ROLE_TECHNICIAN") &&
                     !user.Role.Contains("ROLE_ACCOUNTANT"))
            {
                ErrorTextBlock.Text = "Vous n'êtes pas autorisé à vous connecter.";
            }
            else
            {
                MainWindow main = new MainWindow(user);
                main.Show();
                this.Close();
            }
        }
    }
}