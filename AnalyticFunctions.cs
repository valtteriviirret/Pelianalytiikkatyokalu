using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class AnalyticFunctions
{

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
        Console.WriteLine(Math.Round(a, 4));
    }

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
        median = (size % 2 != 0) ? nums[mid] : (nums[mid] + nums[mid - 1]) / 2;
        Console.WriteLine(Math.Round(median, 4));
    }

    public static void AveragePlaytime(MySqlDataReader reader)
    {
        List<DateTime> starts = new List<DateTime>();
        List<DateTime> ends = new List<DateTime>();
        List<TimeSpan> values = new List<TimeSpan>();
        TimeSpan alltime = new TimeSpan(0, 0, 0);

        int n = 0;
        while (reader.Read())
            for (int i = 0; i < reader.FieldCount; i++)
            {
                // add to lists
                if (n % 2 == 0)
                    starts.Add(reader.GetDateTime(i));
                else
                    if (!reader.IsDBNull(i))
                    ends.Add(reader.GetDateTime(i));

                n++;
            }

        reader.Close();

        // make a list of time's of all sessions
        for (int i = 0; i < ends.Count; i++)
            values.Add(ends[i].Subtract(starts[i]));

        for (int i = 0; i < values.Count; i++)
            alltime += values[i];

        Console.WriteLine(alltime / values.Count);
    }

    public static void CurrentSessions(MySqlDataReader reader)
    {
        while (reader.Read())
            for (int i = 0; i < reader.FieldCount; i++)
                if (reader.IsDBNull(i))
                    Console.WriteLine("Sessio id: " + reader[i - 2] + " Sessio aloitettu: " + reader[i - 1]);
        reader.Close();
    }

    // money transactions with spesific day
    public static void DaysTransActions(MySqlDataReader reader)
    {
        string input;
        Console.WriteLine("Syötä päivä (PP.KK.VVVV)");
        input = Console.ReadLine();
        String[] list = input.Split(".");
        String USinput = list[1] + "/" + list[0] + "/" + list[2];

        while (reader.Read())
            for (int i = 0; i < reader.FieldCount; i++)
                if (USinput + " 12:00:00 AM" == reader[i].ToString())
                    Console.WriteLine(reader[i - 1] + "€");
        reader.Close();
    }


    // Amount of transactions for last 7 days WIP

    public static void TransactionsCount(MySqlDataReader reader)
    {
        var amounts = new List<int>();
        var dates = new List<DateTime>();
        /* 
                while (reader.Read())
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (i == 0)
                            amounts.Add(reader.GetFloat(i));
                        else if (i == 1)
                            dates.Add(reader.GetDateTime(i));
                    }
         */

        var today = new DateTime(2021, 2, 16);
        for (int i = 0; i < 7; i++)
            dates.Add(today.AddDays(-i));

        int sum = 0;
        foreach (var day in dates)
        {
            sum = 0;
            while (reader.Read())
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (i == 1 && day.Equals(reader.GetDateTime(i)))
                    {
                        sum += 1;
                    }
                }
            amounts.Add(sum);
            Console.WriteLine(day.ToString() + ":" + sum.ToString());
        }



        reader.Close();

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
                {
                    started++;
                }
                else
                {
                    ended++;
                }
            }
        }
        if (started == 0 && ended == 0)
        {
            percent = 0;
        }
        else
        {
            percent = (float)ended / started * 100;
        }
        Console.WriteLine("Kenttiä aloitettu " + started + ", kenttiä läpäisty " + started);
        Console.WriteLine("Läpäisyprosentti on: " + Math.Round(percent, 1) + "%");
        reader.Close();
    }

    public static void GameInfo(MySqlDataReader reader)
    {
        List<int> ids = new List<int>();
        List<string> names = new List<string>();

        int n = 0;
        while (reader.Read())
            for (int i = 0; i < reader.FieldCount; i++)
            {
                // add to lists
                if (n % 2 == 0)
                    ids.Add(reader.GetInt32(i));
                else
                    names.Add(reader.GetString(i));
                n++;

            }
        reader.Close();

        for (int i = 0; i < ids.Count; i++)
            Console.WriteLine(ids[i] + " : " + names[i]);

        SpecificGame();
    }

    static void SpecificGame()
    {
        MySqlConnection cnn = Connector.GetConnection();

        Console.WriteLine("Valitse peli id:n perusteella");
        string id;
        id = Console.ReadLine();
        string query = String.Format(@"SELECT studio_nimi, sessio_id, pelaaja_id, etunimi, sukunimi
                        FROM Pelistudio, Pelisessio, Peli, Pelaaja WHERE Peli.peli_studio = Pelistudio.studio_id AND Peli.peli_id = Pelisessio.peli_id AND Peli.peli_id ={0}
                        AND Pelaaja.pelaaja_id = Pelisessio.pelisessio_pelaaja_id;", id);

        List<string> list = new List<string>();

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
}