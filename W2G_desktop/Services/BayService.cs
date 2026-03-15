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

                using var transaction = conn.BeginTransaction();

                try
                {
                    // Calculer le prochain numéro pour le label
                    string countQuery = "SELECT COUNT(*) FROM bay";
                    var countCmd = new MySqlCommand(countQuery, conn, transaction);
                    int nextNumber = Convert.ToInt32(countCmd.ExecuteScalar()) + 1;

                    string label = "B" + nextNumber.ToString("D3"); // B001, B002...

                    // Création de la baie
                    string insertBayQuery = "INSERT INTO bay (label, size) VALUES (@Label, @Size)";
                    var insertBayCmd = new MySqlCommand(insertBayQuery, conn, transaction);
                    insertBayCmd.Parameters.AddWithValue("@Label", label);
                    insertBayCmd.Parameters.AddWithValue("@Size", size);

                    insertBayCmd.ExecuteNonQuery();
                    int bayId = (int)insertBayCmd.LastInsertedId;

                    // Création des unités
                    string insertUnitQuery = @"
            INSERT INTO unit (label, is_occuped, position, bay_id, state_id)
            VALUES (@Label, 0, @Position, @BayId, 1)";

                    for (int i = 1; i <= size; i++)
                    {
                        string unitLabel = "U" + i.ToString("D2"); // U01, U02...

                        var insertUnitCmd = new MySqlCommand(insertUnitQuery, conn, transaction);
                        insertUnitCmd.Parameters.AddWithValue("@Label", unitLabel);
                        insertUnitCmd.Parameters.AddWithValue("@Position", i);
                        insertUnitCmd.Parameters.AddWithValue("@BayId", bayId);

                        insertUnitCmd.ExecuteNonQuery();
                    }

                    transaction.Commit();

                    return bayId;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void UpdateBay(Bay bay)
        {
            using (var conn = db.GetConnection())
            {
                conn.Open();

                using var transaction = conn.BeginTransaction();

                try
                {
                    // récupérer la taille actuelle
                    string getSizeQuery = "SELECT size FROM bay WHERE id = @Id";
                    var getSizeCmd = new MySqlCommand(getSizeQuery, conn, transaction);
                    getSizeCmd.Parameters.AddWithValue("@Id", bay.Id);

                    int currentSize = Convert.ToInt32(getSizeCmd.ExecuteScalar());
                    int newSize = bay.Size;

                    // Si on réduit la taille
                    if (newSize < currentSize)
                    {
                        string checkQuery = @"
                                            SELECT COUNT(*) 
                                            FROM unit 
                                            WHERE bay_id = @BayId 
                                            AND position > @NewSize 
                                            AND is_occuped = 1";

                        var checkCmd = new MySqlCommand(checkQuery, conn, transaction);
                        checkCmd.Parameters.AddWithValue("@BayId", bay.Id);
                        checkCmd.Parameters.AddWithValue("@NewSize", newSize);

                        int occupiedUnits = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (occupiedUnits > 0)
                        {
                            throw new Exception("Impossible de réduire la baie : certaines unités sont occupées.");
                        }

                        // supprimer les unités en trop
                        string deleteQuery = @"
                                            DELETE FROM unit
                                            WHERE bay_id = @BayId
                                            AND position > @NewSize";

                        var deleteCmd = new MySqlCommand(deleteQuery, conn, transaction);
                        deleteCmd.Parameters.AddWithValue("@BayId", bay.Id);
                        deleteCmd.Parameters.AddWithValue("@NewSize", newSize);

                        deleteCmd.ExecuteNonQuery();
                    }

                    // Si on augmente la taille
                    if (newSize > currentSize)
                    {
                        string insertUnitQuery = @"
                                                INSERT INTO unit (label, is_occuped, position, bay_id, state_id)
                                                VALUES (@Label, 0, @Position, @BayId, 1)";

                        for (int i = currentSize + 1; i <= newSize; i++)
                        {
                            string unitLabel = "U" + i.ToString("D2");

                            var insertCmd = new MySqlCommand(insertUnitQuery, conn, transaction);
                            insertCmd.Parameters.AddWithValue("@Label", unitLabel);
                            insertCmd.Parameters.AddWithValue("@Position", i);
                            insertCmd.Parameters.AddWithValue("@BayId", bay.Id);

                            insertCmd.ExecuteNonQuery();
                        }
                    }

                    // mise à jour de la baie
                    string updateQuery = "UPDATE bay SET label = @Label, size = @Size WHERE id = @Id";

                    var updateCmd = new MySqlCommand(updateQuery, conn, transaction);
                    updateCmd.Parameters.AddWithValue("@Label", bay.Label);
                    updateCmd.Parameters.AddWithValue("@Size", bay.Size);
                    updateCmd.Parameters.AddWithValue("@Id", bay.Id);

                    updateCmd.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}