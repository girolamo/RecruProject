using RecruProject.Core.Models;

namespace RecruProject.Core.Services;

public interface IOrderService
{ 
    Task<Order?> ProcessOrderAsync(int orderId);
    Task<int?> AddOrderAsync(Order order);
}