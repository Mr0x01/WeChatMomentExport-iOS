using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeChatMomentExport.Utils
{
    public class StringUtil
    {
        /// <summary>
        /// 移除表情符号
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public string ReplaceHexadecimalSymbols(string txt)
        {
            string r = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
            return Regex.Replace(txt, r, "", RegexOptions.Compiled);
        }
    }
}
