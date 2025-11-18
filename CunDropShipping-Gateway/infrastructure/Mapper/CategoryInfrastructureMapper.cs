using CunDropShipping_Gateway.application.Common;
using CunDropShipping_Gateway.domain.Entity;
using CunDropShipping_Gateway.infrastructure.Entity;

namespace CunDropShipping_Gateway.infrastructure.Mapper;

public class CategoryInfrastructureMapper : IMapper<DomainCategoryEntity, CategoryResponse>
{
    public DomainCategoryEntity ToDomain(CategoryResponse entity)
    {
        if (entity == null) return null;
        return new DomainCategoryEntity
        {
            IdCategory = entity.IdCategory,
            TypeCategory = entity.TypeCategory
        };
    }

    public CategoryResponse ToEntity(DomainCategoryEntity domain)
    {
        if (domain == null) return null;
        return new CategoryResponse
        {
            IdCategory = domain.IdCategory,
            TypeCategory = domain.TypeCategory
        };
    }

    public List<DomainCategoryEntity> ToDomainList(List<CategoryResponse> entityList)
    {
        return entityList?.Select(ToDomain).ToList() ?? new List<DomainCategoryEntity>();
    }

    public List<CategoryResponse> ToEntityList(List<DomainCategoryEntity> domainList)
    {
        return domainList?.Select(ToEntity).ToList() ?? new List<CategoryResponse>();
    }
}