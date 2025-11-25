using CunDropShipping_Gateway.application.Service;
using CunDropShipping_Gateway.domain.Entity;
using CunDropShipping_Gateway.adapter.restful.v1.Controller.Entity;
using CunDropShipping_Gateway.application.Common;
using Microsoft.AspNetCore.Mvc;

namespace CunDropShipping_Gateway.adapter.restful.v1.Controller;

[ApiController]
[Route("api/gateway/v1/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _service;
    private readonly IMapper<DomainCategoryEntity, CategoryDto> _mapper;
    
    public CategoryController(ICategoryService service, IMapper<DomainCategoryEntity, CategoryDto> mapper)
    {
        _service = service;
        _mapper = mapper;
    }
    
    [HttpGet]
    public ActionResult<List<CategoryDto>> GetAllCategories()
    {
        var domainList = _service.GetAllCategories();
        return Ok(_mapper.ToEntityList(domainList));
    }
    
    [HttpGet("Search")]
    public ActionResult<List<CategoryDto>> SearchCategoriesByName([FromQuery] string name)
    {
        var domainList = _service.GetCategoriesByName(name);
        if (domainList == null) return NotFound();
        return Ok(_mapper.ToEntityList(domainList));
    }

    [HttpGet("{id}")]
    public ActionResult<CategoryDto> GetCategoryById(int id)
    {
        var domain = _service.GetCategoryById(id);
        if (domain == null) return NotFound();
        return Ok(_mapper.ToEntity(domain));
    }

    [HttpPost]
    public ActionResult<CategoryDto> CreateCategory([FromBody] CategoryDto dto)
    {
        var domainEntity = _mapper.ToDomain(dto);
        var createdDomain = _service.CreateCategory(domainEntity);
        var responseDto = _mapper.ToEntity(createdDomain);
        return Ok(responseDto);
    }

    [HttpPut("{id}")]
    public ActionResult<CategoryDto> UpdateCategory(int id, [FromBody] CategoryDto dto)
    {
        var domainRequest = _mapper.ToDomain(dto);
        var updatedDomain = _service.UpdateCategory(id, domainRequest);
        if (updatedDomain == null) return NotFound();
        return Ok(_mapper.ToEntity(updatedDomain));
    }
    
    [HttpDelete("{id}")]
    public ActionResult<CategoryDto> DeleteCategoryById(int id)
    {
        var deletedDomain = _service.DeleteCategoryById(id);
        if (deletedDomain == null) return NotFound();
        return Ok(_mapper.ToEntity(deletedDomain));
    }
    
    [HttpDelete("Search")]
    public ActionResult<List<CategoryDto>> DeleteCategoryByName([FromQuery] string name)
    {
        var deletedDomainList = _service.DeleteCategoryByName(name);
        if (deletedDomainList == null) return NotFound();
        return Ok(_mapper.ToEntityList(deletedDomainList));
    }
}