using CountyAssetTracker.Data;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:5000");

var keysDirectory = Path.Combine(Directory.GetCurrentDirectory(), "DataProtection-Keys");
Directory.CreateDirectory(keysDirectory);

builder.Services.AddDataProtection()
    .SetApplicationName("CountyAssetTracker")
    .PersistKeysToFileSystem(new DirectoryInfo(keysDirectory));

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
