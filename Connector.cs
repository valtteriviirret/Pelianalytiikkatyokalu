using System;
using MySql.Data.MySqlClient;

public class Connector
{
    string connectionString = null;
    static MySqlConnection cnn;
    string database;
    string server;
    string uid;
    string password;

    // initializing connection in constuctor
    public Connector(string _server, string _database, string _uid, string _password)
    {
        // getting values from main
        server = _server;
        database = _database;
        uid = _uid;
        password = _password;

        // string used if database already exists
        connectionString = String.Format("server={0};database={1};uid={2};pwd={3};SSL Mode=0", server, database, uid, password); 
        Connect();
        getDatabases();
    }

    void Connect()
    {
        // check if database exists
        bool dbexist = CheckIfExists(connectionString, database);
        
        if(!dbexist)
        {
            Console.WriteLine("Creating new database");

            // connecting with same values
            String connStr = String.Format("server={0};user={1};password={2};SSL Mode=0;", server, uid, password);
            cnn = new MySqlConnection(connStr);
            
            // drop if exists
            cnn.Open();
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

            // here create a instance for new database
            DatabaseCreator creator = new DatabaseCreator();
        }
        else
        {
            // already existing database
            cnn = new MySqlConnection(connectionString);
            cnn.Open();
        }
    
        Console.WriteLine("testistkhgsjk");

        // changed encoding
        MySqlCommand setcmd = new MySqlCommand("SET character_set_results=utf8mb4", cnn);
        int n = setcmd.ExecuteNonQuery();
        setcmd.Dispose();
    } 

    // check if database exists
    static bool CheckIfExists(string connectionString, string database)
    {
        using(var connection = new MySqlConnection(connectionString))
        {
            using(var cmd = new MySqlCommand($"SELECT db_id('{database}')", connection))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    } 
    
    // getter for connection
    public static MySqlConnection GetConnection() => cnn; 
    
    // get table names in selected database
    void getDatabases()
    {
        MySqlCommand cmd = new MySqlCommand("show tables", cnn);
        MySqlDataReader reader = cmd.ExecuteReader();

        while(reader.Read())
            for(int i = 0; i < reader.FieldCount; i++)
                Console.WriteLine(reader[i]);
        
        Console.WriteLine("");
        reader.Close();
    }
}