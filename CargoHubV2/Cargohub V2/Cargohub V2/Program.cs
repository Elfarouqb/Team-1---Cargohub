using Cargohub_V2.Contexts;
using Cargohub_V2.DataConverters;
using Microsoft.EntityFrameworkCore;
using Cargohub_V2.Services;
using System.Text.Json.Serialization;
using Npgsql;



var builder = WebApplication.CreateBuilder(args);
//dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
//dotnet add package Npgsql.Json.NET
// Add services to the container.
builder.Services.AddDbContext<CargoHubDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase")));

NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();

builder.Services.AddScoped<ItemGroupService>();
builder.Services.AddScoped<ItemLineService>();
builder.Services.AddScoped<ItemTypeService>();
builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<ClientsService>();
builder.Services.AddScoped<ShipmentService>();
builder.Services.AddScoped<OrderService>();




builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.MaxDepth = 64;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // Ignores circular references

    });
builder.Services.AddEndpointsApiExplorer();



var app = builder.Build();
if (args.Length > 0 && args[0] == "seed")
{
    SeedData1(app);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void SeedData1(IHost app)
{
    var scopedFactory = app.Services.GetServices<IServiceScopeFactory>();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        DataLoader.ImportData(services.GetRequiredService<CargoHubDbContext>());
    }
}


