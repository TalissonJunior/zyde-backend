using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Zyde.Model;

namespace Zyde.Infrastructure.EntityFrameworkDataAccess;

public class Context : DbContext
{
    private string ConnectionString { get; set; }
    public virtual DbSet<Device> Devices { get; set; }
    public virtual DbSet<Position> Positions { get; set; }
    public virtual DbSet<Address> Addresses { get; set; }
    public virtual DbSet<PositionEvent> PositionEvents { get; set; }
    public virtual DbSet<Simcard> Simcards { get; set; }
    public virtual DbSet<SimcardSubscription> SimcardSubscriptions { get; set; }

    public Context() { }

    public Context(string connectionString)
    {
        ConnectionString = connectionString;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var configurations = Assembly.GetExecutingAssembly().GetTypes().Where(
            cf => cf.IsClass && cf.Namespace == "Zyde.Infrastructure.Configurations" &&
            cf.BaseType.AssemblyQualifiedName.Contains("CoffeeConfiguration")
        );

        foreach (var config in configurations)
        {
            modelBuilder.ApplyConfiguration(Activator.CreateInstance(config) as dynamic);
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (string.IsNullOrEmpty(ConnectionString))
        {
            var tempConnectionFsPath = Path.Combine(Path.GetTempPath(), "Zyde");

            if (File.Exists(tempConnectionFsPath))
            {
                ConnectionString = File.ReadAllText(tempConnectionFsPath);
            }
        }

        optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));
        optionsBuilder.EnableSensitiveDataLogging();
    }
}