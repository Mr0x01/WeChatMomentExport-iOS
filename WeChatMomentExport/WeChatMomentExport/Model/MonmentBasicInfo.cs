using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatMomentExport.Model
{
    public class MonmentBasicInfo
    {
        public int like_count { get; set; }//本条朋友圈的喜欢数
        public DateTime create_time { get; set; }//朋友圈发送时间
        public string content { get; set; }//朋友圈文字内容
        public Dictionary<string, string> liked_friends = new Dictionary<string, string>();//喜欢的朋友
    }
}
