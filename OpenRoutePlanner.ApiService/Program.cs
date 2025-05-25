using Microsoft.EntityFrameworkCore;
using OpenRoutePlanner.Database;
using OpenRoutePlanner.Modules;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

#if DEBUG
string DataLocation = "app.db";
#else
string DataLocation = "/app/data/app.db";
#endif

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlite($"Data Source={DataLocation}"));
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<GeotabModule>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();
app.MapControllers();
app.MapDefaultEndpoints();

using var scope = app.Services.CreateScope();
DatabaseContext db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
db.Database.Migrate();
#if DEBUG
//Seeder.Seed(db);
#endif
db.SaveChanges();

app.Run();
