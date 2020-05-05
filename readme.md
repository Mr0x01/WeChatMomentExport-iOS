# 完全重构的iOS微信朋友圈导出
WeChatMomentExport-iOS是用C#编写的朋友圈导出工具
**(仅适用于iOS的朋友圈数据库)**。

# 使用说明
## 从手机导出微信数据库

**注意：使用前，打开微信，清空一下缓存(此步非必须，但是可以减少备份和拷贝所需的等待时间)，然后直接打开自己的朋友圈，往下翻到最早的一条，将自己所有的朋友圈缓存到本地。如果不放心，可以翻页完成之后断网确认是否还能看到，能看到意味着已经缓存成功。**

**从iOS8.3之后，苹果关闭了沙盒访问，所以无法直接访问微信的Document文件夹了。但是可以曲线救国，用iTunes或iMazing(推荐)备份手机数据，然后从备份数据中提取微信的Document内容。**



在微信的Document中，存在着至少一个以Hash字符串命名的文件夹（如果在这个手机上登陆过多个微信，则可能存在多个）。像这样的↓
> eb8a6093b56e2f1c27fbf471ee97c7f9

这样的文件夹中就存放着微信用户的个人数据。下列是一些已知作用的文件(夹)。

文件(夹)名 | 作用
------|-----
Audio | 语音消息的缓存
DB\MM.sqlite|聊天记录数据库
DB\WCDB_Contact.sqlite|通讯录数据库
Img|聊天图片缓存
Video|聊天小视频缓存
wc\wc005_008.db|朋友圈缓存

拷贝wc文件夹下的wc005_008.db至本项目的Debug文件夹中（找不到的话，生成项目）即可。
## 导出朋友圈数据
修改Main函数中的初始化部分
```CS
 MomentExporterFacade exporterFacade = new MomentExporterFacade("这里改成自己的Hash字符串", true);
```

F5运行。

运行后，会有以下文件(夹)输出：

文件(夹) | 说明
--|--
Plist\ | 存放自己发布过的所有朋友圈(wc005_008.db里导出的原始文件)
Json\  | 存放所有解析好的朋友圈JSON文件
View\LocalFile\  | 存放下载到本地的朋友圈中的文件(图片，视频之类的)
View\static\script\data.js  | 用于展示的朋友圈数据
# 已知问题
1. 除了微视分享外，其他的分享内容没有做兼容，不能正确导出分享的内容。
2. 有的评论没有评论人昵称，但这个不是解析的问题，而是plist里确实没有。此时用评论人id进行了替代。
3. 有的评论没有评论人昵称和评论人id，同样的，plist里确实没有。此时此条评论丢弃，因为没法确定哪个是评论本体。
4. 有的朋友圈明明是发了一张图片，但是plist里的类型却是小视频，所以解析出来的内容也会有误。
# 效果
![Report.txt](https://raw.githubusercontent.com/Mr0x01/WeChatMomentExport-iOS/master/Export1.png "Report.txt")

# 碰碰运气

如果这个项目帮到了你，或者感觉有点儿意思，可以的话还请支持一下。
<div>
<img src="https://raw.githubusercontent.com/Mr0x01/MoneyCode/master/a.jpg" height="300"/>
&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
<img src="https://raw.githubusercontent.com/Mr0x01/MoneyCode/master/w.jpg"height="300" />
</div>