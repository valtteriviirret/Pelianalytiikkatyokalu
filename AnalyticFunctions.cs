using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class AnalyticFunctions
{

    // get table names in selected database
    public static void GetDatabases(MySqlDataReader reader)
    {
        Console.WriteLine("Taulut tietokannassa: ");
        while (reader.Read())
            for (int i = 0; i < reader.FieldCount; i++)
                Console.WriteLine(reader[i]);

        reader.Close();
    }

    // get the average spending
    public static void AverageBuy(MySqlDataReader reader)
    {
        double sum = 0, a = 0;
        int fc = 0;
        while (reader.Read())
            for (int i = 0; i < reader.FieldCount; i++)
            {
                sum += reader.GetDouble(i);
                fc++;
            }
        reader.Close();
        a = (sum / fc);
        Console.WriteLine("Keskiostos: " + Math.Round(a, 2) + "€");
    }

    // get the median value of spending
    public static void MedianBuy(MySqlDataReader reader)
    {
        int size = 0, mid = 0;
        double median;
        List<double> nums = new List<double>();

        while (reader.Read())
            for (int i = 0; i < reader.FieldCount; i++)
                nums.Add(reader.GetDouble(i));

        reader.Close();
        nums.Sort();
        size = nums.Count;
        mid = size / 2;
        // get middle element, or average of two middle elements
        median = (size % 2 != 0) ? nums[mid] : (nums[mid] + nums[mid - 1]) / 2;
        Console.WriteLine("Mediaaniostos: " + Math.Round(median, 2) + "€");
    }

    // get the average playtime
    public static void AveragePlaytime(MySqlDataReader reader)
    {
        var starts = new List<DateTime>();
        var ends = new List<DateTime>();
        var values = new List<TimeSpan>();
        var alltime = new TimeSpan(0, 0, 0);

        while (reader.Read())
            for (int i = 0; i < reader.FieldCount; i++)
            {
                // add to lists
                if (i % 2 == 0)
                    starts.Add(reader.GetDateTime(i));
                else
                    if (!reader.IsDBNull(i))
                        ends.Add(reader.GetDateTime(i));
            }

        reader.Close();

        // make a list of time's of all sessions
        for (int i = 0; i < ends.Count; i++)
            values.Add(ends[i].Subtract(starts[i]));

        // count totaltime
        for (int i = 0; i < values.Count; i++)
            alltime += values[i];

        Console.WriteLine("Keskipeliaika: " + alltime / values.Count);
    }

    // money transactions with specific day
    public static void DaysTransActions(MySqlDataReader reader)
    {
        Console.Write("Syötä päivä (PP.KK.VVVV): ");
        string input = Console.ReadLine();
        // change Finnish date to US
        String[] list = input.Split(".");
        String USinput = list[1] + "/" + list[0] + "/" + list[2];

        while (reader.Read())
            for (int i = 0; i < reader.FieldCount; i++)
                // compare input day with all days
                if (USinput + " 12:00:00 AM" == reader[i].ToString())
                    Console.WriteLine(reader[i - 1] + "€");
        reader.Close();
    }

    // current sessions online
    public static void CurrentSessions(MySqlDataReader reader)
    {
        while (reader.Read())
            for (int i = 0; i < reader.FieldCount; i++)
                if (reader.IsDBNull(i))
                    Console.WriteLine("Sessio id: " + reader[i - 2] + " Sessio aloitettu: " + reader[i - 1]);
        reader.Close();
    }


    // amount of transactions for the last 7 days, outputs text and optionally png chart
    public static void WeeklyTransactions(MySqlDataReader reader)
    {
        var dict = new Dictionary<DateTime, float>();

        var today = new DateTime(2021, 2, 16);
        for (int i = 0; i < 7; i++)
            dict[today.AddDays(-i)] = 0;

        while (reader.Read())
            for (int i = 0; i < reader.FieldCount; i++)
                if (i == 1 && dict.ContainsKey(reader.GetDateTime(i)))
                    dict[reader.GetDateTime(i)] += reader.GetFloat(i - 1);

        reader.Close();

        foreach (var day in dict)
            Console.WriteLine(day.Key + ": " + day.Value);

        Console.Write("Luodaanko kaavio? (y/n): ");
        string ans = Console.ReadLine();

        if (ans == "y")
        {
            var dates = new List<DateTime>(dict.Keys);
            var values = new List<float>(dict.Values);
            dates.Reverse();
            values.Reverse();

            var pt = new PlotTool("7 päivän rahasiirrot", values, dates);
            pt.ExportPng("rahasiirrot", 1);
        }

    }

    public static void CompletePercent(MySqlDataReader reader)
    {
        //Kaikkien pelisessioiden tasojen lopetus suhteessa tasojen aloitukseen
        int started = 0;
        int ended = 0;
        float percent = 0.1f;

        while (reader.Read())
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                int temp = reader.GetInt32(i);
                if (temp == 1)
                    started++;
                else
                    ended++;
            }
        }
        reader.Close();

        if (started == 0 && ended == 0)
            percent = 0;
        else
            percent = (float)ended / started * 100;

        Console.WriteLine("Kenttiä aloitettu " + started + ", kenttiä läpäisty " + started);
        Console.WriteLine("Läpäisyprosentti on: " + Math.Round(percent, 1) + "%");
    }

    // get all games and their ids
    public static void GameInfo(MySqlDataReader reader)
    {
        while (reader.Read())
            for (int i = 0; i < reader.FieldCount; i++)
                Console.Write((i % 2 == 0) ? (reader[i] + " : ") : (reader[i] + "\n"));
        reader.Close();

        SpecificGame();
    }

    // get all sessions from chosen game
    public static void SpecificGame()
    {
        MySqlConnection cnn = Connector.GetConnection();

        Console.Write("Valitse peli id:n perusteella: ");
        string id = Console.ReadLine();
        string query = String.Format(@"SELECT studio_nimi, sessio_id, pelaaja_id, etunimi, sukunimi
                        FROM Pelistudio, Pelisessio, Peli, Pelaaja WHERE Peli.peli_studio = Pelistudio.studio_id 
                        AND Peli.peli_id = Pelisessio.peli_id AND Peli.peli_id = {0}
                        AND Pelaaja.pelaaja_id = Pelisessio.pelisessio_pelaaja_id;", id);

        MySqlCommand cmd = new MySqlCommand(query, cnn);
        MySqlDataReader reader = cmd.ExecuteReader();

        string pelistudio = "";
        Console.WriteLine("Pelisessiot pelille: ");

        int counter = 0;
        while (reader.Read())
        {
            pelistudio = reader.GetString(0);

            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (i % 5 != 0)
                {
                    if ((i - 1) % 4 == 0)
                        Console.Write("Session id: ");
                    else if ((i - 2) % 4 == 0)
                        Console.Write("Pelaajan id: ");
                    else if ((i - 3) % 4 == 0)
                        Console.Write("Etunimi: ");
                    else
                        Console.Write("Sukunimi: ");
                    Console.WriteLine(reader[i]);
                    counter++;
                }
                else
                    Console.WriteLine("");
            }
        }
        reader.Close();

        Console.WriteLine("");
        Console.WriteLine("Pelistudio: " + pelistudio);
        Console.WriteLine("Sessioita yhteensä: " + counter / 4);
    }

    // player that has spent the biggest amount of money
    public static void BiggestSpender(MySqlDataReader reader)
    {
        Console.WriteLine("Eniten rahaa käyttänyt pelaaja on: ");
        while (reader.Read())
            for (int i = 0; i < reader.FieldCount; i++)
                Console.Write((i != 2) ? reader[i] + " " : "summalla: " + reader[i] + "€\n");
        reader.Close();
    }


    // sessions by game, outputs text and optionally png chart
    public static void SessionsByGame(MySqlDataReader reader)
    {
        var dict = new Dictionary<string, float>();

        while (reader.Read())
            for (int i = 0; i < reader.FieldCount; i++)
                if (i == 0)
                    dict[reader[i].ToString()] = reader.GetFloat(i + 1);
        reader.Close();

        foreach (var item in dict)
            Console.WriteLine(item.Key + ": " + item.Value);

        Console.Write("\nLuodaanko kaavio? (y/n): ");
        string ans = Console.ReadLine();

        if (ans == "y")
        {
            var strings = new List<string>(dict.Keys);
            var values = new List<float>(dict.Values);

            var pt = new PlotTool("Sessiot", strings, values);
            pt.ExportPng("Sessiot", 2);
        }
    }

}