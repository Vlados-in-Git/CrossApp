using System;
using System.IO;
using System.Linq;
using System.Text;
using Core.Domain;
using Core.Dto;
using Core.Import;

Console.OutputEncoding = Encoding.UTF8;

string path = args.Length > 0 ? args[0] : Path.Combine("data", "sample.csv");

if (!File.Exists(path))
{
    Console.WriteLine($"File not found: {Path.GetFullPath(path)}");
    return 1;
}

ImportResult<ProductDto> result;
string extension = Path.GetExtension(path).ToLowerInvariant();

result = extension switch
{
    ".json" => ProductJsonImporter.Load(path),
    _ => ProductCsvImporter.Load(path)
};

Console.WriteLine($"Loaded {result.Items.Count} items.");
foreach (var p in result.Items.Take(5))
{
    Console.WriteLine($" {p.Id,-6} {p.Sku,-10} {p.Name,-26} {p.Quantity,5} {p.Unit}");
}

Console.WriteLine($"Errors: {result.Errors.Count}");
foreach (var err in result.Errors)
{
    Console.WriteLine(err);
}

Console.WriteLine();
Console.WriteLine("--- Laboratory Work 4 ---");
Console.WriteLine("Scenario 1 (Success):");
var product = Product.Create("P-100", "SKU-999", "Test Product", "pcs", 100);
Console.WriteLine($"  Created product: {product.Sku} - {product.Name}, stock: {product.Quantity}");
product.RegisterArrival(50);
Console.WriteLine($"  After arrival of 50: {product.Quantity}");
product.Issue(30);
Console.WriteLine($"  After issuing 30: {product.Quantity}");
Console.WriteLine($"  Final state: {product.Quantity}");

Console.WriteLine();
Console.WriteLine("Scenario 2 (Invariant Violations):");
TryDo("Creation with empty ID", () => Product.Create(" ", "SKU-999", "Test", "pcs", 10));
TryDo("Creation with negative stock", () => Product.Create("P-101", "SKU-1000", "Test", "pcs", -5));
TryDo("Issuing more than available stock", () => product.Issue(500));

Console.WriteLine();
Console.WriteLine($"Integrity check: final stock remains {product.Quantity}");

return 0;

static void TryDo(string title, Action action)
{
    try
    {
        action();
        Console.WriteLine($"  {title}: Exception did NOT trigger — invariant is missing!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"  {title}: {ex.GetType().Name} — {ex.Message}");
    }
}
