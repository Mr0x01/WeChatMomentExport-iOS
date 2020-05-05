using Claunia.PropertyList;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WeChatMomentExport.Model;
using WeChatMomentExport.Utils;

namespace WeChatMomentExport.Core
{
    public class MomentExporterFacade
    {
        private bool skipSharedItem { get; set; } = false;
        private string userHash { get; set; }
        private WebClient webClient { get; set; }
        private List<MomentInfo> bigOne { get; set; } = new List<MomentInfo>();

        public MomentExporterFacade(string userHash, bool skipSharedItem = false)
        {
            Init();
            this.userHash = userHash;
            this.skipSharedItem = skipSharedItem;
        }

        public void Start()
        {
            MomentDBUtil.LoadMomentSQLite(userHash);
            DirectoryInfo directoryInfo = new DirectoryInfo("Plist");
            foreach (FileInfo momentPlistFile in directoryInfo.GetFiles())
            {
                NSObject plist = PropertyListParser.Parse(momentPlistFile);
                MomentSerializer serializer = new MomentSerializer((NSDictionary)plist);
                MomentInfo momentInfo = serializer.SerializMoment();

                if (momentInfo != null &&
                    (
                        !(skipSharedItem && momentInfo.momentType == Enum.MomentType.Shared) || (momentInfo.sharedItem != null && momentInfo.sharedItem.sharedFrom == "微视"))
                    )
                {
                    LogUtil.Log($"解析{momentInfo.momentId}");
                    DownloadFile(momentInfo);
                    bigOne.Add(momentInfo);
                    string momentJson = JsonConvert.SerializeObject(momentInfo);
                    File.WriteAllText($"Json\\{momentInfo.momentId}.json", momentJson, Encoding.UTF8);
                }
            }
            string bigOneJson = JsonConvert.SerializeObject(bigOne);

            bigOneJson = "var moment_data = \"" + HttpUtility.UrlEncode(bigOneJson) + "\"";
            File.WriteAllText($"View\\static\\script\\data.js", bigOneJson, Encoding.UTF8);
        }

        private void DownloadFile(MomentInfo momentInfo)
        {
            string guid;
            string localFileName;
            if (momentInfo.momentImgs != null)
            {
                momentInfo.momentImgsLocal = new List<string>();
                foreach (Uri img in momentInfo.momentImgs)
                {
                    guid = Guid.NewGuid().ToString();
                    localFileName = $"View\\LocalFile\\{guid}.jpg";
                    LogUtil.Log($"下载{img}");
                    webClient.DownloadFile(img, localFileName);
                    momentInfo.momentImgsLocal.Add(localFileName);
                }
            }
            if (momentInfo.shortVideoUrl != null)
            {
                guid = Guid.NewGuid().ToString();
                localFileName = $"View\\LocalFile\\{guid}.mp4";
                LogUtil.Log($"下载{momentInfo.shortVideoUrl}");
                webClient.DownloadFile(momentInfo.shortVideoUrl, localFileName);
                momentInfo.shortVideoUrlLocal = localFileName;
            }
            if (momentInfo.sharedItem != null)
            {
                guid = Guid.NewGuid().ToString();
                localFileName = $"View\\LocalFile\\{guid}.mp4";
                LogUtil.Log($"下载{momentInfo.sharedItem.sharedUrl}");
                webClient.DownloadFile(momentInfo.sharedItem.sharedUrl, localFileName);
                momentInfo.sharedItem.sharedFileLocal = localFileName;
            }
        }

        public void Init()
        {
            webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            Directory.CreateDirectory("Plist");
            Directory.CreateDirectory("Json");
            Directory.CreateDirectory("View\\LocalFile");
        }
    }
}
