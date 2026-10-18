using Core.Domain;
using System.Collections.Generic;

namespace Core;

public static class SampleData
{
    public static IReadOnlyList<Product> Products { get; } = BuildProducts();

    private static IReadOnlyList<Product> BuildProducts()
    {
        return new List<Product>
        {
            Product.Create("seed-01", "SKU-CEM-01", "Portland Cement M500", "bag", 250, "50kg bag"),
            Product.Create("seed-02", "SKU-BRK-01", "Red Ceramic Brick", "pcs", 4500, "Standard 250x120x65mm"),
            Product.Create("seed-03", "SKU-BRK-02", "Silicate White Brick", "pcs", 3000),
            Product.Create("seed-04", "SKU-RBR-01", "Steel Rebar 12mm", "m", 1800, "Class A500C"),
            Product.Create("seed-05", "SKU-RBR-02", "Steel Rebar 16mm", "m", 1200, "Class A500C"),
            Product.Create("seed-06", "SKU-SND-01", "River Quartz Sand", "m3", 85, "Bulk storage"),
            Product.Create("seed-07", "SKU-GRV-01", "Crushed Granite Gravel 5-20mm", "m3", 60),
            Product.Create("seed-08", "SKU-LMB-01", "Pine Lumber 50x150x6000mm", "pcs", 350, "Dry timber"),
            Product.Create("seed-09", "SKU-LMB-02", "Pine Wooden Beam 100x100mm", "pcs", 180),
            Product.Create("seed-10", "SKU-DRW-01", "Gypsum Plasterboard 12.5mm", "m2", 900, "Moisture resistant"),
            Product.Create("seed-11", "SKU-INS-01", "Mineral Wool Insulation 50mm", "m2", 640),
            Product.Create("seed-12", "SKU-PIP-01", "PVC Drain Pipe 110mm", "m", 240, "3m segments"),
            Product.Create("seed-13", "SKU-PIP-02", "Polypropylene Pipe 25mm", "m", 500, "Cold/Hot water"),
            Product.Create("seed-14", "SKU-ADH-01", "Tile Adhesive Ceresit CM-11", "bag", 140, "25kg bag"),
            Product.Create("seed-15", "SKU-PNT-01", "Exterior Acrylic Paint White", "kg", 320, "14kg buckets"),
            Product.Create("seed-16", "SKU-SLT-01", "Roofing Slate Shingle", "pcs", 0, "Out of stock item")
        }.AsReadOnly();
    }
}
