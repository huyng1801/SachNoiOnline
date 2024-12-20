using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SachNoiOnline.Domain.ValueObjects
{
    public class Pagination
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public Pagination(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize < 1 ? 10 : pageSize;
        }
    }
}
