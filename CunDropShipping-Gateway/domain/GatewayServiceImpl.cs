using CunDropShipping_Gateway.application.Service;
using CunDropShipping_Gateway.domain.Entity; // Entidad de Dominio (Product)
using CunDropShipping_Gateway.infrastructure.Clients; // El Cliente

// Alias para Infraestructura (para no confundir con el Product de Dominio)
using InfraEntity = CunDropShipping_Gateway.infrastructure.Entity; 

namespace CunDropShipping_Gateway.domain
{
    public class GatewayServiceImp : IGatewayService
    {
        private readonly IProductClient _productClient;

        public GatewayServiceImp(IProductClient productClient)
        {
            _productClient = productClient;
        }

        // ==========================================
        // 🛠️ MAPPERS (Traductores Internos)
        // ==========================================

        // 1. De INFRAESTRUCTURA a DOMINIO (Cuando recibimos datos de la API)
        private Product MapToDomain(InfraEntity.ProductResponse infraProduct)
        {
            if (infraProduct == null) return null;

            return new Product
            {
                IdProduct = infraProduct.IdProduct,
                NameProduct = infraProduct.NameProduct,
                Description = infraProduct.Description,
                Price = infraProduct.Price,
                StockQuantity = infraProduct.StockQuantity
            };
        }

        // 2. De DOMINIO a INFRAESTRUCTURA (Cuando vamos a enviar datos a la API)
        // Este es el que te faltaba para que Save y Update funcionen
        private InfraEntity.ProductResponse MapToInfra(Product domainProduct)
        {
            if (domainProduct == null) return null;

            return new InfraEntity.ProductResponse
            {
                IdProduct = domainProduct.IdProduct,
                NameProduct = domainProduct.NameProduct,
                Description = domainProduct.Description,
                Price = domainProduct.Price,
                StockQuantity = domainProduct.StockQuantity
            };
        }

        // ==========================================
        // 🚀 MÉTODOS PÚBLICOS
        // ==========================================

        public List<Product> GetAllProducts()
        {
            var infraProducts = _productClient.GetAllProducts();
            return infraProducts.Select(p => MapToDomain(p)).ToList();
        }

        public Product GetProductById(int idProduct)
        {
            var infraProduct = _productClient.GetProductById(idProduct);
            return MapToDomain(infraProduct);
        }

        public Product SaveProduct(Product domainRequest)
        {
            // 1. Convertimos Dominio -> Infraestructura
            var infraRequest = MapToInfra(domainRequest);

            // 2. Llamamos al Cliente con el objeto de infraestructura
            var infraResponse = _productClient.SaveProduct(infraRequest);

            // 3. Convertimos la respuesta Infraestructura -> Dominio
            return MapToDomain(infraResponse);
        }

        public Product UpdateProduct(int idProduct, Product domainRequest)
        {
            // 1. Convertimos Dominio -> Infraestructura
            var infraRequest = MapToInfra(domainRequest);

            // 2. Llamamos al Cliente
            var infraResponse = _productClient.UpdateProduct(idProduct, infraRequest);

            // 3. Retornamos Dominio
            return MapToDomain(infraResponse);
        }

        public Product DeleteProduct(int idProduct)
        {
            var infraResponse = _productClient.DeleteProduct(idProduct);
            return MapToDomain(infraResponse);
        }

        public List<Product> SearchProductsByName(string searchTerm)
        {
            var infraProducts = _productClient.SearchProductsByName(searchTerm);
            return infraProducts.Select(p => MapToDomain(p)).ToList();
        }

        public List<Product> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            var infraProducts = _productClient.GetProductsByPriceRange(minPrice, maxPrice);
            return infraProducts.Select(p => MapToDomain(p)).ToList();
        }

        public List<Product> GetProductsWithLowStock(int stockThreshold)
        {
            var infraProducts = _productClient.GetProductsWithLowStock(stockThreshold);
            return infraProducts.Select(p => MapToDomain(p)).ToList();
        }
    }
}