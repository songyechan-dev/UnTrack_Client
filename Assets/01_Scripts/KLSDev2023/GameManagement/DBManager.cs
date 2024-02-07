using System;
using System.Data;
using MySql.Data.MySqlClient;
using Unity.VisualScripting;

public static class DBManager
{
    private static string connectionString = "Server=songdemon.cafe24.com;Database=songdemon;Uid=songdemon;Pwd=klsdev2023@;";

    public static void InsertData(string roomName, string nickName, string team)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = $"INSERT INTO GameInfo (roomName, nickName,team) VALUES ('{roomName}', '{nickName}', '{team}')";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    public static DataTable SelectData(string roomName)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = $"SELECT * FROM GameInfo WHERE roomName = '{roomName}'";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }
    }

    public static DataTable SelectDataPlayer(string roomName, string nickName)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = $"SELECT * FROM GameInfo WHERE roomName = '{roomName}' and nickName = '{nickName}'";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }
    }



    public static void DeleteData(string roomName, string nickName)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = $"DELETE FROM GameInfo WHERE roomName = '{roomName}' and nickName = '{nickName}'";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    public static void DeleteDataAll(string roomName)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = $"DELETE FROM GameInfo WHERE roomName = '{roomName}'";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    public static void InsertTeamDataToRanking(string roomName, int num)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            
            string query = $"INSERT INTO totalranking(teamName, player_{num}) SELECT roomName, nickName FROM GameInfo WHERE roomName = '{roomName}';";
            
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }

        }
    }

    public static void InsertClearDataToRanking(float totalClearTime, string team)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = $"INSERT INTO totalranking(totalClearTime) VALUES('{totalClearTime}') WHERE teamName = '{team}';";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }

        }
    }

    public static void InsertRoundClearData(float roundClearTime, string team, int round)
    {
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = $"INSERT INTO totalranking(round{round}_clearTime) VALUES('{roundClearTime}') WHERE teamName = '{team}';";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }

        }
    }
}
