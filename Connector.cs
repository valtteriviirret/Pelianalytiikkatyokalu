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
        bool dbexist = CheckDatabaseExists(cnn, database);
        if(!dbexist)
        {
            // connecting with same values
            String connStr = String.Format("server={0};user={1};password={2};SSL Mode=0;", server, uid, password);
            cnn = new MySqlConnection(connStr);
            var cmd = cnn.CreateCommand();
            
            // creating new database
            cnn.Open();
            cmd.CommandText = String.Format("CREATE DATABASE IF NOT EXISTS {0};", database);
            cmd.ExecuteNonQuery();

            // use the new database
            var select = cnn.CreateCommand();
            select.CommandText = String.Format("USE {0}", database);
            select.ExecuteNonQuery();

            // here create a instance for new database
        }
        
        else
        {
            // already existing database
            cnn = new MySqlConnection(connectionString);
            cnn.Open();
        }     
    
        // changed encoding
        MySqlCommand setcmd = new MySqlCommand("SET character_set_results=utf8mb4", cnn);
        int n = setcmd.ExecuteNonQuery();
        setcmd.Dispose();
    } 

    // check if database exists
    public static bool CheckDatabaseExists(MySqlConnection cnn, string database)
    {
        string query;
        bool result = false;

        try
        {
            query = string.Format("SELECT database_id FROM sys.databases WHERE Name = {0}", database);
            using (MySqlCommand cmd = new MySqlCommand(query, cnn))
            {
                cnn.Open();
                object resultObj = cmd.ExecuteScalar();
                int databaseID = 0;
                if (resultObj != null)
                {
                    int.TryParse(resultObj.ToString(), out databaseID);
                }
                cnn.Close();
                result = (databaseID > 0);
            }
        }
        catch (Exception)
        {
            result = false;
        }
        return result;
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