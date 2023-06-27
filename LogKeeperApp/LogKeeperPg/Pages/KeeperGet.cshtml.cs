using LogKeeperPg.Data;
using LogKeeperPg.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LogKeeperPg.Pages;

public class KeeperGet : PageModel
{
    private readonly PostgresDbContext _postgresDbContext;

    public LogInformation? LogInfo { get; set; }

    public KeeperGet(PostgresDbContext postgresDbContext)
    {
        _postgresDbContext = postgresDbContext;
    }

    public async Task<IActionResult> OnGet(string id)
    {
        if (!string.IsNullOrEmpty(id))
        {
            try
            {
                LogInfo = await _postgresDbContext.Logs.Where(x => x.Uuid == Guid.Parse(id)).FirstOrDefaultAsync();
                if (LogInfo == null)
                {
                    return StatusCode(200, "Log not found");
                }

                return Page();
            }
            catch (Exception e)
            {
                return StatusCode(200, e);
            }
        }
        else
        {
            return StatusCode(500, "Identifier not set");
        }
    }
}