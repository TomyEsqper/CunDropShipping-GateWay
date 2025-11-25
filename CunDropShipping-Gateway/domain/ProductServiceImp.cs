using CunDropShipping_Gateway.application.Service;
using CunDropShipping_Gateway.domain.Entity; 
using CunDropShipping_Gateway.infrastructure.Clients; 
using CunDropShipping_Gateway.application.Common;
using CunDropShipping_Gateway.infrastructure.Entity;
using System.Linq;

namespace CunDropShipping_Gateway.domain
{
    public class ProductServiceImp : IProductService
    {
        private readonly IProductClient _productClient;
        private readonly IMapper<DomainProductEntity, ProductResponse> _mapper; 
        private readonly ICategoryClient _categoryClient;
        private readonly IMapper<DomainCategoryEntity, CategoryResponse> _categoryInfraMapper;
        
        public ProductServiceImp(IProductClient productClient, 
                                 IMapper<DomainProductEntity, ProductResponse> infraMapper,
                                 ICategoryClient categoryClient,
                                 IMapper<DomainCategoryEntity, CategoryResponse> categoryInfraMapper)
        {
            _productClient = productClient;
            _mapper = infraMapper;
            _categoryClient = categoryClient;
            _categoryInfraMapper = categoryInfraMapper;

        }
        

        public DomainProductEntity EnrichProductWithCategory(DomainProductEntity product)
        {
            if (product == null || product.IdCategory <= 0) return product;

            try
            {
                var infraCategory = _categoryClient.GetCategoryById(product.IdCategory);
                if (infraCategory != null)
                {
                    var domainCategory = _categoryInfraMapper.ToDomain(infraCategory);
                    product.Category = domainCategory;
                }
            }
            catch (Exception ex)
            {
                // En caso de fallo de red o 404/500, no rompemos el producto, solo dejamos la categoría nula.
            }
            return product;
        }
        public List<DomainProductEntity> GetAllProducts()    
        {
            var infraProducts = _productClient.GetAllProducts();
            var domainProducts = _mapper.ToDomainList(infraProducts);
            
            return domainProducts.Select(p => EnrichProductWithCategory(p)).ToList(); 
        }

        public DomainProductEntity GetProductById(int idProduct)
        {
            var infraProduct = _productClient.GetProductById(idProduct);
            if (infraProduct == null) return null;

            var domainProduct = _mapper.ToDomain(infraProduct);

            return EnrichProductWithCategory(domainProduct);
        }

        public DomainProductEntity SaveProduct(DomainProductEntity domainRequest)
        {
            // 1. Domain -> Infra: Usamos el mapper genérico: ToEntity
            var infraRequest = _mapper.ToEntity(domainRequest);

            // 2. Llamamos al Cliente
            var infraResponse = _productClient.SaveProduct(infraRequest);

            // 3. Convertimos Infra -> Domain: Usamos el mapper genérico: ToDomain
            return _mapper.ToDomain(infraResponse);
        }

        public DomainProductEntity UpdateProduct(int idProduct, DomainProductEntity domainRequest)
        {
            var infraRequest = _mapper.ToEntity(domainRequest);

            var infraResponse = _productClient.UpdateProduct(idProduct, infraRequest);

            return _mapper.ToDomain(infraResponse);
        }

        public DomainProductEntity DeleteProduct(int idProduct)
        {
            var infraResponse = _productClient.DeleteProduct(idProduct);
            return _mapper.ToDomain(infraResponse);
        }

        public List<DomainProductEntity> SearchProductsByName(string searchTerm)
        {
            var infraProducts = _productClient.SearchProductsByName(searchTerm);
            var domainProducts = _mapper.ToDomainList(infraProducts);

            return domainProducts.Select(p => EnrichProductWithCategory(p)).ToList();
        }

        public List<DomainProductEntity> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            var infraProducts = _productClient.GetProductsByPriceRange(minPrice, maxPrice);
            var domainProducts = _mapper.ToDomainList(infraProducts);

            return domainProducts.Select(p => EnrichProductWithCategory(p)).ToList();
        }

        public List<DomainProductEntity> GetProductsWithLowStock(int stockThreshold)
        {
            var infraProducts = _productClient.GetProductsWithLowStock(stockThreshold);
            var domainProducts = _mapper.ToDomainList(infraProducts);
           
            return domainProducts.Select(p => EnrichProductWithCategory(p)).ToList();
        }
    }
}