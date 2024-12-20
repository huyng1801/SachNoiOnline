namespace SachNoiOnline.Domain.ApplicationExceptions
{
    public class InternalServerErrorException : ApplicationException
    {
        public InternalServerErrorException(string message)
            : base(message)
        {
            StatusCode = 500; 
        }
    }
}
