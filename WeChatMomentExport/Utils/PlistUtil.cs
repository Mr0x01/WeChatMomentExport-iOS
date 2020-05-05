using Claunia.PropertyList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatMomentExport.Utils
{
    public class PlistUtil
    {
        /// <summary>
        /// 将plist文件转换为xml文件
        /// </summary>
        /// <param name="path">plist文件位置</param>
        /// <param name="saveToFile">是否存储转换的xml文件</param>
        /// <returns></returns>
        public string Plist2XML(string path, bool saveToFile)
        {
            FileInfo plistFileInfo = new FileInfo(path);
            NSObject plist = PropertyListParser.Parse(plistFileInfo);
            string xml = plist.ToXmlPropertyList();
            if (saveToFile)
            {
                File.WriteAllText(plistFileInfo.DirectoryName + "\\" + plistFileInfo.Name + ".xml", xml);
            }
            return xml;
        }
    }
}
