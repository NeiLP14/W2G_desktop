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
                    while (reader.Read())
                    {
                        interventions.Add(new Intervention
                        {
                            Id = reader.GetInt32("id"),
                            DateDeb = reader.GetDateTime("date_deb"),
                            DateFin = reader.GetDateTime("date_fin"),
                            Description = reader.GetString("description"),
                            Technician = reader.GetString("technician"),
                            UnitLabel = reader.GetString("unit_label"),
                            UnitPosition = reader.GetInt32("position"),
                            BayLabel = reader.GetString("bay_label"),
                            TypeIntervention = reader.GetString("type_intervention")
                        });
                    }
                }
            }

            return interventions;
        }
    }
}