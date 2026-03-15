using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using W2G_desktop.Models;

namespace W2G_desktop.Services
{
    public class OfferService
    {
        private string connectionString = "server=localhost;database=w2g;uid=root;pwd=;";

        public List<Offer> GetAllOffers()
        {
            List<Offer> offers = new List<Offer>();

            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = @"
                            SELECT 
                                o.id,
                                o.label,
                                o.nb_unit,
                                o.price,
                                o.reduction,
                                COUNT(r.id) AS nb_reservations
                            FROM offre o
                            LEFT JOIN reservation r ON r.offre_id = o.id
                            GROUP BY o.id";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                offers.Add(new Offer
                {
                    Id = reader.GetInt32("id"),
                    Label = reader.GetString("label"),
                    NbUnit = reader.GetInt32("nb_unit"),
                    Price = reader.GetDecimal("price"),
                    Reduction = reader.GetInt32("reduction"),
                    NbReservations = reader.GetInt32("nb_reservations")
                });
            }

            return offers;
        }

        // ⚡ Nouvelle méthode pour créer une offre
        public void CreateOffer(Offer offer)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "INSERT INTO offre (label, nb_unit, price, reduction) VALUES (@Label, @NbUnit, @Price, @Reduction)";
            using var cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@Label", offer.Label);
            cmd.Parameters.AddWithValue("@NbUnit", offer.NbUnit);
            cmd.Parameters.AddWithValue("@Price", offer.Price);
            cmd.Parameters.AddWithValue("@Reduction", offer.Reduction);

            cmd.ExecuteNonQuery();
        }

        public void UpdateOffer(Offer offer)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = @"UPDATE offre 
                     SET label = @Label,
                         nb_unit = @NbUnit,
                         price = @Price,
                         reduction = @Reduction
                     WHERE id = @Id";

            using var cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@Label", offer.Label);
            cmd.Parameters.AddWithValue("@NbUnit", offer.NbUnit);
            cmd.Parameters.AddWithValue("@Price", offer.Price);
            cmd.Parameters.AddWithValue("@Reduction", offer.Reduction);
            cmd.Parameters.AddWithValue("@Id", offer.Id);

            cmd.ExecuteNonQuery();
        }

        public bool OfferHasReservations(int offerId)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "SELECT COUNT(*) FROM reservation WHERE offre_id = @OfferId";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@OfferId", offerId);

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            return count > 0;
        }

        public void DeleteOffer(int offerId)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "DELETE FROM offre WHERE id = @Id";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", offerId);

            cmd.ExecuteNonQuery();
        }
    }
}