using Claunia.PropertyList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatMomentExport.Model;

namespace WeChatMomentExport.Core.Extractors
{
    public class SharedExtractor : IExtractor<SharedItem>
    {
        public SharedExtractor(NSArray _object, int i) : base(_object, i)
        {
        }

        public override SharedItem Extract()
        {
            SharedItem sharedItem = new SharedItem();
            sharedItem.sharedTitle = _object[i + 4].ToString();
            sharedItem.sharedDigest = _object[i + 5].ToString();
            sharedItem.sharedUrl = new Uri(_object[i + 6].ToString());
            return sharedItem;
        }
    }
}
