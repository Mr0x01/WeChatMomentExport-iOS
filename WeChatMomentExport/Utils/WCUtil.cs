using Claunia.PropertyList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using WeChatMomentExport.Model;

namespace WeChatMomentExport.Utils
{
    public class WCUtil
    {
        public Dictionary<string, FriendInfo> friends_info = new Dictionary<string, FriendInfo>();
        public Dictionary<string, MonmentBasicInfo> moments_info = new Dictionary<string, MonmentBasicInfo>();

        public void LoadMomentSQLite(string profile_hash = "")
        {
            string dbname = "wc005_008.db";
            var db = new SQLiteUtil(dbname);
            var tables = db.SelectTablesName();
            HashSet<string> ids = new HashSet<string>();
            foreach (var table in tables)
            {
                if (table.type == "table" && table.tbl_name.Equals("MyWC01_" + profile_hash))
                {
                    Console.WriteLine($"找到{profile_hash}，开始导出...");
                    var datas = db.SelectList($"select Buffer,Id from {table.tbl_name}");
                    if (!Directory.Exists("Export"))
                    {
                        Directory.CreateDirectory("Export");
                    }
                    foreach (var row in datas)
                    {
                        var localid = row[1].ToString();
                        if (ids.Contains(localid))
                        {
                            continue;
                        }
                        else
                        {
                            ids.Add(localid);
                            var bolb_data = (byte[])row[0];
                            var path = $"Export\\{localid}.plist";
                            File.WriteAllBytes(path, bolb_data);
                            string xml = FormatPlist(path);
                            MonmentBasicInfo basic_info = GetBasicInfo(xml);
                            moments_info.Add(localid, basic_info);
                            Console.WriteLine(basic_info.content);
                        }
                    }
                    return;
                }
            }
            Console.WriteLine($"未找到{profile_hash}，再次确认输入，或重建朋友圈缓存。");
        }

        /// <summary>
        /// 导出简单的分析文本
        /// </summary>
        /// <param name="top_friends">点赞最多的前N个好友，默认10个</param>
        /// <param name="top_moments">被赞最多的前N个朋友圈，默认5条</param>
        public void ExportAnalysis(int top_friends = 10, int top_moments = 5)
        {
            var top10_friends = friends_info.OrderByDescending(a => a.Value.like_count).Take(top_friends).ToList();
            var top10_moments = moments_info.OrderByDescending(a => a.Value.like_count).Take(top_moments)/*.OrderByDescending(a=>a.Value.create_time)*/.ToList();
            string top = "";
            int total_like = 0, moments_amount = 0;
            foreach (var moment in moments_info.OrderByDescending(a => a.Value.create_time))
            {
                top += $"内容：{moment.Value.content}\r\n日期：{moment.Value.create_time}\r\n点赞：{moment.Value.like_count}\r\n\r\n";
                total_like += moment.Value.like_count;
                moments_amount++;
            }
            File.WriteAllText("AllMoments.txt", top);

            top = "";
            top += $"共发朋友圈：{moments_amount}\r\n共收到赞：{total_like}\r\n";
            top += "点赞Top10\r\n";
            foreach (var friend in top10_friends)
            {
                top += $"WXID：{""}\t\t\t\t昵称：{friend.Value.nick_name[0]}\t\t\t\t点赞：{friend.Value.like_count}\r\n";
            }
            top += "被赞Top5\r\n";
            foreach (var moment in top10_moments)
            {
                top += $"内容：{moment.Value.content}\r\n日期：{moment.Value.create_time}\r\n点赞：{moment.Value.like_count}\r\n\r\n";
            }
            File.WriteAllText("Report.txt", top);
        }
        /// <summary>
        /// 格式化plist为XML
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string FormatPlist(string path)
        {
            var old_one = new FileInfo(path);
            var plist = PropertyListParser.Parse(old_one);
            string xml = plist.ToXmlPropertyList();
            File.WriteAllText(old_one.DirectoryName + "\\" + old_one.Name + ".xml", xml);
            return xml;
        }
        /// <summary>
        /// 解析xml的内容，取出朋友圈的基础信息
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private MonmentBasicInfo GetBasicInfo(string xml)
        {
            MonmentBasicInfo info = new MonmentBasicInfo();
            XmlDocument xmlDocument = new XmlDocument();

            try
            {
                xmlDocument.LoadXml(xml);
            }
            catch (Exception)
            {
                xmlDocument.LoadXml(ReplaceHexadecimalSymbols(xml));
            }

            var node_list = xmlDocument.SelectSingleNode("/plist/dict/array/dict[1]").ChildNodes;//取点赞数和发布时间
            var node = xmlDocument.SelectSingleNode("/plist");
            for (int i = 0; i < node_list.Count; i += 2)
            {
                var temp_key = node_list[i];
                var temp_val = node_list[i + 1];
                var key = temp_key.InnerText;
                var val = temp_val.InnerText;
                if (key == "createtime")
                {
                    DateTime converted = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    DateTime newDateTime = converted.AddSeconds(long.Parse(val));
                    info.create_time = newDateTime.ToLocalTime();
                    continue;
                }
                else if (key == "realLikeCount")
                {
                    info.like_count = int.Parse(val);
                    break;
                }
            }

            node = xmlDocument.SelectSingleNode("/plist/dict/array/string[last()]");//取自己发表的内容
            if (node.InnerText.Contains("foursquare") || node.InnerText.Contains("qqmap_") || node.InnerText.Contains("dianping"))//处理有坐标的情况
            {
                node = xmlDocument.SelectSingleNode("/plist/dict/array/string[last()-4]");
                info.content = node.InnerText.Trim();
            }
            else if (node.InnerText.EndsWith("="))
            {
                node = xmlDocument.SelectSingleNode("/plist/dict/array/string[last()-2]");
                info.content = node.InnerText;
                if (node.InnerText.Contains("http://") || node.InnerText.Contains("https://"))
                {
                    info.content = "";
                }
            }
            else if (node.InnerText.StartsWith("wx"))
            {
                node = xmlDocument.SelectSingleNode("/plist/dict/array/string[last()-1]");
                info.content = node.InnerText.Trim();
            }
            else
            {
                info.content = node.InnerText.Trim();
            }
            node_list = xmlDocument.SelectSingleNode("/plist/dict/array/dict[2]/array").ChildNodes; //取点赞人的索引
            int[] liked_index = new int[node_list.Count];
            for (int i = 0; i < node_list.Count; i++)
            {
                liked_index[i] = Convert.ToInt32(node_list[i].InnerText, 16) + 1;
            }
            node_list = xmlDocument.SelectNodes("/plist/dict/array/string");
            bool empty = false;
            if (info.like_count != 0)
            {
                for (int i = 4; i < info.like_count * 2 + 5; i++)
                {
                    var val = node_list[i].InnerText;
                    if (val == "")
                    {
                        empty = true; //处理空行
                        continue;
                    }
                    if (empty == false)
                    {
                        if (i % 2 == 0)
                        {
                            info.liked_friends.Add(val, node_list[i + 1].InnerText);
                            if (friends_info.ContainsKey(val))
                            {
                                friends_info[val].like_count++;
                            }
                            else
                            {
                                friends_info.Add(val, new FriendInfo(node_list[i + 1].InnerText));
                            }
                        }
                    }
                    else
                    {
                        if (i % 2 != 0)
                        {
                            info.liked_friends.Add(val, node_list[i + 1].InnerText);
                            if (friends_info.ContainsKey(val))
                            {
                                friends_info[val].like_count++;
                            }
                            else
                            {
                                friends_info.Add(val, new FriendInfo(node_list[i + 1].InnerText));
                            }
                        }
                    }
                }
            }

            return info;
        }

        /// <summary>
        /// 替换xml中的无效字符
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        static string ReplaceHexadecimalSymbols(string txt)
        {
            //12655080931128840365
            return txt;
            string r = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
            return Regex.Replace(txt, r, "", RegexOptions.Compiled);
        }
    }
}
