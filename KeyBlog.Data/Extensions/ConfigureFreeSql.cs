using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KeyBlog.Data.Extensions;

public static class ConfigureFreeSql
{
    public static void AddFreeSql(this IServiceCollection services, IConfiguration configuration)
    {
        var freeSql = FreeSqlFactory.Create(configuration.GetConnectionString("SQLite"));

        services.AddSingleton(freeSql);

        // 仓储模式支持
        services.AddFreeRepository();
    }
}