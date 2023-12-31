using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Repositories;
using NZWalks.API.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer; //ASP.NET Core middleware that enables an application to receive an OpenID Connect bearer token.
using Microsoft.IdentityModel.Tokens; //Includes types that provide support for SecurityTokens, Cryptographic operations: Signing, Verifying Signatures, Encryption.
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;
using Serilog;
using NZWalks.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// --- Serilog Logging
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/NzWalks_Log.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Warning()
    .CreateLogger();

builder.Logging.ClearProviders(); // Removes all ILoggerProviders from builder.
builder.Logging.AddSerilog(logger); // Add Serilog to the logging pipeline
// ---

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor(); //Adds default implementation to the HttpContextAccessor service.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "NZ Walks API", Version = "v1" });

    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header, // The location of the API key: [query, header, path, or cookie]
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    var securityScheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = JwtBearerDefaults.AuthenticationScheme
        },
        Scheme = "Oauth2",
        Name = JwtBearerDefaults.AuthenticationScheme,
        In = ParameterLocation.Header
    };

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            securityScheme,
            new List<string>()
        }
    });
});

// Dependency Injection of DbContext - We used dependency injection to inject this DBContext class which we can later on use in different places in our app. e.g. In controllers or repositories
builder.Services.AddDbContext<NZWalksDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionString")));
// The app will manage all instances of this DbContext class whenever we call it inside controllers or repositories

builder.Services.AddDbContext<NZWalksAuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksAuthConnectionString")));

builder.Services.AddScoped<IRegionRepository, SqlRegionRepository>();

builder.Services.AddScoped<IWalkRepository, SqlWalkRepository>();

builder.Services.AddScoped<ITokenRepository, TokenRepository>();

builder.Services.AddScoped<IImageRepository, LocalImageRepository>();

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

app.UseMiddleware<ExceptionHandlerMiddleware>(); // Global Exception Handling

app.UseHttpsRedirection();

app.UseAuthentication(); // Adds the Microsoft.AspNetCore.Authentication.AuthenticationMiddleware to the IAppApplicationBuilder to enable authentication capabilities

app.UseAuthorization();

/***
 * This means that when a request is made to a URL like "https://localhost:port/Images/file.jpg", 
 * the static file middleware will attempt to find and serve the corresponding file from the "Images" directory specified by the PhysicalFileProvider.
 */
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Images")), // exposes the contents of a physical directory on the file system.
    RequestPath = "/Images"  // specifies the URL path at which the static files will be served.
});

// You can add another middleware registration un case you need to expose another physical directory to another RequestPath
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Uploads")), // exposes the contents of a physical directory on the file system.
    RequestPath = "/Uploads"  // specifies the URL path at which the static files will be served.
});

app.MapControllers();

app.Run();
