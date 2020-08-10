using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.AggregateFactory
{
    public class PrinterRepository
    {
        private string c_str;

        public PrinterRepository()
        {
            c_str = @"Data Source=./LMdatabase.db;Version=3;";

            CreateTablesIfNotExists();
        }

        public void CreateTablesIfNotExists()
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS printers (" +
                "id TEXT," +
                "name TEXT" +
                ")";
            cmd.ExecuteNonQuery();


            con.Close();

        }

        public IEnumerable<string> GetAllPrinterNames()
        {
            var names = new List<string>();

            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "SELECT * FROM printers";

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                names.Add(rdr.SafeGetString(1));
            }

            con.Close();

            return names;
        }

        public void SaveAll(List<string> names)
        {
            ClearPrintersTable();

            foreach (var name in names)
            {
                AddPrinter(name);
            }
        }

        private void ClearPrintersTable()
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "DELETE FROM printers";

            cmd.ExecuteNonQuery();

            con.Close();
        }

        public void AddPrinter(string name)
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "INSERT INTO printers VALUES (" +
                "@id," +
                "@name)";

            cmd.Parameters.AddWithValue("@id", Guid.NewGuid());
            cmd.Parameters.AddWithValue("@name", name);
            
            cmd.ExecuteNonQuery();

            con.Close();
        }


    }

}
