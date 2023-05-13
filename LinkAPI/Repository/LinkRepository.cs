using LinkAPI.Context;
using LinkAPI.Models;
using Microsoft.AspNetCore.SignalR;

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
            return linkCheck != null ? true : false; 
        }

        public override IEnumerable<Link> GetAll()
        {
            return base.GetAll();
        }
        public override Link Get(long id)
        {
            return base.Get(id);
        }
        public override bool Delete(long id)
        {
            return base.Delete(id);
        }
    }
}
