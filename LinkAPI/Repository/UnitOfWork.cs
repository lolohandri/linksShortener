using LinkAPI.Context;
using LinkAPI.Interfaces;

namespace LinkAPI.Repository
{
    public class UnitOfWork : IUnitOfWork
    { 
        private readonly DataContext _context;
        public LinkRepository LinkRepository { get; private set; }
        public UserRepository UserRepository { get; private set; }
        public UnitOfWork(DataContext context)
        {
            _context = context;
            LinkRepository = new LinkRepository(_context);
            UserRepository = new UserRepository(_context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
