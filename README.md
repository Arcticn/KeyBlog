# KeyBlog
## 😊项目介绍
通过学习 Deali-Axy 大佬的 [StarBlog](https://github.com/Deali-Axy/StarBlog) 而编写的 Vue + .Net 博客项目

集成了前端博客页面与简易的后台管理

进行了一定的部署优化，确保提供开箱即用的体验

## 🙂技术选型
- 前端：Vue.js 3 + ElementPlus + md-editor-v3
- 后端：.Net 8 + ASP .Net Core + FreeSQL + JWT身份验证 + PostgreSQL/SQLite

## 😶‍🌫️项目结构
- KeyBlog.Data：数据模型+初始化程序，运行该程序进行数据库初始化+markdown文章批量导入
- KeyBlog.Server：项目后端部分
- KeyBlog.client：Vue 前端网页部分
 
## 🥰Build
本项目基于 .Net8 开发，构建之前请确保已经安装 .Net8 SDK。

### 🐬最简部署方法
```
git clone https://github.com/Arcticn/KeyBlog.git
cd .\KeyBlog\
dotnet publish "KeyBlog.Server" --configuration Release --output "KeyBlog.Server\publish"
```
如果选择使用SQLite，在publish完成后，将代码随附的初始化后的SQlite数据库app.db复制进入publish文件夹下

进入publish文件夹，点击KeyBlog.Server.exe，默认监听在[localhost:5179](localhost:5179)上

**如果需要初始时批量导入markdown或者更换数据库，请先调整并运行KeyBlog.Data相关内容以进行初始化**

### ☁️部署到Azure
如果您想要部署到Azure，建议使用Azure远程PostgreSQL数据库服务并更改相关代码
- 使用虚拟机服务直接部署publish文件夹。需要注意调整端口到80或443
- 使用AppService应用服务。可以使用Github Action自动部署，无需调整端口，申请AppService可能花费较长时间（约几天）。
  - Action推送配置可以使用Visual Studio发布选项自动生成，建议跳过API管理部分



## 📚项目展示
### 主页
![图片](https://github.com/Arcticn/KeyBlog/assets/46252987/81f7d467-c266-4caf-87a8-158368686826)
### 文章页面
![图片](https://github.com/Arcticn/KeyBlog/assets/46252987/dd1a56d0-11bb-47e4-9119-5276cf01deb2)
### 在线编辑（管理员）
![图片](https://github.com/Arcticn/KeyBlog/assets/46252987/dd757176-5e58-4de5-b38f-230a56aa193e)
### 在线编辑（游客，仅提供md本地下载）
![图片](https://github.com/Arcticn/KeyBlog/assets/46252987/5bfc1770-2d88-420d-bb70-9c1a4b309e7a)
### 管理页面（仅管理员）
![图片](https://github.com/Arcticn/KeyBlog/assets/46252987/60687bb0-a465-45c0-85b2-f2cc8a9fef1f)


