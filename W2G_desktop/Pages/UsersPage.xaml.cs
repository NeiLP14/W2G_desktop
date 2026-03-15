using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using W2G_desktop.Models;
using W2G_desktop.Services;

namespace W2G_desktop.Pages
{
    public partial class UsersPage : Page
    {
        private UserService userService = new UserService();
        private User currentUser;

        public UsersPage(User user)
        {
            InitializeComponent();
            currentUser = user;
            LoadUsers();
        }

        private void LoadUsers()
        {
            List<User> users = userService.GetAllUsers();
            UsersGrid.ItemsSource = users;
        }

        private void UsersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool isSelected = UsersGrid.SelectedItem != null;
            EditUserButton.IsEnabled = isSelected;
            DeleteUserButton.IsEnabled = isSelected;
        }

        private void CreateUserButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CreateUserPage(currentUser));
        }

        private void EditUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem is User selectedUser)
            {
                NavigationService?.Navigate(new EditUserPage(selectedUser));
            }
        }

        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem is User selectedUser)
            {
                MessageBoxResult result = MessageBox.Show(
                    $"Voulez-vous vraiment supprimer l'utilisateur {selectedUser.Username} et toutes ses réservations ?",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result == MessageBoxResult.Yes)
                {
                    bool success = userService.DeleteUser(selectedUser.Id);
                    if (success)
                    {
                        MessageBox.Show("Utilisateur supprimé avec succès !");
                        LoadUsers(); // Recharge la liste
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la suppression de l'utilisateur.");
                    }
                }
            }
        }
    }
}