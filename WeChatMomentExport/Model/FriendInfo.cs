using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatMomentExport.Model
{
    public class FriendInfo
    {
        public List<string> nick_name = new List<string>();//好友昵称
        public int like_count { get; set; }//好友点赞数

        public FriendInfo(string nick_name)
        {
            this.nick_name.Add(nick_name);
            like_count = 1;
        }
    }
}
