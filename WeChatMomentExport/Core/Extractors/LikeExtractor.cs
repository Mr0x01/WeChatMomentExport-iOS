using Claunia.PropertyList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatMomentExport.Model;

namespace WeChatMomentExport.Core.Extractors
{
    public class LikeExtractor : IExtractor<LikedInfo>
    {
        private DateTime time { get; set; }
        public LikeExtractor(NSArray _object, int i, DateTime time) : base(_object, i)
        {
            this.time = time;
        }

        public override LikedInfo Extract()
        {
            LikedInfo likedInfo = new LikedInfo();
            likedInfo.likedTime = time;
            likedInfo.likedUser = new UserInfo();
            likedInfo.likedUser.userId = _object[++i].ToString();
            likedInfo.likedUser.userName = _object[++i].ToString();
            return likedInfo;
        }
    }
}
