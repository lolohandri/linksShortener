using LinkAPI.Utils;

namespace LinkAPI.Dto.User
{
    public class UserDto
    {
        public string Username { get; set; }
        
        public string Password { get; set; }
        
        public string UserRole { get; set; } = "user";
    }
}
