namespace SachNoiOnline.Domain.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } 
        public DateTime? DeletedAt { get; set; }
        public ICollection<Story> Stories { get; set; }
    }
}
