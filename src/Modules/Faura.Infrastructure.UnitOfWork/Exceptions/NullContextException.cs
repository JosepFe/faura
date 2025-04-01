namespace Faura.Infrastructure.UnitOfWork.Exceptions;

using System.Runtime.Serialization;

[Serializable]
public class NullContextException : Exception
{
    public NullContextException() : base()
    {
    }

    public NullContextException(string message) : base(message)
    {
    }

    public NullContextException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected NullContextException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
    {
    }
}