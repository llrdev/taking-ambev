﻿using Ambev.Domain.Enums;

namespace Ambev.Application.DTOs.Products;

public class ProductGetResponseDTO
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Name { get; set; }
    public ProductCategory Category { get; set; }
    public decimal BasePrice { get; set; }
    public bool IsActive { get; set; }
}