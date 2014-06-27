using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
//using Examples.Scharfschiessen.Gui;

namespace Examples.Scharfschiessen
{
    public class DbConnection
    {
        private MySqlConnection _connection;
        private string _server;
        private string _database;
        private string _uid;
        private string _password;
        private Gui _gui;
        // Construktor
        public DbConnection(GameHandler gameHandler)
        {
            _gui = gameHandler.Gui;
            Initialize();
        }

        private void Initialize()
        {
            _server = "87.106.27.181";
            _database = "schafschiessenDB"; 
            _uid = "db_anwendung";
            _password = "FEKnnqERy7MHEDpr";

            string connectionString = "SERVER=" + _server + ";" + "DATABASE=" + _database + ";" + "UID=" + _uid + ";" + "PASSWORD=" + _password + ";";

            _connection = new MySqlConnection(connectionString);

        }


        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                _connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show(@"Kann keine Verbindung zum Server erstellt werden. Kontaktieren Sie bitte den Administrator");
                        break;

                    case 1045:
                        MessageBox.Show(@"Benutzername/Kennwort ungültig, bitte wiederholen!");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                _connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        // Show HighScore
        public void ShowFirstFiveHighScore()
        {
            string query = "SELECT `key`, PlayerName, HighScore FROM PlayerPoints ORDER BY HighScore DESC LIMIT 5";
            
            //open connection
            if (this.OpenConnection() == true)
            {
                string s = "";
                using (MySqlCommand cmd = new MySqlCommand(query, _connection))
                using (MySqlDataReader reader = cmd.ExecuteReader())
                while (reader.Read())
                {
                   s += string.Format("PlayerName: {0} HighScore: {1}\n", reader.GetString(1), reader.GetString(2));
                }
                DialogResult dialogResult = MessageBox.Show(s);

                //close connection
                this.CloseConnection();
            }
        }

      
        //Insert statement
        public void Insert( string name, int points)
        {
             string query = "INSERT INTO PlayerPoints (PlayerName, HighScore) Values('"+ name + "'," + points + ")";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, _connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Update statement
        public void Update()
        {
            string query = "UPDATE PlayerPoints SET PlayerName='ThePlayer', HighScore='999' WHERE PlayerName='Ramo'";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                var cmd = new MySqlCommand {Connection = _connection};

                //Assign the query using CommandText
                //cmd.CommandText = query;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Delete statement
        public void Delete()
        {
            var query = "DELETE FROM PlayerPoints WHERE PlayerName='Ramo'";

            if (this.OpenConnection() == true)
            {
                //MySqlCommand cmd = new MySqlCommand(query, _connection);
                //cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }
        //Delete all rows in a table without deleting the table. This means that the table structure, attributes, and indexes will be intact
        public void DeleteAllRows()
        {
            var query = "DELETE FROM PlayerPoints";

            if (this.OpenConnection() == true)
            {
                //MySqlCommand cmd = new MySqlCommand(query, _connection);
                //cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }


        //Select statement
        public List<string>[] Select()
        {
            const string query = "SELECT * FROM PlayerPoints";

            //Create a list to store the result
            var list = new List<string>[2];
            list[0] = new List<string>();
            list[1] = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                var cmd = new MySqlCommand(query, _connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    list[1].Add(dataReader["PlayerName"] + "");
                    list[2].Add(dataReader["HighScore"] + "");
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }
            else
            {
                return list;
            }
        }

        //Count statement
        public int Count()
        {
            const string query = "SELECT Count(*) FROM PlayerPoints";
            int count = -1;

            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                var cmd = new MySqlCommand(query, _connection);

                //ExecuteScalar will return one value
                count = int.Parse(cmd.ExecuteScalar() + "");

                //close Connection
                this.CloseConnection();

                return count;
            }
            else
            {
                return count;
            }
        }

    }
}
