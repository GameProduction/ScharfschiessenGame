using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Examples.Scharfschiessen
{
    class DbConnection
    {
        protected void CreateDbConnection(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Server=RAMO-PC; Database=Schafschiessen; Integreted Security=SSPI;");
            con.Open();

            //SQL-Befehle erstellen
            SqlCommand cmd = new SqlCommand("Select Name from Points", con);
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                //Spaltenwerte aus der Tabelle lesen
                Console.Out.WriteLine(dr["Name"].ToString() + "<br>");
            }
            con.Close();
            con.Dispose();

        }
    }
}
