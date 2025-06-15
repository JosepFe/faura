namespace Faura.Infrastructure.UnitOfWork.Exceptions;

/// <summary>
/// Exception for errors during repository operations (create, update, delete)
/// </summary>
public class RepositoryOperationException : RepositoryException
{
    public RepositoryOperationException()
    {
    }

    public RepositoryOperationException(string message)
        : base(message)
    {
    }

    public RepositoryOperationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
