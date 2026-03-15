using MySql.Data.MySqlClient;
using W2G_desktop.Database;
using W2G_desktop.Models;
using System.Collections.Generic;

namespace W2G_desktop.Services
{
    public class BayService
    {
        private Database.Database db = new Database.Database();

        public List<Bay> GetBays()
        {
            List<Bay> bays = new List<Bay>();

            using (var conn = db.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM bay";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    bays.Add(new Bay
                    {
                        Id = reader.GetInt32("id"),
                        Size = reader.GetInt32("size"),
                        Label = reader.GetString("label")
                    });
                }
            }

            return bays;
        }

        public int CreateBay(int size)
        {
            using (var conn = db.GetConnection())
            {
                conn.Open();

                // Calculer le prochain numéro pour le label
                string countQuery = "SELECT COUNT(*) FROM bay";
                var countCmd = new MySqlCommand(countQuery, conn);
                int nextNumber = Convert.ToInt32(countCmd.ExecuteScalar()) + 1;
                string label = "B0" + nextNumber;

                string insertQuery = "INSERT INTO bay (label, size) VALUES (@Label, @Size)";
                var insertCmd = new MySqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@Label", label);
                insertCmd.Parameters.AddWithValue("@Size", size);

                insertCmd.ExecuteNonQuery();
                return (int)insertCmd.LastInsertedId;
            }
        }

        public void UpdateBay(Bay bay)
        {
            using (var conn = db.GetConnection())
            {
                conn.Open();

                string query = "UPDATE bay SET label = @Label, size = @Size WHERE id = @Id";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Label", bay.Label);
                cmd.Parameters.AddWithValue("@Size", bay.Size);
                cmd.Parameters.AddWithValue("@Id", bay.Id);

                cmd.ExecuteNonQuery();
            }
        }
    }
}