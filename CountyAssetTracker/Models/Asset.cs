namespace CountyAssetTracker.Models;

public class Asset
{
    public int AssetID { get; set; }
    public string AssetName { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public int LocationID { get; set; }
    
    public string? LocationName { get; set; }
    public string? Building { get; set; }
}
