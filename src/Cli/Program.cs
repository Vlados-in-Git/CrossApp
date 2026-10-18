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
Console.WriteLine("=== Scenario 1: Success ===");
var product = Product.Create("P-001", "SKU-001", "Cement M400 25kg", "pcs", 100);
Console.WriteLine($"{product.Id} [{product.Sku}] {product.Name} — {product.Quantity} {product.Unit}");
product.RegisterArrival(20);
Console.WriteLine($"{product.Id} [{product.Sku}] {product.Name} — {product.Quantity} {product.Unit}");

Console.WriteLine("=== Scenario 2: Invariant Violations ===");
TryDo("issue greater than stock", () => product.Issue(1000), product);
TryDo("issue negative amount", () => product.Issue(-50), product);
TryDo("empty SKU", () => Product.Create("P-002", "", "Cement M400 25kg", "pcs", 10), product);
TryDo("negative stock", () => Product.Create("P-003", "SKU-003", "Cement M400 25kg", "pcs", -5), product);

Console.WriteLine("Observed fact: In Scenario 1, state changes only via methods; in Scenario 2,");
Console.WriteLine($"no attempt modified the object — after all rejections, stock remains {product.Quantity} ({product}).");

return 0;

static void TryDo(string title, Action action, Product? targetEntity = null)
{
    try
    {
        action();
        Console.WriteLine($"{title}: Exception was NOT thrown — invariant missing!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"{title}: {ex.GetType().Name} — {ex.Message.TrimEnd()}");
        if (targetEntity != null)
        {
            Console.WriteLine($"  Entity state (unchanged): {targetEntity}");
        }
    }
}

