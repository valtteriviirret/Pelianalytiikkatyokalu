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
                        database = "test";
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
                        // hide password
                        while(true)
                        {
                            var key = System.Console.ReadKey(true);
                            if(key.Key == ConsoleKey.Enter)
                                break;
                            password += key.KeyChar;
                        }
                        authInfo = true;
                        break;
                    default:
                        break;
                }
            }

            // connecting to database
            new Connector(server, database, uid, password);

            while (true)
            {
                Console.Write("\nSyötä komento, \"q\" poistumiseen tai \"help\": ");
                string input = Console.ReadLine();
                if (input == "q")
                    break;

                var rsqlf = new RawSqlFunctions();
                rsqlf.AssingQuery(input);
            }
        }
    }
}