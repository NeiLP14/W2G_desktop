using System.Collections.Generic;
using System.Windows.Controls;
using W2G_desktop.Models;
using W2G_desktop.Services;

namespace W2G_desktop.Pages
{
    public partial class Reporting : Page
    {
        public Reporting()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            ReportingService service = new ReportingService();

            List<ReportingData> data = service.ChiffreAffaire();

            ReportingGrid.ItemsSource = data;
        }
    }
}