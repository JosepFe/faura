namespace Faura.Infrastructure.UnitOfWork.Exceptions;

public class NullTransactionException : Exception
{
    public NullTransactionException()
    {
    }

    public NullTransactionException(string message)
        : base(message)
    {
    }

    public NullTransactionException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
