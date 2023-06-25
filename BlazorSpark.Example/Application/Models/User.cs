using BlazorSpark.Library.Database;

namespace BlazorSpark.Example.Application.Models
{
    public class User : BaseModel
    {
        public User()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string? RememberToken { get; set; }

        public DateTime? EmailVerifiedAt { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
