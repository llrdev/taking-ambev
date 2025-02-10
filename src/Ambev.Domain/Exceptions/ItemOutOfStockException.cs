using Ambev.Domain.Base;

namespace Ambev.Domain.Exceptions;

public class ItemOutOfStockException : BaseException
{
    public ItemOutOfStockException(string message) : base(message)
    {
    }
}