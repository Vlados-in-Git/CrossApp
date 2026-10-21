using Core.Abstractions;
using Core.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Core.Storage;

public sealed class InMemoryCatalogStore : ICatalogStore
{
    private readonly Dictionary<string, Product> _dict = new(StringComparer.OrdinalIgnoreCase);

    public InMemoryCatalogStore(IEnumerable<Product>? seed = null)
    {
        if (seed is not null)
        {
            foreach (var p in seed)
            {
                _dict[p.Id] = p;
            }
        }
    }

    public IReadOnlyList<Product> List() => _dict.Values.ToList().AsReadOnly();

    public Product? GetById(string id) => _dict.TryGetValue(id, out var p) ? p : null;

    public void Add(Product item)
    {
        if (_dict.ContainsKey(item.Id))
            throw new InvalidOperationException($"Product with Id '{item.Id}' already exists.");

        _dict[item.Id] = item;
    }

    public void Update(Product item)
    {
        if (!_dict.ContainsKey(item.Id))
            throw new InvalidOperationException($"Product with Id '{item.Id}' not found for update.");

        _dict[item.Id] = item;
    }

    public bool Remove(string id) => _dict.Remove(id);
}
