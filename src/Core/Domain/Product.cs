using Core.Dto;

namespace Core.Domain;

public sealed class Product
{
    private int _quantity;

    public string Id { get; }
    public string Sku { get; }
    public string Name { get; }
    public string Unit { get; }
    public string? Note { get; }
    public int Quantity => _quantity;

    public static Product Create(string id, string sku, string name, string unit, int quantity, string? note = null)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new System.ArgumentException("Product Id cannot be empty or whitespace.", nameof(id));
        if (string.IsNullOrWhiteSpace(sku))
            throw new System.ArgumentException("Product SKU cannot be empty or whitespace.", nameof(sku));
        if (string.IsNullOrWhiteSpace(name))
            throw new System.ArgumentException("Product Name cannot be empty or whitespace.", nameof(name));
        if (string.IsNullOrWhiteSpace(unit))
            throw new System.ArgumentException("Product Unit cannot be empty or whitespace.", nameof(unit));
        if (quantity < 0)
            throw new System.ArgumentOutOfRangeException(nameof(quantity), quantity, $"Initial quantity cannot be negative. Provided: {quantity}");

        return new Product(id, sku, name, unit, quantity, note);
    }

    public void RegisterArrival(int amount)
    {
        if (amount <= 0)
            throw new System.ArgumentOutOfRangeException(nameof(amount), amount, $"Arrival amount must be greater than zero. Provided: {amount}");

        _quantity += amount;
    }

    public void Issue(int amount)
    {
        if (amount <= 0)
            throw new System.ArgumentOutOfRangeException(nameof(amount), amount, $"Issue amount must be greater than zero. Provided: {amount}");
            
        if (_quantity < amount)
            throw new System.InvalidOperationException($"Cannot issue {amount} units. Current stock for {Sku} is {_quantity}.");

        _quantity -= amount;
    }

    public ProductDto ToDto() => new(Id, Sku, Name, Unit, Quantity, Note);

    public static Product FromDto(ProductDto dto) => Create(dto.Id, dto.Sku, dto.Name, dto.Unit, dto.Quantity, dto.Note);

    private Product(string id, string sku, string name, string unit, int quantity, string? note)
    {
        Id = id;
        Sku = sku;
        Name = name;
        Unit = unit;
        _quantity = quantity;
        Note = note;
    }
}
