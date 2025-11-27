namespace CunDropShipping_Gateway.application.Common;

public interface IDomainValidatorService
{ 
    /// <summary>
    /// Verifica si una categorai existe. Si no, lanza una excepcion.
    /// </summary>
    void ValidateCategoryExists(int idCategory);
}