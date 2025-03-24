using System;
using System.Data;

namespace MySQLClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Application de connexion MySQL");
            Console.WriteLine("-----------------------------");
            
            // Demander les informations de connexion à l'utilisateur
            Console.Write("Serveur (localhost par défaut): ");
            string server = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(server))
                server = "localhost";
            
            Console.Write("Port (3306 par défaut): ");
            string portStr = Console.ReadLine();
            int port = string.IsNullOrWhiteSpace(portStr) ? 3306 : int.Parse(portStr);
            
            Console.Write("Base de données: ");
            string database = Console.ReadLine();
            
            Console.Write("Nom d'utilisateur: ");
            string username = Console.ReadLine();
            
            Console.Write("Mot de passe: ");
            string password = Console.ReadLine();
            
            // Créer une instance de DatabaseManager
            DatabaseManager dbManager = new DatabaseManager(server, database, username, password, port);
            
            // Tester la connexion
            Console.WriteLine("\nTest de connexion à la base de données...");
            bool connectionSuccessful = dbManager.TestConnection();
            
            if (connectionSuccessful)
            {
                Console.WriteLine("Connexion réussie!");
                
                // Menu simple pour interagir avec la base de données
                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine("\nMenu:");
                    Console.WriteLine("1. Exécuter une requête SELECT");
                    Console.WriteLine("2. Exécuter une commande (INSERT, UPDATE, DELETE)");
                    Console.WriteLine("3. Quitter");
                    Console.Write("Votre choix: ");
                    
                    string choice = Console.ReadLine();
                    
                    switch (choice)
                    {
                        case "1":
                            ExecuteSelect(dbManager);
                            break;
                        case "2":
                            ExecuteCommand(dbManager);
                            break;
                        case "3":
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Choix invalide!");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Erreur de connexion à la base de données!");
            }
            
            Console.WriteLine("\nAppuyez sur une touche pour quitter...");
            Console.ReadKey();
        }
        
        static void ExecuteSelect(DatabaseManager dbManager)
        {
            Console.Write("Entrez votre requête SELECT: ");
            string query = Console.ReadLine();
            
            if (!string.IsNullOrWhiteSpace(query))
            {
                DataTable result = dbManager.ExecuteQuery(query);
                
                if (result.Rows.Count > 0)
                {
                    // Afficher les noms des colonnes
                    foreach (DataColumn column in result.Columns)
                    {
                        Console.Write($"{column.ColumnName,-15}");
                    }
                    Console.WriteLine();
                    
                    // Afficher une ligne de séparation
                    Console.WriteLine(new string('-', result.Columns.Count * 15));
                    
                    // Afficher les données
                    foreach (DataRow row in result.Rows)
                    {
                        foreach (var item in row.ItemArray)
                        {
                            Console.Write($"{item,-15}");
                        }
                        Console.WriteLine();
                    }
                    
                    Console.WriteLine($"\n{result.Rows.Count} ligne(s) retournée(s).");
                }
                else
                {
                    Console.WriteLine("Aucun résultat.");
                }
            }
        }
        
        static void ExecuteCommand(DatabaseManager dbManager)
        {
            Console.Write("Entrez votre commande SQL (INSERT, UPDATE, DELETE): ");
            string command = Console.ReadLine();
            
            if (!string.IsNullOrWhiteSpace(command))
            {
                int affectedRows = dbManager.ExecuteNonQuery(command);
                Console.WriteLine($"{affectedRows} ligne(s) affectée(s).");
            }
        }
    }
}
