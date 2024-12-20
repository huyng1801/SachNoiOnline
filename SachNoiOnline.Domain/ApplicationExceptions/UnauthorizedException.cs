namespace SachNoiOnline.Domain.ApplicationExceptions
{
    public class UnauthorizedException : ApplicationException
    {
        public UnauthorizedException(string message)
            : base(message)
        {
            StatusCode = 401; // HTTP status code for "Unauthorized"
        }
    }
}
