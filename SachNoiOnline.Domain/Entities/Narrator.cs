namespace SachNoiOnline.Domain.Entities
{
    public class Narrator
    {
        public int Id { get; set; }
        public string NarratorName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; } 
        public DateTime? DeletedAt { get; set; } 
        public ICollection<Story> Stories { get; set; }
    }
}
