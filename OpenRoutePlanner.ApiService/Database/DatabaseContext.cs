using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OpenRoutePlanner.Models;

namespace OpenRoutePlanner.Database;

public class DatabaseContext : DbContext
{
    public DbSet<DriverProfile> Drivers => Set<DriverProfile>();

    public DbSet<BusinessAccount> Accounts => Set<BusinessAccount>();

    public DbSet<RoutePlan> Routes => Set<RoutePlan>();

    public DbSet<Tractor> Tractors => Set<Tractor>();

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    private ValueConverter<DateTimeOffset, long> DateTimeOffsetConvertver { get; } = new ValueConverter<DateTimeOffset, long>(x => x.ToUnixTimeSeconds(), x => DateTimeOffset.FromUnixTimeSeconds(x));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTimeOffset))
                {
                    property.SetValueConverter(DateTimeOffsetConvertver);
                }
            }
        }
    }
}
