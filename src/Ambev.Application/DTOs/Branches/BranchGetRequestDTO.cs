﻿namespace Ambev.Application.DTOs.Branches;

public class BranchGetRequestDTO
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public bool? IsActive { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int Page { get; set; } = 1;
    public int MaxResults { get; set; } = 10;
}