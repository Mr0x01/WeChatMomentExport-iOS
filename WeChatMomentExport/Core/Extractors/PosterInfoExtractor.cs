using Claunia.PropertyList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatMomentExport.Model;

namespace WeChatMomentExport.Core.Extractors
{
    public class PosterInfoExtractor : IExtractor<UserInfo>
    {
        public PosterInfoExtractor(NSArray _object, int i) : base(_object, i)
        {
        }

        public PosterInfoExtractor(NSArray _object) : base(_object, 0)
        {
        }

        public override UserInfo Extract()
        {
            UserInfo userInfo = new UserInfo();
            userInfo.userId = _object[3].ToString();
            userInfo.userName = _object[4].ToString();
            return userInfo;
        }
    }
}
