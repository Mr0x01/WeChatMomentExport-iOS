using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatMomentExport.Utils
{
    public class TimeUtil
    {
        /// <summary>
        /// 时间戳转Datetime
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static DateTime TimeStamp2Datetime(string ts)
        {
            DateTime t1 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime time = t1.AddSeconds(long.Parse(ts));
            return time.ToLocalTime();
        }
    }
}
