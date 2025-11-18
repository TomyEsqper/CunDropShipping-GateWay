using CunDropShipping_Gateway.application.Common;
using CunDropShipping_Gateway.domain.Entity;
using CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity;

namespace CunDropShipping_Gateway.adapter.restful.v1.Controller.Mapper
{
    /// <summary>
    /// Implementa el mapeo entre la Entidad de Dominio (DomainProductEntity) 
    /// y el DTO de la API (ProductDto).
    /// </summary>
    public class ProductAdapterMapper : IMapper<DomainProductEntity, ProductDto>
    {
        // 1. Mapeo Individual: Dominio -> DTO (Usado para las respuestas HTTP)
        public ProductDto ToEntity(DomainProductEntity domain)
        {
            if (domain == null) return null;
            
            return new ProductDto
            {
                IdProduct = domain.IdProduct,
                NameProduct = domain.NameProduct,
                Description = domain.Description,
                Price = domain.Price,
                StockQuantity = domain.StockQuantity
            };
        }

        // 2. Mapeo Individual: DTO -> Dominio (Usado para las peticiones HTTP POST/PUT)
        public DomainProductEntity ToDomain(ProductDto entity)
        {
            if (entity == null) return null;
            
            return new DomainProductEntity
            {
                IdProduct = entity.IdProduct,
                NameProduct = entity.NameProduct,
                Description = entity.Description,
                Price = entity.Price,
                StockQuantity = entity.StockQuantity
            };
        }

        // 3. Mapeo de Lista: DTOs -> Dominio (No muy usado en esta capa, pero requerido por IMapper)
        public List<DomainProductEntity> ToDomainList(List<ProductDto> entityList)
        {
            // Usamos LINQ para iterar y llamar al método ToDomain individual.
            return entityList?.Select(ToDomain).ToList() ?? new List<DomainProductEntity>();
        }

        // 4. Mapeo de Lista: Dominio -> DTOs (¡ESTE ES EL MÉTODO QUE ESTABA FALLANDO!)
        public List<ProductDto> ToEntityList(List<DomainProductEntity> domainList)
        {
            // Usamos LINQ para iterar y llamar al método ToEntity individual.
            return domainList?.Select(ToEntity).ToList() ?? new List<ProductDto>();
        }
    }
}