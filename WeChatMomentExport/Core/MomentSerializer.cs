using Claunia.PropertyList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatMomentExport.Core.Extractors;
using WeChatMomentExport.Enum;
using WeChatMomentExport.Model;

namespace WeChatMomentExport.Utils
{
    public class MomentSerializer
    {
        private NSDictionary momentPlist { get; set; }
        private MomentInfo momentInfo { get; set; }
        private NSArray _object { get; set; }

        private List<Uri> imgs { get; set; } = new List<Uri>();
        private List<LikedInfo> likes { get; set; } = new List<LikedInfo>();
        private List<CommentInfo> comments { get; set; } = new List<CommentInfo>();

        public MomentSerializer(NSDictionary plist)
        {
            this.momentPlist = plist;
            momentInfo = new MomentInfo();
        }

        public MomentInfo SerializMoment()
        {
            _object = (NSArray)momentPlist.ObjectForKey("$objects");
            IExtractor<MomentInfo> basicMomentInfo = new BasicInfoExtractor(_object);
            momentInfo = basicMomentInfo.Extract();

            IExtractor<UserInfo> poterInfoExtractor = new PosterInfoExtractor(_object);
            momentInfo.posterInfo = poterInfoExtractor.Extract();

            ExtractItems();
            momentInfo.momentImgs = imgs;
            momentInfo.like = likes;
            momentInfo.comment = comments;

            return momentInfo;
        }

        private void ExtractItems()
        {
            bool extractText = false;
            bool extractImgs = false;
            bool extractVideo = false;
            bool extractSharedItem = false;

            switch (momentInfo.momentType)
            {
                case MomentType.TextOnly:
                    extractText = true;
                    break;
                case MomentType.WithImg:
                    extractText = true;
                    extractImgs = true;
                    break;
                case MomentType.WithShortVideo:
                    extractText = true;
                    extractVideo = true;
                    break;
                case MomentType.Shared:
                    extractText = true;
                    extractSharedItem = true;
                    break;
            }



            for (int i = 0; i < _object.Count; i++)
            {
                Type currentNodeType = _object[i].GetType();
                if (currentNodeType == typeof(NSDictionary))
                {
                    NSDictionary currentItem = (NSDictionary)_object[i];
                    if (currentItem.ContainsKey("bDeleted"))//评论或点赞
                    {
                        DateTime time = TimeUtil.TimeStamp2Datetime(currentItem["createTime"].ToString());
                        string type = currentItem["type"].ToString();
                        if (type.ToString() == "1")//点赞
                        {
                            IExtractor<LikedInfo> likeExtractor = new LikeExtractor(_object, i, time);
                            likes.Add(likeExtractor.Extract());
                        }
                        else if (type.ToString() == "2")//评论
                        {
                            IExtractor<CommentInfo> commentExtractor = new CommentExtractor(_object, i, time);
                            CommentInfo comment = commentExtractor.Extract();
                            if (comment != null)
                            {
                                comments.Add(comment);
                            }
                        }
                    }
                    else if (extractText && currentItem.ContainsValue("WCAppInfo"))//文字
                    {
                        IExtractor<string> textExtractor = new TextExtractor(_object, i, momentInfo.posterInfo);
                        momentInfo.momentText = textExtractor.Extract();
                    }
                    else if (extractImgs && currentItem.ContainsKey("encIdx"))//附图
                    {
                        IExtractor<Uri> imgExtractor = new ImgExtractor(_object, i);
                        Uri imgUrl = imgExtractor.Extract();
                        if (imgUrl != null)
                        {
                            imgs.Add(imgUrl);
                        }
                    }
                    else if (extractVideo && currentItem.ContainsValue("WCUrl"))//视频
                    {
                        IExtractor<Uri> shortVideoExtractor = new ShortVideoExtractor(_object, i);
                        momentInfo.shortVideoUrl = shortVideoExtractor.Extract();
                    }
                    //else if (extractSharedItem)//分享
                    //{

                    //    IExtractor<SharedItem> sharedExtractor = new SharedExtractor(_object, i);
                    //    momentInfo.sharedItem = sharedExtractor.Extract();
                    //}
                }
                else if (extractSharedItem && currentNodeType == typeof(NSString))
                {
                    NSString currentItem = (NSString)_object[i];
                    if (currentItem.Content == "WeChat Sight")//微视分享
                    {
                        IExtractor<SharedItem> weishiSharedExtractor = new WeishiSharedExtractor(_object, i);
                        momentInfo.sharedItem = weishiSharedExtractor.Extract();
                    }
                }
            }
        }
    }
}
