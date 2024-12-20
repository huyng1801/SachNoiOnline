namespace SachNoiOnline.Domain.ApplicationExceptions
{
    public class ForbiddenException : ApplicationException
    {
        public ForbiddenException(string message)
            : base(message)
        {
            StatusCode = 403;
        }
    }
}
