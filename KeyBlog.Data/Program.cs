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
var adminFreeSql = FreeSqlFactory.CreatePostgreSql("Host=localhost;Port=5432;Username=postgres;Password=admin");
try
{
    // 删除旧数据库
    adminFreeSql.Ado.ExecuteNonQuery("DROP DATABASE IF EXISTS keyblog");
    Console.WriteLine("旧数据库已删除");

    // 创建新数据库
    adminFreeSql.Ado.ExecuteNonQuery("CREATE DATABASE keyblog");
    Console.WriteLine("新数据库已创建");
}
catch (Exception ex)
{
    Console.WriteLine($"数据库操作失败: {ex.Message}");
}
finally
{
    adminFreeSql.Dispose();
}

// 更新 FreeSQL 连接到新创建的数据库
var freeSql = FreeSqlFactory.CreatePostgreSql("Host=localhost;Port=5432;Username=postgres;Password=admin;Database=keyblog");

var postRepo = freeSql.GetRepository<Post>();
var categoryRepo = freeSql.GetRepository<Category>();
var userRepo = freeSql.GetRepository<User>();
userRepo.Insert(new User { Username = "admin", PasswordHash = "admin".ToSM3Hash(), IsAdmin = true });

LocalUploader uploader = new LocalUploader(importDir, postRepo, categoryRepo);
uploader.Upload();