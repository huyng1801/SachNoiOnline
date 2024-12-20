namespace SachNoiOnline.Domain.ApplicationExceptions
{
    public class ConflictException : ApplicationException
    {
        public ConflictException(string message)
            : base(message)
        {
            StatusCode = 409; 
        }
    }
}
