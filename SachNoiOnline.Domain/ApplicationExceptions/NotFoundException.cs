namespace SachNoiOnline.Domain.ApplicationExceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string entityName, object key)
            : base($"Entity {entityName} with key {key} was not found.")
        {
            StatusCode = 404;
        }
    }
}
