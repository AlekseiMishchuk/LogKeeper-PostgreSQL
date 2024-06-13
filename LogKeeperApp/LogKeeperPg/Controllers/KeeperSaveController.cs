// using LogKeeperPg.Data;
// using LogKeeperPg.Models;
// using Microsoft.AspNetCore.Mvc;
//
// namespace LogKeeperPg.Controllers;
//
// public class KeeperSaveController : Controller
// {
//     private readonly PostgresDbContext _postgresDbContext;
//     private readonly IHttpContextAccessor _httpContextAccessor;
//     private readonly string? _token;
//
//     private const string GetValuePath = "/get-log?id={0}";
//
//     public KeeperSaveController(PostgresDbContext postgresDbContext, IHttpContextAccessor httpContextAccessor,  IConfiguration configuration)
//     {
//         _postgresDbContext = postgresDbContext;
//         _httpContextAccessor = httpContextAccessor;
//         _token = configuration["PassToken"];
//     }
//
//     
//
//     [HttpPost]
//     [Route("/keeper-save")]
//     public async Task<IActionResult> KeeperSave(string token, string title, string author, string project,
//         string contents)
//     {
//         if (string.IsNullOrWhiteSpace(token))
//         {
//             return StatusCode(500, "Tokens not set");
//         }
//
//         if (!string.IsNullOrWhiteSpace(_token) && token != _token)
//         {
//             return StatusCode(401, "Auth failed");
//         }
//
//         try
//         {
//             var logId = Guid.NewGuid();
//             await _postgresDbContext.AddAsync(new LogInformation(logId, title, author, project, DateTime.UtcNow, contents));
//             await _postgresDbContext.SaveChangesAsync();
//             var urlFormat = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{GetValuePath}";
//             return StatusCode(200, new
//             {
//                 id = logId,
//                 url_format = urlFormat
//             });
//         }
//         catch (Exception e)
//         {
//             return StatusCode(200, e);
//         }
//     }
// }