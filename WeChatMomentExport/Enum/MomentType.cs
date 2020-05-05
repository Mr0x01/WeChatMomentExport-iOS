using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatMomentExport.Enum
{
    public enum MomentType
    {
        /// <summary>
        /// 有视频(不一定有文字)
        /// </summary>
        WithShortVideo = 0,
        /// <summary>
        /// 只有文字
        /// </summary>
        TextOnly = 2,
        /// <summary>
        /// 有图片(不一定有文字)
        /// </summary>
        WithImg = 3,
        /// <summary>
        /// 分享(不一定有文字)
        /// </summary>
        Shared = 4
    }
}
