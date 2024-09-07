using LogKeeperPg.Components;
using LogKeeperPg.Components.Pages;
using LogKeeperPg.Data;
using LogKeeperPg.Services;
using LogKeeperPg.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var route = builder.Configuration["LogKeeperUrl"] ?? throw new Exception("LogKeeperUrl is not set"); 

builder.Services.AddRazorComponents(options => 
        options.DetailedErrors = builder.Environment.IsDevelopment())
        .AddInteractiveServerComponents();
builder.Services.AddDbContext<PostgresDbContext>(options => 
        options.UseNpgsql(builder.Configuration.GetConnectionString("PgConnection")));
builder.Services.AddHttpClient<GetLog>(client => client.BaseAddress = new Uri(route));
builder.Services.AddSingleton<PaginationSettings>();
builder.Services.AddScoped<LogSavingService>();
builder.Services.AddScoped<GetLogService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddBlazorBootstrap();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.MapPost("/keeper-save", async (LogSavingService logSavingService,
    [FromForm] string token,
    [FromForm] string title,
    [FromForm] string author,
    [FromForm] string project,
    [FromForm] string contents) =>
    {
        var savingResult= await logSavingService.SaveLogAsync(token, title, author, project, contents);
        return Results.Json(statusCode: savingResult.StatusCode, data: savingResult.Result);
    })
    .DisableAntiforgery();

app.Run();