using LinkAPI.Context;
using LinkAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LinkAPI.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly DataContext _context;
        internal DbSet<T> DbSet { get; set; }
        public GenericRepository(DataContext context)
        {
            _context = context;
            this.DbSet = _context.Set<T>();
        }

        public virtual IEnumerable<T> GetAll()
        {
            return this.DbSet.ToList();
        }

        public virtual T Get(long id)
        {
            return this.DbSet.Find(id);
        }

        public virtual void Create(T item)
        {
            this.DbSet.Add(item);
        }

        public virtual bool Update(long id, T item)
        {
            throw  new NotImplementedException();
        }

        public virtual bool Delete(long id)
        {
            var item = this.DbSet.Find(id);
            if (item != null)
            {
                _context.Remove(item);
                return true;
            }
            return false;
        }
    }
}
