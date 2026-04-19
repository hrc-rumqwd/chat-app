using ChatApp.Infrastructure;
using ChatApp.Application;
using Microsoft.AspNetCore.Authentication.Cookies;
using ChatApp.Web.Extensions;
using ChatApp.Web.Exceptions;
using ChatApp.Application.Conversations.Hubs;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.IdentityModel.Tokens;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Authorisation
builder.Services.AddAuthentication(cfg => cfg.DefaultAuthenticateScheme = BearerTokenDefaults.AuthenticationScheme)
    .AddJwtBearer(BearerTokenDefaults.AuthenticationScheme, cfg =>
    {
        cfg.Audience = builder.Configuration["JwtConfiguration:Audience"];
        cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["JwtConfiguration:Issuer"],
            ValidateIssuer = bool.Parse(builder.Configuration["JwtConfiguration:ValidateIssuer"]),
            ValidAudience = builder.Configuration["JwtConfiguration:Audience"],
            ValidateAudience = bool.Parse(builder.Configuration["JwtConfiguration:ValidateAudience"]),
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JwtConfiguration:SecretKey"])),
        };
    })
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

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.RegisterSignalR();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(cfg =>
{
    cfg.AddPolicy("AllowLocalOnly", pol =>
    {
        pol.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddControllersWithViews()
     .AddRazorRuntimeCompilation();

builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

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

app.UseCors("AllowLocalOnly");

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
