using EFCore.BulkExtensions;
using LogKeeperPg.Components;
using LogKeeperPg.Components.Pages;
using LogKeeperPg.Data;
using LogKeeperPg.Models;
using LogKeeperPg.PreviousLogsToDb;
using LogKeeperPg.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var route = builder.Configuration["LogKeeperUrl"] ?? 
            throw new Exception("LogKeeperUrl is not set"); 

// builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorComponents(options => 
        options.DetailedErrors = builder.Environment.IsDevelopment())
        .AddInteractiveServerComponents();

builder.Services.AddDbContext<PostgresDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PgConnection")));
builder.Services.AddHttpClient<GetLog>(client => client.BaseAddress = new Uri(route));
builder.Services.AddSingleton<GlobalVariablesService>();
builder.Services.AddScoped<LogService>();

builder.Services.AddBlazorBootstrap();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();

// app.UseRouting();
app.UseAntiforgery();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

var links = await SlackScraper.GetLinksFromChannelAsync();
var logEntries = new List<LogInformation>();

foreach (var link in links)
{
    var logEntry = await WebScraper.ScrapeLogInformation(link);
    logEntries.Add(logEntry);
}

using var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<PostgresDbContext>();

await context.BulkInsertAsync(logEntries);

app.Run();