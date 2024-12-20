namespace SachNoiOnline.Domain.ApplicationExceptions
{
    public class ValidationException : ApplicationException
    {
        public IEnumerable<string> Errors { get; set; }

        public ValidationException(IEnumerable<string> errors)
            : base("One or more validation errors occurred.")
        {
            StatusCode = 400; 
            Errors = errors;
        }

        public ValidationException(string error)
            : this(new[] { error })
        {
        }
    }
}
