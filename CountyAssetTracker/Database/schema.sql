-- County Asset Tracker Database Schema
-- Designed for SQLite but structured for easy migration to MS SQL Server
-- Demonstrates proper normalization and relational database design

-- Locations Table: Stores physical locations where assets can be assigned
-- Primary Key: LocationID
CREATE TABLE IF NOT EXISTS Locations (
    LocationID INTEGER PRIMARY KEY AUTOINCREMENT,
    LocationName TEXT NOT NULL,
    Building TEXT NOT NULL
);

-- Assets Table: Core table storing all tracked county assets
-- Primary Key: AssetID
-- Foreign Key: LocationID references Locations(LocationID)
CREATE TABLE IF NOT EXISTS Assets (
    AssetID INTEGER PRIMARY KEY AUTOINCREMENT,
    AssetName TEXT NOT NULL,
    SerialNumber TEXT NOT NULL UNIQUE,
    PurchaseDate TEXT NOT NULL,
    Status TEXT NOT NULL CHECK (Status IN ('Active', 'Inactive', 'Maintenance', 'Disposed')),
    LocationID INTEGER NOT NULL,
    FOREIGN KEY (LocationID) REFERENCES Locations(LocationID)
);

-- Employees Table: Stores employee/custodian information with asset assignments
-- Primary Key: EmployeeID
-- Foreign Key: AssetID references Assets(AssetID)
CREATE TABLE IF NOT EXISTS Employees (
    EmployeeID INTEGER PRIMARY KEY AUTOINCREMENT,
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL,
    AssetID INTEGER,
    FOREIGN KEY (AssetID) REFERENCES Assets(AssetID)
);

-- Insert sample location data
INSERT OR IGNORE INTO Locations (LocationID, LocationName, Building) VALUES
    (1, 'IT Department', 'Building A'),
    (2, 'Human Resources', 'Building B'),
    (3, 'Finance Office', 'Building A'),
    (4, 'Records Department', 'Building C'),
    (5, 'Public Services', 'Building D');

-- Insert sample asset data
INSERT OR IGNORE INTO Assets (AssetID, AssetName, SerialNumber, PurchaseDate, Status, LocationID) VALUES
    (1, 'Dell Laptop XPS 15', 'DL-2023-001', '2023-01-15', 'Active', 1),
    (2, 'HP Desktop Pro', 'HP-2022-042', '2022-06-20', 'Active', 2),
    (3, 'Cisco Network Switch', 'CS-2021-015', '2021-03-10', 'Maintenance', 1),
    (4, 'Canon Printer MX920', 'CP-2023-008', '2023-04-01', 'Active', 3),
    (5, 'Microsoft Surface Pro', 'MS-2024-001', '2024-01-10', 'Active', 4);

-- Insert sample employee data
INSERT OR IGNORE INTO Employees (EmployeeID, FirstName, LastName, AssetID) VALUES
    (1, 'John', 'Smith', 1),
    (2, 'Sarah', 'Johnson', 2),
    (3, 'Michael', 'Williams', NULL),
    (4, 'Emily', 'Brown', 4),
    (5, 'David', 'Davis', 5);
