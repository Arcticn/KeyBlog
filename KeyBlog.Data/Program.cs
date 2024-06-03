using KeyBlog.Data;
using KeyBlog.Data.Extensions;
using KeyBlog.Data.Models.DTOs;
using KeyBlog.Data.Models.Entities;
using KeyBlog.Data.Services;
using KeyBlog.Data.Utils;

const string importDir = @"E:\Code\NEUQ\ACM\";

// 删除旧文件
var removeFileList = new List<string> { "app.db", "app.db-shm", "app.db-wal" };
foreach (var filename in removeFileList.Where(File.Exists))
{
    Console.WriteLine($"删除旧文件：{filename}");
    File.Delete(filename);
}

// 数据库
var freeSql = FreeSqlFactory.CreatePostgreSql("Host=localhost;Port=5432;Username=postgres;Password=admin;Database=keyblog");
var postRepo = freeSql.GetRepository<Post>();
var categoryRepo = freeSql.GetRepository<Category>();
var userRepo = freeSql.GetRepository<User>();
userRepo.Insert(new User { Username = "admin", PasswordHash = "admin".ToSM3Hash(), IsAdmin = true });

LocalUploader uploader = new LocalUploader(importDir, postRepo, categoryRepo);
uploader.Upload();