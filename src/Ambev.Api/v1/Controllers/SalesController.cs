﻿using Ambev.Application.DTOs.Sales;
using Ambev.Application.Mappers.Sales;
using Ambev.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ambev_server.v1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class SalesController : ControllerBase
{
    private readonly ISaleService _saleService;

    public SalesController(ISaleService saleService)
    {
        _saleService = saleService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SaleGetRequestDTO>>> GetAsync([FromQuery] SaleGetRequestDTO request)
    {
        var sales = await _saleService.GetAllAsync(request.Id,
                                                   request.BranchId,
                                                   request.CustomerId,
                                                   request.Status,
                                                   request.StartDate,
                                                   request.EndDate,
                                                   request.Page,
                                                   request.MaxResults);

        var response = sales.ToDTO();

        if (response is not null && response.Any())
            return Ok(response);

        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SaleGetDetailResponseDTO>> GetAsync([FromRoute] int id)
    {
        var sale = await _saleService.GetByIdAsync(id);

        if (sale is null)
            return NoContent();

        var response = sale.ToDetailDTO();
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<SalePostResponseDTO>> PostAsync([FromBody] SalePostRequestDTO request)
    {
        var createdSale = await _saleService.CreateAsync(request.ToEntity());
        var response = createdSale.ToPostResponseDTO();
        return Created(string.Empty, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync([FromRoute] int id, [FromBody] SalePutRequestDTO request)
    {
        var sale = await _saleService.UpdateAsync(id, request.ToEntity());
        return Ok(sale.ToPutResponseDTO());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _saleService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelAsync([FromRoute] int id)
    {
        await _saleService.CancelAsync(id);

        return NoContent();
    }

    [HttpPut("{id}/Items/{sequence}/cancel")]
    public async Task<IActionResult> CancelItemAsync([FromRoute] int id, [FromRoute] int sequence)
    {
        var sale = await _saleService.CancelItemAsync(id, sequence);

        return Ok(sale.ToDetailDTO());
    }

    [HttpGet("{id}/Items/{sequence}")]
    public async Task<IActionResult> GetItemAsync([FromRoute] int id, [FromRoute] int sequence)
    {
        var saleItem = await _saleService.GetItemAsync(id, sequence);

        return Ok(saleItem.ToDetailDTO());
    }
}