# OpenRoutePlanner

**OpenRoutePlanner** is a modular and extensible .NET Aspire web application for planning and managing commercial trucking routes. It provides account, driver, tractor, and route management capabilities with a clean, mobile-friendly UI built using Blazor and Bootstrap.

## Features

* ✅ **Account Management**: View, create, and manage business accounts.
* 🚛 **Driver Profiles**: Assign drivers, track endorsements, and view contact info.
* 🚜 **Tractor Management**: Track tractor assignments, VINs, odometer, and license info.
* 🧭 **Route Planning**:

  * Assign drivers, tractors, accounts
  * Track route stops, mileage, and time windows
  * Manage trailers dynamically
  * Mark completion and endorsement requirements
* 🔍 **Search & Sort**: Built-in search and sortable views for quick access.
* 📱 **Mobile-Responsive UI**: All views use card-based Bootstrap layouts for optimal responsiveness.

## Technology Stack

* **.NET Aspire** (.NET 8+)
* **Blazor Server** with StreamRendering
* **Entity Framework Core** (with SQLite persistence)
* **Bootstrap 5** UI framework

## Getting Started

### Prerequisites

* [.NET SDK 8+](https://dotnet.microsoft.com/en-us/download)
* SQLite (or any other configured database provider)

### Run the app

```bash
# Run the Aspire dashboard (if used)
dotnet run --project OpenRoutePlanner.AppHost

# Or run the web + API manually
dotnet run --project OpenRoutePlanner.Web
```

### Database Migrations

If using EF Core migrations:

```bash
dotnet ef migrations add InitialCreate -p OpenRoutePlanner.Data -s OpenRoutePlanner.Web
```

## Folder Structure

```
OpenRoutePlanner/
├── Models/                 # Core entity models (RoutePlan, DriverProfile, etc.)
├── ModelManagers/         # Logic classes for manipulating models
├── Components/FormControls# Custom Blazor components
├── Web/                   # UI Project
├── ApiService/            # REST API project (if applicable)
├── Data/                  # EF Core DbContext & migrations
```

## Development Notes

* All data pages use `@rendermode InteractiveServer` with `[StreamRendering(true)]`
* Filtering and sorting is done via LINQ on `ImmutableArray<T>` collections
* Bootstrap cards are used instead of tables for better mobile layout
* Dynamic list binding is used for trailers (List<string>) on routes

## Future Enhancements

* 🗺️ Map integration for route visualization
* 📦 Load assignments and shipment tracking
* 📊 Reporting dashboard
* 🔐 Role-based authentication/authorization

## License

MIT License

---

Built with ❤️ for fleet logistics and planning by Austin Vandersluis
