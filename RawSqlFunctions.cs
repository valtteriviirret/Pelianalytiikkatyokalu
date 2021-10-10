using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class RawSqlFunctions
{
    MySqlConnection cnn = Connector.GetConnection();
    
    // assing the query for proper SQL statement
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
            case "9": SelectQuery(@"SELECT etunimi, sukunimi, SUM(summa) as 'yhteensä' FROM Pelaaja,
                                Rahasiirto, Pelisessio WHERE pelisessio_pelaaja_id = pelaaja_id AND sessio = sessio_id
                                GROUP BY etunimi ORDER BY SUM(summa) DESC LIMIT 1;", 9);
                                break;
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
            case 9: AnalyticFunctions.BiggestSpender(reader); break;
            default: DefaultSelect(query, reader); break;
        }
    }

    // update, delete and insert querys
    public void NonQuery(string input)
    {
        MySqlCommand cmd = new MySqlCommand(input, cnn);
        cmd.ExecuteNonQuery();
    }

    // select query with SQL syntax
    void DefaultSelect(string query, MySqlDataReader reader)
    {
        List<string> columns = new List<string>();
        while (reader.Read())
            for (int i = 0; i < reader.FieldCount; i++)
                Console.Write(reader[i] + "\n");
        reader.Close();
    }

    // show all commands
    void Help()
    {
        Console.Write("Mahdolliset kyselyt\n" +
        "(1) Pelin keskiostos\n" + 
        "(2) Pelin mediaaniostos\n" +
        "(3) Pelin keskipeliaika\n" +
        "(4) Rahasiirrot\n" +
        "(5) Käynnissä olevat pelisessiot\n" +
        "(6) Pelin läpäilyprosentti\n" +
        "(7) Tietoja tietystä pelistä\n" +
        "(8) Viikon sisällä tehdyt ostot\n" +
        "(9) Eniten rahaa käyttänyt pelaaja\n");
    
    }

}

