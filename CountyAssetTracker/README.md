# County Asset Tracker System

An Internal Asset Tracking System demonstrating proficiency in C#/.NET and SQL/Database Management for San Bernardino County IT.

## Technology Stack

- **Framework**: ASP.NET Core 8.0 (C#)
- **Database**: SQLite with ADO.NET/Dapper
- **Frontend**: Razor Pages with Bootstrap 5
- **Data Access**: Parameterized SQL queries via Dapper

## Database Design

The database follows proper normalization principles with three related tables:

### Tables

#### 1. Locations Table
```sql
CREATE TABLE Locations (
    LocationID INTEGER PRIMARY KEY AUTOINCREMENT,
    LocationName TEXT NOT NULL,
    Building TEXT NOT NULL
);
```
Stores physical locations where assets can be assigned.

#### 2. Assets Table
```sql
CREATE TABLE Assets (
    AssetID INTEGER PRIMARY KEY AUTOINCREMENT,
    AssetName TEXT NOT NULL,
    SerialNumber TEXT NOT NULL UNIQUE,
    PurchaseDate TEXT NOT NULL,
    Status TEXT NOT NULL CHECK (Status IN ('Active', 'Inactive', 'Maintenance', 'Disposed')),
    LocationID INTEGER NOT NULL,
    FOREIGN KEY (LocationID) REFERENCES Locations(LocationID)
);
```
Core table storing all tracked county assets with a foreign key relationship to Locations.

#### 3. Employees Table
```sql
CREATE TABLE Employees (
    EmployeeID INTEGER PRIMARY KEY AUTOINCREMENT,
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL,
    AssetID INTEGER,
    FOREIGN KEY (AssetID) REFERENCES Assets(AssetID)
);
```
Stores employee/custodian information with optional asset assignments.

### Relationships
- **Assets → Locations**: Many-to-One (Each asset belongs to one location)
- **Employees → Assets**: Many-to-One (Each employee can be assigned one asset)

## C# Code Structure

### Data Access Layer (`Data/DatabaseManager.cs`)

The `DatabaseManager` class demonstrates:

1. **Connection Management**
   ```csharp
   private SqliteConnection GetConnection()
   {
       return new SqliteConnection(_connectionString);
   }
   ```

2. **Parameterized SQL Queries** (Prevents SQL Injection)
   ```csharp
   const string sql = @"
       INSERT INTO Assets (AssetName, SerialNumber, PurchaseDate, Status, LocationID)
       VALUES (@AssetName, @SerialNumber, @PurchaseDate, @Status, @LocationID);";
   
   await connection.ExecuteAsync(sql, parameters);
   ```

3. **JOIN Queries for Related Data**
   ```csharp
   const string sql = @"
       SELECT a.*, l.LocationName, l.Building
       FROM Assets a
       INNER JOIN Locations l ON a.LocationID = l.LocationID";
   ```

4. **Object Mapping** (Database rows to C# objects)
   ```csharp
   return await connection.QueryAsync<Asset>(sql);
   ```

### Model Classes (`Models/`)

- `Asset.cs` - Represents asset records with location data
- `Location.cs` - Represents physical locations
- `Employee.cs` - Represents employees/custodians

### CRUD Operations

| Operation | SQL | C# Method |
|-----------|-----|-----------|
| Create | INSERT INTO Assets... | `AddAssetAsync()` |
| Read | SELECT with JOIN | `GetAllAssetsAsync()` |
| Update | UPDATE Assets SET... | `UpdateAssetStatusAsync()` |

## Running the Application

1. The application runs on port 5000
2. Database is automatically initialized on startup
3. Sample data is inserted if tables are empty

## Key Features Demonstrated

- Proper foreign key relationships
- Parameterized queries preventing SQL injection
- Async/await patterns for database operations
- Object-relational mapping with Dapper
- Clean separation of concerns (Models, Data, Pages)
