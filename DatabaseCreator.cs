using System;
using MySql.Data.MySqlClient;

public class DatabaseCreator
{
    MySqlConnection cnn = Connector.GetConnection();

    public DatabaseCreator()
    {
        CreateaTables();
        InsertValues();
    }

    void CreateaTables()
    {
        String query = @"CREATE TABLE Pelistudio (
                        studio_id INT NOT NULL AUTO_INCREMENT,
                        studio_nimi VARCHAR(100) NOT NULL,
                        PRIMARY KEY (studio_id)
                        );";


        MySqlCommand cmd = new MySqlCommand(query, cnn);
        cmd.ExecuteNonQuery();

    }


    void InsertValues()
    {
        String query = @"INSERT INTO Pelistudio(studio_nimi) VALUES
                        ('Valve'),
                        ('DICE'),
                        ('Pirkka'),
                        ('Lidl'),
                        ('Rainbow');";
        
        MySqlCommand cmd = new MySqlCommand(query, cnn);
        cmd.ExecuteNonQuery();
    }
}