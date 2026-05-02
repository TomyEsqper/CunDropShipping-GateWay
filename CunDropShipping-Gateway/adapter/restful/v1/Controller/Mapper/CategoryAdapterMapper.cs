using CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity;
using CunDropShipping_Gateway.application.Common;
using CunDropShipping_Gateway.domain.Entity;
using CunDropShipping_Gateway.infrastructure.Entity;

namespace CunDropShipping_Gateway.adapter.restful.v1.Controller.Mapper;

public class CategoryAdapterMapper : IMapper<DomainCategoryEntity, CategoryDto>
{
    public DomainCategoryEntity ToDomain(CategoryDto dto)
    {
        if (dto == null) return null;
        return new DomainCategoryEntity
        {
            IdCategory = dto.IdCategory,
            TypeCategory = dto.TypeCategory
        };
    }

    public CategoryDto ToEntity(DomainCategoryEntity domain)
    {
        if (domain == null) return null;
        return new CategoryDto
        {
            IdCategory = domain.IdCategory,
            TypeCategory = domain.TypeCategory
        };
    }

    public List<DomainCategoryEntity> ToDomainList(List<CategoryDto> dtoList)
    {
        return dtoList?.Select(ToDomain).ToList() ?? new List<DomainCategoryEntity>();
    }

    public List<CategoryDto> ToEntityList(List<DomainCategoryEntity> domainList)
    {
        return domainList?.Select(ToEntity).ToList() ?? new List<CategoryDto>();
    }
}