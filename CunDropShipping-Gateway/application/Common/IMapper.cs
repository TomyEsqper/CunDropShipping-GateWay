namespace CunDropShipping_Gateway.application.Common;

public interface IMapper <TDomain, TEntity>
{
    // Convierte de "Afuera" hacia "Adentro"
    TDomain ToDomain(TEntity entity);
    
    // Convierte de "Adentro" hacia "Afuera"
    TEntity ToEntity(TDomain domain);
    
    // Listas
    List<TDomain> ToDomainList(List<TEntity> entityList);
    List<TEntity> ToEntityList(List<TDomain> domainList);
}