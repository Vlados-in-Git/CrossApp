using Core.Abstractions;
using Core.Domain;
using Core.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Core.Storage;

public sealed class FileCatalogStore : ICatalogStore
{
    private readonly string _filePath;
    private Dictionary<string, Product>? _cache;
    private static readonly JsonSerializerOptions _jsonOpts = new() { WriteIndented = true };

    public FileCatalogStore(string filePath)
    {
        _filePath = Path.GetFullPath(filePath);
    }

    private void EnsureLoaded()
    {
        if (_cache is not null) return;

        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            var dtos = string.IsNullOrWhiteSpace(json)
                ? null
                : JsonSerializer.Deserialize<List<ProductDto>>(json);

            _cache = new(StringComparer.OrdinalIgnoreCase);
            if (dtos is not null)
            {
                foreach (var dto in dtos)
                {
                    _cache[dto.Id] = Product.FromDto(dto);
                }
            }
        }
        else
        {
            _cache = new(StringComparer.OrdinalIgnoreCase);
        }
    }

    private void Flush()
    {
        EnsureLoaded();
        var dtos = _cache!.Values.Select(p => p.ToDto()).ToList();
        var json = JsonSerializer.Serialize(dtos, _jsonOpts);
        File.WriteAllText(_filePath, json);
    }

    public IReadOnlyList<Product> List()
    {
        EnsureLoaded();
        return _cache!.Values.ToList().AsReadOnly();
    }

    public Product? GetById(string id)
    {
        EnsureLoaded();
        return _cache!.TryGetValue(id, out var p) ? p : null;
    }

    public void Add(Product item)
    {
        EnsureLoaded();
        if (_cache!.ContainsKey(item.Id))
            throw new InvalidOperationException($"Product with Id '{item.Id}' already exists.");

        _cache[item.Id] = item;
        Flush();
    }

    public void Update(Product item)
    {
        EnsureLoaded();
        if (!_cache!.ContainsKey(item.Id))
            throw new InvalidOperationException($"Product with Id '{item.Id}' not found for update.");

        _cache[item.Id] = item;
        Flush();
    }

    public bool Remove(string id)
    {
        EnsureLoaded();
        if (_cache!.Remove(id))
        {
            Flush();
            return true;
        }

        return false;
    }
}
