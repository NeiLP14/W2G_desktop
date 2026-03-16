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
                Interventions.Visibility = Visibility.Collapsed;

                UserPanel.Visibility = Visibility.Collapsed; // cacher le nom et le bouton déco
                return;
            }

            UsersButton.Visibility = currentUser.Role.Contains("ROLE_ADMIN")
                ? Visibility.Visible
                : Visibility.Collapsed;

            CustomersButton.Visibility = Visibility.Visible;
            ReservationsButton.Visibility = Visibility.Visible;
            OffersButton.Visibility = Visibility.Visible;
            BaysButton.Visibility = Visibility.Visible;
            Interventions.Visibility = Visibility.Visible;

            // Afficher le panel utilisateur
            UserPanel.Visibility = Visibility.Visible;

            // Afficher le username
            CurrentUserTextBlock.Text = currentUser.Username;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Réinitialiser l'utilisateur courant
            currentUser = null;

            // Mettre à jour l'interface (boutons + header)
            UpdateMenu();

            // Revenir à la page de login
            MainFrame.Navigate(new LoginPage(this));

            // Mettre à jour le titre
            this.Title = "W2G Desktop";
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

        private void Interventions_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new InterventionsPage());
        }
    }
}