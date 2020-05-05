using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatMomentExport.Model
{
    public class UserInfo
    {
        /// <summary>
        /// 用户微信id
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 用户微信昵称
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 用户微信头像
        /// </summary>
        public Uri userAvatar { get; set; }
    }
}
