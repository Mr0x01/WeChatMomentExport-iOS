using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatMomentExport.Model;

namespace WeChatMomentExport.Utils
{
    public class SQLiteUtil
    {
        private SQLiteConnection db;
        public SQLiteUtil(string db_name)
        {
            if (File.Exists(db_name))
            {
                db = new SQLiteConnection($"Data Source={db_name};Version=3;");
                db.Open();
            }
        }

        public SQLiteUtil(string db_name, int db_version)
        {
            if (File.Exists(db_name))
            {
                db = new SQLiteConnection($"Data Source={db_name};Version={db_version};");
                db.Open();
            }
        }

        public List<TableInfo> SelectTablesName()
        {
            string sql = "select * from sqlite_master";
            List<TableInfo> table = new List<TableInfo>();
            var list = SelectList(sql);
            foreach (var row in list)
            {
                TableInfo info = new TableInfo();
                info.type = row[0].ToString();
                info.name = row[1].ToString();
                info.tbl_name = row[2].ToString();
                info.rootpage = int.Parse(row[3].ToString());
                info.sql = row[4].ToString();
                table.Add(info);
            }
            return table;
        }

        public List<List<Object>> SelectList(string sql)
        {
            SQLiteCommand command = new SQLiteCommand(sql, db);
            SQLiteDataReader reader = command.ExecuteReader();
            List<List<Object>> list = new List<List<object>>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    List<Object> temp = new List<object>();
                    for (int i = 0; i < reader.VisibleFieldCount; i++)
                    {
                        temp.Add(reader[i]);
                    }
                    list.Add(temp);
                }
            }
            return list;
        }


    }
}
