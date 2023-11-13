using Catalog.Models;
using Catalog.Repositories;
using Catalog.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<CatalogDbContext>(o => 
{
    o.UseNpgsql(Environment.GetEnvironmentVariable("POSTGRES_CONNECT_STR"),
        options => options.MigrationsAssembly("Catalog"));
});
builder.Services.AddTransient<ICatalogService, CatalogService>();
builder.Services.AddTransient<ICatalogRepository, CatalogRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
    if (context.Database.GetPendingMigrations().Any())
        context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();