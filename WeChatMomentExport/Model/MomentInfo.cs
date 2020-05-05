using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatMomentExport.Enum;

namespace WeChatMomentExport.Model
{
    public class MomentInfo
    {
        /// <summary>
        /// 朋友圈ID
        /// </summary>
        public string momentId { get; set; }
        /// <summary>
        /// 发送人信息
        /// </summary>
        public UserInfo posterInfo { get; set; } = new UserInfo();
        /// <summary>
        /// 朋友圈的文字内容
        /// </summary>
        public string momentText { get; set; }
        /// <summary>
        /// 朋友圈的配图
        /// </summary>
        public List<Uri> momentImgs { get; set; }
        /// <summary>
        /// 朋友圈的配图(本地文件)
        /// </summary>
        public List<string> momentImgsLocal { get; set; }
        /// <summary>
        /// 朋友圈url(仅在分享的朋友圈中存在)
        /// </summary>
        public SharedItem sharedItem { get; set; }
        /// <summary>
        /// 朋友圈小视频
        /// </summary>
        public Uri shortVideoUrl { get; set; }
        /// <summary>
        /// 朋友圈小视频
        /// </summary>
        public string shortVideoUrlLocal { get; set; }
        /// <summary>
        /// 朋友圈发布的时间
        /// </summary>
        public DateTime momentTime { get; set; }
        /// <summary>
        /// 朋友圈类型
        /// </summary>
        public MomentType momentType { get; set; }
        /// <summary>
        /// 点赞数
        /// </summary>
        public int likeCount { get; set; }
        /// <summary>
        /// 喜欢列表
        /// </summary>
        public List<LikedInfo> like { get; set; }
        /// <summary>
        /// 评论数
        /// </summary>
        public int commentCount { get; set; }
        /// <summary>
        /// 评论列表
        /// </summary>
        public List<CommentInfo> comment { get; set; }
    }
}
