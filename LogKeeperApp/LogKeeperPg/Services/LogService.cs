using LogKeeperPg.Data;
using LogKeeperPg.Models;
using Microsoft.EntityFrameworkCore;

namespace LogKeeperPg.Services;

public class LogService
{
    private static PostgresDbContext Context;
    private static GlobalVariablesService Variables;
    
    public LogService(PostgresDbContext context, GlobalVariablesService variables)
    {
        Context = context;
        Variables = variables;
    }

    public static async Task<List<LogInformation>> GetLogsAsync(int pageNumber)
    {
        
        var logList =  await Context.Logs
            .OrderByDescending(log => log.Time)
            .Skip((pageNumber - 1) * Variables.ElementsPerPage)
            .Take(Variables.ElementsPerPage)
            .ToListAsync();
        return logList;
    }
    
    public static async Task<int> GetLogsCountAsync()
    {
        return await Context.Logs.CountAsync();
    }
}