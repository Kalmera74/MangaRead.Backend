public class DuplicateEntityException : Exception
{
    public DuplicateEntityException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
