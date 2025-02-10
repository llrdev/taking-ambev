using Ambev.Domain.Base;

namespace Ambev.Domain.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string message) : base(message)
    {
    }
}