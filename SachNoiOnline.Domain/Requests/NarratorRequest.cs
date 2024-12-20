using System.ComponentModel.DataAnnotations;

namespace SachNoiOnline.Domain.Requests
{
    public class NarratorRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Narrator name is required.")]
        [StringLength(255, ErrorMessage = "Narrator name cannot exceed 255 characters.")]
        public string NarratorName { get; set; }

    }
}
