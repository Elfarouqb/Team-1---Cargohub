using Cargohub_V2.Contexts;
using Cargohub_V2.DataConverters;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CargoHubDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

SeedData1(app);

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