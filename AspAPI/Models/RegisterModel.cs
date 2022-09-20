using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspAPI.Models
{
    public class RegisterModel
    {
        /// <summary>
        /// Gets user name
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        /// <summary>
        /// Gets user email.
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets user password.
        /// </summary>
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        /// <summary>
        /// Password confirm
        /// </summary>
        [DataType(DataType.Password)]
        [Compare("Password")]
        [DisplayName("Confirmation password")]
        [Required(ErrorMessage = "Confirm your password")]
        public string ConfirmPassword { get; set; }
    }
}
