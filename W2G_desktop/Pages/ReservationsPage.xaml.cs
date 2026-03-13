using System.Collections.Generic;
using System.Windows.Controls;
using W2G_desktop.Models;
using W2G_desktop.Services;

namespace W2G_desktop.Pages
{
    public partial class ReservationsPage : Page
    {
        private ReservationService reservationService = new ReservationService();

        public ReservationsPage()
        {
            InitializeComponent();
            LoadReservations();
        }

        private void LoadReservations()
        {
            List<Reservation> reservations = reservationService.GetAllReservations();
            ReservationsGrid.ItemsSource = reservations;
        }
    }
}