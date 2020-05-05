using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatMomentExport.Utils
{
    public class LogUtil
    {
        public static void Log(string l)
        {
            string log = string.Format("[{0}]:{1}", DateTime.Now.ToString("HH:mm:ss"), l);
            Console.WriteLine(log);
        }
    }
}
