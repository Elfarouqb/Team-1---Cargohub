using Cargohub_V2.Contexts;
using Cargohub_V2.DataConverters;
using Microsoft.EntityFrameworkCore;
using Cargohub_V2.Services;
using System.Text.Json.Serialization;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CargoHubDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase")));


builder.Services.AddScoped<ItemGroupService>();
builder.Services.AddScoped<ItemLineService>();
builder.Services.AddScoped<ItemTypeService>();
builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<ClientsService>();
builder.Services.AddScoped<ShipmentService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<OrderItemsProcessor>();


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
    // Use GetRequiredService to get a single IServiceScopeFactory instance
    var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

    using (var scope = scopeFactory.CreateScope())
    {
        var services = scope.ServiceProvider;

        // Resolve the DbContext and the OrderItemsProcessor
        var dbContext = services.GetRequiredService<CargoHubDbContext>();
        var orderItemsProcessor = services.GetRequiredService<OrderItemsProcessor>();

        // Load initial data into the database using DataLoader
        DataLoader.ImportData(dbContext);

        // Define the path to your JSON file
        var orderJsonFilePath = "data/orders.json"; // Adjust this path based on your file location

        try
        {
            // Process the order items from the JSON file
            orderItemsProcessor.ProcessOrderItemsAsync(orderJsonFilePath).GetAwaiter().GetResult();
            Console.WriteLine("Order items processed and loaded successfully.");
        }
        catch (Exception ex)
        {
            // Log any errors that occur during processing
            Console.WriteLine($"An error occurred while processing order items: {ex.Message}");
        }
    }
}


