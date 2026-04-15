using ChatApp.Infrastructure;
using ChatApp.Application;
using ChatApp.Shared.Constants;
using ChatApp.Data.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using ChatApp.Web.Extensions;
using ChatApp.Web.Exceptions;
using ChatApp.Application.Conversations.Hubs;
using ChatApp.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Authorisation
builder.Services.AddAuthentication(cfg => cfg.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme)
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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AppRoles.Admin, policy => policy.RequireRole(AppRoles.Admin));
});
#endregion

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.RegisterSignalR();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews()
     .AddRazorRuntimeCompilation();

builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

await app.Services.SeedIdentityDataAsync();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    _ = app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    _ = app.UseHsts();

    _ = app.UseSwaggerUI(options =>
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
