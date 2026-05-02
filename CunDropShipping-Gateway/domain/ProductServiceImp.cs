using CunDropShipping_Gateway.application.Service;
using CunDropShipping_Gateway.domain.Entity; 
using CunDropShipping_Gateway.infrastructure.Clients; 
using CunDropShipping_Gateway.application.Common;
using CunDropShipping_Gateway.infrastructure.Entity;
using System.Linq;
using Microsoft.Extensions.Caching.Memory; // [EDUCATIVO] Namespace necesario para usar IMemoryCache

namespace CunDropShipping_Gateway.domain
{
    public class ProductServiceImp : IProductService
    {
        private readonly IProductClient _productClient;
        private readonly IMapper<DomainProductEntity, ProductResponse> _mapper; 
        private readonly ICategoryClient _categoryClient;
        private readonly IMapper<DomainCategoryEntity, CategoryResponse> _categoryInfraMapper;
        private readonly IDomainValidatorService _validator;
        
        // [EDUCATIVO] Nueva dependencia: IMemoryCache
        private readonly IMemoryCache _cache;
        
        public ProductServiceImp(IProductClient productClient, 
                                 IMapper<DomainProductEntity, ProductResponse> infraMapper,
                                 ICategoryClient categoryClient,
                                 IMapper<DomainCategoryEntity, CategoryResponse> categoryInfraMapper,
                                 IDomainValidatorService validator,
                                 IMemoryCache cache) // [EDUCATIVO] Inyección por constructor
        {
            _productClient = productClient;
            _mapper = infraMapper;
            _categoryClient = categoryClient;
            _categoryInfraMapper = categoryInfraMapper;
            _validator = validator;
            _cache = cache;
        }
        
        public async Task<DomainProductEntity> EnrichProductWithCategory(DomainProductEntity product)
        {
            if (product == null || product.IdCategory <= 0) return product;

            // [EDUCATIVO] Optimización N+1 con Caché
            // Clave única para guardar/recuperar esta categoría específica
            var cacheKey = $"category_{product.IdCategory}";

            // Intentamos obtener del caché. Si no existe, ejecutamos la función (factory) para obtenerlo.
            var domainCategory = await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                // [EDUCATIVO] Configuración del caché
                // Expiración absoluta: En 10 minutos se borra sí o sí (para refrescar datos nuevos).
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                
                // Expiración deslizante: Si nadie la pide en 2 minutos, se borra para ahorrar RAM.
                entry.SlidingExpiration = TimeSpan.FromMinutes(2);

                // Esta es la lógica "lenta" que solo se ejecutará si NO está en caché
                try 
                {
                    var infraCategory = await _categoryClient.GetCategoryById(product.IdCategory);
                    if (infraCategory != null)
                    {
                        return _categoryInfraMapper.ToDomain(infraCategory);
                    }
                }
                catch
                {
                    // Si falla la red, devolvemos null y NO cacheamos el error (o sí, depende de la estrategia).
                    // Aquí decidimos no devolver nada para que la próxima vez se reintente.
                }
                return null;
            });

            // Asignamos lo que obtuvimos (del caché o de la red)
            product.Category = domainCategory;
            
            return product;
        }

        public async Task<List<DomainProductEntity>> GetAllProducts()    
        {
            var infraProducts = await _productClient.GetAllProducts();
            var domainProducts = _mapper.ToDomainList(infraProducts);
            
            // [EDUCATIVO] Gracias al caché, si 50 productos tienen la misma categoría (ej. ID=1),
            // solo se hará 1 petición HTTP real. Las otras 49 serán lecturas de RAM instantáneas.
            var tasks = domainProducts.Select(p => EnrichProductWithCategory(p));
            var results = await Task.WhenAll(tasks);
            
            return results.ToList();
        }

        public async Task<DomainProductEntity?> GetProductById(int idProduct)
        {
            var infraProduct = await _productClient.GetProductById(idProduct);
            if (infraProduct == null) return null;

            var domainProduct = _mapper.ToDomain(infraProduct);

            return await EnrichProductWithCategory(domainProduct);
        }

        public async Task<DomainProductEntity?> SaveProduct(DomainProductEntity domainRequest)
        {
            _validator.ValidateCategoryExists(domainRequest.IdCategory);
            
            var infraRequest = _mapper.ToEntity(domainRequest);
            var infraResponse = await _productClient.SaveProduct(infraRequest);
            
            if (infraResponse == null) return null;

            return _mapper.ToDomain(infraResponse);
        }

        public async Task<DomainProductEntity?> UpdateProduct(int idProduct, DomainProductEntity domainRequest)
        {
            _validator.ValidateCategoryExists(domainRequest.IdCategory);
            var infraRequest = _mapper.ToEntity(domainRequest);

            var infraResponse = await _productClient.UpdateProduct(idProduct, infraRequest);
            
            if (infraResponse == null) return null;

            return _mapper.ToDomain(infraResponse);
        }

        public async Task<DomainProductEntity?> DeleteProduct(int idProduct)
        {
            var infraResponse = await _productClient.DeleteProduct(idProduct);
            
            if (infraResponse == null) return null;
            
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
