namespace SachNoiOnline.Domain.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public int AccountId { get; set; } 
        public int StoryId { get; set; }
        public int RatingValue { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Story Story { get; set; }
        public Account Account { get; set; }
    }
}
