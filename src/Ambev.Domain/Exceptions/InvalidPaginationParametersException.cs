using Ambev.Domain.Base;

namespace Ambev.Domain.Exceptions;

public class InvalidPaginationParametersException : BaseException
{
    public InvalidPaginationParametersException(string message) : base(message)
    {
    }
}