using System;
using System.Collections.Generic;
using System.IO;
using MySql.Data.MySqlClient;

namespace Database
{
    internal class Program
    {
        private const string InitialConnectionString = "Server=localhost;Uid=root;Pwd=Welcome123;AutoEnlist=false";
        private const string MasterConnectionString = "Server=localhost;Database=master;Uid=root;Pwd=Welcome123;AutoEnlist=false";
        private const string LogsConnectionString = "Server=localhost;Database=logs;Uid=root;Pwd=Welcome123;AutoEnlist=false";

        private static readonly Dictionary<string, string> ConnectionStrings = new Dictionary<string, string>
        {
            { "team_international", "Server=localhost;Database=team_international;Uid=root;Pwd=Welcome123;AutoEnlist=false" },
            { "team_international_tests", "Server=localhost;Database=team_international_tests;Uid=root;Pwd=Welcome123;AutoEnlist=false" },
            { "company", "Server=localhost;Database=company;Uid=root;Pwd=Welcome123;AutoEnlist=false" },
            { "company_tests", "Server=localhost;Database=company_tests;Uid=root;Pwd=Welcome123;AutoEnlist=false" }
        };

        private static int Main(string[] args)
        {
            try
            {
                InitLogsDatabase();
                InitMasterDatabase();

                foreach (KeyValuePair<string, string> connectionString in ConnectionStrings)
                {
                    InitDatabase(connectionString.Key, connectionString.Value);
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Success!");
                Console.ResetColor();

                return 0;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }
        }
        
        
        private static void InitLogsDatabase()
        {
            CreateDatabase("logs");
            List<string> commandTexts = GetCommandTextList(@"\Tables\Logs");

            foreach (string commandText in commandTexts)
            {
                ExecuteCommand(LogsConnectionString, commandText);
            }
        }

        private static void InitMasterDatabase()
        {
            CreateDatabase("master");

            List<string> commandTexts = GetCommandTextList(@"\Tables\Master");
            commandTexts.AddRange(GetCommandTextList(@"\StoredProcedures\Master"));
            commandTexts.AddRange(GetCommandTextList(@"\Scripts\DefaultData\Master"));

            foreach (string commandText in commandTexts)
            {
                ExecuteCommand(MasterConnectionString, commandText);
            }
        }

        private static void InitDatabase(string database, string connectionString)
        {
            CreateDatabase(database);

            List<string> commandTexts = GetCommandTextList(@"\Tables\");
            commandTexts.AddRange(GetCommandTextList(@"\StoredProcedures\"));
            commandTexts.AddRange(GetCommandTextList(@"\Scripts\DefaultData\"));

            foreach (string commandText in commandTexts)
            {
                ExecuteCommand(connectionString, commandText);
            }
        }

        private static void CreateDatabase(string database)
        {
            string commandText = $"CREATE DATABASE IF NOT EXISTS `{database}`";

            ExecuteCommand(InitialConnectionString, commandText);
        }

        private static void ExecuteCommand(string connectionString, string commandText)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = commandText;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static List<string> GetCommandTextList(string subFolder)
        {
            List<string> result = new List<string>();
            
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string[] filePathes = Directory.GetFiles(projectDirectory + subFolder);

            foreach (string path in filePathes)
            {
                string text = File.ReadAllText(path);

                result.Add(text);
            }

            return result;
        }
    }
}