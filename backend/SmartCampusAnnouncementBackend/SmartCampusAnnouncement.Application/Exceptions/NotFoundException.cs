namespace SmartCampusAnnouncement.Application.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public static NotFoundException For<TEntity>(int id)
    {
        string entityName = typeof(TEntity).Name;
        return new NotFoundException($"{entityName} with id '{id}' was not found.");
    }
}
