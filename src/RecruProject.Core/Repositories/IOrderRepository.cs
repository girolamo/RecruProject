using RecruProject.Core.Models;

namespace RecruProject.Core.Repositories;

public interface IOrderRepository
{
    Task<Order> GetOrderAsync(int orderId);
    Task<int?> AddOrderAsync(Order order);
}