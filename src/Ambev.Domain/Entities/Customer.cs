﻿using Ambev.Domain.Base;

namespace Ambev.Domain.Entities;

public class Customer : BaseEntity
{
    public string? Name { get; set; }
    public string? Document { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
}