using LogKeeperPg.Data;
using Microsoft.EntityFrameworkCore;

namespace LogKeeperPg.FakeLogs;

public static class SeedFakeData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new PostgresDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<PostgresDbContext>>());
        if (context.Logs.Any())
        {
            return;
        }

        var logEntryProvider = new FakeLogProvider();
        var logEntries = logEntryProvider.GenerateLog(30);

        context.Logs.AddRange(logEntries);
        context.SaveChanges();
    } 
}