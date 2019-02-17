# iOS微信朋友圈导出
WeChatMomentExport-iOS是用C#编写的朋友圈导出工具
**(仅适用于iOS的朋友圈数据库)**
。

# 使用说明
## 从手机导出微信数据库

**从iOS8.3之后，苹果关闭了沙盒访问，所以无法直接访问微信的Document文件夹了。但是可以曲线救国，用iTunes或iMazing(推荐)备份手机数据，然后从备份数据中提取微信的Document内容。**

**注意：备份前，打开自己的朋友圈，快速的翻到最早的一条，确保所有数据都被缓存了。如果不放心，可以翻完之后断网确认是否还能看到，能看到意味着已经缓存成功。**

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
WC.LoadMomentSQLite("这里改成自己的Hash字符串");
```

F5运行。

运行后，默认会有两个文件输出：

文件 | 说明
--|--
AllMoments.txt | 自己发布过的所有朋友圈
Report.txt|对自己朋友圈的一个简单报告
# 其他说明
由于iOS版微信朋友圈缓存是存在sqlite里的plist(是的没错，数据库里存另一个数据库)，而plist里大多的数据没有键，只有值，所以解析起来比较费劲，目前了一些基础功能。后期的可玩性还是很大的。
> 赶快看看自己的塑料姐妹情吧~
# 效果
![Report.txt](https://raw.githubusercontent.com/Mr0x01/WeChatMomentExport-iOS/master/Export1.png "Report.txt")