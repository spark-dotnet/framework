using System.ComponentModel.DataAnnotations;

namespace Spark.Templates.Api.Application.ViewModels
{
    public class ProfilePasswordEditor
    {
        [Required(ErrorMessage = "Current Password is required")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; set; }
    }
}
