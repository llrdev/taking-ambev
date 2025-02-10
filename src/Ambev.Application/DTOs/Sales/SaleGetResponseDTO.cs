﻿using Ambev.Domain.Enums;

namespace Ambev.Application.DTOs.Sales;

public class SaleGetResponseDTO
{
    public int Id { get; set; }
    public SaleStatus Status { get; set; }
    public DateTime Date { get; set; }
    public int CustomerId { get; set; }
    public int BranchId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime? CancelledAt { get; set; }
}