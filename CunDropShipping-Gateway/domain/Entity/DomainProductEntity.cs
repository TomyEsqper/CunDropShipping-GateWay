namespace CunDropShipping_Gateway.domain.Entity
{
    // Esta es tu entidad pura de negocio.
    // El Service la produce y el Controller la consume.
    public class DomainProductEntity
    {
        public int IdProduct { get; set; } // OJO: Unificamos nombres aquí
        public required string NameProduct { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        
        public int IdCategory { get; set; }
        public DomainCategoryEntity? Category { get; set; }
    }
}