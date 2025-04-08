
namespace AdventureWork.Data.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T>
    {
        protected readonly DbConnectionFactory ConnectionFactory;

        protected RepositoryBase(DbConnectionFactory connectionFactory)
        {
            ConnectionFactory = connectionFactory;
        }

        public abstract IEnumerable<T> GetAll();
        public abstract T GetById(int id);
        public abstract void Add(T entity);
        public abstract void Update(T entity);
        public abstract void Delete(int id);
    }
}
