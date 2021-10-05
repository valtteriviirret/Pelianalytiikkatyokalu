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
            case "sessiot": SelectQuery("select sessio_id, alkuaika, loppuaika from Pelisessio", 5); break;
            case "läpäisyprosentti": SelectQuery("select tapahtuma_tyyppi_id from Pelitapahtuma where tapahtuma_tyyppi_id < 3;", 6); break;
            case "peli": SelectQuery("select peli_id, peli_nimi from Peli", 7); break;
            case "help" : Help(); break;
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
            case 4: AnalyticFunctions.DaysTransActions(reader); break;
            case 5: AnalyticFunctions.CurrentSessions(reader); break;
            case 6: AnalyticFunctions.CompletePercent(reader); break;
            case 7: AnalyticFunctions.GameInfo(reader); break;
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

    void Help()
    {
        Console.WriteLine("Kaikki mahdolliset kyselyt mitä mitä ohjelmassa voi toteuttaa");
        Console.WriteLine("Pelin keskiostos -> \"keskiostos\"");
        Console.WriteLine("Pelin mediaaniostos -> \"mediaaniostos\"");
        Console.WriteLine("Pelin keskipeliaika -> \"keskipeliaika\"");
        // rahasiirrot
        Console.WriteLine("Käynnissä olevat pelisessiot -> sessiot");
        Console.WriteLine("Pelin läpäilyprosentti -> \"läpäisyprosentti\"");
        Console.WriteLine("Tietoja tietystä pelistä -> \"peli\"");


        // mahdollinen muutos ? 

        Console.WriteLine("Mahdolliset kyselyt");
        Console.WriteLine("(1) Pelin keskiostos -> \"keskiostos\"");
        Console.WriteLine("(2) Pelin mediaaniostos -> \"mediaaniostos\"");
        Console.WriteLine("(3) Pelin keskipeliaika -> \"keskipeliaika\"");
        // rahasiirrot
        Console.WriteLine("(4) Käynnissä olevat pelisessiot -> sessiot");
        Console.WriteLine("(5) Pelin läpäilyprosentti -> \"läpäisyprosentti\"");
        Console.WriteLine("(6) Tietoja tietystä pelistä -> \"peli\"");
    }

}

