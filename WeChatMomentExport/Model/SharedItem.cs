using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatMomentExport.Model
{
    public class SharedItem
    {
        public string sharedTitle { get; set; }
        public string sharedDigest { get; set; }
        public Uri sharedUrl { get; set; }
        public string sharedFileLocal { get; set; }
        public string sharedFrom { get; set; }
    }
}
