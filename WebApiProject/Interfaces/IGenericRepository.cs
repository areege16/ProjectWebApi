namespace WebApiProject.Interfaces
{
    public interface IGenericRepository<T>
    {
        void Insert(T obj);
        void Update(T obj);
        void Delete(int id);
        void Save();
        List<T> GetAll();
        T GetById(int id);
    }
}
