using LogKeeperPg.Components;
using LogKeeperPg.Components.Pages;
using LogKeeperPg.Data;
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
// app.MapControllers();

app.Run();