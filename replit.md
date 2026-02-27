# County Asset Tracker System

## Overview
An Internal Asset Tracking System demonstrating proficiency in C#/.NET and SQL/Database Management for San Bernardino County IT. Built with ASP.NET Core 8.0 and SQLite using ADO.NET/Dapper for data access.

## Current State
- Fully functional web-based asset tracking system
- Dashboard with asset statistics
- CRUD operations for assets (Create, Read, Update)
- View locations and employees
- Parameterized SQL queries for security

## Project Architecture

```
CountyAssetTracker/
├── Data/
│   └── DatabaseManager.cs     # Data access layer with Dapper
├── Database/
│   └── schema.sql             # SQL schema with CREATE TABLE statements
├── Models/
│   ├── Asset.cs               # Asset entity model
│   ├── Employee.cs            # Employee entity model
│   └── Location.cs            # Location entity model
├── Pages/
│   ├── Assets/                # Asset management pages
│   │   ├── Create.cshtml      # Add new asset form
│   │   ├── Edit.cshtml        # Edit asset form
│   │   ├── Index.cshtml       # Asset list with JOIN query
│   │   └── UpdateStatus.cshtml # Status update form
│   ├── Employees/
│   │   └── Index.cshtml       # Employee list with LEFT JOIN
│   ├── Locations/
│   │   └── Index.cshtml       # Location list
│   ├── Shared/
│   │   └── _Layout.cshtml     # Main layout template
│   └── Index.cshtml           # Dashboard
├── wwwroot/
│   └── css/site.css           # Custom styles
├── Program.cs                 # Application entry point
└── README.md                  # Technical documentation
```

## Database Schema
Three normalized tables with proper foreign key relationships:
- **Assets**: AssetID (PK), AssetName, SerialNumber, PurchaseDate, Status, LocationID (FK)
- **Locations**: LocationID (PK), LocationName, Building
- **Employees**: EmployeeID (PK), FirstName, LastName, AssetID (FK)

## Key Technologies
- ASP.NET Core 8.0 with Razor Pages
- SQLite database (structured for MS SQL Server migration)
- Dapper for lightweight ORM
- Bootstrap 5 for UI styling
- Parameterized SQL queries (SQL injection prevention)

## Running the Application
The application runs on port 5000 with the workflow "County Asset Tracker".

## Recent Changes
- 2025-02-27: Updated README with comprehensive documentation, screenshots, and IDE instructions
- 2025-02-27: Fixed Recent Assets ordering by Purchase Date (newest first)
- 2025-02-27: Initial implementation with full CRUD operations
