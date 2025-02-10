using Ambev.Domain.Base;

namespace Ambev.Domain.Exceptions;

public class SaleAlreadyCanceledException : BaseException
{
    public SaleAlreadyCanceledException(string message) : base(message)
    {
    }
}
