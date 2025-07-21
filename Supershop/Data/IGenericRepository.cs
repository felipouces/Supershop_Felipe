using System.Linq;
using System.Threading.Tasks;

namespace Supershop.Data
{
    public interface IGenericRepository<T> where T : class
    {
          IQueryable<T> GetAll();
        // method to get all entities of type T

        Task<T> GetByIdAsync(int id);
        // method to get an entity by its ID

        Task CreateAsync(T entity);
        // method to create a new entity of type T

        Task UpdateAsync(T entity);
        // method to update an existing entity of type T

        Task DeleteAsync(T entity);
        // method to delete an entity by its ID

        //Task<bool> ExistsAsync(int id); 
        Task<bool> ExistAsync(int id);
        // method to check if an entity exists by its ID

        //Task<bool> SaveChangesAsync();
    }

}
