using System;

namespace TietokantaTesti
{
    class Program
    {
        static void Main(string[] args)
        {
            string server = "", database = "", uid = "", password = "";
            Console.WriteLine("Use credentials?(y/n)");
            string ans = Console.ReadLine();
            if(ans == "n")
            {
                server = "localhost";
                database = "Pelianalytiikka";
                uid = "valtteri"; // change me
                password = "password"; // and me
            }
            else if(ans == "y")
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
            
            Console.WriteLine("All tables in this database:");
            Connector connector = new Connector(server, database, uid, password);

            while(true)
            {
                Console.WriteLine("Enter command, \"q\" to quit or \"help\" : ");
                string input = Console.ReadLine();
                if(input == "q")
                    break;
                
                RawSqlFunctions rsqlf = new RawSqlFunctions();
                rsqlf.AssingQuery(input);                
            }
        }
    }
}