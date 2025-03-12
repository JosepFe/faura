namespace Faura.Infrastructure.UnitOfWork.Exceptions;

/// <summary>
/// Exception for errors during repository queries
/// </summary>
public class RepositoryQueryException : RepositoryException
{
    public RepositoryQueryException(string message) : base(message)
    {
    }

    public RepositoryQueryException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
