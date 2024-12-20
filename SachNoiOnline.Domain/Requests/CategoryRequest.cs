using System.ComponentModel.DataAnnotations;

namespace SachNoiOnline.Domain.Requests
{
    public class CategoryRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Category name is required.")]
        [StringLength(255, ErrorMessage = "Category name cannot exceed 200 characters.")]
        public string CategoryName { get; set; }
    }
}
