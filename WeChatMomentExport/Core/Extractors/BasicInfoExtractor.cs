using Claunia.PropertyList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatMomentExport.Enum;
using WeChatMomentExport.Model;
using WeChatMomentExport.Utils;

namespace WeChatMomentExport.Core.Extractors
{
    public class BasicInfoExtractor : IExtractor<MomentInfo>
    {
        public BasicInfoExtractor(NSArray _object, int i) : base(_object, i)
        {
        }
        public BasicInfoExtractor(NSArray _object) : base(_object, 0)
        {
        }

        public override MomentInfo Extract()
        {
            MomentInfo tempInfo = new MomentInfo();
            NSDictionary item1 = (NSDictionary)_object[1];
            tempInfo.momentTime = TimeUtil.TimeStamp2Datetime(item1.ObjectForKey("createtime").ToString());
            tempInfo.momentType = (MomentType)int.Parse(item1.ObjectForKey("contentDescScene").ToString());
            tempInfo.likeCount = int.Parse(item1.ObjectForKey("realLikeCount").ToString());
            tempInfo.commentCount = int.Parse(item1.ObjectForKey("realCommentCount").ToString());
            tempInfo.momentId = _object[2].ToString();

            return tempInfo;
        }
    }
}
