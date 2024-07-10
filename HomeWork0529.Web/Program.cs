using Microsoft.AspNetCore.Authentication.Cookies;

namespace HomeWork0529.Web;

public class Program
{
    private static string CookieScheme = "ReactAuthDemo";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();
        builder.Services.AddAuthentication(CookieScheme)
           .AddCookie(CookieScheme, options =>
           {
               options.Events = new CookieAuthenticationEvents
               {
                   OnRedirectToLogin = context =>
                   {
                       context.Response.StatusCode = 403;
                       context.Response.ContentType = "application/json";
                       var result = System.Text.Json.JsonSerializer.Serialize(new { error = "You are not authenticated" });
                       return context.Response.WriteAsync(result);
                   }
               };
           });

        builder.Services.AddSession();
        builder.Services.AddSignalR();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseHsts();
        }

        //app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseSession();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapHub<TaskItemHub>("/api/taskitem");


        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");

        app.MapFallbackToFile("index.html");

        app.Run();
    }
}