namespace CountyAssetTracker.Models;

public class Employee
{
    public int EmployeeID { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? AssetID { get; set; }
    
    public string FullName => $"{FirstName} {LastName}";
    public string? AssetName { get; set; }
}
