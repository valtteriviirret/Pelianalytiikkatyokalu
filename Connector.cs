using System;
using MySql.Data.MySqlClient;

public class Connector
{
    string connectionString = null;
    static MySqlConnection cnn;
    string database;

    // initializing connection in constuctor
    public Connector(string server, string _database, string uid, string password)
    {
        database = _database;
        connectionString = "server=" + server + ";database=" + database + ";uid=" + uid + ";pwd=" + password  + ";SSL Mode=0"; 
        Connect();
        getDatabases();
    }

    void Connect()
    {
        bool dbexist = CheckDatabaseExists(cnn, database);

        if(dbexist)
        {
            MySqlConnection newcon = new MySqlConnection("Server=localhost;Integrated security=SSPI;database=master");
            String str = "CREATE DATABASE " + database  + ";";
            MySqlCommand cmd = new MySqlCommand(str, newcon);
            try
            {
                newcon.Open();
                cmd.ExecuteNonQuery();
            }

            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        
        else
        {
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
            query = string.Format("SELECT database_id FROM sys.databases WHERE Name = '{0}'", database);
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