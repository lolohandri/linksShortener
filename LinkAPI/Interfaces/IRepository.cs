namespace LinkAPI.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public IEnumerable<T>? GetAll();
        
        public T? Get(long id);
        
        public void Create(T item);
        
        public bool Update(long id, T item);
        
        public bool Delete(long id);
    }
}
