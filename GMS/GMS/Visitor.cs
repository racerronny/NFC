using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace GMS
{
    public class Visitor
    {
        //database stuff
        private const String SERVER = "127.0.0.1";
        private const String DATABASE = "guest_management_system";
        private const String UID = "root";
        private const String PASSWORD = "root";
        private static MySqlConnection dbConn;
        
        // Visitor class stuff
        public int Id { get; private set; }

        public String Name { get; private set; }

        public String Purpose { get; private set; }

        private Visitor(int id, String n, String p)
        {
            Id = id;
            Name = n;
            Purpose = p;
        }

        public static void InitializeDB()
        {
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = SERVER;
            builder.UserID = UID;
            builder.Password = PASSWORD;
            builder.Database = DATABASE;

            String connString = builder.ToString();

            builder = null;

            Console.WriteLine(connString);

            dbConn = new MySqlConnection(connString);

            Application.ApplicationExit += (sender, args) => {
                if (dbConn != null) 
                {
                    dbConn.Dispose();
                    dbConn = null;
                }
            };
        }

        public static List<Visitor> GetVisitors()
        {
            List<Visitor> visitors = new List<Visitor>();

            String query = "SELECT * FROM visitor";

            MySqlCommand cmd = new MySqlCommand(query, dbConn);

            dbConn.Open();

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = (int)reader["id"];
                String name = reader["name"].ToString();
                String purpose = reader["purpose"].ToString();

                Visitor v = new Visitor(id, name, purpose);

                visitors.Add(v);
            }

            reader.Close();

            dbConn.Close();

            return visitors;
        }

        public static Visitor Insert(String n, String p)
        {
            String query = string.Format("INSERT INTO visitor(name, purpose) VALUES ('{0}', '{1}')", n, p);

            MySqlCommand cmd = new MySqlCommand(query, dbConn);

            dbConn.Open();

            cmd.ExecuteNonQuery();
            int id = (int)cmd.LastInsertedId;

            Visitor visitor = new Visitor(id, n, p);

            dbConn.Close();

            return visitor;

        }
        public void Update(string n, string p)
        {
            String query = string.Format("UPDATE visitor SET name='{0}', purpose='{1}' WHERE ID={2}", n, p, Id);

            MySqlCommand cmd = new MySqlCommand(query, dbConn);

            dbConn.Open();

            cmd.ExecuteNonQuery();

            dbConn.Close();
        }

        public void Delete()
        {
            String query = string.Format("DELETE FROM visitor WHERE ID={0}", Id);

            MySqlCommand cmd = new MySqlCommand(query, dbConn);

            dbConn.Open();

            cmd.ExecuteNonQuery();

            dbConn.Close();
        }
    }
}
