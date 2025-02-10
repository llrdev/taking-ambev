using Ambev.Domain.Base;

namespace Ambev.Domain.Exceptions;

public class ServiceException : BaseException
{

    public ServiceException(string message, Exception innerException) : base(message, innerException)
    {
    }
}