using CunDropShipping_Gateway.infrastructure.Clients;

namespace CunDropShipping_Gateway.application.Common;

public class DomainValidatorService : IDomainValidatorService
{
    private readonly ICategoryClient _categoryClient;
    
    public DomainValidatorService(ICategoryClient categoryClient)
    {
        _categoryClient = categoryClient;
    }

    public void ValidateCategoryExists(int idCategory)
    {
        // 1. Si el ID es 0 o negativo, podemos decidir si es valido o no.
        // Asumimmos que un producto DEBE tener categoria valida.
        
        if (idCategory <= 0) throw new ArgumentException("El ID de la categoria no es valido.", nameof(idCategory));
       
        // 2. Prguntamos al microservicio de categorias si existe la categoria.
        // (Esto usa el metodo GetCategoryById)
        var category = _categoryClient.GetCategoryById(idCategory);
        
        // 3. Si devuelve null, es que no existe la categoria. -> Error
        if (category == null) throw new ArgumentException($"La categoria no ID {idCategory} no existe en el sistema.");
        
        
    }
}