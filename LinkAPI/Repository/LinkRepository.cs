using LinkAPI.Context;
using LinkAPI.Interfaces;
using LinkAPI.Models;

namespace LinkAPI.Repository
{
    public class LinkRepository(DataContext context) : GenericRepository<Link>(context), IPagination<Link>
    {
        private readonly DataContext _context = context;
        
        public bool IsUrlExists(string url)
        {
            var linkCheck = _context.Links.FirstOrDefault(link => link.OriginLink == url);
            return linkCheck != null; 
        }

        public override bool Update(long id, Link item)
        {
            var existingLink = _context.Links.FirstOrDefault(l => l.Id == id);
            
            if (existingLink == null)
            {
                return false;
            }
            
            existingLink.OriginLink = item.OriginLink;
            
            existingLink.ShortLink = item.ShortLink;

            _context.Links.Update(existingLink);
            
            return true;
        }

        public IEnumerable<Link> GetPaged(int pageNumber, int pageSize)
        {
            return _context.Links
                .OrderByDescending(l => l.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        public int Count()
        {
            return _context.Links.Count();
        }
    }
}
