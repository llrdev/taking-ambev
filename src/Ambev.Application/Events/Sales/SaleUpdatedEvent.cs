using Ambev.Domain.Base;
using Ambev.Domain.Entities;

namespace Ambev.Application.Events.Sales;

public class SaleUpdatedEvent : BaseEvent
{
    public SaleUpdatedEvent(Sale sale) : base("Sale")
    {
        Id = sale.Id;
        UpdatedAt = sale.UpdatedAt;
    }

    public int Id { get; set; }
    public DateTime UpdatedAt { get; set; }
}