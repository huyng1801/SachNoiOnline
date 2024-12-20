using System;
using System.Collections.Generic;

namespace SachNoiOnline.Domain.Responses
{
    public class NarratorResponse
    {
        public int Id { get; set; }

        public string NarratorName { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public int TotalStories { get; set; }
    }
}
