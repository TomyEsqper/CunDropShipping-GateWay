using CunDropShipping_Gateway.infrastructure.Entity; 

namespace CunDropShipping_Gateway.infrastructure.Clients
{
    public interface IProductClient
    {
        // [EDUCATIVO] Cambio 1: Envolvemos el retorno en Task<...>.
        // Esto indica que el método no devuelve la lista YA, sino una "promesa" (Tarea)
        // que se completará en el futuro. Esto permite que el servidor haga otras cosas mientras espera.
        Task<List<ProductResponse>> GetAllProducts();
        
        Task<ProductResponse> GetProductById(int idProduct);
        
        Task<ProductResponse> SaveProduct(ProductResponse product);
        
        Task<ProductResponse> UpdateProduct(int idProduct, ProductResponse product);
        
        Task<ProductResponse> DeleteProduct(int idProduct);
        
        Task<List<ProductResponse>> SearchProductsByName(string searchTerm);
        
        Task<List<ProductResponse>> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);
        
        Task<List<ProductResponse>> GetProductsWithLowStock(int stockThreshold);
    }
}
