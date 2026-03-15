namespace W2G_desktop.Models
{
    public class Unit
    {
        public int Id { get; set; }

        public string Label { get; set; } // U01, U02

        public int Position { get; set; }

        public bool IsOccupied { get; set; }

        public int BayId { get; set; }

        public int StateId { get; set; }

        public int? TypeId { get; set; }

        public int? ReservationId { get; set; }

        // propriétés pour affichage
        public string StateLabel { get; set; }

        public string Username { get; set; }
    }
}