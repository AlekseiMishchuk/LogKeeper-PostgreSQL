using LogKeeperPg.Models;
using Microsoft.EntityFrameworkCore;

namespace LogKeeperPg.Data;

public sealed class PostgresDbContext : DbContext
{
    public DbSet<LogInformation> Logs { get; set; }
    
    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LogInformation>().ToTable("Logs");
    }
}