namespace CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity
{
    public class ProductDto
    {
        // Usamos tus nombres de variables tal cual me los pasaste
        public int IdProduct { get; set; }
        public string? NameProduct { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        
        public int IdCategory { get; set; }
        public CategoryDto? Category { get; set; }
    }
}