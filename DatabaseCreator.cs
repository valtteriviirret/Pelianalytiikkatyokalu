using System;
using MySql.Data.MySqlClient;

public class DatabaseCreator
{
    MySqlConnection cnn = Connector.GetConnection();

    // make the database in constructor
    public DatabaseCreator()
    {
        CreateaTables();
        Console.Write("Luodaanko esimerkkidataa? (y/n): ");
        string ans = Console.ReadLine();
        if(ans == "y")
            InsertValues();
    }

    // creating all tables
    void CreateaTables()
    {
        String query = @"CREATE TABLE Pelistudio (
                        studio_id INT NOT NULL AUTO_INCREMENT,
                        studio_nimi VARCHAR(100) NOT NULL,
                        PRIMARY KEY (studio_id)
                        );
                        
                        CREATE TABLE Pelaaja (
                        pelaaja_id INT NOT NULL AUTO_INCREMENT,
                        etunimi VARCHAR(100) NOT NULL,
                        sukunimi VARCHAR(100),
                        email VARCHAR(100),
                        PRIMARY KEY (pelaaja_id)
                        );
                        
                        CREATE TABLE Pelitapahtuma_tyyppi (
                            tyyppi_id INT NOT NULL AUTO_INCREMENT,
                            tyyppi_nimi VARCHAR(100) NOT NULL,
                            PRIMARY KEY (tyyppi_id)
                        );

                        CREATE TABLE Peli (
                            peli_id INT NOT NULL AUTO_INCREMENT,
                            peli_nimi VARCHAR(100) NOT NULL,
                            peli_studio INT NOT NULL,
                            PRIMARY KEY (peli_id),
                            FOREIGN KEY (peli_studio) REFERENCES Pelistudio(studio_id)
                        );

                        CREATE TABLE Pelisessio (
                            sessio_id INT NOT NULL AUTO_INCREMENT,
                            alkuaika DATETIME NOT NULL,
                            loppuaika DATETIME, 
                            peli_id INT NOT NULL,
                            pelisessio_pelaaja_id INT NOT NULL,
                            FOREIGN KEY(pelisessio_pelaaja_id) REFERENCES Pelaaja(pelaaja_id),
                            FOREIGN KEY(peli_id) REFERENCES Peli(peli_id),
                            PRIMARY KEY(sessio_ID)
                        );

                        CREATE TABLE Pelitapahtuma (
                            tapahtuma_id INT NOT NULL AUTO_INCREMENT,
                            aikaleima DATETIME NOT NULL,
                            tapahtuma_tyyppi_id INT NOT NULL,
                            sessio_id INT NOT NULL,
                            PRIMARY KEY (tapahtuma_id),
                            FOREIGN KEY (tapahtuma_tyyppi_id) REFERENCES Pelitapahtuma_tyyppi(tyyppi_id),
                            FOREIGN KEY (sessio_id) REFERENCES Pelisessio(sessio_id)
                        );

                        CREATE TABLE Rahasiirto (
                            siirto_id INT NOT NULL AUTO_INCREMENT,
                            summa FLOAT(100, 2) NOT NULL,
                            aikaleima DATETIME NOT NULL,
                            sessio INT NOT NULL,
                            PRIMARY KEY (siirto_id),
                            FOREIGN KEY (sessio) REFERENCES Pelisessio(sessio_id)
                        );


                        CREATE TABLE Pelaa (
                            pelaa_id INT NOT NULL AUTO_INCREMENT,
                            nimimerkki VARCHAR(100),
                            pelaaja_id INT NOT NULL,
                            peli_id INT NOT NULL,
                            PRIMARY KEY (pelaa_id),
                            FOREIGN KEY (pelaaja_id) REFERENCES Pelaaja(pelaaja_id),
                            FOREIGN KEY (peli_id) REFERENCES Peli(peli_id)
                        )";

        MySqlCommand cmd = new MySqlCommand(query, cnn);
        cmd.ExecuteNonQuery();

    }

    // inserting values to tables
    void InsertValues()
    {
        String query = @"INSERT INTO Pelistudio(studio_nimi) VALUES
                        ('Valve'),
                        ('DICE'),
                        ('Pirkka'),
                        ('Lidl'),
                        ('Rainbow');
                        
                        INSERT INTO Pelaaja(etunimi, sukunimi, email) VALUES
                        ('Jere', 'Vuola', 'jere.vuola@metropolia.fi'),
                        ('Santeri', 'Hytönen', 'santeri.hytonen@metropolia.fi'),
                        ('Valtteri', 'Viirret', 'valtteri.viirret@metropolia.fi'),
                        ('Erkki', 'Esimerkki', 'erkki.esimerkki@maansiirtovirma.fi'),
                        ('Mikko', 'Mallikas', 'mikko.mallikas@hotmail.com');

                        INSERT INTO Pelistudio(studio_nimi) VALUES
                        ('Valve'),
                        ('DICE'),
                        ('Pirkka'),
                        ('Lidl'),
                        ('Rainbow');

                        INSERT INTO Peli(peli_nimi, peli_studio) VALUES
                        ('Modern battle tactics 2: Vuola\'s revenge', 3),
                        ('Bloons tower defense 6', 5),
                        ('Counter Strike: Global Offensive', 1),
                        ('Rainbow Six: Siege', 5),
                        ('Cookie Clicker', 5);

                        INSERT INTO Pelitapahtuma_tyyppi(tyyppi_nimi) VALUES
                        ('Taso aloitettu'),
                        ('Taso läpäisty'),
                        ('Kuolema'),
                        ('Ase päivitetty'),
                        ('Pelihahmon tason nousu');

                        INSERT INTO Pelisessio(alkuaika, loppuaika, peli_id, pelisessio_pelaaja_id) VALUES
                        ('2021-02-10 20:20:45', '2021-02-10 20:22:45', 1, 1),
                        ('2021-02-10 20:10:15', '2021-02-10 21:20:05', 1, 1),
                        ('2021-02-10 21:00:12', '2021-02-10 21:02:12', 5, 5),
                        ('2021-04-23 23:33:22', '2021-04-24 04:33:22', 4, 2),
                        ('2021-05-20 10:22:32', '2021-05-20 12:22:12', 1, 1),
                        ('2021-05-21 23:55:12', null, 1, 1);

                        INSERT INTO Pelitapahtuma(aikaleima, tapahtuma_tyyppi_id, sessio_id) VALUES
                        ('2021-02-10 20:10:15', 1, 1),
                        ('2021-02-10 20:20:45', 3, 1),
                        ('2021-02-10 21:01:12', 2, 1),
                        ('2021-02-9 15:15:20', 5, 2),
                        ('2021-02-9 17:34:01', 4, 2);

                        INSERT INTO Rahasiirto(summa, aikaleima, sessio) VALUES
                        (49.99, '2021-02-10 20:32:25', 1),
                        (9.99, '2021-02-13 11:36:15', 2),
                        (4.99, '2021-02-15 17:33:40', 3),
                        (299.99, '2021-02-16 23:32:05', 4),
                        (99.99, '2021-02-16 14:36:59', 4),
                        (69.69, '2021-10-9 15:15:15', 4),
                        (70.69, '2021-10-8 15:15:15', 4),
                        (72.69, '2021-10-7 15:15:15', 4),
                        (75.69, '2021-10-6 15:15:15', 4);


                        INSERT INTO Pelaa(nimimerkki, pelaaja_id, peli_id) VALUES
                        ('xxxGamerxxx', 1, 1),
                        ('NormalDude', 3, 5),
                        ('Grunder', 1, 3),
                        ('VelatonMies', 1, 2),
                        ('HäkkerMän', 3, 2);
                        ";
            
        MySqlCommand cmd = new MySqlCommand(query, cnn);
        cmd.ExecuteNonQuery();
    }
}