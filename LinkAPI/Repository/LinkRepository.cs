using LinkAPI.Context;
using LinkAPI.Models;

namespace LinkAPI.Repository
{
    public class LinkRepository(DataContext context) : GenericRepository<Link>(context)
    {
        private readonly DataContext _context = context;

        public bool IsUrlExists(string url)
        {
            var linkCheck = _context.Links.FirstOrDefault(link => link.OriginLink == url);
            return linkCheck != null; 
        }
    }
}
