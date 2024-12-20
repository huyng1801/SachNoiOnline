using SachNoiOnline.Domain.ApplicationExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SachNoiOnline.Domain.ApplicationExceptions
{
    public class ApplicationException : Exception
    {
        public int StatusCode { get; set; }
        public ApplicationException(string message) : base(message)
        {
        }

        public ApplicationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
      
    }
}
//SachNoiOnline.Domain
//│
//├── ApplicationExceptions
//│   ├── ApplicationException.cs
//│   ├── NotFoundException.cs
//│   ├── ValidationException.cs
//│   ├── ForbiddenException.cs
//│   ├── ConflictException.cs
//│   ├── UnauthorizedException.cs
//│   └── InternalServerErrorException.cs
//└── Entities
