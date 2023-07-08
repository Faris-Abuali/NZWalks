using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Repositories;
using NZWalks.API.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer; //ASP.NET Core middleware that enables an application to receive an OpenID Connect bearer token.
using Microsoft.IdentityModel.Tokens; //Includes types that provide support for SecurityTokens, Cryptographic operations: Signing, Verifying Signatures, Encryption.
using System.Text;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection of DbContext - We used dependency injection to inject this DBContext class which we can later on use in different places in our app. e.g. In controllers or repositories
builder.Services.AddDbContext<NZWalksDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionString")));
// The app will manage all instances of this DbContext class whenever we call it inside controllers or repositories

builder.Services.AddDbContext<NZWalksAuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksAuthConnectionString")));

builder.Services.AddScoped<IRegionRepository, SqlRegionRepository>();

builder.Services.AddScoped<IWalkRepository, SqlWalkRepository>();

builder.Services.AddScoped<ITokenRepository, TokenRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
// This `AddAutoMapper` comes from the `AutoMapper.Extensions.Microsoft.DependencyInjection` package

// -- Setting up Identity --
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
    .AddEntityFrameworkStores<NZWalksAuthDbContext>()
    .AddDefaultTokenProviders();

// -- Setting up Identity Options --
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});


// Adding JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
        // In case of 401 unauthorized and you want to know what is going on:
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = c =>
            {
                // break point here, debug and see the `exception` message 
                return Task.CompletedTask;
            }
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Adds the Microsoft.AspNetCore.Authentication.AuthenticationMiddleware to the IAppApplicationBuilder to enable authentication capabilities

app.UseAuthorization();

app.MapControllers();

app.Run();
