using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Core.Dto;

namespace Core.Import;

public static class ProductJsonImporter
{
    public static ImportResult<ProductDto> Load(string path)
    {
        var json = File.ReadAllText(path);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var items = JsonSerializer.Deserialize<List<ProductDto>>(json, options) ?? new List<ProductDto>();
        return new ImportResult<ProductDto>(items, new List<string>());
    }
}
