using Claunia.PropertyList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatMomentExport.Model;

namespace WeChatMomentExport.Core.Extractors
{
    public class CommentExtractor : IExtractor<CommentInfo>
    {
        private DateTime time { get; set; }
        public CommentExtractor(NSArray _object, int i, DateTime time) : base(_object, i)
        {
            this.time = time;
        }

        public override CommentInfo Extract()
        {
            CommentInfo commentInfo = new CommentInfo();
            commentInfo.commentTime = time;
            commentInfo.commentUser = new UserInfo();
            commentInfo.commentUser.userId = _object[i + 1].ToString();
            try
            {
                commentInfo.commentUser.userName = _object[i + 2].ToString();
                commentInfo.commentContent = _object[i + 3].ToString();
                commentInfo.commentOrder = int.Parse(_object[i + 4].ToString());
            }
            catch (Exception)
            {
                try
                {
                    commentInfo.commentUser.userName = _object[i + 1].ToString();
                    commentInfo.commentContent = _object[i + 2].ToString();
                    commentInfo.commentOrder = int.Parse(_object[i + 3].ToString());
                }
                catch (Exception)
                {
                    return null;
                }
            }

            if (_object[i + 5].GetType() == typeof(NSString))
            {
                commentInfo.commentReplyto = _object[i + 5].ToString();
            }
            return commentInfo;
        }
    }
}
