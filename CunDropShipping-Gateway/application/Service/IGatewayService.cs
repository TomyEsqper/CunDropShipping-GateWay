using CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity;
using CunDropShipping_Gateway.domain.Entity;
using CunDropShipping_Gateway.infrastructure.Entity;
using ProductResponse = CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity;

namespace CunDropShipping_Gateway.application.Service
{
    public interface IGatewayService
    {
        List<Product> GetAllProducts();
        Product GetProductById(int idProduct);
        Product SaveProduct(Product request);
        Product UpdateProduct(int idProduct, Product request);
        Product DeleteProduct(int idProduct);
        List<Product> SearchProductsByName(string searchTerm);
        List<Product> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);
        List<Product> GetProductsWithLowStock(int stockThreshold);
    }
}