namespace CunDropShipping_Gateway.domain.Entity
{
    // Esta es tu entidad pura de negocio.
    // El Service la produce y el Controller la consume.
    public class Product
    {
        public int IdProduct { get; set; } // OJO: Unificamos nombres aquí
        public string NameProduct { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        // Aquí puedes agregar lógica de negocio en el futuro
    }
}