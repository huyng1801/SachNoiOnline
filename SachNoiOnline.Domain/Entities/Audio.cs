namespace SachNoiOnline.Domain.Entities
{
    public class Audio
    {
        public int Id { get; set; }
        public int StoryId { get; set; }
        public string Title { get; set; }
        public string AudioFileUrl { get; set; }
        public int Duration { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } 
        public DateTime? DeletedAt { get; set; }
        public Story Story { get; set; }
    }
}
