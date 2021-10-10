using System;

namespace TietokantaTesti
{
    class Program
    {
        static void Main(string[] args)
        {
            string server = "", database = "", uid = "", password = "";
            Console.WriteLine("Game analytics tool");

            bool authInfo = false;
            while (!authInfo)
            {
                Console.Write("Login manually? (y/n): ");
                string ans = Console.ReadLine();
                switch (ans)
                {
                    case "n":
                        server = "localhost";
                        database = "Pelianalytiik";
                        uid = "jere"; // change me
                        password = "password";
                        authInfo = true;
                        break;

                    case "y":
                        Console.Write("Enter server ip: ");
                        server = Console.ReadLine();
                        Console.Write("Enter database name: ");
                        database = Console.ReadLine();
                        Console.Write("Enter uid: ");
                        uid = Console.ReadLine();
                        Console.Write("Enter password: ");
                        password = Console.ReadLine();
                        authInfo = true;
                        break;
                    default:
                        break;
                }
            }

            Connector connector = new Connector(server, database, uid, password);

            while (true)
            {
                Console.Write("Enter command, \"q\" to quit or \"help\": ");
                string input = Console.ReadLine();
                if (input == "q")
                    break;

                RawSqlFunctions rsqlf = new RawSqlFunctions();
                rsqlf.AssingQuery(input);
            }
        }
    }
}