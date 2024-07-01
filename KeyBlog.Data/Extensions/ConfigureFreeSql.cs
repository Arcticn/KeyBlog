using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KeyBlog.Data.Extensions;

public static class ConfigureFreeSql
{
    public static void AddFreeSql(this IServiceCollection services, IConfiguration configuration)
    {
        var freeSql = FreeSqlFactory.CreateSqlite(configuration.GetConnectionString("SQLite"));
        // var freeSql = FreeSqlFactory.CreatePostgreSql(configuration.GetConnectionString("PostgreSQL"));
        //var freeSql = FreeSqlFactory.CreateMySql(configuration.GetConnectionString("MySQL"));

        services.AddSingleton(freeSql);

        // 仓储模式支持
        services.AddFreeRepository();
    }
}