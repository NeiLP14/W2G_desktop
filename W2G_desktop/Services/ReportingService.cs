using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using W2G_desktop.Models;

namespace W2G_desktop.Services
{
    public class ReportingService
    {
        private Database.Database db = new Database.Database();

        public List<ReportingData> ChiffreAffaire()
        {
            List<ReportingData> results = new List<ReportingData>();

            using var conn = db.GetConnection();
            conn.Open();

            string query = @"
                SELECT 
                    DATE_FORMAT(r.date_deb, '%Y-%m') AS mois,
                    o.label AS offre,
                    COUNT(r.id) AS nb_reservations,
                    SUM(o.price) AS chiffre_affaire
                FROM reservation r
                JOIN offre o ON r.offre_id = o.id
                GROUP BY mois, o.label
                ORDER BY mois ASC;
            ";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                results.Add(new ReportingData
                {
                    Mois = reader.GetString("mois"),
                    Offre = reader.GetString("offre"),
                    NbReservations = reader.GetInt32("nb_reservations"),
                    ChiffreAffaire = reader.GetDecimal("chiffre_affaire")
                });
            }

            return results;
        }
    }
}
