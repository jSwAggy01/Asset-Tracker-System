using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CountyAssetTracker.Data;
using CountyAssetTracker.Models;

namespace CountyAssetTracker.Pages.Assets;

public class CreateModel : PageModel
{
    private readonly DatabaseManager _db;

    [BindProperty]
    public Asset Asset { get; set; } = new Asset();

    public IEnumerable<Location> Locations { get; set; } = new List<Location>();

    public CreateModel(DatabaseManager db)
    {
        _db = db;
    }

    public async Task OnGetAsync()
    {
        Locations = await _db.GetAllLocationsAsync();
        Asset.Status = "Active";
        Asset.PurchaseDate = DateTime.Today;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Locations = await _db.GetAllLocationsAsync();
            return Page();
        }

        await _db.AddAssetAsync(Asset);
        TempData["Success"] = $"Asset '{Asset.AssetName}' was added successfully!";
        return RedirectToPage("Index");
    }
}
