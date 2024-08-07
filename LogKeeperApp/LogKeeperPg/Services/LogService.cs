using LogKeeperPg.Data;
using LogKeeperPg.Models;
using Microsoft.EntityFrameworkCore;

namespace LogKeeperPg.Services;

public class LogService
{
    private readonly PostgresDbContext _context;
    private readonly GlobalVariablesService _variables;
    
    public LogService(PostgresDbContext context, GlobalVariablesService variables)
    {
        _context = context;
        _variables = variables;
    }

    public async Task<List<LogInformation>> GetLogsByPageAsync(int pageNumber)
    {
        
        var logList =  await _context.Logs
            .OrderByDescending(log => log.Time)
            .Skip((pageNumber - 1) * _variables.ElementsPerPage)
            .Take(_variables.ElementsPerPage)
            .ToListAsync();
        return logList;
    }
    
    public async Task<int> GetLogsCountAsync()
    {
        return await _context.Logs.CountAsync();
    }
}