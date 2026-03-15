using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using W2G_desktop.Database;
using W2G_desktop.Models;

namespace W2G_desktop.Services
{
    public class UnitService
    {
        private Database.Database db = new Database.Database();

        public List<Unit> GetUnitsByBay(int bayId)
        {
            List<Unit> units = new List<Unit>();

            using (var conn = db.GetConnection())
            {
                conn.Open();

                string query = @"
                                SELECT 
                                    unit.id,
                                    unit.label,
                                    unit.position,
                                    unit.is_occuped,
                                    unit.bay_id,
                                    unit.state_id,
                                    unit.type_id,
                                    unit.reservation_id,
                                    state.label AS state_label,
                                    user.username
                                FROM unit
                                LEFT JOIN state ON unit.state_id = state.id
                                LEFT JOIN reservation ON unit.reservation_id = reservation.id
                                LEFT JOIN user ON reservation.user_id = user.id
                                WHERE unit.bay_id = @bayId
                                ORDER BY unit.position";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@BayId", bayId);

                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    units.Add(new Unit
                    {
                        Id = reader.GetInt32("id"),
                        Label = reader.GetString("label"),
                        Position = reader.GetInt32("position"),
                        IsOccupied = reader.GetBoolean("is_occuped"),
                        BayId = reader.GetInt32("bay_id"),
                        StateId = reader.GetInt32("state_id"),

                        TypeId = reader.IsDBNull(reader.GetOrdinal("type_id"))
                            ? null
                            : reader.GetInt32("type_id"),

                        ReservationId = reader.IsDBNull(reader.GetOrdinal("reservation_id"))
                            ? null
                            : reader.GetInt32("reservation_id"),

                        StateLabel = reader.IsDBNull(reader.GetOrdinal("state_label"))
                            ? "-"
                            : reader.GetString("state_label"),

                        Username = reader.IsDBNull(reader.GetOrdinal("username"))
                            ? "-"
                            : reader.GetString("username")
                    });
                }
            }

            return units;
        }

        public string GetOccupantName(int reservationId)
        {
            using (var conn = db.GetConnection())
            {
                conn.Open();
                string query = @"SELECT u.username 
                         FROM user u
                         INNER JOIN reservation r ON r.user_id = u.id
                         WHERE r.id = @ReservationId";
                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ReservationId", reservationId);
                var result = cmd.ExecuteScalar();
                return result != null ? result.ToString() : "";
            }
        }
    }
}