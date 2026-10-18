# CrossApp
End-to-end project for cross-platform programming.

## Domain
**Warehouse**
- **Purpose**: Inventory accounting of products in warehouses by batches and movement control.
- **Core Entities**: `Product`, `StockBatch`, `Warehouse`, `Movement`.

## Run Instructions
Build the project:
```bash
dotnet build
```
Run the console client:
```bash
dotnet run --project src/Cli
```
Run in JSON output mode:
```bash
dotnet run --project src/Cli -- --json
```

## Environment
- **.NET SDK**: 10.0.11 (Runtime) / 10.0.400 (SDK)
- **OS**: Windows 10 x64 (Microsoft Windows 10.0.26200)

## Project Architecture (Laboratory Work 2)
- `src/Core`: Class library containing domain logic and system information collection.
- `src/Cli`: Console client responsible only for output and formatting.

## Publication Modes Analysis (FDD vs SCD)
| Publication Mode | RID | Size (MB) | Runtime Autonomy |
| :--- | :--- | :--- | :--- |
| Framework-dependent (FDD) | N/A (Portable IL) | ~0.2 MB | Requires installed .NET Runtime |
| Self-contained (SCD) | win-x64 | ~77 MB | Included in the package |
| Self-contained (SCD) | linux-x64 | ~79 MB | Included in the package |
| Native AOT | win-x64 | ~10.3 MB | Native executable (no CLR required) |

### Conclusions
- **FDD** generates a compact artifact but requires the presence of a corresponding .NET SDK/Runtime on the target machine.
- **SCD** creates a fully autonomous package for a specific OS/architecture, but significantly increases the file volume due to packaging CLR and BCL.

## Laboratory Work 3: Importer & CSV

### Input Data Format
- **Format**: CSV
- **Delimiter**: `;`
- **Encoding**: UTF-8
- **Header**: Required (`id;sku;name;unit;quantity`)

### DTO Design Decisions
`record` was chosen instead of `class` for the DTO (`ProductDto`) because:
- **Immutability**: Data transfer objects should not change state after creation.
- **Value Equality**: Records provide built-in value-based equality.

### Pattern Matching in CSV Parsing
The `ParseLine` method leverages C# `switch` expression pattern matching:
1. **Property Pattern**: `{ Length: < 5 }` ensures the row has enough columns.
2. **List Pattern & Constants**: `[_, "", _, _, _]` catches empty mandatory fields.
3. **Guard Clause (`when`)**: `when !int.TryParse(...) || q < 0` safely handles numerical validation without exceptions.
4. **List Pattern with Variable Binding**: `[var id, ...]` extracts all values elegantly when perfectly formed.

## Domain Model and Invariants (Laboratory Work 4)

| Rule (Invariant) | Exception Type | Verification Method | Error Message |
| :--- | :--- | :--- | :--- |
| SKU cannot be empty or whitespace | `ArgumentException` | `Product.Create` | "SKU cannot be empty or whitespace." |
| Initial stock $\ge 0$ | `ArgumentOutOfRangeException` | `Product.Create` | "Initial quantity cannot be negative." |
| Issuing more than available stock is not allowed | `InvalidOperationException` | `Product.Issue` | "Cannot issue {amount}: stock for {Sku} = {_quantity}" |
| Operation amount (arrival / issue) > 0 | `ArgumentOutOfRangeException` | `Product.RegisterArrival`, `Product.Issue` | "... amount must be greater than zero." |
| Id, Name, Unit cannot be empty or whitespace | `ArgumentException` | `Product.Create` | "Product ... cannot be empty or whitespace." |

## Laboratory Work 5: Service Layer & Manual DI

### Overview
Laboratory Work 5 introduces a service layer and storage abstraction using pure Constructor Injection and manual Dependency Injection, eliminating any hard dependencies on third-party DI containers.

### Architecture Components
| Component | Location | Role |
| :--- | :--- | :--- |
| `ICatalogStore` | `src/Core/Abstractions/ICatalogStore.cs` | Storage abstraction interface contract (`List`, `GetById`, `Add`, `Update`, `Remove`). |
| `InMemoryCatalogStore` | `src/Core/Storage/InMemoryCatalogStore.cs` | In-memory dictionary-backed store with duplicate key protection and seed support. |
| `FileCatalogStore` | `src/Core/Storage/FileCatalogStore.cs` | JSON-backed store with lazy loading, directory auto-creation, auto-flush, and `ProductDto` serialization. |
| `CatalogService` | `src/Core/Services/CatalogService.cs` | Domain orchestrator service decoupled from storage implementations via Constructor Injection. |
| `SampleData` | `src/Core/SampleData.cs` | Static seed provider containing warehouse domain products (zero I/O dependencies). |
| `Program` | `src/Cli/Program.cs` | Composition Root and CLI scenario runner demonstrating storage switching, operations, and error handling. |

### Dependency Injection Design
- **No DI container**: Neither `Microsoft.Extensions.DependencyInjection` nor other external containers are used.
- **Composition Root**: `Program.cs` is the only place in the entire solution where concrete store implementations (`InMemoryCatalogStore` or `FileCatalogStore`) are instantiated.
- **Pure Constructor Injection**: `CatalogService` receives `ICatalogStore` via its constructor.

### Run Instructions
Run in **In-Memory mode** (pre-populated with seed data, non-persistent):
```bash
dotnet run --project src/Cli
```

Run in **File mode** (persists state to `data/catalog.json`):
```bash
dotnet run --project src/Cli -- --file
```

