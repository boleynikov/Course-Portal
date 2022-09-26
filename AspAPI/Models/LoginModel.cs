using System.ComponentModel.DataAnnotations;

namespace AspAPI.Models
{
    public class LoginModel
    {
        /// <summary>
        /// Gets user email.
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets user password.
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
