using Microsoft.AspNetCore.Mvc.RazorPages;
using CountyAssetTracker.Data;
using CountyAssetTracker.Models;

namespace CountyAssetTracker.Pages;

public class IndexModel : PageModel
{
    private readonly DatabaseManager _db;

    public int TotalAssets { get; set; }
    public int ActiveAssets { get; set; }
    public int MaintenanceAssets { get; set; }
    public int TotalLocations { get; set; }
    public int TotalEmployees { get; set; }
    public IEnumerable<Asset> RecentAssets { get; set; } = new List<Asset>();

    public IndexModel(DatabaseManager db)
    {
        _db = db;
    }

    public async Task OnGetAsync()
    {
        var allAssets = await _db.GetAllAssetsAsync();
        var assets = allAssets.ToList();
        
        TotalAssets = assets.Count;
        ActiveAssets = assets.Count(a => a.Status == "Active");
        MaintenanceAssets = assets.Count(a => a.Status == "Maintenance");
        
        var locations = await _db.GetAllLocationsAsync();
        TotalLocations = locations.Count();
        
        var employees = await _db.GetAllEmployeesAsync();
        TotalEmployees = employees.Count();
        
        RecentAssets = assets.Take(5);
    }
}
