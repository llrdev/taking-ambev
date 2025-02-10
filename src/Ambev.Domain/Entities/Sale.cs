﻿using Ambev.Domain.Base;
using Ambev.Domain.Enums;

namespace Ambev.Domain.Entities;

public class Sale : BaseEntity
{
    public SaleStatus Status { get; set; }
    public DateTime Date { get; set; }
    public int CustomerId { get; set; }
    public int BranchId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime? CancelledAt { get; set; }

    public virtual Customer? Customer { get; set; }
    public virtual Branch? Branch { get; set; }
    public virtual List<SaleItem>? Items { get; set; }
}