namespace CunDropShipping_Gateway.infrastructure.Entity;

// DTO de entrada para crear/actualizar productos hacia ProductApi
public class ProductRequest
{
    public int IdProduct { get; set; }
    public string nameProduct { get; set; }
    public string Description { get; set; }
    public decimal price { get; set; }
    public int stockQuantity { get; set; }
}
