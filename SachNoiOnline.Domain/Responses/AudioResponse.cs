namespace SachNoiOnline.Domain.Responses
{
    public class AudioResponse
    {
        public int Id { get; set; }
        public int StoryId { get; set; }
        public string storyTitle { get; set; } // Added StoryName property
        public string Title { get; set; }
        public string AudioFileUrl { get; set; }
        public int Duration { get; set; } // Duration in seconds
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
   
    }
}
