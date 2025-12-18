using LinkAPI.Context;
using LinkAPI.Models;

namespace LinkAPI.Repository
{
    public class UserRepository(DataContext context) : GenericRepository<User>(context)
    {
        private readonly DataContext _context = context;

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
    }
}
