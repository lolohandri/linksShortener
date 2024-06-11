using LinkAPI.Context;
using LinkAPI.Models;

namespace LinkAPI.Repository
{
    public class UserRepository : GenericRepository<User>
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context) : base(context)
        {
            _context = context;
        }
        public bool IsUserExists(string username)
        {
            var userCheck = _context.Users.FirstOrDefault(user => user.Username == username);
            return userCheck != null;
        }
        public User GetUserByUsername(string username)
        {
            var returnUser =  _context.Users.FirstOrDefault(user => user.Username == username);
            return returnUser ?? new User();
        }

        public override void Create(User item)
        {
            base.Create(item);
        }
    }
}
