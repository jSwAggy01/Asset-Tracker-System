using Microsoft.AspNetCore.Mvc.RazorPages;
using CountyAssetTracker.Data;
using CountyAssetTracker.Models;

namespace CountyAssetTracker.Pages.Locations;

public class IndexModel : PageModel
{
    private readonly DatabaseManager _db;

    public IEnumerable<Location> Locations { get; set; } = new List<Location>();

    public IndexModel(DatabaseManager db)
    {
        _db = db;
    }

    public async Task OnGetAsync()
    {
        Locations = await _db.GetAllLocationsAsync();
    }
}
