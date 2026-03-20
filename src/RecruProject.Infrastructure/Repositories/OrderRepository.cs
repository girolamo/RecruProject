using System.Collections.Concurrent;
using RecruProject.Core.Models;
using RecruProject.Core.Repositories;

namespace RecruProject.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    // In practice, this part wouldn't in production code
    // In real world scenario, repository would interact with external resource (e.g. db)
    // Because in this task, we don't have REAL external resource, ConcurrentDictionary is used as substitute
    private static readonly ConcurrentDictionary<int, Order> Orders = new();
    
    public async Task<string> GetOrderAsync(int orderId)
    {
        await Task.Delay(100);
        
        if (orderId <= 0)
        {
            throw new ArgumentOutOfRangeException($"{orderId}", "OrderId cannot be zero or negative.");
        }
        
        var orderFound = Orders.TryGetValue(orderId, out var order);
        if (!orderFound || order is null)
        {
            throw new KeyNotFoundException($"Order with id {orderId} not found.");
        }
        
        return order.Description;
    }

    public async Task AddOrderAsync(Order order)
    { 
        await Task.Delay(100);
        
        var addedSuccessfully = Orders.TryAdd(order.Id, order);
        if (!addedSuccessfully)
        {
            throw new ArgumentException($"Order with id {order.Id} already exists.");
        }
    }
}