using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class RawSqlFunctions
{
    MySqlConnection cnn = Connector.GetConnection();
    public void AssingQuery(string input)
    {
        string[] tokens = input.Split(' ');

        switch(tokens[0].ToLower())
        {
            case "select": SelectQuery(input, 0); break;
            case "update": NonQuery(input); break;
            case "delete": NonQuery(input); break;
            case "insert": NonQuery(input); break;
            
            case "keskiostos": SelectQuery("select summa from Rahasiirto", 1); break;
            case "mediaaniostos": SelectQuery("select summa from Rahasiirto", 2); break;
            case "keskipeliaika": SelectQuery("select alkuaika, loppuaika from Pelisessio", 3); break;
            //case "rahasiirrot": SelectQuery("select peli_id, pelisessio_pelaaja_id from Pelisessio where loppuaika =" + null, 4); break;
            //case "sessiot": SelectQuery(input, 5); break;
            default: break;
        }
    }

    public void SelectQuery(string query, int id)
    {
        MySqlCommand cmd = new MySqlCommand(query, cnn);
        MySqlDataReader reader = cmd.ExecuteReader();
        
        switch(id)
        {
            case 1: AnalyticFunctions.AverageBuy(reader); break;
            case 2: AnalyticFunctions.MedianBuy(reader); break;
            case 3: AnalyticFunctions.AveragePlaytime(reader); break;
            case 4: AnalyticFunctions.CurrentSessions(reader); break;
            case 5: AnalyticFunctions.DaysTransActions(reader); break;
            default: DefaultSelect(query, reader); break;
        }
    }

    public void NonQuery(string input)
    {
        MySqlCommand cmd = new MySqlCommand(input, cnn);
        cmd.ExecuteNonQuery();
    }

    void DefaultSelect(string query, MySqlDataReader reader)
    {
        List<string> columns = new List<string>();
        while(reader.Read())
            for(int i = 0; i < reader.FieldCount; i++)
                Console.Write(reader[i] + "\n");
        reader.Close();
    }
}

