using Core;
using Core.Abstractions;
using Core.Domain;
using Core.Services;
using Core.Storage;
using System;
using System.IO;
using System.Linq;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

// Composition Root -- storage selection
var useFile = args.Contains("--file");
var dataPath = Path.GetFullPath(
    Path.Combine(AppContext.BaseDirectory,
        "..", "..", "..", "..", "..", "data", "catalog.json"));

ICatalogStore store = useFile
    ? new FileCatalogStore(dataPath)
    : new InMemoryCatalogStore(SampleData.Products);

var service = new CatalogService(store);

Console.WriteLine(useFile
    ? $"Storage: File ({dataPath})"
    : "Storage: In-Memory (seed data)");

// Scenario A -- List all
Console.WriteLine();
Console.WriteLine("=== A: All Products ===");
var allProducts = service.All();
Console.WriteLine($"Total products count: {allProducts.Count}");
foreach (var p in allProducts)
{
    Console.WriteLine($"  {p}");
}

// Scenario B -- Add new product
Console.WriteLine();
Console.WriteLine("=== B: Add New Product ===");
var newId = service.Add("SKU-NEW", "New Test Cement Bag", "bag", 50);
Console.WriteLine($"Added: {service.Find(newId)}");

// Scenario C -- Receive stock
Console.WriteLine();
Console.WriteLine("=== C: Receive Stock ===");
var targetId = service.All().First().Id;
service.Receive(targetId, 100);
Console.WriteLine($"After receive: {service.Find(targetId)}");

// Scenario D -- Issue stock
Console.WriteLine();
Console.WriteLine("=== D: Issue Stock ===");
service.Issue(targetId, 10);
Console.WriteLine($"After issue: {service.Find(targetId)}");

// Scenario E -- Error cases
Console.WriteLine();
Console.WriteLine("=== E: Error Scenarios ===");

// E1: Duplicate Add directly to store
try
{
    var dup = Product.Create("dup-01", "SKU-DUP", "Duplicate Test Product", "pcs", 10);
    if (store.GetById("dup-01") is null)
    {
        store.Add(dup);
    }
    store.Add(dup);
}
catch (Exception ex)
{
    Console.WriteLine($"  E1: {ex.GetType().Name} -- {ex.Message.TrimEnd()}");
}

// E2: Receive for non-existent product
try
{
    service.Receive("ghost-id-999", 5);
}
catch (Exception ex)
{
    Console.WriteLine($"  E2: {ex.GetType().Name} -- {ex.Message.TrimEnd()}");
}

// E3: Issue for non-existent product
try
{
    service.Issue("ghost-id-999", 5);
}
catch (Exception ex)
{
    Console.WriteLine($"  E3: {ex.GetType().Name} -- {ex.Message.TrimEnd()}");
}

// E4: Issue quantity greater than stock
try
{
    service.Issue(targetId, 999999);
}
catch (Exception ex)
{
    Console.WriteLine($"  E4: {ex.GetType().Name} -- {ex.Message.TrimEnd()}");
}
