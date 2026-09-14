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
