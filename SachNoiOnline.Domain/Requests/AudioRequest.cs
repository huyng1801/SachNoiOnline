using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SachNoiOnline.Domain.Requests
{
    public class AudioRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(255, ErrorMessage = "Title cannot exceed 255 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Story ID is required.")]
        public int StoryId { get; set; }
        [Required(ErrorMessage = "Duration ID is required.")]
        public int Duration { get; set; }
        public IFormFile? AudioFile { get; set; }
    }
}
