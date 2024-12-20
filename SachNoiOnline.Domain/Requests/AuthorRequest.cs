using System.ComponentModel.DataAnnotations;

namespace SachNoiOnline.Domain.Requests
{
    public class AuthorRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Author name is required.")]
        [StringLength(255, ErrorMessage = "Author name cannot exceed 255 characters.")]
        public string AuthorName { get; set; }

    }
}
