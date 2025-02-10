using Ambev.Domain.Base;

namespace Ambev.Domain.Exceptions;

public class EntityAlreadyDeletedException : BaseException
{
    public EntityAlreadyDeletedException(string message) : base(message)
    {
    }
}