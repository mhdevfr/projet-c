using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace MySQLClientApp
{
    public class DatabaseManager
    {
        private string connectionString;

        public DatabaseManager(string server, string database, string uid, string password, int port = 3306)
        {
            connectionString = $"Server={server};Database={database};Port={port};Uid={uid};Pwd={password};";
        }

        public DatabaseManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Teste la connexion à la base de données
        /// </summary>
        /// <returns>True si la connexion est réussie, False sinon</returns>
        public bool TestConnection()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Exécute une requête SQL et retourne les résultats sous forme de DataTable
        /// </summary>
        /// <param name="query">Requête SQL à exécuter</param>
        /// <param name="parameters">Paramètres de la requête (optionnel)</param>
        /// <returns>DataTable contenant les résultats</returns>
        public DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null)
        {
            DataTable dataTable = new DataTable();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Ajouter les paramètres si fournis
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value);
                            }
                        }

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'exécution de la requête : {ex.Message}");
            }

            return dataTable;
        }

        /// <summary>
        /// Exécute une commande SQL non-query (INSERT, UPDATE, DELETE)
        /// </summary>
        /// <param name="query">Requête SQL à exécuter</param>
        /// <param name="parameters">Paramètres de la requête (optionnel)</param>
        /// <returns>Nombre de lignes affectées</returns>
        public int ExecuteNonQuery(string query, Dictionary<string, object> parameters = null)
        {
            int affectedRows = 0;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Ajouter les paramètres si fournis
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value);
                            }
                        }

                        affectedRows = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'exécution de la commande : {ex.Message}");
            }

            return affectedRows;
        }

        /// <summary>
        /// Exécute une requête SQL et retourne une seule valeur
        /// </summary>
        /// <param name="query">Requête SQL à exécuter</param>
        /// <param name="parameters">Paramètres de la requête (optionnel)</param>
        /// <returns>Valeur retournée ou null</returns>
        public object ExecuteScalar(string query, Dictionary<string, object> parameters = null)
        {
            object result = null;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Ajouter les paramètres si fournis
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value);
                            }
                        }

                        result = command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'exécution de la requête : {ex.Message}");
            }

            return result;
        }
    }
} 