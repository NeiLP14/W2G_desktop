using System.Windows;
using W2G_desktop.Models;
using W2G_desktop.Pages;

namespace W2G_desktop
{
    public partial class MainWindow : Window
    {
        private User currentUser;

        // Constructeur sans paramètre obligatoire pour WPF
        public MainWindow() : this(null) { }

        // Constructeur principal avec paramètre utilisateur
        public MainWindow(User user)
        {
            InitializeComponent();
            currentUser = user;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (currentUser == null)
            {
                // Affiche la page de login si utilisateur non connecté
                MainFrame.Navigate(new LoginPage(this));
                CreateUserButton.Visibility = Visibility.Collapsed;
                CustomersButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                // Met à jour l’interface en fonction du rôle
                this.Title += $" - Connecté : {currentUser.Role}";
                CreateUserButton.Visibility = currentUser.Role.Contains("ROLE_ADMIN") ? Visibility.Visible : Visibility.Collapsed;
                CustomersButton.Visibility = Visibility.Visible;
                // Charge une page d’accueil ou autre
                MainFrame.Navigate(new CustomersPage());
            }
        }

        public void SetCurrentUser(User user)
        {
            currentUser = user;
            this.Title = "W2G Desktop - Connecté : " + currentUser.Role;

            CreateUserButton.Visibility = currentUser.Role.Contains("ROLE_ADMIN") ? Visibility.Visible : Visibility.Collapsed;
            CustomersButton.Visibility = Visibility.Visible;

            MainFrame.Navigate(new CustomersPage());
        }

        private void CreateUserButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CreateUserPage());
        }

        private void CustomersButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CustomersPage());
        }

        private void ReservationsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ReservationsPage());
        }
    }
}