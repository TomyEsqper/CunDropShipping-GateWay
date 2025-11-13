using CunDropShipping_Gateway.infrastructure.Entity;

namespace CunDropShipping_Gateway.application.Service
{
    public interface IGatewayService
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
}