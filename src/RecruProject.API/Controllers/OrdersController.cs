using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RecruProject.API.DTOs;
using RecruProject.Core.Models;
using RecruProject.Core.Services;

namespace RecruProject.API.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController(IOrderService orderService, IMapper mapper) : ControllerBase
{
    [HttpGet("{orderId:int}")]
    public async Task<IActionResult> GetAsync([FromRoute] int orderId)
    {
        var order = await orderService.ProcessOrderAsync(orderId);
        if (order is null)
        {
            return BadRequest();    
        }
        
        var orderResponse = mapper.Map<Order, OrderResponseDto>(order);
        
        return Ok(orderResponse);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] OrderRequestDto orderRequest)
    {
        var order = mapper.Map<OrderRequestDto, Order>(orderRequest);
        var orderId = await orderService.AddOrderAsync(order);
        
        return Created($"/orders/{orderId}", mapper.Map<Order, OrderResponseDto>(order));
    }
}