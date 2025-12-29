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
        private readonly IMapper<DomainCategoryEntity, CategoryResponse> _mapper;

        public CategoryServiceImp(ICategoryClient client, IMapper<DomainCategoryEntity, CategoryResponse> mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        public async Task<List<DomainCategoryEntity>> GetAllCategories()    
        {
            // [EDUCATIVO] CORRECTO: Primero esperamos (await) a tener la lista de infra...
            var infraList = await _client.GetAllCategories();
            
            // ... y LUEGO la mapeamos. El mapper trabaja con objetos reales, no con Tasks.
            return _mapper.ToDomainList(infraList);
        }

        public async Task<List<DomainCategoryEntity>> GetCategoriesByName(string name)
        {
            var infraList = await _client.GetCategoriesByName(name);
            
            if (infraList == null) return new List<DomainCategoryEntity>();
            
            return _mapper.ToDomainList(infraList);
        }

        public async Task<DomainCategoryEntity?> GetCategoryById(int id)
        {
            try 
            {
                var response = await _client.GetCategoryById(id);
                return response == null ? null : _mapper.ToDomain(response);
            }
            catch (Exception)
            {
                // [EDUCATIVO] Si falla el cliente (ej. 404), devolvemos null para que el controlador devuelva NotFound
                return null;
            }
        }

        public async Task<DomainCategoryEntity?> CreateCategory(DomainCategoryEntity category)
        {
            var infraRequest = _mapper.ToEntity(category);
            
            var infraResponse = await _client.CreateCategory(infraRequest);

            if (infraResponse == null) return null;

            return _mapper.ToDomain(infraResponse);
        }

        public async Task<DomainCategoryEntity?> UpdateCategory(int id, DomainCategoryEntity category)
        {
            var infraRequest = _mapper.ToEntity(category);
            
            try 
            {
                var infraResponse = await _client.UpdateCategory(id, infraRequest);
                if (infraResponse == null) return null;
                return _mapper.ToDomain(infraResponse);
            }
            catch(Exception)
            {
                return null;
            }
        }

        public async Task<DomainCategoryEntity?> DeleteCategoryById(int id)
        {
            try
            {
                var infraResponse = await _client.DeleteCategoryById(id);
                if (infraResponse == null) return null;
                return _mapper.ToDomain(infraResponse);
            }
            catch(Exception)
            {
                return null;
            }
        }

        public async Task<List<DomainCategoryEntity>> DeleteCategoryByName(string name)
        {
            try
            {
                var infraResponseList = await _client.DeleteCategoryByName(name);
                return _mapper.ToDomainList(infraResponseList);
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
