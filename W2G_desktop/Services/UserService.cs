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

        public List<User> GetAllUsers()
        {
            var users = new List<User>();

            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "SELECT id, email, username, roles, discr FROM user";

            var cmd = new MySqlCommand(query, conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new User
                {
                    Id = reader.GetInt32("id"),
                    Email = reader.GetString("email"),
                    Username = reader.GetString("username"),
                    Role = reader.GetString("roles"),
                    Discr = reader.GetString("discr")
                });
            }

            return users;
        }

        public bool CreateUser(string email, string username, string password, string role)
        {
            if (EmailExists(email))
                return false;

            // Déterminer le discr automatiquement selon le rôle
            string discr;
            switch (role)
            {
                case "ROLE_ADMIN":
                    discr = "admin";
                    break;
                case "ROLE_TECHNICIAN":
                    discr = "technician";
                    break;
                case "ROLE_ACCOUNTANT":
                    discr = "accountant";
                    break;
                default:
                    discr = "staff"; // fallback si un rôle inconnu est passé
                    break;
            }

            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = "INSERT INTO user (email, username, password, roles, discr) VALUES (@Email, @Username, @Password, @Roles, @Discr)";
            using var cmd = new MySqlCommand(query, conn);

            // Hash du mot de passe
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@Password", hashedPassword);
            string jsonRole = $"\"{role}\""; // stocker en JSON si tu le fais pour tous les rôles
            cmd.Parameters.AddWithValue("@Roles", jsonRole);
            cmd.Parameters.AddWithValue("@Discr", discr);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool UpdateUser(User user)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            string query = @"UPDATE user 
                            SET email = @Email,
                                username = @Username,
                                roles = @Roles,
                                discr = @Discr
                            WHERE id = @Id";

            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Email", user.Email);
            cmd.Parameters.AddWithValue("@Username", user.Username);
            cmd.Parameters.AddWithValue("@Roles", $"\"{user.Role}\""); // JSON comme avant
            cmd.Parameters.AddWithValue("@Discr", user.Discr);
            cmd.Parameters.AddWithValue("@Id", user.Id);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool DeleteUser(int userId)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();

            using var transaction = conn.BeginTransaction();

            try
            {
                // Supprimer d'abord les réservations de cet utilisateur
                string deleteReservationsQuery = "DELETE FROM reservation WHERE user_id = @UserId";
                using var cmdReservations = new MySqlCommand(deleteReservationsQuery, conn, transaction);
                cmdReservations.Parameters.AddWithValue("@UserId", userId);
                cmdReservations.ExecuteNonQuery();

                // Supprimer ensuite l'utilisateur
                string deleteUserQuery = "DELETE FROM user WHERE id = @UserId";
                using var cmdUser = new MySqlCommand(deleteUserQuery, conn, transaction);
                cmdUser.Parameters.AddWithValue("@UserId", userId);
                cmdUser.ExecuteNonQuery();

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }
    }
}