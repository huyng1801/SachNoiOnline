namespace SachNoiOnline.Domain.Responses
{
    public class AuthorResponse
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int TotalStories { get; set; } // Total number of stories associated with the author
    }
}
