using CountyAssetTracker.Data;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:5000");

builder.Services.AddRazorPages();

var connectionString = "Data Source=CountyAssets.db";
var dbManager = new DatabaseManager(connectionString);
dbManager.InitializeDatabase();

builder.Services.AddSingleton(dbManager);

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Run();
