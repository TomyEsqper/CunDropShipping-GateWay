using CunDropShipping_Gateway.application.Common;
using CunDropShipping_Gateway.application.Service;
using CunDropShipping_Gateway.domain.Entity;
using CunDropShipping_Gateway.infrastructure.Clients;
using CunDropShipping_Gateway.infrastructure.Entity;

namespace CunDropShipping_Gateway.domain
{
    public class CategoryServiceImp : ICategoryService
    {
        private readonly ICategoryClient _client;
        
        // Inyección genérica: "Necesito un mapper que traduzca entre DomainCategoryEntity y CategoryResponse"
        private readonly IMapper<DomainCategoryEntity, CategoryResponse> _mapper;

        public CategoryServiceImp(ICategoryClient client, IMapper<DomainCategoryEntity, CategoryResponse> mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        public List<DomainCategoryEntity> GetAllCategories()    
        {
            var infraList = _client.GetAllCategories();
            return _mapper.ToDomainList(infraList);
        }

        public List<DomainCategoryEntity>? GetCategoriesByName(string name)
        {
            var infraList = _client.GetCategoriesByName(name);
            // Si la infraestructura devuelve null, devolvemos null o lista vacía según prefieras.
            // El mapper ToDomainList maneja null devolviendo lista vacía, pero si quieres ser explícito con el null:
            if (infraList == null) return null;
            
            return _mapper.ToDomainList(infraList);
        }

        public DomainCategoryEntity CreateCategory(DomainCategoryEntity category)
        {
            // 1. Convertir Dominio -> Infra (Para enviar)
            var infraRequest = _mapper.ToEntity(category);
            
            // 2. Llamar al API externo
            var infraResponse = _client.CreateCategory(infraRequest);

            // 3. Convertir Infra -> Dominio (Para devolver)
            return _mapper.ToDomain(infraResponse);
        }

        // --- MÉTODOS COMPLETADOS ---

        public DomainCategoryEntity? UpdateCategory(int id, DomainCategoryEntity category)
        {
            // 1. Convertir el objeto de dominio a DTO de infraestructura
            var infraRequest = _mapper.ToEntity(category);
            
            // 2. Intentar actualizar en el microservicio
            var infraResponse = _client.UpdateCategory(id, infraRequest);

            // 3. Si infra devuelve null (no encontrado), devolvemos null
            if (infraResponse == null) return null;

            // 4. Traducir la respuesta y devolverla
            return _mapper.ToDomain(infraResponse);
        }

        public DomainCategoryEntity? DeleteCategoryById(int id)
        {
            // 1. Llamar al borrado en el cliente
            var infraResponse = _client.DeleteCategoryById(id);
            
            // 2. Validar si existía
            if (infraResponse == null) return null;

            // 3. Devolver el objeto borrado traducido
            return _mapper.ToDomain(infraResponse);
        }

        public List<DomainCategoryEntity>? DeleteCategoryByName(string name)
        {
            // 1. Llamar al borrado por nombre
            var infraResponseList = _client.DeleteCategoryByName(name);
            
            // 2. Validar
            if (infraResponseList == null) return null;

            // 3. Devolver la lista de objetos borrados traducida
            return _mapper.ToDomainList(infraResponseList);
        }
    }
}