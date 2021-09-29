using System;
using MySql.Data.MySqlClient;

public class Connector
{
    string connectionString = null;
    static MySqlConnection cnn;

    // initializing connection in constuctor
    public Connector(string server, string database, string uid, string password)
    {
        connectionString = "server=" + server + ";database=" + database + ";uid=" + uid + ";pwd=" + password  + ";SSL Mode=0"; 
        Connect();
        getDatabases();
    }

    void Connect()
    {
        cnn = new MySqlConnection(connectionString);
        cnn.Open();

        MySqlCommand setcmd = new MySqlCommand("SET character_set_results=utf8", cnn);
        int n = setcmd.ExecuteNonQuery();
        setcmd.Dispose();
    } 

    public static MySqlConnection GetConnection() { return cnn; }
    
    // get table names in selected database
    void getDatabases()
    {
        MySqlCommand cmd = new MySqlCommand("show tables", cnn);
        MySqlDataReader reader = cmd.ExecuteReader();

        while(reader.Read())
        {
            for(int i = 0; i < reader.FieldCount; i++)
                Console.WriteLine(reader[i]);
        }
        Console.WriteLine("");
        reader.Close();
    }
}