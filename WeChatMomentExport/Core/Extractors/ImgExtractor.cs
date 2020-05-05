using Claunia.PropertyList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatMomentExport.Core.Extractors
{
    public class ImgExtractor : IExtractor<Uri>
    {
        public ImgExtractor(NSArray _object, int i) : base(_object, i)
        {
        }

        public override Uri Extract()
        {
            Uri url = new Uri(_object[i + 1].ToString());
            if (url.OriginalString.Contains("/150"))
            {
                return null;
            }
            else if (url.OriginalString.Contains("/0"))
            {
                return url;
            }
            return null;
        }
    }
}
