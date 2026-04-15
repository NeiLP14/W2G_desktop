using MySql.Data.MySqlClient;
using System.Collections.Generic;
using W2G_desktop.Models;
using W2G_desktop.Database;

namespace W2G_desktop.Services
{
    public class InterventionService
    {
        private Database.Database db = new Database.Database();

        public List<Intervention> GetAllInterventions()
        {
            List<Intervention> interventions = new List<Intervention>();

            using (var conn = db.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT 
                        i.id,
                        i.date_deb,
                        i.date_fin,
                        i.description,
                        u.username AS technician,
                        un.label AS unit_label,
                        un.position,
                        b.label AS bay_label,
                        ti.label AS type_intervention
                    FROM intervention i
                    LEFT JOIN user u ON i.technician_id = u.id
                    LEFT JOIN type_intervention ti ON i.type_intervention_id = ti.id
                    LEFT JOIN intervention_unit iu ON iu.intervention_id = i.id
                    LEFT JOIN unit un ON iu.unit_id = un.id
                    LEFT JOIN bay b ON un.bay_id = b.id
                    ORDER BY i.date_deb DESC";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    // Récupération des index (optimisation)
                    int ordId = reader.GetOrdinal("id");
                    int ordDateDeb = reader.GetOrdinal("date_deb");
                    int ordDateFin = reader.GetOrdinal("date_fin");
                    int ordDescription = reader.GetOrdinal("description");
                    int ordTechnician = reader.GetOrdinal("technician");
                    int ordUnitLabel = reader.GetOrdinal("unit_label");
                    int ordPosition = reader.GetOrdinal("position");
                    int ordBayLabel = reader.GetOrdinal("bay_label");
                    int ordType = reader.GetOrdinal("type_intervention");

                    while (reader.Read())
                    {
                        var intervention = new Intervention
                        {
                            Id = reader.GetInt32(ordId),
                            DateDeb = reader.GetDateTime(ordDateDeb),
                            DateFin = reader.GetDateTime(ordDateFin),
                            Description = reader.GetString(ordDescription),

                            Technician = reader.IsDBNull(ordTechnician)
                                ? null
                                : reader.GetString(ordTechnician),

                            UnitLabel = reader.IsDBNull(ordUnitLabel)
                                ? null
                                : reader.GetString(ordUnitLabel),

                            UnitPosition = reader.IsDBNull(ordPosition)
                                ? null
                                : reader.GetInt32(ordPosition),

                            BayLabel = reader.IsDBNull(ordBayLabel)
                                ? null
                                : reader.GetString(ordBayLabel),

                            TypeIntervention = reader.IsDBNull(ordType)
                                ? null
                                : reader.GetString(ordType)
                        };

                        interventions.Add(intervention);
                    }
                }
            }

            return interventions;
        }
    }
}