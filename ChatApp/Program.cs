using ChatApp.Infrastructure;
using ChatApp.Application;
using Microsoft.AspNetCore.Authentication.Cookies;
using ChatApp.Web.Extensions;
using ChatApp.Web.Hubs;
using ChatApp.Web.Exceptions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.RegisterSignalR();

#region Authorisation
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(cfg =>
    {
        cfg.LoginPath = "/login";
        cfg.ExpireTimeSpan = TimeSpan.FromMinutes(15);
        cfg.SlidingExpiration = true;
        cfg.Cookie.HttpOnly = true;
        cfg.Cookie.SameSite = SameSiteMode.Strict;
    });

builder.Services.ConfigureApplicationCookie(cfg =>
{
    cfg.LoginPath = "/login";
});
#endregion

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews()
     .AddRazorRuntimeCompilation();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
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
