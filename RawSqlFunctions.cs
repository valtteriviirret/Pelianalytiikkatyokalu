using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class RawSqlFunctions
{
    MySqlConnection cnn = Connector.GetConnection();
    public void AssingQuery(string input)
    {
        string[] tokens = input.Split(' ');

        switch (tokens[0].ToLower())
        {
            case "select": SelectQuery(input, 0); break;
            case "update": NonQuery(input); break;
            case "delete": NonQuery(input); break;
            case "insert": NonQuery(input); break;

            case "1": SelectQuery("select summa from Rahasiirto", 1); break;
            case "2": SelectQuery("select summa from Rahasiirto", 2); break;
            case "3": SelectQuery("select alkuaika, loppuaika from Pelisessio", 3); break;
            case "4": SelectQuery("select summa, date(aikaleima), sessio from Rahasiirto;", 4); break;
            case "5": SelectQuery("select sessio_id, alkuaika, loppuaika from Pelisessio", 5); break;
            case "6": SelectQuery("select tapahtuma_tyyppi_id from Pelitapahtuma where tapahtuma_tyyppi_id < 3;", 6); break;
            case "7": SelectQuery("select peli_id, peli_nimi from Peli", 7); break;
            case "8": SelectQuery("select summa, date(aikaleima), sessio from Rahasiirto;", 8); break;
            case "help": Help(); break;
            default: break;
        }
    }

    public void SelectQuery(string query, int id)
    {
        MySqlCommand cmd = new MySqlCommand(query, cnn);
        MySqlDataReader reader = cmd.ExecuteReader();

        switch (id)
        {
            case 1: AnalyticFunctions.AverageBuy(reader); break;
            case 2: AnalyticFunctions.MedianBuy(reader); break;
            case 3: AnalyticFunctions.AveragePlaytime(reader); break;
            case 4: AnalyticFunctions.DaysTransActions(reader); break;
            case 5: AnalyticFunctions.CurrentSessions(reader); break;
            case 6: AnalyticFunctions.CompletePercent(reader); break;
            case 7: AnalyticFunctions.GameInfo(reader); break;
            case 8: AnalyticFunctions.TransactionsCount(reader); break;
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
        while (reader.Read())
            for (int i = 0; i < reader.FieldCount; i++)
                Console.Write(reader[i] + "\n");
        reader.Close();
    }

    void Help()
    {

        /* english
        Console.WriteLine("Functions");
        Console.WriteLine("(1) Average spending");
        Console.WriteLine("(2) Median spending");
        Console.WriteLine("(3) Average playtime");
        Console.WriteLine("(4) Game transactions");
        Console.WriteLine("(5) Current game sesssions");
        Console.WriteLine("(6) Game completion percentage");
        Console.WriteLine("(7) About a specific game "); 
        */

        // finnish
        Console.WriteLine("Mahdolliset kyselyt");
        Console.WriteLine("(1) Pelin keskiostos");
        Console.WriteLine("(2) Pelin mediaaniostos");
        Console.WriteLine("(3) Pelin keskipeliaika");
        Console.WriteLine("(4) Rahasiirrot");
        Console.WriteLine("(5) Käynnissä olevat pelisessiot");
        Console.WriteLine("(6) Pelin läpäilyprosentti");
        Console.WriteLine("(7) Tietoja tietystä pelistä");

        Console.WriteLine("8");
        Console.WriteLine("\n(0) Tietokannat");
    }

}

