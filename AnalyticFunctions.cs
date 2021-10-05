using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class AnalyticFunctions
{
    
    public static void AverageBuy(MySqlDataReader reader)
    {
        double sum = 0, a = 0;
        int fc = 0;
        while(reader.Read())
            for(int i = 0; i < reader.FieldCount; i++)
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

        while(reader.Read())
            for(int i = 0; i < reader.FieldCount; i++)
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
        while(reader.Read())
            for(int i = 0; i < reader.FieldCount; i++)
            {
                // add to lists
                if(n % 2 == 0)
                    starts.Add(reader.GetDateTime(i));
                else
                    if(!reader.IsDBNull(i))
                        ends.Add(reader.GetDateTime(i));

                n++;
            }
        
        reader.Close();
        
        // make a list of time's of all sessions
        for(int i = 0; i < ends.Count; i++)
            values.Add(ends[i].Subtract(starts[i]));

        for(int i = 0; i < values.Count; i++)
            alltime += values[i];
        
        Console.WriteLine(alltime / values.Count);
    }
    
    public static void CurrentSessions(MySqlDataReader reader)
    {
        int a = 0;
        while(reader.Read())
            for(int i = 0; i < reader.FieldCount; i++)
            {
                if(a % 3 != 0)
                    if(reader.IsDBNull(i))
                        Console.WriteLine("Sessio id: " + reader[i - 2] + " Sessio aloitettu: " + reader[i - 1]);
                a++;
            }
        reader.Close();
    }

    public static void DaysTransActions(MySqlDataReader reader)
    {
        DateTime selected;
        string input;
        Console.WriteLine("Type the day(DD/MM/YYYY)");
        input = Console.ReadLine();
        selected = Convert.ToDateTime(input);
        //MySqlDataReader reader = Helper("select summa from Rahasiirto where aikaleima =" + selected);
        while(reader.Read())
        {
            for(int i = 0; i < reader.FieldCount; i++)
            {
                Console.WriteLine(reader[i]);
            }
        }
        reader.Close();
    }

    public static void CompletePercent(MySqlDataReader reader)
    {
        //Kaikkien pelisessioiden tasojen lopetus suhteessa tasojen aloitukseen
        int started = 0;
        int ended = 0;
        float percent = 0.1f;

        while(reader.Read()) {
            for(int i = 0; i < reader.FieldCount; i++) {
                int temp = reader.GetInt32(i);
                if(temp == 1) {
                    started++;
                } else {
                    ended++;
                }
            }
        }
        if (started == 0 && ended == 0) {
            percent = 0;
        } else {
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
        while(reader.Read())
            for(int i = 0; i < reader.FieldCount; i++)
            {
                // add to lists
                if(n % 2 == 0)
                    ids.Add(reader.GetInt32(i));
                else
                    names.Add(reader.GetString(i));
                n++;

            }
        reader.Close();

        for(int i = 0; i < ids.Count; i++)
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
        Console.WriteLine("Pelisessiot pelille: \n");
        int r = 0, n = 0, a = 0, b = 0;
        while(reader.Read())
        {
            pelistudio = reader.GetString(0);
            for(int i = 0; i < reader.FieldCount; i++)
            {
                if(n % 5 != 0)
                {
                    if(r % 4 != 0)
                    {
                        if(a % 3 != 0)
                        {
                            if(b % 2 != 0)
                                Console.WriteLine("Sukunimi: " + reader[i] + "\n");
                            else
                                Console.WriteLine("Etunimi: " + reader[i]);
                            b++;
                        }
                        else
                            Console.WriteLine("Pelaajan id: " + reader[i]);
                        a++;
                    }
                    else
                        Console.WriteLine("Session id: " + reader[i]);
                    r++;
                }
                n++;
            }
        }
        reader.Close();
        int sessioM = b / 2;

        Console.WriteLine("Pelistudio: " + pelistudio);
        Console.WriteLine("Sessioita yhteensä: " + sessioM);
    }
}