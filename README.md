# 🔑KeyBlog
English | [中文](https://github.com/Arcticn/KeyBlog/blob/master/README-CN.md)

## 😊Project Introduction
KeyBlog is a Vue.js + ASP .Net Core blogging project inspired by Deali-Axy's [StarBlog](https://github.com/Deali-Axy/StarBlog).

It integrates a frontend blog page and a simple backend management system.

Deployment optimizations have been made to ensure an out-of-the-box experience, including providing free image hosting for all users.

## 🙂Technology Stack
- Frontend: Vue.js 3 + ElementPlus + md-editor-v3
- Backend: .Net 8 + ASP .Net Core + FreeSQL + JWT Authentication + PostgreSQL/SQLite

## 😶‍🌫️Project Structure
- `KeyBlog.Data`: Data model and initializer. Run this to initialize the database and bulk import markdown articles.
- `KeyBlog.Server`: Backend part of the project.
- `KeyBlog.client`: Vue frontend webpage part.

## 🐳Blog Features
- [X] Bulk import local markdown files during initialization and automatically build hierarchical categories based on folders.
- [x] View blog content with a directory for navigation.
- [ ] Add more information to the blog content display page, such as last updated time.
- [ ] Implement a comment system at the bottom of the blog content page.
- [x] Search blogs & sort blogs.
- [x] Online editing feature (normal users can only download locally, while admins can choose to publish remotely or save locally).
- [x] Upload images or paste from the clipboard during online editing, with images automatically uploaded to the image host and links inserted (for normal users, a free image host is provided and you can change it in `OnlineEditor.vue`; for admin users, please set the image host address and token manually in `appsettings.json`).
- [ ] Version history & auto-save during online editing.
- [x] Change article themes & code themes & night mode.
- [ ] More custom theme options.
- [ ] Wallpaper rotation.
- [x] Management system for CRUD operations on blogs, covering most scenarios.
- [ ] Upload local folders or files within the management system.
- [ ] Access statistics.

## 🥰Build
This project is developed based on `.Net8`. Ensure `.Net8 SDK` is installed before building.

### 🐬Simplest Deployment Method
```
git clone https://github.com/Arcticn/KeyBlog.git
cd .\KeyBlog\
dotnet publish "KeyBlog.Server" --configuration Release --output "KeyBlog.Server\publish"
```
The `dotnet publish` operation will automatically execute `npm run build` and store the generated static files in the publish folder.

If using `SQLite`, after `dotnet publish` is complete, copy the provided initialized `SQLite` database app.db into the `KeyBlog.Server\publish` folder.

Enter the publish folder and start KeyBlog.Server.exe, which by default listens on [localhost:5179](localhost:5179).

**If you need to bulk import markdown files or change the database during initialization, adjust and run KeyBlog.Data accordingly for initialization.**

### ☁️Deploying to Azure
If you wish to deploy to Azure, it is recommended to use Azure's remote `PostgreSQL` database service and modify the related code.

Two ways:
- Use virtual machine services ( **More flexible, while the free machine isn't reliable** )
  Directly deploy the publish folder. Remember to change the port to 80 or 443.
- Use AppService ( **Free for F1 and more convenient** ) 
  You can automatically deploy using `Github Action` without changing the port. Note that applying for `AppService` may take a very long time (about a few days).
  - The push configuration for the Action can be auto-generated using Visual Studio's publishing option. Recommended to skip the API management part.

## 📚Project Showcase
### Home Page
![Home Page](https://p.inari.site/usr/876/66840404dc567.png)

### Article Page
![Article Page](https://p.inari.site/usr/876/6684043da589d.png)

### Online Editing (Admin)
![Online Editing](https://p.inari.site/usr/876/66840443cb8ce.png)

### Online Editing (Guest, only provides md local download)
![Online Editing Guest](https://p.inari.site/usr/876/668404493eb87.png)

### Management Page (Admin Only)
![Management Page](https://p.inari.site/usr/876/66840453e9f9a.png)
