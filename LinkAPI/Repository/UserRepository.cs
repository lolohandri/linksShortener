using LinkAPI.Context;
using LinkAPI.Models;

namespace LinkAPI.Repository
{
    public class UserRepository : GenericRepository<User>
    {
        public DataContext _context;

        public UserRepository(DataContext context) : base(context)
        {
            _context = context;
        }
        public bool IsUserExists(string username)
        {
            var userCheck = _context.Users.FirstOrDefault(user => user.Username == username);
            return userCheck != null ? true : false;
        }
        public User GetUserByUsername(string username)
        {
            var returnUser =  _context.Users.FirstOrDefault(user => user.Username == username);
            if (returnUser != null)
            {
                return returnUser;
            }
            return new User();
        }

        public override void Create(User item)
        {
            base.Create(item);
        }
    }
}
