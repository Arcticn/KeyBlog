# 🔑KeyBlog
中文 | [English](https://github.com/Arcticn/KeyBlog/blob/master/README-EN.md)

## 😊项目介绍
通过学习 Deali-Axy 大佬的 [StarBlog](https://github.com/Deali-Axy/StarBlog) 而编写的 Vue.js + ASP .Net Core 博客项目

集成了前端博客页面与简易的后台管理

进行了一定的部署优化，确保提供开箱即用的体验，包括为所有用户提供了免费图床

## 🙂技术选型
- 前端：Vue.js 3 + ElementPlus + md-editor-v3
- 后端：.Net 8 + ASP .Net Core + FreeSQL + JWT身份验证 + PostgreSQL/SQLite

## 😶‍🌫️项目结构
- `KeyBlog.Data`：数据模型+初始化程序，运行该程序进行数据库初始化+markdown文章批量导入
- `KeyBlog.Server`：项目后端部分
- `KeyBlog.client`：Vue 前端网页部分

## 🐳博客功能
- [X] 初始化时批量导入本地markdown文件，并自动根据文件夹构建层级分类
- [x] 查看博客正文，提供目录用于跳转
- [ ] 博客正文显示页面增加更多信息，如更新时间等
- [ ] 博客正文页面底部实现评论系统
- [x] 搜索博客 & 对博客进行排序
- [x] 在线编辑功能（实现对普通用户和管理员的不同在线编辑：普通用户仅能下载到本地，管理员可自由选择远程发布或本地保存）
- [x] 在线编辑时可上传图片或从剪贴板粘贴，之后，图片自动上传到图床并插入链接（对于普通用户，提供了免费图床的调用，设置在`OnlineEditor.vue`；对于管理员用户，图床地址和token请在appsettings.json手动设置）
- [ ] 在线编辑时的版本历史记录 & 自动保存
- [x] 文章主题 & 代码主题变更 & 黑夜模式
- [ ] 更多自定义主题选项
- [ ] 壁纸轮换
- [x] 管理系统，实现对博客的增删改查，覆盖大多数场景
- [ ] 在管理系统内上传本地文件夹或文件
- [ ] 访问统计

## 🥰Build
本项目基于 `.Net8` 开发，构建之前请确保已经安装 `.Net8 SDK`。

### 🐬最简部署方法
```
git clone https://github.com/Arcticn/KeyBlog.git
cd .\KeyBlog\
dotnet publish "KeyBlog.Server" --configuration Release --output "KeyBlog.Server\publish"
```
`dotnet publish`操作会自动执行`npm run build`并将生成的静态文件存放到publish文件夹

如果选择使用`SQLite`，在`dotnet publish`完成后，将代码随附的初始化后的`SQlite`数据库app.db复制进入`KeyBlog.Server\publish`文件夹下

进入publish文件夹，启动KeyBlog.Server.exe，默认监听在[localhost:5179](localhost:5179)上

**如果需要初始时批量导入markdown或者更换数据库，请先调整并运行KeyBlog.Data相关内容以进行初始化**

### ☁️部署到Azure
如果您想要部署到Azure，建议使用Azure远程`PostgreSQL`数据库服务并更改相关代码
- 使用虚拟机服务(**更灵活，但免费级别的机器不可靠**)
  
  直接部署publish文件夹。需要注意调整端口到80或443
  
- 使用AppService应用服务（**F1层免费，且更方便**）
  
  可以使用`Github Action`自动部署，无需调整端口，申请`AppService`可能花费较长时间（约几天）。
  - Action推送配置可以使用Visual Studio发布选项自动生成，建议跳过API管理部分



## 📚项目展示
### 主页
![主页](https://p.inari.site/usr/876/66840404dc567.png)

### 文章页面
![文章页面](https://p.inari.site/usr/876/6684043da589d.png)

### 在线编辑（管理员）
![在线编辑](https://p.inari.site/usr/876/66840443cb8ce.png)

### 在线编辑（游客，仅提供md本地下载）
![在线编辑游客](https://p.inari.site/usr/876/668404493eb87.png)

### 管理页面（仅管理员）
![管理页面](https://p.inari.site/usr/876/66840453e9f9a.png)



