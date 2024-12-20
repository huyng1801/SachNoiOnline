using System;
using System.ComponentModel.DataAnnotations;

namespace SachNoiOnline.Domain.Requests
{
    public class RatingRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Account ID is required.")]
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Story ID is required.")]
        public int StoryId { get; set; }

        [Required(ErrorMessage = "Rating value is required.")]
        [Range(1, 5, ErrorMessage = "Rating value must be between 1 and 5.")]
        public int RatingValue { get; set; }

        [StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters.")]
        public string Comment { get; set; }
    }
}
