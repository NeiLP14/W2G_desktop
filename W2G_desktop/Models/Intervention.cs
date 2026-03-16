namespace W2G_desktop.Models
{
    public class Intervention
    {
        public int Id { get; set; }
        public DateTime DateDeb { get; set; }
        public DateTime DateFin { get; set; }
        public string Description { get; set; }

        public string Technician { get; set; }

        public string UnitLabel { get; set; }
        public int UnitPosition { get; set; }

        public string BayLabel { get; set; }

        public string TypeIntervention { get; set; }
    }
}