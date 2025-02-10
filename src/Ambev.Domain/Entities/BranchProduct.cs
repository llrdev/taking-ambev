﻿using Ambev.Domain.Base;
using Ambev.Domain.Enums;

namespace Ambev.Domain.Entities;

public class BranchProduct : BaseEntity
{
    public int BranchId { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public ProductCategory ProductCategory { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; }

    public virtual Product? Product { get; set; }
    public virtual Branch? Branch { get; set; }
}