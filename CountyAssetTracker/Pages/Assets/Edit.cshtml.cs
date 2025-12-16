using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CountyAssetTracker.Data;
using CountyAssetTracker.Models;

namespace CountyAssetTracker.Pages.Assets;

public class EditModel : PageModel
{
    private readonly DatabaseManager _db;

    [BindProperty]
    public Asset Asset { get; set; } = new Asset();

    public IEnumerable<Location> Locations { get; set; } = new List<Location>();

    public EditModel(DatabaseManager db)
    {
        _db = db;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var asset = await _db.GetAssetByIdAsync(id);
        if (asset == null)
        {
            return NotFound();
        }

        Asset = asset;
        Locations = await _db.GetAllLocationsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Locations = await _db.GetAllLocationsAsync();
            return Page();
        }

        await _db.UpdateAssetAsync(Asset);
        TempData["Success"] = $"Asset '{Asset.AssetName}' was updated successfully!";
        return RedirectToPage("Index");
    }
}
