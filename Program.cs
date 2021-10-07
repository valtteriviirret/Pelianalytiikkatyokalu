using System;
using System.Collections.Generic;

namespace TietokantaTesti
{
    class Program
    {
        static void Main(string[] args)
        {
            string server = "", database = "", uid = "", password = "";
            Console.WriteLine("Login manually?(y/n)");
            string ans = Console.ReadLine();
            if (ans == "n")
            {
                server = "localhost";
                database = "data";
                uid = "jere"; // change me
                password = "password"; // and me
            }
            else if (ans == "y")
            {
                // auth
                Console.WriteLine("Enter server ip:");
                server = Console.ReadLine();
                Console.WriteLine("Enter database name:");
                database = Console.ReadLine();
                Console.WriteLine("Enter uid:");
                uid = Console.ReadLine();
                Console.WriteLine("Enter password:");
                password = Console.ReadLine();
            }
            else
                Console.WriteLine("Type again");


            var date1 = DateTime.Now;
            var date2 = date1.AddDays(1);
            var date3 = date1.AddDays(2);
            var date4 = date1.AddDays(3);

            List<double> data = new List<double> { 4.0, 5.0, 6.0, 7.0 };
            List<DateTime> dates = new List<DateTime> { date1, date2, date3, date4 };


            var plotTool = new PlotTool("hello", data, dates);
            plotTool.DrawPlot();
            plotTool.ExportPng("hallo");

            Console.WriteLine("All tables in this database:");
            Connector connector = new Connector(server, database, uid, password);

            while (true)
            {
                Console.WriteLine("Enter command, \"q\" to quit or \"help\" : ");
                string input = Console.ReadLine();
                if (input == "q")
                    break;

                RawSqlFunctions rsqlf = new RawSqlFunctions();
                rsqlf.AssingQuery(input);
            }
        }
    }
}