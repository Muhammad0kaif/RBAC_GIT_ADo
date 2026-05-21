using AdoApi2.Filters;
using AdoApi2.Infrastructure;
using AdoApi2.Middleware;
using AdoApi2.Repositories.Implemenetation;
using AdoApi2.Repositories.Interfaces;
using AdoApi2.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

using System.Text;

namespace AdoApi2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Console.WriteLine("ROOT => " + builder.Environment.ContentRootPath);

            Console.WriteLine(
                "APPSETTINGS EXISTS => " +
                File.Exists("appsettings.json")
            );

            Console.WriteLine(
                "CONNECTION => " +
                builder.Configuration.GetConnectionString("Sql")
            );

            builder.Configuration .SetBasePath(Directory.GetCurrentDirectory()) .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Console.WriteLine(
                "SQL => " +
                builder.Configuration.GetConnectionString("Sql")
            );

            // Controllers
            builder.Services.AddControllers(options =>
            {
                options.Filters.AddService<AuditLogFilter>();
            });
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) .AddJwtBearer(options =>
            {
                  options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            builder.Configuration["Jwt:Key"]))
                    };
            });

           

            builder.Services.AddAuthorization();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(
                    "Bearer",
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Name = "Authorization",

                        Type =
                            Microsoft.OpenApi.Models.SecuritySchemeType.Http,

                        Scheme = "bearer",

                        BearerFormat = "JWT",

                        In =
                            Microsoft.OpenApi.Models.ParameterLocation.Header,

                        Description =
                            "Enter JWT Token"
                    });

                options.AddSecurityRequirement(
                    new Microsoft.OpenApi.Models.OpenApiSecurityRequirement                    {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme                {
                    Reference =
                        new Microsoft.OpenApi.Models.OpenApiReference                        {
                            Type =
                                Microsoft.OpenApi.Models.ReferenceType                                    .SecurityScheme,

                            Id = "Bearer"                        }
                },

                Array.Empty<string>()
            }
                    });
            });



            // Services
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<JwtService>();
            builder.Services.AddScoped<PermissionService>();
            builder.Services.AddScoped<ReportService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<OrderService>();
            builder.Services.AddScoped<AuditLogService>();
            builder.Services.AddScoped<AuditLogFilter>();

            // Repositories
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();

            //Service Interfaces
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            builder.Services.AddScoped<IPasswordService, PasswordService>();

            // DB Factory
            builder.Services.AddSingleton<DbConnectionFactory>(sp =>
            {
                var configuration =
                    sp.GetRequiredService<IConfiguration>();

                return new DbConnectionFactory(configuration);
            });
           
            var app = builder.Build();

            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
              
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseMiddleware<ExceptionMiddleware>();
           
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            Console.WriteLine(builder.Configuration.GetConnectionString("Sql"));
            app.Run();
        }
    }
}