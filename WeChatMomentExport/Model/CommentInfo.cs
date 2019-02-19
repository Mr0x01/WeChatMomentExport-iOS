using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatMomentExport.Model
{
    public class CommentInfo
    {
        public string comment_user_id { get; set; }
        public string comment_user_name { get; set; }
        public string comment_cotent { get; set; }
        public DateTime comment_time { get; set; }
        public string comment_reply { get; set; }
        public int type { get; set; }
    }
}
