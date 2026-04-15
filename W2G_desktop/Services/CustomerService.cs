using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using W2G_desktop.Models;

namespace W2G_desktop.Services
{
    public class CustomerService
    {
        private string connectionString = "server=localhost;database=w2g;uid=root;pwd=;";

        public List<Customer> GetCustomers()
        {
            List<Customer> customers = new List<Customer>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string query = @"SELECT u.id, u.email, u.username, u.nom, u.prenom
                                 FROM user u
                                 JOIN customer c ON u.id = c.id";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    customers.Add(new Customer
                    {
                        Id = reader.GetInt32("id"),
                        Email = reader.GetString("email"),
                        Username = reader.GetString("username"),
                        Nom = reader.IsDBNull("nom") ? "" : reader.GetString("nom"),
                        Prenom = reader.IsDBNull("prenom") ? "" : reader.GetString("prenom")
                    });
                }
            }

            return customers;
        }
    }
}