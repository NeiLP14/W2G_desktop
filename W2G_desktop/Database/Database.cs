using MySql.Data.MySqlClient;

namespace W2G_desktop.Database
{
    public class Database
    {
        private string connectionString =
            "server=localhost;database=w2g;user=root;password=;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
