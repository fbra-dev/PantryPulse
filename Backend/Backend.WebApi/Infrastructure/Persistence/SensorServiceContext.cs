using Backend.WebApi.Configuration;
using Backend.WebApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace Backend.WebApi.Infrastructure.Persistence;

public class SensorServiceContext : DbContext
{
    private readonly ConnectionStrings _connectionStrings;

    public DbSet<Sensor> Sensors { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<CallWebServiceAction> CallWebServiceActions { get; set; }
    
    public DbSet<PushNotificationAction> PushNotificationActions { get; set; }

    public SensorServiceContext(DbContextOptions<SensorServiceContext> contextOptions,
        IOptions<ConnectionStrings> connectionStrings)
        : base(contextOptions)
    {
        this._connectionStrings = connectionStrings.Value;
    }

    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(this._connectionStrings.DefaultConnection);

        base.OnConfiguring(optionsBuilder);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AutomatedAction>().HasKey(x => x.ID);
        
        EntityTypeBuilder<Sensor> sensorBuilder = modelBuilder.Entity<Sensor>();

        sensorBuilder.ToTable("Sensors");
        sensorBuilder.HasKey(x => x.ID);
        sensorBuilder.HasOne(x => x.Product);
        sensorBuilder.Property(x => x.Name).HasMaxLength(100);

        EntityTypeBuilder<Product> productBuilder = modelBuilder.Entity<Product>();

        productBuilder.ToTable("Products");
        productBuilder.HasKey(x => x.ID);
        productBuilder.HasMany(x => x.AutomatedActions);
        productBuilder.Property(x => x.Name).HasMaxLength(100);
        
        EntityTypeBuilder<AutomatedAction> automatedActionsBuilder = modelBuilder.Entity<AutomatedAction>();

        automatedActionsBuilder.UseTpcMappingStrategy();

        base.OnModelCreating(modelBuilder);
    }
}