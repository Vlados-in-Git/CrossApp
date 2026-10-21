using System;
using System.Collections.Generic;
using Core.Dto;

namespace Core.Domain;

public static class ImportResultExtensions
{
    public static ImportResult<Product> ToDomainResult(this ImportResult<ProductDto> dtoResult)
    {
        var items = new List<Product>();
        var errors = new List<string>(dtoResult.Errors);

        foreach (var dto in dtoResult.Items)
        {
            try
            {
                items.Add(Product.FromDto(dto));
            }
            catch (Exception ex)
            {
                errors.Add($"Row error for SKU '{dto.Sku}': {ex.Message}");
            }
        }

        return new ImportResult<Product>(items, errors);
    }
}
