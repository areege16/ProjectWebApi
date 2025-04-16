using Microsoft.EntityFrameworkCore;
using WebApiProject.Interfaces;
using WebApiProject.Models;

namespace WebApiProject.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private LostFoundContext context;
        protected DbSet<T> dbSet;
        public GenericRepository(LostFoundContext context) 
        {
            this.context = context;
            dbSet = context.Set<T>();
        }
        public async Task DeleteAsync(int id)
        {
            T entity = await GetByIdAsync(id);
            if (entity != null)
            {
                dbSet.Remove(entity);
            }
        }

        public IQueryable<T> GetAllAsync()
        {
            return dbSet;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task InsertAsync(T obj)
        {
            await dbSet.AddAsync(obj);
        }

        public async Task SaveAsync()
        {
             await context.SaveChangesAsync();
        }

        public void Update(T obj)
        {
            dbSet.Update(obj);
        }
    }
}
