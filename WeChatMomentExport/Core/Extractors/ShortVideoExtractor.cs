using Claunia.PropertyList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatMomentExport.Core.Extractors
{
    public class ShortVideoExtractor : IExtractor<Uri>
    {
        public ShortVideoExtractor(NSArray _object, int i) : base(_object, i)
        {
        }

        public override Uri Extract()
        {
            Uri url = new Uri(_object[i + 2].ToString());
            return url;
        }
    }
}
