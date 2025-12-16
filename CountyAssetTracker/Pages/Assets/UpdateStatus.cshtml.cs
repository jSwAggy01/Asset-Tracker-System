using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CountyAssetTracker.Data;
using CountyAssetTracker.Models;

namespace CountyAssetTracker.Pages.Assets;

public class UpdateStatusModel : PageModel
{
    private readonly DatabaseManager _db;

    public Asset? Asset { get; set; }

    [BindProperty]
    public string NewStatus { get; set; } = string.Empty;

    public UpdateStatusModel(DatabaseManager db)
    {
        _db = db;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Asset = await _db.GetAssetByIdAsync(id);
        if (Asset == null)
        {
            return NotFound();
        }

        NewStatus = Asset.Status;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        await _db.UpdateAssetStatusAsync(id, NewStatus);
        
        var asset = await _db.GetAssetByIdAsync(id);
        TempData["Success"] = $"Asset '{asset?.AssetName}' status updated to '{NewStatus}'!";
        return RedirectToPage("Index");
    }
}
