using CunDropShipping_Gateway.domain.Entity;

namespace CunDropShipping_Gateway.application.Service;
public interface ICategoryService
{
    List<DomainCategoryEntity> GetAllCategories();
    List<DomainCategoryEntity>? GetCategoriesByName(string name);
    DomainCategoryEntity? GetCategoryById(int id);
    DomainCategoryEntity CreateCategory(DomainCategoryEntity category);
    DomainCategoryEntity? UpdateCategory(int id, DomainCategoryEntity category);
    DomainCategoryEntity? DeleteCategoryById(int id);
    List<DomainCategoryEntity>? DeleteCategoryByName(string name);
}