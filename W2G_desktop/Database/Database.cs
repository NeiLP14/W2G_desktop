using MySql.Data.MySqlClient;

namespace W2G_desktop.Database
{
    public class Database
    {
        private string connectionString =
            "server=10.192.111.6;database=w2g;user=worktogether;password=w2g_iia;port=3306;";
            //"server=localhost;database=w2g;user=root;password=;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
