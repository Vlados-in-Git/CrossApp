using Core.Abstractions;
using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Services;

public sealed class CatalogService
{
    private readonly ICatalogStore _store;

    public CatalogService(ICatalogStore store)
    {
        _store = store ?? throw new ArgumentNullException(nameof(store));
    }

    public string Add(string sku, string name, string unit, int quantity, string? note = null)
    {
        var id = Guid.NewGuid().ToString("N")[..8];
        var product = Product.Create(id, sku, name, unit, quantity, note);
        _store.Add(product);
        return id;
    }

    public void Receive(string id, int quantity)
    {
        var product = _store.GetById(id)
            ?? throw new InvalidOperationException($"Product '{id}' not found.");

        product.RegisterArrival(quantity);
        _store.Update(product);
    }

    public void Issue(string id, int quantity)
    {
        var product = _store.GetById(id)
            ?? throw new InvalidOperationException($"Product '{id}' not found.");

        product.Issue(quantity);
        _store.Update(product);
    }

    public IReadOnlyList<Product> All(string? nameFilter = null)
    {
        var list = _store.List();
        return nameFilter is null
            ? list
            : list.Where(p => p.Name.Contains(nameFilter, StringComparison.OrdinalIgnoreCase))
                  .ToList()
                  .AsReadOnly();
    }

    public Product? Find(string id) => _store.GetById(id);
}
