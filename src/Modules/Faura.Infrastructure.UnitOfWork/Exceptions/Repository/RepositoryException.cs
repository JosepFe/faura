namespace Faura.Infrastructure.UnitOfWork.Exceptions.Repository;

/// <summary>
/// Base exception for repository-related errors
/// </summary>
public abstract class RepositoryException : UnitOfWorkException
{
    protected RepositoryException(string message) : base(message)
    {
    }

    protected RepositoryException(string message, Exception innerException) : base(message, innerException)
    {
    }
}