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
        private readonly IDomainValidatorService _validator;
        
        public ProductServiceImp(IProductClient productClient, 
                                 IMapper<DomainProductEntity, ProductResponse> infraMapper,
                                 ICategoryClient categoryClient,
                                 IMapper<DomainCategoryEntity, CategoryResponse> categoryInfraMapper,
                                 IDomainValidatorService validator)
        {
            _productClient = productClient;
            _mapper = infraMapper;
            _categoryClient = categoryClient;
            _categoryInfraMapper = categoryInfraMapper;
            _validator = validator;

        }
        
        // [EDUCATIVO] CAMBIO IMPORTANTE: Ahora este método es 'async Task<...>'
        // porque _categoryClient.GetCategoryById ahora devuelve una Task.
        public async Task<DomainProductEntity> EnrichProductWithCategory(DomainProductEntity product)
        {
            if (product == null || product.IdCategory <= 0) return product;

            try
            {
                // [EDUCATIVO] Usamos 'await' aquí. Ahora no bloqueamos mientras buscamos la categoría.
                var infraCategory = await _categoryClient.GetCategoryById(product.IdCategory);
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

        public async Task<List<DomainProductEntity>> GetAllProducts()    
        {
            var infraProducts = await _productClient.GetAllProducts();
            
            var domainProducts = _mapper.ToDomainList(infraProducts);
            
            // [EDUCATIVO] Como EnrichProductWithCategory ahora es async, Select normal no funciona bien con await.
            // Tenemos que esperar todas las tareas.
            // Usamos Task.WhenAll para lanzar todas las peticiones de enriquecimiento en paralelo.
            var tasks = domainProducts.Select(p => EnrichProductWithCategory(p));
            var results = await Task.WhenAll(tasks);
            
            return results.ToList();
        }

        public async Task<DomainProductEntity> GetProductById(int idProduct)
        {
            var infraProduct = await _productClient.GetProductById(idProduct);
            
            if (infraProduct == null) return null;

            var domainProduct = _mapper.ToDomain(infraProduct);

            // [EDUCATIVO] Usamos await aquí también
            return await EnrichProductWithCategory(domainProduct);
        }

        public async Task<DomainProductEntity> SaveProduct(DomainProductEntity domainRequest)
        {
            _validator.ValidateCategoryExists(domainRequest.IdCategory);
            
            var infraRequest = _mapper.ToEntity(domainRequest);

            var infraResponse = await _productClient.SaveProduct(infraRequest);

            return _mapper.ToDomain(infraResponse);
        }

        public async Task<DomainProductEntity> UpdateProduct(int idProduct, DomainProductEntity domainRequest)
        {
            _validator.ValidateCategoryExists(domainRequest.IdCategory);
            var infraRequest = _mapper.ToEntity(domainRequest);

            var infraResponse = await _productClient.UpdateProduct(idProduct, infraRequest);

            return _mapper.ToDomain(infraResponse);
        }

        public async Task<DomainProductEntity> DeleteProduct(int idProduct)
        {
            var infraResponse = await _productClient.DeleteProduct(idProduct);
            return _mapper.ToDomain(infraResponse);
        }

        public async Task<List<DomainProductEntity>> SearchProductsByName(string searchTerm)
        {
            var infraProducts = await _productClient.SearchProductsByName(searchTerm);
            var domainProducts = _mapper.ToDomainList(infraProducts);

            var tasks = domainProducts.Select(p => EnrichProductWithCategory(p));
            var results = await Task.WhenAll(tasks);
            
            return results.ToList();
        }

        public async Task<List<DomainProductEntity>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            var infraProducts = await _productClient.GetProductsByPriceRange(minPrice, maxPrice);
            var domainProducts = _mapper.ToDomainList(infraProducts);

            var tasks = domainProducts.Select(p => EnrichProductWithCategory(p));
            var results = await Task.WhenAll(tasks);
            
            return results.ToList();
        }

        public async Task<List<DomainProductEntity>> GetProductsWithLowStock(int stockThreshold)
        {
            var infraProducts = await _productClient.GetProductsWithLowStock(stockThreshold);
            var domainProducts = _mapper.ToDomainList(infraProducts);
           
            var tasks = domainProducts.Select(p => EnrichProductWithCategory(p));
            var results = await Task.WhenAll(tasks);
            
            return results.ToList();
        }
    }
}
