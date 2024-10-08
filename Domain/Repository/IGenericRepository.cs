namespace Domain.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        T GetByIdAsync(Guid id);
        List<T> ListAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void SaveChanges();
    }
}
