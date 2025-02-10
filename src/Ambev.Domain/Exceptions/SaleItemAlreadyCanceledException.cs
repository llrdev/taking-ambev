using Ambev.Domain.Base;

namespace Ambev.Domain.Exceptions;

public class SaleItemAlreadyCanceledException : BaseException
{
    public SaleItemAlreadyCanceledException(string message) : base(message)
    {
    }
}
