using SachNoiOnline.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SachNoiOnline.Domain.Requests
{
    public class AccountRequest
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
        public string Username { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public Role Role { get; set; }
    }
}
