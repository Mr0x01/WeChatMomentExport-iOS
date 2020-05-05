using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatMomentExport.Model
{
    public class CommentInfo
    {
        /// <summary>
        /// 评论内容
        /// </summary>
        public string commentContent { get; set; }
        /// <summary>
        /// 评论时间
        /// </summary>
        public DateTime commentTime { get; set; }
        /// <summary>
        /// 评论的用户
        /// </summary>
        public UserInfo commentUser { get; set; }
        /// <summary>
        /// 回复给谁(可能为空)
        /// </summary>
        public string commentReplyto { get; set; }
        /// <summary>
        /// 评论的排列顺序(越大越靠后)
        /// </summary>
        public int commentOrder { get; set; }
    }
}
