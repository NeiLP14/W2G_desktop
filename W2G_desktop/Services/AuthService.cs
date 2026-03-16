using MySql.Data.MySqlClient;
using W2G_desktop.Database;
using W2G_desktop.Models;

namespace W2G_desktop.Services
{
    public class AuthService
    {
        Database.Database db = new Database.Database();

        public User Login(string email, string password)
        {
            using (var conn = db.GetConnection())
            {
                conn.Open();

                string query = @"SELECT id, email, role 
                                 FROM user
                                 WHERE email = @email 
                                 AND password = @password";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", password);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = reader.GetInt32("id"),
                            Email = reader.GetString("email"),
                            Role = reader.GetString("role") // récupère le rôle
                        };
                    }
                }
            }

            return null; // utilisateur non trouvé
        }
    }
}