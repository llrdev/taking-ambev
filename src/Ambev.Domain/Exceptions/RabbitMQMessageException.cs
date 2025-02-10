using Ambev.Domain.Base;

namespace Ambev.Domain.Exceptions;

public class RabbitMQMessageException : BaseException
{
    public RabbitMQMessageException(string message) : base(message)
    {
    }

    public RabbitMQMessageException(string message, Exception innerException) : base(message, innerException)
    {
    }
}