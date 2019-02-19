using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatMomentExport.Utils;

namespace WeChatMomentExport
{
    class Program
    {
        static void Main(string[] args)
        {
            var WC = new WCUtil();
            WC.LoadMomentSQLite("91ec3e7cf49aae1ce8a6437dc58aa759");
            WC.ExportAnalysis();
        }
    }
}
