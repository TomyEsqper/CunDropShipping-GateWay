using CunDropShipping_Gateway.infrastructure.Entity;
namespace CunDropShipping_Gateway.infrastructure.Clients;

public interface ICategoryClient
{
    List<CategoryResponse> GetAllCategories();
    List<CategoryResponse>? GetCategoriesByName(string name);
    CategoryResponse? GetCategoryById(int id);
    CategoryResponse CreateCategory(CategoryResponse category);
    CategoryResponse? UpdateCategory(int id, CategoryResponse category);
    CategoryResponse? DeleteCategoryById(int id);
    List<CategoryResponse>? DeleteCategoryByName(string name);
}