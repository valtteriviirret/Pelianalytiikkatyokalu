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
        if (ans == "y")
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
                        ('Bandai Namco'),
                        ('Nintendo'),
                        ('Rockstar Games'),
                        ('Ubisoft'),
                        ('Capcom'),
                        ('Activision Blizzard');
                        
                        INSERT INTO Pelaaja(etunimi, sukunimi, email) VALUES
                        ('Jere', 'Vuola', 'jere.vuola@metropolia.fi'),
                        ('Santeri', 'Hytönen', 'santeri.hytonen@metropolia.fi'),
                        ('Valtteri', 'Viirret', 'valtteri.viirret@metropolia.fi'),
                        ('Erkki', 'Esimerkki', 'erkki.esimerkki@maansiirtovirma.fi'),
                        ('Mikko', 'Mallikas', 'mikko.mallikas@hotmail.com'),
                        ('Ismo', 'Taalasmaa', 'ismo.taalasmaa@salattu.fi'),
                        ('Pentti', 'Pouta', 'pentti.pouta@pilvinen.fi');

                        INSERT INTO Peli(peli_nimi, peli_studio) VALUES
                        ('Counter Strike: Global Offensive', 1),
                        ('Battlefield V', 2),
                        ('Pac-Man', 3),
                        ('Mario Kart 8 Deluxe', 4),
                        ('Grand Theft Auto 5', 5),
                        ('Rainbow Six: Siege', 6),
                        ('Resident Evil Village', 7),
                        ('World of Warcraft', 8);

                        INSERT INTO Pelitapahtuma_tyyppi(tyyppi_nimi) VALUES
                        ('Taso aloitettu'),
                        ('Taso läpäisty'),
                        ('Kuolema'),
                        ('Ase päivitetty'),
                        ('Pelihahmon tason nousu');

                        INSERT INTO Pelisessio(alkuaika, loppuaika, peli_id, pelisessio_pelaaja_id) VALUES
                        ('2021-10-08 16:20:45', '2021-10-08 19:02:45', 1, 2),
                        ('2021-10-08 18:10:15', '2021-10-08 23:20:05', 2, 3),
                        ('2021-10-09 13:00:12', '2021-10-09 14:05:12', 3, 4),
                        ('2021-10-10 15:33:22', '2021-10-10 18:33:22', 4, 5),
                        ('2021-10-10 19:22:32', '2021-10-10 23:22:12', 5, 6),
                        ('2021-10-10 20:20:45', '2021-10-10 20:59:45', 6, 7),
                        ('2021-10-10 23:10:15', '2021-10-11 01:20:05', 7, 4),
                        ('2021-10-11 11:00:12', '2021-10-11 14:02:12', 8, 1),
                        ('2021-10-11 18:33:22', '2021-10-11 04:33:22', 1, 2),
                        ('2021-10-12 01:55:12', null, 1, 1);

                        INSERT INTO Pelitapahtuma(aikaleima, tapahtuma_tyyppi_id, sessio_id) VALUES
                        ('2021-10-08 16:25:45', 1, 1),
                        ('2021-10-08 17:10:15', 3, 1),
                        ('2021-10-08 17:30:05', 3, 1),
                        ('2021-10-08 17:40:09', 3, 1),
                        ('2021-10-08 19:00:12', 2, 1),

                        ('2021-10-08 18:20:22', 1, 2),
                        ('2021-10-08 20:10:15', 3, 2),
                        ('2021-10-08 23:15:32', 2, 2),

                        ('2021-10-09 13:20:45', 1, 3),
                        ('2021-10-09 13:30:15', 4, 3),
                        ('2021-10-09 13:35:15', 3, 3),
                        ('2021-10-09 13:50:12', 5, 3),
                        ('2021-10-09 14:00:22', 3, 3),

                        ('2021-10-10 15:35:45', 1, 4),
                        ('2021-10-10 18:30:22', 2, 4);


                        INSERT INTO Rahasiirto(summa, aikaleima, sessio) VALUES
                        (4.99, '2021-10-08 17:15:15', 1),
                        (9.99, '2021-10-08 17:35:05', 1),
                        (4.99, '2021-10-08 17:42:09', 1),

                        (49.99, '2021-10-08 20:10:15', 2),
                        (19.99, '2021-10-08 20:11:35', 2),
                        (69.69, '2021-10-08 20:19:05', 2),

                        (9.99, '2021-10-09 13:30:54', 3),

                        (24.99, '2021-10-10 15:50:45', 4),
                        (10.99, '2021-10-10 16:25:15', 4);


                        INSERT INTO Pelaa(nimimerkki, pelaaja_id, peli_id) VALUES
                        ('xxxGamerxxx', 2, 1),
                        ('NormalDude', 3, 2),
                        ('Grunder', 4, 3),
                        ('VelatonMies', 1, 8),
                        ('Rattler', 5, 4),
                        ('DeathMachine', 6, 5),
                        ('BaldGuy', 7, 7);
                        ";

        MySqlCommand cmd = new MySqlCommand(query, cnn);
        cmd.ExecuteNonQuery();
    }
}