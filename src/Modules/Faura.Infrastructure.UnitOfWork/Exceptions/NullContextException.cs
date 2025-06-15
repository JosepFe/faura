namespace Faura.Infrastructure.UnitOfWork.Exceptions;

public class NullContextException : Exception
{
    public NullContextException()
    {
    }

    public NullContextException(string message)
        : base(message)
    {
    }

    public NullContextException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
