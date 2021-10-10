using System;

namespace TietokantaTesti
{
    class Program
    {
        static void Main(string[] args)
        {
            string server = "", database = "", uid = "", password = "";
            Console.WriteLine("Pelianalytiikkatyökalu");

            bool authInfo = false;
            while (!authInfo)
            {
                Console.Write("Kirjaudu manuaalisesti (y/n): ");
                string ans = Console.ReadLine();
                switch (ans)
                {
                    case "n":
                        server = "localhost";
                        database = "testi7";
                        uid = "valtteri"; // change me
                        password = "password";
                        authInfo = true;
                        break;

                    case "y":
                        Console.Write("Syötä serverin ip: ");
                        server = Console.ReadLine();
                        Console.Write("Syötä tietokannan nimi: ");
                        database = Console.ReadLine();
                        Console.Write("Syötä käyttäjänimi: ");
                        uid = Console.ReadLine();
                        Console.Write("Syötä salasana: ");
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