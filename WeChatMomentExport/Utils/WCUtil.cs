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

        public void LoadMomentSQLite(string profile_hash)
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
                top += $"编号：{moment.Key}\r\n内容：{moment.Value.content}\r\n日期：{moment.Value.create_time}\r\n点赞：{moment.Value.like_count}\r\n\r\n";
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
            var new_one = new FileInfo(path);
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
                    info.create_time = TStoLocalTime(long.Parse(val));
                    continue;
                }
                else if (key == "realLikeCount")
                {
                    info.like_count = int.Parse(val);
                    break;
                }
            }

            try
            {
                node = xmlDocument.SelectSingleNode("/plist/dict/array//key[. = 'isForceUpdate']").ParentNode.NextSibling;//取自己发表的内容
                if (node.Name.Equals("dict"))
                {
                    while (true)
                    {
                        if (node.NextSibling.Name.Equals("dict"))
                        {
                            info.content = node.InnerText;
                            if (info.content.Equals("$classnameWCAppInfo$classesWCAppInfoNSObject"))
                            {
                                node = xmlDocument.SelectSingleNode("/plist/dict/array//key[. = 'appMsgShareInfo']").ParentNode.NextSibling;
                                info.content = "[文章标题]" + node.InnerText;
                            }
                            break;
                        }
                        else
                        {
                            node = node.NextSibling;
                        }
                    }
                }
                else if (node.Name.Equals("string"))
                {
                    while (true)
                    {
                        node = node.NextSibling;
                        if (node.Name.Equals("dict"))
                        {
                            info.content = node.NextSibling.InnerText;
                            break;
                        }
                    }
                }
            }
            catch (NullReferenceException)
            {
                node = xmlDocument.SelectSingleNode("/plist/dict/array//key[. = 'bUseXorEncrypt']").ParentNode;//取自己发表的内容
                if (node.Name.Equals("dict"))
                {
                    node = node.NextSibling.NextSibling;
                    info.content = node.InnerText;
                }
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

            node = xmlDocument.SelectSingleNode("/plist/dict/array//string[. = 'NSMutableArray']").ParentNode.NextSibling;
            int comment_friends_index = node.SelectNodes("array/string").Count;
            //if (comment_friends_index > 0)
            //{
            //    for (int i = 0; i < comment_friends_index; i++)
            //    {
            //        CommentInfo comment_info = new CommentInfo();
            //        node = node.NextSibling;//评论的属性节点

            //        long comment_ts = long.Parse(node.SelectSingleNode("key[. = 'createTime']").NextSibling.InnerText);
            //        comment_info.comment_time = TStoLocalTime(comment_ts);
            //        node = node.NextSibling;//评论人微信号

            //        string wxid = node.InnerText;
            //        comment_info.comment_user_id = wxid;
            //        node = node.NextSibling;//评论人昵称


            //        string nick_name = node.InnerText;
            //        comment_info.comment_user_name = nick_name;
            //        node = node.NextSibling;//评论内容

            //        string comment = node.InnerText;
            //        comment_info.comment_cotent = comment;
            //        node = node.NextSibling;//某种奇怪的ID，会以32递增

            //        if (node.NextSibling.Name.Equals("string"))
            //        {
            //            node = node.NextSibling;//回复对象
            //            string reply = node.InnerText;
            //            comment_info.comment_reply = reply;
            //            comment_info.type = 0;
            //        }
            //        else
            //        {
            //            comment_info.type = 1;
            //        }
            //        info.comment_list.Add(comment_info);

            //    }
            //}
            return info;
        }

        /// <summary>
        /// 替换xml中的无效字符
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        private static string ReplaceHexadecimalSymbols(string txt)
        {
            string r = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
            return Regex.Replace(txt, r, "", RegexOptions.Compiled);
        }

        /// <summary>
        /// 时间戳转本地时间
        /// </summary>
        /// <param name="TS"></param>
        /// <returns></returns>
        private static DateTime TStoLocalTime(long TS)
        {
            DateTime converted = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime newDateTime = converted.AddSeconds(TS);
            return newDateTime.ToLocalTime();
        }
    }
}
