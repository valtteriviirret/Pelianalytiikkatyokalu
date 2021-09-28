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
    
    // NOT PROPERLY WORKING
    public static void CurrentSessions(MySqlDataReader reader)
    {
        List<int> nums = new List<int>();
        while(reader.Read())
            for(int i = 0; i < reader.FieldCount; i++)
                nums.Add(reader.GetInt32(i));
        
        reader.Close();
        for(int i = 0; i < nums.Count; i++)
            Console.WriteLine(nums[i]);
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
}