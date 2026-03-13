using System.Windows.Controls;
using System.Collections.Generic;
using W2G_desktop.Models;
using W2G_desktop.Services;

namespace W2G_desktop.Pages
{
    public partial class CustomersPage : Page
    {
        public CustomersPage()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            UserService userService = new UserService();
            List<User> customers = userService.GetCustomers();
            CustomersGrid.ItemsSource = customers;
        }
    }
}