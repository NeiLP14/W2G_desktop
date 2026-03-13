using System;
using System.Collections.Generic;
using System.Text;

namespace W2G_desktop.Models
{
    public class Unit
    {
        public int Id { get; set; }

        public int Position { get; set; }

        public bool IsOccupied { get; set; }

        public int BayId { get; set; }

        public int StateId { get; set; }

        public int TypeId { get; set; }

        public int? ReservationId { get; set; }
    }
}
