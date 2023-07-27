using System.ComponentModel.DataAnnotations;

namespace Spark.Templates.Api.Application.ViewModels
{
    public class Register
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
