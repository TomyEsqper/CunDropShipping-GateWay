using CunDropShipping_Gateway.application.Service;
using CunDropShipping_Gateway.domain.Entity; 
using CunDropShipping_Gateway.infrastructure.Clients; 
using CunDropShipping_Gateway.application.Common; 
using Product = CunDropShipping_Gateway.domain.Entity; 
using ProductResponse = CunDropShipping_Gateway.infrastructure.Entity.ProductResponse; 

namespace CunDropShipping_Gateway.domain
{
    public class ProductServiceImp : IProductService
    {
        private readonly IProductClient _client;
        private readonly IMapper<DomainProductEntity, ProductResponse> _mapper; 

        public ProductServiceImp(IProductClient productClient, IMapper<DomainProductEntity, ProductResponse> infraMapper)
        {
            _client = productClient;
            _mapper = infraMapper;
        }

        // ==============================================================\n
        // ❌ MAPPERS INTERNOS ELIMINADOS: MapToDomain y MapToInfra
        // ==============================================================\n

        public List<DomainProductEntity> GetAllProducts()
        {
            var infraProducts = _client.GetAllProducts();
            // ✅ Usamos el mapper genérico: ToDomainList
            return _mapper.ToDomainList(infraProducts);
        }

        public DomainProductEntity GetProductById(int idProduct)
        {
            var infraProduct = _client.GetProductById(idProduct);
            // ✅ Usamos el mapper genérico: ToDomain
            return _mapper.ToDomain(infraProduct);
        }

        public DomainProductEntity SaveProduct(DomainProductEntity domainRequest)
        {
            // 1. Domain -> Infra: Usamos el mapper genérico: ToEntity
            var infraRequest = _mapper.ToEntity(domainRequest);

            // 2. Llamamos al Cliente
            var infraResponse = _client.SaveProduct(infraRequest);

            // 3. Convertimos Infra -> Domain: Usamos el mapper genérico: ToDomain
            return _mapper.ToDomain(infraResponse);
        }

        public DomainProductEntity UpdateProduct(int idProduct, DomainProductEntity domainRequest)
        {
            var infraRequest = _mapper.ToEntity(domainRequest);

            var infraResponse = _client.UpdateProduct(idProduct, infraRequest);

            return _mapper.ToDomain(infraResponse);
        }

        public DomainProductEntity DeleteProduct(int idProduct)
        {
            var infraResponse = _client.DeleteProduct(idProduct);
            return _mapper.ToDomain(infraResponse);
        }

        public List<DomainProductEntity> SearchProductsByName(string searchTerm)
        {
            var infraProducts = _client.SearchProductsByName(searchTerm);
            return _mapper.ToDomainList(infraProducts);
        }

        public List<DomainProductEntity> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            var infraProducts = _client.GetProductsByPriceRange(minPrice, maxPrice);
            return _mapper.ToDomainList(infraProducts);
        }

        public List<DomainProductEntity> GetProductsWithLowStock(int stockThreshold)
        {
            var infraProducts = _client.GetProductsWithLowStock(stockThreshold);
            return _mapper.ToDomainList(infraProducts);
        }
    }
}