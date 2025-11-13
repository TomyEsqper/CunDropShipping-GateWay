using CunDropShipping_Gateway.infrastructure.Entity;

namespace CunDropShipping_Gateway.infrastructure.Clients;

public interface IProductClient
{
    List<ProductResponse> GetAllProducts();
    ProductResponse GetProductById(int idProduct);
    ProductResponse SaveProduct(ProductRequest request);
    ProductResponse UpdateProduct(int idProduct, ProductRequest request);
    ProductResponse DeleteProduct(int idProduct);
    List<ProductResponse> SearchProductsByName(string searchTerm);
    List<ProductResponse> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);
    List<ProductResponse> GetProductsWithLowStock(int stockThreshold);
}