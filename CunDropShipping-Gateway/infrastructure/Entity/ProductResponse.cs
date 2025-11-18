namespace CunDropShipping_Gateway.infrastructure.Entity;

// Esta calse es un "DTO" o "Contraro".
// Debe tener las mismas propiedades que el JSON que devuelve el ProductApi.
public class ProductResponse
{
    public int IdProduct { get; set; }
    public string? NameProduct { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}