﻿using Ambev.Domain.Base;

namespace Ambev.Domain.Entities;

public class Branch : BaseEntity
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public bool IsActive { get; set; }
}