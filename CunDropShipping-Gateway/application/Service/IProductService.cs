using CunDropShipping_Gateway.domain.Entity;

namespace CunDropShipping_Gateway.application.Service;

public interface IProductService
{
    // [EDUCATIVO] Propagamos el cambio: La interfaz del servicio también debe devolver Task<T>.
    // El Controlador esperará a que el Servicio termine, y el Servicio esperará al Cliente.
    Task<List<DomainProductEntity>> GetAllProducts();
    
    Task<DomainProductEntity> GetProductById(int idProduct);
    
    Task<DomainProductEntity> SaveProduct(DomainProductEntity request);
    
    Task<DomainProductEntity> UpdateProduct(int idProduct, DomainProductEntity request);
    
    Task<DomainProductEntity> DeleteProduct(int idProduct);
    
    Task<List<DomainProductEntity>> SearchProductsByName(string searchTerm);
    
    Task<List<DomainProductEntity>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);
    
    Task<List<DomainProductEntity>> GetProductsWithLowStock(int stockThreshold);
}
