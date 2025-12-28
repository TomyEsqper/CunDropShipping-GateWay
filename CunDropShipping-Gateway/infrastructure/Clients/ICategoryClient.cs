using CunDropShipping_Gateway.infrastructure.Entity;

namespace CunDropShipping_Gateway.infrastructure.Clients;

public interface ICategoryClient
{
    // [EDUCATIVO] Mantenemos Task<...> para operaciones asíncronas.
    // Eliminamos los '?' innecesarios en el retorno de Task (Task en sí no suele ser null, su resultado sí).
    Task<List<CategoryResponse>> GetAllCategories();
    
    Task<List<CategoryResponse>> GetCategoriesByName(string name);
    
    Task<CategoryResponse> GetCategoryById(int id);
    
    Task<CategoryResponse> CreateCategory(CategoryResponse category);
    
    Task<CategoryResponse> UpdateCategory(int id, CategoryResponse category);
    
    Task<CategoryResponse> DeleteCategoryById(int id);
    
    Task<List<CategoryResponse>> DeleteCategoryByName(string name);
}
