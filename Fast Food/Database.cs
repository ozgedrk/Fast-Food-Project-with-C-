using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Windows.Forms;
using System.Data;

namespace Fast_Food
{
    class Database
    {
       public static NpgsqlConnection connection = new NpgsqlConnection("server=localhost; port=5432; database=fast_food; user ID=postgres; password=1234");

       public static NpgsqlDataAdapter dbAdapter(string sorgu)
       {
           NpgsqlDataAdapter adapter = null;
           try
           {
               if (Database.dbConnection().State == ConnectionState.Closed)
                   Database.dbConnection().Open();
               adapter = new NpgsqlDataAdapter(sorgu, connection);
               if (Database.dbConnection().State == ConnectionState.Open)
                   Database.dbConnection().Close();
           }
           catch(Exception e){
               MessageBox.Show("Veritabanı hatası: " + e.ToString());
           }
           return adapter;
       }

       public static NpgsqlCommand dbCommand(string sorgu){
           NpgsqlCommand command = null;
           try
           {
               if (Database.dbConnection().State == ConnectionState.Closed)
                   Database.dbConnection().Open();
               command = new NpgsqlCommand(sorgu, connection);
           }
           catch (NpgsqlException e)
           {
               MessageBox.Show("Veritabanı hatası: " + e.ToString());
           }
           return command;
       }

       public static NpgsqlConnection dbConnection()
       {
           return connection;
       }

    }
}
