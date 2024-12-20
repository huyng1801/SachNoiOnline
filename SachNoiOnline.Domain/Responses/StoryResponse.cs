namespace SachNoiOnline.Domain.Responses
{
    public class StoryResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CoverImageUrl { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int NarratorId { get; set; }
        public string NarratorName { get; set; }
        public int ListenersCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int TotalAudios { get; set; } 
        public double AverageRating { get; set; } 
    }
}
