using LinkAPI.Utils;

namespace LinkAPI.Models
{
    public class User
    {
        public string Username { get; set; }
        
        public byte[] PasswordHash { get; set; }
        
        public byte[] PasswordSalt { get; set; }
        
        public string UserRole { get; set; }
    }
}
