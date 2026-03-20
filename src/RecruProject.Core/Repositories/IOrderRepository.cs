using RecruProject.Core.Models;

namespace RecruProject.Core.Repositories;

public interface IOrderRepository
{
    Task<string> GetOrderAsync(int orderId);
    Task AddOrderAsync(Order order);
}