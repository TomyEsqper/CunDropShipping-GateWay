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
    public async Task<ActionResult<List<CategoryDto>>> GetAllCategories()
    {
        // [EDUCATIVO] El controlador espera (await) a que el servicio le dé los datos.
        var domainList = await _service.GetAllCategories();
        return Ok(_mapper.ToEntityList(domainList));
    }
    
    [HttpGet("Search")]
    public async Task<ActionResult<List<CategoryDto>>> SearchCategoriesByName([FromQuery] string name)
    {
        var domainList = await _service.GetCategoriesByName(name);
        if (domainList == null) return NotFound();
        return Ok(_mapper.ToEntityList(domainList));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
    {
        var domain = await _service.GetCategoryById(id);
        if (domain == null) return NotFound();
        return Ok(_mapper.ToEntity(domain));
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CategoryDto dto)
    {
        var domainEntity = _mapper.ToDomain(dto);
        var createdDomain = await _service.CreateCategory(domainEntity);
        var responseDto = _mapper.ToEntity(createdDomain);
        return Ok(responseDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, [FromBody] CategoryDto dto)
    {
        var domainRequest = _mapper.ToDomain(dto);
        var updatedDomain = await _service.UpdateCategory(id, domainRequest);
        if (updatedDomain == null) return NotFound();
        return Ok(_mapper.ToEntity(updatedDomain));
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<CategoryDto>> DeleteCategoryById(int id)
    {
        var deletedDomain = await _service.DeleteCategoryById(id);
        if (deletedDomain == null) return NotFound();
        return Ok(_mapper.ToEntity(deletedDomain));
    }
    
    [HttpDelete("Search")]
    public async Task<ActionResult<List<CategoryDto>>> DeleteCategoryByName([FromQuery] string name)
    {
        var deletedDomainList = await _service.DeleteCategoryByName(name);
        if (deletedDomainList == null) return NotFound();
        return Ok(_mapper.ToEntityList(deletedDomainList));
    }
}
