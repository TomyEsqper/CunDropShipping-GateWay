using CunDropShipping_Gateway.infrastructure.Entity; // Único using necesario

namespace CunDropShipping_Gateway.infrastructure.Clients
{
    public interface IProductClient
    {
        List<ProductResponse> GetAllProducts();
        ProductResponse GetProductById(int idProduct);
        ProductResponse SaveProduct(ProductResponse product);
        ProductResponse UpdateProduct(int idProduct, ProductResponse product);
        ProductResponse DeleteProduct(int idProduct);
        List<ProductResponse> SearchProductsByName(string searchTerm);
        List<ProductResponse> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);
        List<ProductResponse> GetProductsWithLowStock(int stockThreshold);
    }
}