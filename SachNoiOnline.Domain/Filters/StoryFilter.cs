using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SachNoiOnline.Domain.Filters
{
    public class StoryFilter
    {
        public int? AuthorId { get; set; } 
        public int? CategoryId { get; set; }
        public int? NarratorId { get; set; } 
    }
}
