using LinkAPI.Context;
using LinkAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LinkAPI.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly DataContext _context;
        private DbSet<T> DbSet { get; set; }

        protected GenericRepository(DataContext context)
        {
            _context = context;
            DbSet = _context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return DbSet.ToList();
        }

        public virtual T Get(long id)
        {
            return DbSet.Find(id)!;
        }

        public virtual void Create(T item)
        {
            DbSet.Add(item);
        }

        public virtual bool Update(long id, T item)
        {
            throw  new NotImplementedException();
        }

        public virtual bool Delete(long id)
        {
            var item = DbSet.Find(id);
            if (item == null) return false;
            _context.Remove(item);
            return true;
        }
    }
}
