using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using W2G_desktop.Models;

namespace W2G_desktop.Services
{
    public class ReservationService
    {
        private string connectionString = "server=localhost;database=w2g;uid=root;pwd=;";

        // Récupérer toutes les réservations
        public List<Reservation> GetAllReservations()
        {
            List<Reservation> reservations = new List<Reservation>();

            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "SELECT id, user_id, bay_id, start_time, end_time, status FROM reservation";
            var cmd = new MySqlCommand(query, conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                reservations.Add(new Reservation
                {
                    Id = reader.GetInt32("id"),
                    UserId = reader.GetInt32("user_id"),
                    BayId = reader.GetInt32("bay_id"),
                    StartTime = reader.GetDateTime("start_time"),
                    EndTime = reader.GetDateTime("end_time"),
                    Status = reader.GetString("status")
                });
            }

            return reservations;
        }

        // Optionnel : récupérer les réservations pour un utilisateur spécifique
        public List<Reservation> GetReservationsByUser(int userId)
        {
            List<Reservation> reservations = new List<Reservation>();

            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "SELECT id, user_id, bay_id, start_time, end_time, status FROM reservation WHERE user_id=@UserId";
            var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                reservations.Add(new Reservation
                {
                    Id = reader.GetInt32("id"),
                    UserId = reader.GetInt32("user_id"),
                    BayId = reader.GetInt32("bay_id"),
                    StartTime = reader.GetDateTime("start_time"),
                    EndTime = reader.GetDateTime("end_time"),
                    Status = reader.GetString("status")
                });
            }

            return reservations;
        }
    }
}