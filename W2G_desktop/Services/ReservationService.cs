using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using W2G_desktop.Models;

namespace W2G_desktop.Services
{
    public class ReservationService
    {
        private Database.Database db = new Database.Database();

        // Récupérer toutes les réservations
        public List<Reservation> GetAllReservations()
        {
            List<Reservation> reservations = new List<Reservation>();

            using var conn = db.GetConnection();
            conn.Open();

            string query = @"
                SELECT 
                    r.id,
                    r.user_id,
                    r.offre_id,
                    r.date_deb,
                    r.date_fin,
                    u.username,
                    o.label AS offre
                FROM reservation r
                JOIN user u ON r.user_id = u.id
                JOIN offre o ON r.offre_id = o.id
            ";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                reservations.Add(new Reservation
                {
                    Id = reader.GetInt32("id"),
                    UserId = reader.GetInt32("user_id"),
                    OffreId = reader.GetInt32("offre_id"),
                    DateDeb = reader.GetDateTime("date_deb"),
                    DateFin = reader.GetDateTime("date_fin"),
                    Username = reader.GetString("username"),
                    Offre = reader.GetString("offre")
                });
            }

            return reservations;
        }

        // Récupérer les réservations d’un utilisateur
        public List<Reservation> GetReservationsByUser(int userId)
        {
            List<Reservation> reservations = new List<Reservation>();

            using var conn = db.GetConnection();
            conn.Open();

            string query = @"
                SELECT 
                    r.id,
                    r.user_id,
                    r.offre_id,
                    r.date_deb,
                    r.date_fin,
                    u.username,
                    o.label AS offre
                FROM reservation r
                JOIN user u ON r.user_id = u.id
                JOIN offre o ON r.offre_id = o.id
                WHERE r.user_id = @UserId
            ";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                reservations.Add(new Reservation
                {
                    Id = reader.GetInt32("id"),
                    UserId = reader.GetInt32("user_id"),
                    OffreId = reader.GetInt32("offre_id"),
                    DateDeb = reader.GetDateTime("date_deb"),
                    DateFin = reader.GetDateTime("date_fin"),
                    Username = reader.GetString("username"),
                    Offre = reader.GetString("offre")
                });
            }

            return reservations;
        }
    }
}