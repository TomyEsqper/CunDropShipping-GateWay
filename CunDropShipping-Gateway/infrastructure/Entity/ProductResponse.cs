using System.Text.Json.Serialization;

namespace CunDropShipping_Gateway.infrastructure.Entity;

public class ProductResponse
{
    public int IdProduct { get; set; }
    public string? NameProduct { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int IdCategory { get; set; }
}