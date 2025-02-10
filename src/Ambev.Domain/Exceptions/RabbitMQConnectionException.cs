using Ambev.Domain.Base;

namespace Ambev.Domain.Exceptions;

public class RabbitMQConnectionException : BaseException
{
    public RabbitMQConnectionException(string message) : base(message)
    {
    }
}