using LinkAPI.Context;
using LinkAPI.Models;

namespace LinkAPI.Repository
{
    public class LinkRepository : GenericRepository<Link>
    {
        private readonly DataContext _context;
        public LinkRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public bool IsUrlExists(string url)
        {
            var linkCheck = _context.Links.FirstOrDefault(link => link.OriginLink == url);
            return linkCheck != null; 
        }
    }
}
