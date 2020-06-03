using System.ComponentModel.DataAnnotations;

namespace Epam.AspNet.Module1.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
