namespace Faura.Infrastructure.UnitOfWork.Exceptions;

/// <summary>
/// Base exception for repository-related errors.
/// </summary>
public abstract class RepositoryException : Exception
{
    protected RepositoryException()
    {
    }

    protected RepositoryException(string message)
        : base(message)
    {
    }

    protected RepositoryException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
