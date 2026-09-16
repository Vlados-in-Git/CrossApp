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

publish
```
dotnet publish .\src\Cli\ -c Release -r win-x64 --self-contained true
```

## Environment
- **.NET SDK**: 10.0.11 (Runtime) / 10.0.400 (SDK)
- **OS**: Windows 10 x64 (Microsoft Windows 10.0.26200)

## Analysis of Publish Modes (Self-Contained)
| RID | Configuration | Publish Directory Size |
| :--- | :--- | :--- |
| `win-x64` | Release (Self-contained) | \~77 MB |
| `linux-x64` | Release (Self-contained) | \~79 MB |
