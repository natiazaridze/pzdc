using KartuliAPI1.Auth;
using KartuliAPI1.Auth.Model;
using KartuliAPI1.Data.Repositories.KartuliAPI1.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");
builder.Services.AddTransient<IUsersRepository, UsersRepository>();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<JwtTokenService>();

builder.Services.AddIdentity<ForumRestUser, IdentityRole>(options =>
{
    // Configure identity options here if needed
})
.AddDefaultTokenProviders()
.AddRoles<string>();
/*.AddEntityFrameworkStores<YourDbContext>(); // Replace YourDbContext with your actual DbContext
*/
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer("Jwt", options =>
    {
        options.TokenValidationParameters.ValidAudience = builder.Configuration["Jwt:Audience"];
        options.TokenValidationParameters.ValidIssuer = builder.Configuration["Jwt:Issuer"];
        options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]));
    })
    .AddJwtBearer("JwtRecipes", options =>
    {
        options.TokenValidationParameters.ValidAudience = builder.Configuration["JwtRecipes:Audience"];
        options.TokenValidationParameters.ValidIssuer = builder.Configuration["JwtRecipes:Issuer"];
        options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtRecipes:SecretKey"]));
    })
    .AddJwtBearer("JwtWines", options =>
    {
        options.TokenValidationParameters.ValidAudience = builder.Configuration["JwtWines:Audience"];
        options.TokenValidationParameters.ValidIssuer = builder.Configuration["JwtWines:Issuer"];
        options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtWines:SecretKey"]));
    });

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    // Seed roles
    foreach (var role in ForumRoles.All)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

app.Run();

