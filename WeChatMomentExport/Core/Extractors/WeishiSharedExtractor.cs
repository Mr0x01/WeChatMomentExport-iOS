using Claunia.PropertyList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatMomentExport.Model;

namespace WeChatMomentExport.Core.Extractors
{
    public class WeishiSharedExtractor : IExtractor<SharedItem>
    {
        public WeishiSharedExtractor(NSArray _object, int i) : base(_object, i)
        {
        }

        public override SharedItem Extract()
        {
            SharedItem sharedItem = new SharedItem();
            sharedItem.sharedFrom = "微视";
            sharedItem.sharedUrl = new Uri(_object[i + 14].ToString());
            return sharedItem;
        }
    }
}
