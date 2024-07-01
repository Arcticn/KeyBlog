using KeyBlog.Data;
using KeyBlog.Data.Extensions;
using KeyBlog.Data.Models.DTOs;
using KeyBlog.Data.Models.Entities;
using KeyBlog.Data.Services;
using KeyBlog.Data.Utils;

//初始化数据库
//用来从本地全新创建数据库，导入数据

var dbType = "Sqlite";//"Sqlite";"PostgreSql";

if (dbType == "PostgreSql")
{
    // 连接PostgreSQL数据库 并 删除现有数据库
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
    Console.WriteLine("数据库初始化成功");

    // const string importDir = @"E:\Code\Example\";
    const string? importDir = null;
    if (importDir != null)
    {
        Console.WriteLine("开始导入数据");
        LocalUploader uploader = new LocalUploader(importDir, postRepo, categoryRepo);
        uploader.Upload();
    }

}
else if (dbType == "Sqlite")
{
    // 删除sqlite旧文件
    var removeFileList = new List<string> { "app.db", "app.db-shm", "app.db-wal" };
    foreach (var filename in removeFileList.Where(File.Exists))
    {
        Console.WriteLine($"删除旧文件：{filename}");
        File.Delete(filename);
    }

    var freeSql = FreeSqlFactory.CreateSqlite("Data Source=app.db;Synchronous=Off;Cache Size=5000;");

    var postRepo = freeSql.GetRepository<Post>();
    var categoryRepo = freeSql.GetRepository<Category>();
    var userRepo = freeSql.GetRepository<User>();
    userRepo.Insert(new User { Username = "admin", PasswordHash = "admin".ToSM3Hash(), IsAdmin = true });
    Console.WriteLine("数据库初始化成功");

    // const string importDir = @"E:\Code\Example\";
    const string? importDir = null;
    if (importDir != null)
    {
        Console.WriteLine("开始导入数据");
        LocalUploader uploader = new LocalUploader(importDir, postRepo, categoryRepo);
        uploader.Upload();
    }
    // Sqlite复制数据库
    var destFile = Path.GetFullPath("../../../../KeyBlog.Server/app.db");
    if (File.Exists("app.db"))
    {
        Console.WriteLine($"复制数据库：{destFile}");
        File.Copy("app.db", destFile, true);
    }
}
else
{
    Console.WriteLine("未知数据库类型");
    return;
}


