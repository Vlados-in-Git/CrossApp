using System;
using System.IO;
using System.Linq;
using System.Text;
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

return 0;
