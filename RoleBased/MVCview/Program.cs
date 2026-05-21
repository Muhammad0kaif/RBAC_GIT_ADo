using MVCview.Services;

namespace MVCview
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<TokenService>();
            builder.Services.AddScoped<ApiClientService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSession();
            app.Use(async (context, next) =>
            {
                var mustChangePassword =context.Session.GetString("mustChangePassword");

                var token =context.Session.GetString("token");

                var path = context.Request.Path.Value?.ToLower();

                bool isLoggedIn = !string.IsNullOrEmpty(token);

                bool mustChange =
                    mustChangePassword == "True";

                bool allowedPath =path.Contains("/profile/changepassword") || path.Contains("/account/logout") || path.Contains("/account/login") ||  path.Contains("/css") ||  path.Contains("/js") || path.Contains("/lib");

                if (isLoggedIn && mustChange && !allowedPath)
                {
                    context.Response.Redirect("/Profile/ChangePassword");
                    return;
                }

                await next();
            });

            app.UseAuthentication();
            app.UseAuthorization();
           


            app.MapStaticAssets();
            app.MapControllerRoute
               (
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
               ).WithStaticAssets();

            app.Run();
        }
    }
}
