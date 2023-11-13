using Cart.Configurations;
using Cart.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure
builder.Services.Configure<TokenOptions>(builder.Configuration.GetSection("Token"));

// Add services to the container.

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddStackExchangeRedisCache(x =>
{
    x.Configuration = Environment.GetEnvironmentVariable("REDIS_CONFIG");
    x.InstanceName = "local";
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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