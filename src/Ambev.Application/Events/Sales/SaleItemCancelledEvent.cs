﻿using Ambev.Domain.Base;
using Ambev.Domain.Entities;

namespace Ambev.Application.Events.Sales;

public class SaleItemCancelledEvent : BaseEvent
{
    public SaleItemCancelledEvent(SaleItem saleItem) : base("Sale")
    {
        SaleId = saleItem.SaleId;
        SaleItemId = saleItem.Id;
        Sequence = saleItem.Sequence;
        CancelledAt = saleItem.CancelledAt ?? DateTime.Now;
    }

    public int SaleId { get; set; }
    public int SaleItemId { get; set; }
    public short Sequence { get; set; }
    public DateTime CancelledAt { get; set; }
}
