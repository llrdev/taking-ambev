using Ambev.Domain.Base;
using Ambev.Domain.Entities;

namespace Ambev.Application.Events.Sales;

public class SaleCancelledEvent : BaseEvent
{
    public SaleCancelledEvent(Sale sale) : base("Sale")
    {
        Id = sale.Id;
        CancelledAt = sale.CancelledAt ?? DateTime.Now;
    }

    public int Id { get; set; }
    public DateTime CancelledAt { get; set; }
}