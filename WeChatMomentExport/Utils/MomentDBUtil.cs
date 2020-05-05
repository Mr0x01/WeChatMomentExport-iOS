using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatMomentExport.Utils
{
    public class MomentDBUtil
    {
        public static void LoadMomentSQLite(string profile_hash = "")
        {
            string dbname = "wc005_008.db";
            var db = new SQLiteUtil(dbname);
            var tables = db.SelectTablesName();
            HashSet<string> ids = new HashSet<string>();
            foreach (var table in tables)
            {
                if (table.type == "table" && table.tbl_name.Equals("MyWC01_" + profile_hash))
                {
                    LogUtil.Log($"找到{profile_hash}，开始导出plist...");
                    var datas = db.SelectList($"select Buffer,Id from {table.tbl_name}");
                    for (int i = 0; i < datas.Count; i++)
                    {
                        var localid = datas[i][1].ToString();
                        if (ids.Contains(localid))
                        {
                            continue;
                        }
                        else
                        {
                            ids.Add(localid);
                            var bolb_data = (byte[])datas[i][0];
                            var path = $"Plist\\{localid}.plist";
                            File.WriteAllBytes(path, bolb_data);
                        }
                    }
                    LogUtil.Log($"共找到朋友圈数据{datas.Count}条");
                    return;
                }
            }
            LogUtil.Log($"未找到{profile_hash}，再次确认输入，或重建朋友圈缓存。");
        }
    }
}
