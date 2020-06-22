﻿using MySql.Data.MySqlClient;
using ProjektSemestrIV.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektSemestrIV.DAL.Repositories
{
    class TargetRepository
    {
        #region CRUD
        public static List<Target> GetTargetsWhere(uint shooter_id, uint stage_id)
        {
            var query = $"SELECT * FROM tarcza WHERE strzelec_id = '{shooter_id}' and trasa_id = '{stage_id}'";
            List<Target> targets = new List<Target>();

            using (MySqlConnection connection = DatabaseConnection.Instance.Connection)
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    targets.Add(new Target(reader));
                }
                connection.Close();
            }
            return targets;
        }

        public static bool AddTargetToDatabase(Target target)
        {
            bool executed = false;
            var query = @"INSERT INTO tarcza (`strzelec_id`, `trasa_id`, `alpha`, `charlie`, `delta`, `miss`, `n-s`, `proc`, `extra`)
                            VALUES (@strzelec_id, @trasa_id, @alpha, @charlie, @delta, @miss, @n-s, @proc, @extra)";

            using (MySqlConnection connection = DatabaseConnection.Instance.Connection)
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                foreach (var parameter in target.GetParameters())
                {
                    command.Parameters.Add(parameter);
                }

                connection.Open();
                if (command.ExecuteNonQuery() == 1) executed = true;
                connection.Close();
            }
            return executed;
        }

        public static bool EditTargetInDatabase(Target target, uint target_id)
        {
            Boolean executed = false;
            var query = $@"UPDATE `tarcza` 
                            SET `strzelec_id` = @strzelec_id, `trasa_id` = @trasa_id, 
                                `alpha` = @alpha, `charlie` = @charlie, 
                                `delta` = @delta, `miss` = @miss, `n-s` = @n-s, 
                                `proc` = @proc, `extra` = @extra 
                            WHERE strzelec_id = '{target_id}'";

            using (MySqlConnection connection = DatabaseConnection.Instance.Connection)
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                foreach (var parameter in target.GetParameters())
                {
                    command.Parameters.Add(parameter);
                }

                connection.Open();
                if (command.ExecuteNonQuery() == 1) executed = true;
                connection.Close();
            }
            return executed;
        }

        public static bool DeleteTargetFromDatabase(uint targetID)
        {
            bool executed = false;
            var query = $"DELETE FROM tarcza WHERE (`id` = '{targetID}')";

            using (MySqlConnection connection = DatabaseConnection.Instance.Connection)
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                connection.Open();
                if (command.ExecuteNonQuery() == 1) executed = true;
                connection.Close();
            }
            return executed;
        }
        #endregion
    }
}
