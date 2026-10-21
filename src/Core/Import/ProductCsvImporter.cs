using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Core.Dto;

namespace Core.Import;

public static class ProductCsvImporter
{
    private const char Separator = ';';

    private abstract record ParseOutcome;
    private sealed record ParseOk(ProductDto Value) : ParseOutcome;
    private sealed record ParseFailed(string Reason) : ParseOutcome;

    private static ParseOutcome ParseLine(string line)
    {
        return line.Split(Separator, StringSplitOptions.TrimEntries) switch
        {
            { Length: < 5 } => new ParseFailed("Insufficient columns"),
            [_, "", _, _, _] or [_, _, "", _, _] => new ParseFailed("SKU or Name cannot be empty"),
            [_, _, _, _, var qty] when !int.TryParse(qty, CultureInfo.InvariantCulture, out int q) || q < 0 => new ParseFailed($"Invalid or negative quantity: {qty}"),
            [var id, var sku, var name, var unit, var qty] => new ParseOk(new ProductDto(id, sku, name, unit, int.Parse(qty, CultureInfo.InvariantCulture))),
            _ => new ParseFailed("Unknown format error")
        };
    }

    public static ImportResult<ProductDto> Load(string path)
    {
        var lines = File.ReadAllLines(path);
        var items = new List<ProductDto>();
        var errors = new List<string>();

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];

            if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith('#') || line.StartsWith("id", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            switch (ParseLine(line))
            {
                case ParseOk ok:
                    items.Add(ok.Value);
                    break;
                case ParseFailed failed:
                    errors.Add($"Row {i + 1}: {failed.Reason}");
                    break;
            }
        }

        return new ImportResult<ProductDto>(items, errors);
    }
}
