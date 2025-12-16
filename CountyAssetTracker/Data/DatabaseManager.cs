using Microsoft.Data.Sqlite;
using Dapper;
using CountyAssetTracker.Models;

namespace CountyAssetTracker.Data;

public class DatabaseManager
{
    private readonly string _connectionString;

    public DatabaseManager(string connectionString)
    {
        _connectionString = connectionString;
    }

    private SqliteConnection GetConnection()
    {
        return new SqliteConnection(_connectionString);
    }

    public void InitializeDatabase()
    {
        using var connection = GetConnection();
        connection.Open();

        var schemaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "schema.sql");
        
        if (!File.Exists(schemaPath))
        {
            schemaPath = Path.Combine(Directory.GetCurrentDirectory(), "Database", "schema.sql");
        }

        if (File.Exists(schemaPath))
        {
            var schema = File.ReadAllText(schemaPath);
            connection.Execute(schema);
        }
        else
        {
            CreateTablesManually(connection);
        }
    }

    private void CreateTablesManually(SqliteConnection connection)
    {
        var createLocations = @"
            CREATE TABLE IF NOT EXISTS Locations (
                LocationID INTEGER PRIMARY KEY AUTOINCREMENT,
                LocationName TEXT NOT NULL,
                Building TEXT NOT NULL
            );";

        var createAssets = @"
            CREATE TABLE IF NOT EXISTS Assets (
                AssetID INTEGER PRIMARY KEY AUTOINCREMENT,
                AssetName TEXT NOT NULL,
                SerialNumber TEXT NOT NULL UNIQUE,
                PurchaseDate TEXT NOT NULL,
                Status TEXT NOT NULL,
                LocationID INTEGER NOT NULL,
                FOREIGN KEY (LocationID) REFERENCES Locations(LocationID)
            );";

        var createEmployees = @"
            CREATE TABLE IF NOT EXISTS Employees (
                EmployeeID INTEGER PRIMARY KEY AUTOINCREMENT,
                FirstName TEXT NOT NULL,
                LastName TEXT NOT NULL,
                AssetID INTEGER,
                FOREIGN KEY (AssetID) REFERENCES Assets(AssetID)
            );";

        connection.Execute(createLocations);
        connection.Execute(createAssets);
        connection.Execute(createEmployees);

        var insertLocations = @"
            INSERT OR IGNORE INTO Locations (LocationID, LocationName, Building) VALUES
                (1, 'IT Department', 'Building A'),
                (2, 'Human Resources', 'Building B'),
                (3, 'Finance Office', 'Building A'),
                (4, 'Records Department', 'Building C'),
                (5, 'Public Services', 'Building D');";

        connection.Execute(insertLocations);
    }

    public async Task<IEnumerable<Asset>> GetAllAssetsAsync()
    {
        using var connection = GetConnection();
        
        const string sql = @"
            SELECT 
                a.AssetID,
                a.AssetName,
                a.SerialNumber,
                a.PurchaseDate,
                a.Status,
                a.LocationID,
                l.LocationName,
                l.Building
            FROM Assets a
            INNER JOIN Locations l ON a.LocationID = l.LocationID
            ORDER BY a.AssetID";

        return await connection.QueryAsync<Asset>(sql);
    }

    public async Task<Asset?> GetAssetByIdAsync(int assetId)
    {
        using var connection = GetConnection();
        
        const string sql = @"
            SELECT 
                a.AssetID,
                a.AssetName,
                a.SerialNumber,
                a.PurchaseDate,
                a.Status,
                a.LocationID,
                l.LocationName,
                l.Building
            FROM Assets a
            INNER JOIN Locations l ON a.LocationID = l.LocationID
            WHERE a.AssetID = @AssetID";

        return await connection.QueryFirstOrDefaultAsync<Asset>(sql, new { AssetID = assetId });
    }

    public async Task<int> AddAssetAsync(Asset asset)
    {
        using var connection = GetConnection();
        
        const string sql = @"
            INSERT INTO Assets (AssetName, SerialNumber, PurchaseDate, Status, LocationID)
            VALUES (@AssetName, @SerialNumber, @PurchaseDate, @Status, @LocationID);
            SELECT last_insert_rowid();";

        var parameters = new
        {
            asset.AssetName,
            asset.SerialNumber,
            PurchaseDate = asset.PurchaseDate.ToString("yyyy-MM-dd"),
            asset.Status,
            asset.LocationID
        };

        return await connection.ExecuteScalarAsync<int>(sql, parameters);
    }

    public async Task<bool> UpdateAssetStatusAsync(int assetId, string newStatus)
    {
        using var connection = GetConnection();
        
        const string sql = @"
            UPDATE Assets 
            SET Status = @Status 
            WHERE AssetID = @AssetID";

        var rowsAffected = await connection.ExecuteAsync(sql, new { AssetID = assetId, Status = newStatus });
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateAssetAsync(Asset asset)
    {
        using var connection = GetConnection();
        
        const string sql = @"
            UPDATE Assets 
            SET AssetName = @AssetName,
                SerialNumber = @SerialNumber,
                PurchaseDate = @PurchaseDate,
                Status = @Status,
                LocationID = @LocationID
            WHERE AssetID = @AssetID";

        var parameters = new
        {
            asset.AssetID,
            asset.AssetName,
            asset.SerialNumber,
            PurchaseDate = asset.PurchaseDate.ToString("yyyy-MM-dd"),
            asset.Status,
            asset.LocationID
        };

        var rowsAffected = await connection.ExecuteAsync(sql, parameters);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAssetAsync(int assetId)
    {
        using var connection = GetConnection();
        
        const string sql = "DELETE FROM Assets WHERE AssetID = @AssetID";
        var rowsAffected = await connection.ExecuteAsync(sql, new { AssetID = assetId });
        return rowsAffected > 0;
    }

    public async Task<IEnumerable<Location>> GetAllLocationsAsync()
    {
        using var connection = GetConnection();
        
        const string sql = "SELECT LocationID, LocationName, Building FROM Locations ORDER BY Building, LocationName";
        return await connection.QueryAsync<Location>(sql);
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        using var connection = GetConnection();
        
        const string sql = @"
            SELECT 
                e.EmployeeID,
                e.FirstName,
                e.LastName,
                e.AssetID,
                a.AssetName
            FROM Employees e
            LEFT JOIN Assets a ON e.AssetID = a.AssetID
            ORDER BY e.LastName, e.FirstName";

        return await connection.QueryAsync<Employee>(sql);
    }
}
