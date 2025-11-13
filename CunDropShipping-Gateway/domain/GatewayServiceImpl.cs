using CunDropShipping_Gateway.application.Service;
using CunDropShipping_Gateway.infrastructure.Clients;
using CunDropShipping_Gateway.infrastructure.Entity;

namespace CunDropShipping_Gateway.domain
{
    // Asume que ya creaste IGatewayService en la capa 'application'
    public class GatewayServiceImp : IGatewayService
    {
        private readonly IProductClient _productClient;

        // Inyectamos el cliente que registramos en Program.cs
        public GatewayServiceImp(IProductClient productClient)
        {
            _productClient = productClient;
        }

        public List<ProductResponse> GetAllProducts()
        {
            // El servicio solo le pasa la orden al cliente
            return _productClient.GetAllProducts();
        }

        public ProductResponse GetProductById(int idProduct)
        {
            return _productClient.GetProductById(idProduct);
        }

        public ProductResponse SaveProduct(ProductRequest request)
        {
            return _productClient.SaveProduct(request);
        }

        public ProductResponse UpdateProduct(int idProduct, ProductRequest request)
        {
            return _productClient.UpdateProduct(idProduct, request);
        }

        public ProductResponse DeleteProduct(int idProduct)
        {
            return _productClient.DeleteProduct(idProduct);
        }

        public List<ProductResponse> SearchProductsByName(string searchTerm)
        {
            return _productClient.SearchProductsByName(searchTerm);
        }

        public List<ProductResponse> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return _productClient.GetProductsByPriceRange(minPrice, maxPrice);
        }

        public List<ProductResponse> GetProductsWithLowStock(int stockThreshold)
        {
            return _productClient.GetProductsWithLowStock(stockThreshold);
        }
    }
}