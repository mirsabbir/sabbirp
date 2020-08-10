using Live_Menu_Point_Of_Sale.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.AggregateFactory
{
    public class TablesRepository
    {
        private string c_str;

        public TablesRepository()
        {
            c_str = @"Data Source=./LMdatabase.db;Version=3;";

            CreateTablesIfNotExists();
        }

        public void CreateTablesIfNotExists()
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "CREATE TABLE IF NOT EXISTS tables (" +
                "id TEXT," +
                "serving INTEGER," +
                "seats INTEGER," +
                "cart_id TEXT" +
                ")";
            cmd.ExecuteNonQuery();

            

            con.Close();
        }

        public IEnumerable<Table> GetTables()
        {
            var tableList = new List<Table>();

            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "SELECT * FROM tables";

            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                var t = new Table
                {
                    Id = rdr.GetGuid(0),
                    Serving = rdr.GetInt32(1),
                    Seats = rdr.GetInt32(2),
                    CartId = rdr.GetGuid(3),
                };

                tableList.Add(t);
            }

            con.Close();

            return tableList;
        }

        public void SaveTables(List<Table> tables)
        {
            ClearAll();
            foreach (var table in tables)
            {
                AddTable(table);
            }
        }


        public void AddTable(Table table)
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "INSERT INTO tables VALUES(@id, @serving, @seats, @CartId)";
            cmd.Parameters.Add(new SQLiteParameter("@id", table.Id));
            cmd.Parameters.Add(new SQLiteParameter("@serving", table.Serving));
            cmd.Parameters.Add(new SQLiteParameter("@seats", table.Seats));
            cmd.Parameters.Add(new SQLiteParameter("@CartId", table.CartId));

            cmd.ExecuteNonQuery();

            con.Close();
        }

        public void ClearAll()
        {
            using var con = new SQLiteConnection(c_str);
            con.Open();

            using var cmd = new SQLiteCommand(con);

            cmd.CommandText = "DELETE FROM tables";
            
            cmd.ExecuteNonQuery();

            con.Close();
        }

        

    }
}
