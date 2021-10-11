using System;
using MySql.Data.MySqlClient;

public class Connector
{
    static MySqlConnection cnn;
    string connectionString, database, server, uid, password;

    // initializing connection in constuctor
    public Connector(string server, string database, string uid, string password)
    {
        // getting values from main
        this.server = server;
        this.database = database;
        this.uid = uid;
        this.password = password;

        // string used if database already exists
        connectionString = String.Format("server={0};database={1};uid={2};pwd={3};SSL Mode=0", server, database, uid, password);
        Connect();
    }

    void Connect()
    {
        // check if database exists
        bool dbexist = CheckIfExists(connectionString, database);

        if (!dbexist)
        {
            Console.WriteLine("Luodaan uusi tietokanta");

            // connecting with same values
            String connStr = String.Format("server={0};user={1};password={2};SSL Mode=0;", server, uid, password);
            cnn = new MySqlConnection(connStr);

            // drop if exists
            cnn.Open();
            Encoding();
            String drop = String.Format("DROP DATABASE IF EXISTS {0};", database);
            MySqlCommand a = new MySqlCommand(drop, cnn);
            a.ExecuteNonQuery();

            // create new
            String create = String.Format("CREATE DATABASE {0};", database);
            MySqlCommand b = new MySqlCommand(create, cnn);
            b.ExecuteNonQuery();

            // use new
            String use = String.Format("USE {0}", database);
            MySqlCommand c = new MySqlCommand(use, cnn);
            c.ExecuteNonQuery();

            // creating database
            new DatabaseCreator();
        }
        else
        {
            // already existing database
            cnn = new MySqlConnection(connectionString);
            cnn.Open();
            Encoding();
        }
    }

    // check if database exists
    static bool CheckIfExists(string connectionString, string database)
    {
        using (var connection = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand($"SELECT db_id('{database}')", cnn))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch { return false; }
            }
    }

    // getter for connection
    public static MySqlConnection GetConnection() => cnn;

    // change encoding to utf8mb4
    void Encoding()
    {
        MySqlCommand setcmd = new MySqlCommand("SET character_set_results=utf8mb4", cnn);
        setcmd.ExecuteNonQuery();
        setcmd.Dispose();
    }
}