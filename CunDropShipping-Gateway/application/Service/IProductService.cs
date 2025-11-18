using CunDropShipping_Gateway.domain.Entity;

namespace CunDropShipping_Gateway.application.Service;

public interface IProductService
{
    List<DomainProductEntity> GetAllProducts();
    DomainProductEntity GetProductById(int idProduct);
    DomainProductEntity SaveProduct(DomainProductEntity request);
    DomainProductEntity UpdateProduct(int idProduct, DomainProductEntity request);
    DomainProductEntity DeleteProduct(int idProduct);
    List<DomainProductEntity> SearchProductsByName(string searchTerm);
    List<DomainProductEntity> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);
    List<DomainProductEntity> GetProductsWithLowStock(int stockThreshold);
}