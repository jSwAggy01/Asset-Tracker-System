using Microsoft.AspNetCore.Mvc.RazorPages;
using CountyAssetTracker.Data;
using CountyAssetTracker.Models;

namespace CountyAssetTracker.Pages.Assets;

public class IndexModel : PageModel
{
    private readonly DatabaseManager _db;

    public IEnumerable<Asset> Assets { get; set; } = new List<Asset>();

    public IndexModel(DatabaseManager db)
    {
        _db = db;
    }

    public async Task OnGetAsync()
    {
        Assets = await _db.GetAllAssetsAsync();
    }
}
