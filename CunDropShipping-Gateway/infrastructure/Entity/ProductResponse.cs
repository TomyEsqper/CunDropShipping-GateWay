namespace CunDropShipping_Gateway.infrastructure.Entity;

// Esta calse es un "DTO" o "Contraro".
// Debe tener las mismas propiedades que el JSON que devuelve el ProductApi.
public class ProductResponse
{
    public int IdProduct { get; set; }
    public string nameProduct { get; set; }
    public string Description { get; set; }
    public decimal price { get; set; }
    public int stockQuantity { get; set; }
}