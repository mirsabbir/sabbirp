using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale
{
    public static class SqliteExtentions
    {
        public static string SafeGetString(this SQLiteDataReader rdr, int idx)
        {
            return rdr.IsDBNull(idx) ? null : rdr.GetString(idx);
        }
    }
}
