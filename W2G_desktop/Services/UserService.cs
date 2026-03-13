using MySql.Data.MySqlClient;
using BCrypt.Net;
using System.Collections.Generic;
using W2G_desktop.Models;

namespace W2G_desktop.Services
{
    public class UserService
    {
        private string connectionString = "server=localhost;database=w2g;uid=root;pwd=;";

        public User Authenticate(string email, string password)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "SELECT * FROM user WHERE email=@Email LIMIT 1";
            var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string hash = reader.GetString("password");
                if (BCrypt.Net.BCrypt.Verify(password, hash))
                {
                    return new User
                    {
                        Id = reader.GetInt32("id"),
                        Email = reader.GetString("email"),
                        Username = reader.GetString("username"),
                        Role = reader.GetString("roles"),
                        Discr = reader.GetString("discr")
                    };
                }
            }

            return null;
        }

        public bool EmailExists(string email)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "SELECT COUNT(*) FROM user WHERE email=@Email";
            var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            return (long)cmd.ExecuteScalar() > 0;
        }

        public bool CreateUser(string email, string username, string password, string role)
        {
            if (EmailExists(email))
                return false;

            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            string query = "INSERT INTO user (email, username, password, roles) VALUES (@Email, @Username, @Password, @Roles)";
            var cmd = new MySqlCommand(query, conn);

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", hashedPassword);
            string jsonRole = $"\"{role}\"";
            cmd.Parameters.AddWithValue("@Roles", jsonRole);

            return cmd.ExecuteNonQuery() > 0;
        }

        public List<User> GetCustomers()
        {
            var customers = new List<User>();
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "SELECT id, email, username, roles, discr FROM user WHERE discr='customer' OR discr='company'";
            var cmd = new MySqlCommand(query, conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                customers.Add(new User
                {
                    Id = reader.GetInt32("id"),
                    Email = reader.GetString("email"),
                    Username = reader.GetString("username"),
                    Role = reader.GetString("roles"),
                    Discr = reader.GetString("discr")
                });
            }

            return customers;
        }
    }
}