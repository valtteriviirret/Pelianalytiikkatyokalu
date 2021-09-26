﻿using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace TietokantaTesti
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Tietokannan taulut:");
            QueryTool tool = new QueryTool();
            Console.WriteLine("");

            bool loop = true;
            while(loop)
            {
                Console.WriteLine("Enter command or \"q\" to quit: ");
                string input = Console.ReadLine();
                if(input == "q")
                    loop = false;                
                
                // here assign the input
                tool.setInput(input); 
                tool.AssingQuery();
            }
        }
    }

    public class QueryTool
    {
        string connectionString = null;
        MySqlConnection cnn;
        string input;

        // initializing QueryTool in constuctor
        public QueryTool()
        {
            connectionString = "server=localhost;database=Pelianalytiikka;uid=valtteri;pwd=password;SSL Mode=0"; 
            Connect();
            getDatabases();
        }

        public void setInput(string _input)
        {
            input = _input;
        }
        
        // basic function to get table names in database
        void getDatabases()
        {
            MySqlCommand cmd = new MySqlCommand("show tables", cnn);
            MySqlDataReader reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                for(int i = 0; i < reader.FieldCount; i++)
                    Console.WriteLine(reader[i]);
            }
            reader.Close();
        }


        void Connect()
        {
            cnn = new MySqlConnection(connectionString);
            cnn.Open();
        }
        
        public void AssingQuery()
        {
            string[] tokens = input.Split(' ');
        
            switch(tokens[0].ToLower())
            {
                case "select": SelectQuery(tokens[1]); break;
                case "update": NonQuery(); break;
                case "delete": NonQuery(); break;
                case "insert": NonQuery(); break;
                default: break;
            }
        }

        public void SelectQuery(string target)
        {
            MySqlCommand cmd = new MySqlCommand(input, cnn);
            MySqlDataReader reader = cmd.ExecuteReader();
            
            List<string> columns = new List<string>();
            while(reader.Read())
            {
                for(int i = 0; i < reader.FieldCount; i++)
                    Console.Write(reader[i] + "\n");
            }
            reader.Close();
        }

        public void NonQuery()
        {
            MySqlCommand cmd = new MySqlCommand(input, cnn);
            cmd.ExecuteNonQuery();
        }
    }
} 