using Microsoft.AspNetCore.Mvc.RazorPages;
using CountyAssetTracker.Data;
using CountyAssetTracker.Models;

namespace CountyAssetTracker.Pages.Employees;

public class IndexModel : PageModel
{
    private readonly DatabaseManager _db;

    public IEnumerable<Employee> Employees { get; set; } = new List<Employee>();

    public IndexModel(DatabaseManager db)
    {
        _db = db;
    }

    public async Task OnGetAsync()
    {
        Employees = await _db.GetAllEmployeesAsync();
    }
}
