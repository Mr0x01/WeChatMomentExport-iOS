using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatMomentExport.Model
{
    public class LikedInfo
    {
        /// <summary>
        /// 点赞时间
        /// </summary>
        public DateTime likedTime { get; set; }
        /// <summary>
        /// 点赞的用户
        /// </summary>
        public UserInfo likedUser { get; set; }
    }
}
