namespace SachNoiOnline.Domain.Responses
{
    public class RatingResponse
    {
        public int Id { get; set; } 
        public int AccountId { get; set; }
        public string Username { get; set; } 
        public int StoryId { get; set; }
        public string StoryTitle { get; set; } 
        public int RatingValue { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; } 
    }
}
