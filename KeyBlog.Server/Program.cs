using KeyBlog.Data.Extensions;
using KeyBlog.Data.Services;
using KeyBlog.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace KeyBlog.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddFreeSql(builder.Configuration);

        // Add services to the container.
        builder.Services.AddScoped<PostService>();
        builder.Services.AddScoped<CategoryService>();
        builder.Services.AddScoped<BlogPostService>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

        builder.Services.AddEndpointsApiExplorer();
        // 配置 Swagger
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            // 配置 JWT 认证
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "请输入 JWT 令牌。例如：Bearer your_token"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
            });
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });

        // 配置 JWT 认证
        var key = builder.Configuration["JwtConfig:SecretKey"];
        var issuer = builder.Configuration["JwtConfig:Issuer"];
        var audience = builder.Configuration["JwtConfig:Audience"];

        builder.Services.AddSingleton(new JWTHelper(key, issuer, audience));

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key))
                // ValidateIssuer = true, //是否验证Issuer
                // ValidIssuer = issuer, //发行人Issuer
                // ValidateAudience = true, //是否验证Audience
                // ValidAudience = audience, //订阅人Audience
                // ValidateIssuerSigningKey = true, //是否验证SecurityKey
                // IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)), //SecurityKey
                // ValidateLifetime = true, //是否验证失效时间
                // ClockSkew = TimeSpan.FromSeconds(30), //过期时间容错值，解决服务器端时间不同步问题（秒）
                // RequireExpirationTime = true //是否要求Token的Claims中必须包含Expires
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine("Authentication failed: " + context.Exception.Message);
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Console.WriteLine("Token validated: " + context.SecurityToken);
                    return Task.CompletedTask;
                }
            };
        });

        // 配置授权策略
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
        });

        var app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseAuthorization();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }

        // 注释以阻止HTTPS
        //app.UseHttpsRedirection();

        // 使用 CORS
        app.UseCors("AllowAllOrigins");

        app.UseAuthorization();

        app.MapControllers();

        app.MapFallbackToFile("/index.html");

        app.Run();
    }
}
