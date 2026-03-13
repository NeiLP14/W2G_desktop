using System;
using System.Collections.Generic;
using System.Text;

namespace W2G_desktop.Models
{
    public class Offer
    {
        public int Id { get; set; }

        public string Label { get; set; }

        public int NbUnit { get; set; }

        public decimal Price { get; set; }

        public int Reduction { get; set; }
    }
}
