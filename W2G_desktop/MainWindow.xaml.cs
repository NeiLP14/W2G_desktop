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
            UpdateMenu();

            if (currentUser == null)
            {
                MainFrame.Navigate(new LoginPage(this));
            }
            else
            {
                this.Title += $" - Connecté : {currentUser.Role}";
                MainFrame.Navigate(new CustomersPage());
            }
        }

        private void UpdateMenu()
        {
            if (currentUser == null)
            {
                UsersButton.Visibility = Visibility.Collapsed;
                CustomersButton.Visibility = Visibility.Collapsed;
                ReservationsButton.Visibility = Visibility.Collapsed;
                OffersButton.Visibility = Visibility.Collapsed;
                BaysButton.Visibility = Visibility.Collapsed;
                return;
            }

            // Seul l'admin peut créer des utilisateurs
            UsersButton.Visibility = currentUser.Role.Contains("ROLE_ADMIN")
                ? Visibility.Visible
                : Visibility.Collapsed;

            // Tous les utilisateurs connectés voient ces boutons
            CustomersButton.Visibility = Visibility.Visible;
            ReservationsButton.Visibility = Visibility.Visible;
            OffersButton.Visibility = Visibility.Visible; 
            BaysButton.Visibility = Visibility.Visible;
        }

        public void SetCurrentUser(User user)
        {
            currentUser = user;

            this.Title = "W2G Desktop - Connecté : " + currentUser.Role;

            UpdateMenu();

            MainFrame.Navigate(new CustomersPage());
        }

        private void CreateUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser != null)
            {
                MainFrame.Navigate(new CreateUserPage(currentUser));
            }
        }

        private void CustomersButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CustomersPage());
        }

        private void ReservationsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ReservationsPage());
        }

        private void OffersButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new OffresPage(currentUser));
        }
        
        private void BaysButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser != null)
            {
                MainFrame.Navigate(new BaysPage(currentUser));
            }
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser != null)
            {
                MainFrame.Navigate(new UsersPage(currentUser));
            }
        }
    }
}