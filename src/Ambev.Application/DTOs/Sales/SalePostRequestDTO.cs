namespace Ambev.Application.DTOs.Sales;

public class SalePostRequestDTO
{
    public int CustomerId { get; set; }
    public int BranchId { get; set; }

    public List<SaleItemPostDTO>? Items { get; set; }
}