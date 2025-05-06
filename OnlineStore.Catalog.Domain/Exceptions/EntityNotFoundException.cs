namespace OnlineStore.Catalog.Domain.Exceptions
{
    public class EntityNotFoundException(string message) : Exception(message)
    { }
}
