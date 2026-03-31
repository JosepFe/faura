namespace Faura.Infrastructure.UnitOfWork.Exceptions;

using System.Runtime.Serialization;

/// <summary>
/// Base exception for all Unit of Work related errors
/// </summary>
[Serializable]
public abstract class UnitOfWorkException : Exception
{
    protected UnitOfWorkException() : base()
    {
    }

    protected UnitOfWorkException(string message) : base(message)
    {
    }

    protected UnitOfWorkException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected UnitOfWorkException(SerializationInfo serializationInfo, StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
    {
    }
}
