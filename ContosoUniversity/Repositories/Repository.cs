using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Models;

namespace ContosoUniversity.Repositories
{
    public class Repository<T, TContext> : IRepository<T> where T: BaseModel where TContext : DbContext
    {
        private TContext _context;
        private DbSet<T> _entities;

        public Repository(TContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public IQueryable<T> Get(int id)
        {
            return _entities.Where(s => s.ID == id).AsQueryable<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _entities.AsQueryable<T>();
        }

        public void Add(T entity)
        {
            _context.Add<T>(entity);
        }

        public async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Remove(entity);
            await Task.FromResult(true);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await Task.FromResult(true);
        }
        public async Task<int> ExecuteSqlCommandAsync(string queryString)
        {
            return await _context.Database.ExecuteSqlRawAsync(queryString);
        }
    }
}
