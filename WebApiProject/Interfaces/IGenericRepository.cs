namespace WebApiProject.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task InsertAsync(T obj);
        void Update(T obj);
        Task DeleteAsync(int id);
        Task SaveAsync();
        IQueryable<T> GetAllAsync();
        Task<T> GetByIdAsync(int id);
    }
}
