using CunDropShipping_Gateway.domain.Entity;

namespace CunDropShipping_Gateway.application.Service;

public interface ICategoryService
{
    // [EDUCATIVO] Propagamos el async: Devolvemos Task<...>
    Task<List<DomainCategoryEntity>> GetAllCategories();
    
    Task<List<DomainCategoryEntity>> GetCategoriesByName(string name);
    
    Task<DomainCategoryEntity> GetCategoryById(int id);
    
    Task<DomainCategoryEntity> CreateCategory(DomainCategoryEntity category);
    
    Task<DomainCategoryEntity> UpdateCategory(int id, DomainCategoryEntity category);
    
    Task<DomainCategoryEntity> DeleteCategoryById(int id);
    
    Task<List<DomainCategoryEntity>> DeleteCategoryByName(string name);
}
