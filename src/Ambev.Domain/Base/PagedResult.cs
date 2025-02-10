using System.Diagnostics.CodeAnalysis;

namespace Ambev.Domain.Base;

[ExcludeFromCodeCoverage]
public class PagedResult<T>
{
    public int Total { get; set; }

    public List<T> Items { get; set; }

    public PagedResult(int total, List<T> items)
    {
        Total = total;
        Items = items;
    }
}