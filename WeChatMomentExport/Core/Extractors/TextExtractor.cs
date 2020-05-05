using Claunia.PropertyList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatMomentExport.Model;

namespace WeChatMomentExport.Core.Extractors
{
    public class TextExtractor : IExtractor<string>
    {
        private UserInfo posterInfo { get; set; }
        public TextExtractor(NSArray _object, int i) : base(_object, i)
        {
        }
        public TextExtractor(NSArray _object, int i, UserInfo posterInfo) : base(_object, i)
        {
            this.posterInfo = posterInfo;
        }

        public override string Extract()
        {
            for (int j = 1; j < 5; j++)
            {
                Type type = _object[i + j].GetType();
                string content = _object[i + j].ToString();
                if (type == typeof(NSString) && !content.StartsWith("gh_") &&
                    content != posterInfo.userId &&
                    content != posterInfo.userName)
                {
                    return _object[i + j].ToString();
                }
            }
            return "";
        }
    }
}
