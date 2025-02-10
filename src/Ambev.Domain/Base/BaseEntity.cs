using Ambev.Domain.Base.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Ambev.Domain.Base;

[ExcludeFromCodeCoverage]
public abstract class BaseEntity : IBaseEntity
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}