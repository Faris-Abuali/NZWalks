using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Repositories;
using NZWalks.API.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection of DbContext - We used dependency injection to inject this DBContext class which we can later on use in different places in our app. e.g. In controllers or repositories
builder.Services.AddDbContext<NZWalksDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("NZWalksConnectionString"));
});
// The app will manage all instances of this DbContext class whenever we call it inside controllers or repositories

builder.Services.AddScoped<IRegionRepository, SqlRegionRepository>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
// This `AddAutoMapper` comes from the `AutoMapper.Extensions.Microsoft.DependencyInjection` package

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
