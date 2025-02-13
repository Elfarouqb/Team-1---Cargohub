using Cargohub_V2.Contexts;
using Cargohub_V2.DataConverters;
using Microsoft.EntityFrameworkCore;
using Cargohub_V2.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CargoHubDbContext>((serviceProvider, options) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("WebApiDatabase");
    options.UseNpgsql(connectionString); // Use the connection string from IConfiguration
});

// Register application services
builder.Services.AddScoped<ItemGroupService>();
builder.Services.AddScoped<ItemLineService>();
builder.Services.AddScoped<ItemTypeService>();
builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<ClientsService>();
builder.Services.AddScoped<ShipmentService>();
builder.Services.AddScoped<OrderService>();

// Configure JSON options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.MaxDepth = 64;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // Ignores circular references
    });

// Enable endpoint API explorer
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Check for the "seed" argument and call the SeedData1 method if true
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




