namespace Faura.Infrastructure.UnitOfWork.Exceptions;

using System.Runtime.Serialization;

[Serializable]
public class NullTransactionException : Exception
{
    public NullTransactionException() : base()
    {
    }

    public NullTransactionException(string message) : base(message)
    {
    }

    public NullTransactionException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected NullTransactionException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
    {
    }
}