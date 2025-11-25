using System.Text.Json.Serialization;
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
    
    [JsonPropertyName("idCategory")]
    public int IdCategory { get; set; }
    
    // Alias comunes que podrían venir del API (no se serializan al responder)
    [JsonPropertyName("categoryId")]
    public int CategoryIdAlias
    {
        // getter solo para cumplir con el deserializador; no se usa
        get => IdCategory;
        set => IdCategory = value;
    }
    
    [JsonPropertyName("id_category")]
    public int IdCategorySnakeAlias
    {
        get => IdCategory;
        set => IdCategory = value;
    }
    
    // Variante en MAYÚSCULAS con guion bajo
    [JsonPropertyName("ID_CATEGORY")]
    public int IdCategoryUpperSnakeAlias
    {
        get => IdCategory;
        set => IdCategory = value;
    }
    
    // Si el Product API retorna el objeto Category, lo capturamos aquí
    // para poder propagarlo hacia el Dominio/DTO.
    [JsonPropertyName("category")]
    public CategoryResponse? Category { get; set; }
}