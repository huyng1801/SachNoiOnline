namespace SachNoiOnline.Domain.Entities
{
    public class Story
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CoverImageUrl { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public int NarratorId { get; set; }
        public int ListenersCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } 
        public DateTime? DeletedAt { get; set; } 
        public Author Author { get; set; }
        public Category Category { get; set; }
        public Narrator Narrator { get; set; }
        public ICollection<Audio> Audios { get; set; }
        public ICollection<Rating> Ratings { get; set; }
    }
}
