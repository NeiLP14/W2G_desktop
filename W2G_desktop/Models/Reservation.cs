namespace W2G_desktop.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int OffreId { get; set; }

        public DateTime DateDeb { get; set; }

        public DateTime DateFin { get; set; }

        public string Username { get; set; }

        public string Offre { get; set; }
    }
}