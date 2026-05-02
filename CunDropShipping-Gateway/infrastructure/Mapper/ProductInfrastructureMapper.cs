using CunDropShipping_Gateway.application.Common;
using CunDropShipping_Gateway.domain.Entity;
using CunDropShipping_Gateway.infrastructure.Entity;

namespace CunDropShipping_Gateway.infrastructure.Mapper;

public class ProductInfrastructureMapper : IMapper<DomainProductEntity, ProductResponse>
{
    private readonly IMapper<DomainCategoryEntity, CategoryResponse> _categoryMapper;

    public ProductInfrastructureMapper(IMapper<DomainCategoryEntity, CategoryResponse> categoryMapper)
    {
        _categoryMapper = categoryMapper;
    }

    public DomainProductEntity ToDomain(ProductResponse entity)
    {
        if (entity == null) return null;
        return new DomainProductEntity
        {
            IdProduct = entity.IdProduct,
            NameProduct = entity.NameProduct,
            Description = entity.Description,
            Price = entity.Price,
            StockQuantity = entity.StockQuantity,
            IdCategory =  entity.IdCategory
        };
    }

    public ProductResponse ToEntity(DomainProductEntity domain)
    {
        if (domain == null) return null;
        return new ProductResponse
        {
            IdProduct = domain.IdProduct,
            NameProduct = domain.NameProduct,
            Description = domain.Description,
            Price = domain.Price,
            StockQuantity = domain.StockQuantity,
            IdCategory = domain.IdCategory
        };
    }

    public List<DomainProductEntity> ToDomainList(List<ProductResponse> entityList)
    {
        return entityList?.Select(ToDomain).ToList() ?? new List<DomainProductEntity>();
    }

    public List<ProductResponse> ToEntityList(List<DomainProductEntity> domainList)
    {
        return domainList?.Select(ToEntity).ToList() ?? new List<ProductResponse>();
    }
}