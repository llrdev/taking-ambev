using Ambev.Domain.Base;
using Ambev.Domain.Entities;

namespace Ambev.Application.Events.Sales
{
    public class SaleCreatedEvent : BaseEvent
    {
        public SaleCreatedEvent(Sale sale) : base("Sale")
        {
            Id = sale.Id;
            Date = sale.Date;
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
    }
}
