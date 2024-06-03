using FreeSql;
using FreeSql.Internal;

using System.Diagnostics;

namespace KeyBlog.Data;

public class FreeSqlFactory
{
    public static IFreeSql Create(DataType dataType, string connectionString)
    {
        return new FreeSql.FreeSqlBuilder()
            .UseConnectionString(dataType, connectionString)
            .UseNameConvert(NameConvertType.PascalCaseToUnderscoreWithLower)
            .UseAutoSyncStructure(true) //自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。
            .UseMonitorCommand(cmd => Console.WriteLine($"Sql：{cmd.CommandText}"))//监听SQL语句
            .Build(); //请务必定义成 Singleton 单例模式
    }

    public static IFreeSql CreateSqlite(string connectionString)
    {
        return Create(DataType.Sqlite, connectionString);
    }

    public static IFreeSql CreateMySql(string connectionString)
    {
        return Create(DataType.MySql, connectionString);
    }

    public static IFreeSql CreatePostgreSql(string connectionString)
    {
        return Create(DataType.PostgreSQL, connectionString);
    }

}