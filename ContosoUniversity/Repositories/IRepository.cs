using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Repositories
{ 
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        IQueryable<T> Get(int id);
        void Add(T entity);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<int> ExecuteSqlCommandAsync(string queryString);
    }
}
