using ChatApp.Infrastructure;
using ChatApp.Application;
using Microsoft.AspNetCore.Authentication.Cookies;
using ChatApp.Web.Extensions;
using ChatApp.Web.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);
builder.Services.RegisterSignalR();

#region Authorisation
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(cfg =>
    {
       cfg.ExpireTimeSpan = TimeSpan.FromMinutes(15);
       cfg.SlidingExpiration = true; 
    });

builder.Services.ConfigureApplicationCookie(cfg =>
{
    cfg.LoginPath = "/login";
});
#endregion

// Add services to the container.
builder.Services.AddControllersWithViews()
     .AddRazorRuntimeCompilation();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapHub<ChatHub>("/hubs/chat");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
