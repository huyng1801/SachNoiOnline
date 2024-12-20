namespace SachNoiOnline.Domain.Responses
{
    public class CategoryResponse
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int TotalStories { get; set; } // Total number of stories in this category
    }
}
