using MySql.Data.MySqlClient;
using W2G_desktop.Database;
using W2G_desktop.Models;

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
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    bays.Add(new Bay
                    {
                        Id = reader.GetInt32("id"),
                        Size = reader.GetInt32("size")
                    });
                }
            }

            return bays;
        }
    }
}
