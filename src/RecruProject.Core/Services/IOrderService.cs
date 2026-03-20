using RecruProject.Core.Models;

namespace RecruProject.Core.Services;

public interface IOrderService
{ 
    Task ProcessOrderAsync(int orderId);
    Task AddOrderAsync(Order order);
}