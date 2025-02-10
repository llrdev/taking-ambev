using System.Diagnostics.CodeAnalysis;

namespace Ambev.Domain.Base;

[ExcludeFromCodeCoverage]
public class BaseEvent
{
    public BaseEvent(string domain)
    {
        Domain = domain;
    }

    public string Domain { get; set; }
}