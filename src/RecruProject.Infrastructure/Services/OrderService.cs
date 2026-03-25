using RecruProject.Core.Logger;
using RecruProject.Core.Models;
using RecruProject.Core.Repositories;
using RecruProject.Core.Services;
using RecruProject.Core.Validators;

namespace RecruProject.Infrastructure.Services;

public class OrderService(
    IOrderRepository orderRepository,
    IOrderValidator orderValidator,
    INotificationService notificationService,
    ILogger logger) : IOrderService
{
    public async Task<Order?> ProcessOrderAsync(int orderId)
    {
        logger.LogInfo($"Validating order. OrderId: {orderId}");
        var isOrderValid = orderValidator.IsValid(orderId);
        if (!isOrderValid)
        {
            logger.LogInfo($"Order validation failed. OrderId: {orderId}. Reason: OrderId cannot be zero or negative.");
            return null;
        }
        logger.LogInfo($"Order validation passed. OrderId: {orderId}");
        
        logger.LogInfo($"Processing order started. OrderId: {orderId}");
        try
        {
            var order = await orderRepository.GetOrderAsync(orderId);
            
            var processingDoneMessage = $"Processing order finished. OrderId: {orderId} OrderName: {order.Description}";
            logger.LogInfo(processingDoneMessage);
            await notificationService.Send(processingDoneMessage);

            return order;
        }
        catch (Exception ex)
        {
            logger.LogError($"Exception thrown during processing orderId: {orderId}", ex);
        }

        return null;
    }

    public async Task<int?> AddOrderAsync(Order order)
    {
        logger.LogInfo($"Validating order. OrderId: {order.Id}");
        var isOrderValid = orderValidator.IsValid(order.Id);
        if (!isOrderValid)
        {
            logger.LogInfo($"Order validation failed. OrderId: {order.Id}. Reason: OrderId cannot be zero or negative.");
            return null;
        }
        logger.LogInfo($"Order validation passed. OrderId: {order.Id}");
        
        logger.LogInfo($"Adding new order. OrderId: {order.Id}, OrderName: {order.Description}.");
        try
        {
            var orderId = await orderRepository.AddOrderAsync(order);
            
            var addingDoneMessage = $"New order added. OrderId: {orderId}, OrderName: {order.Description}.";
            logger.LogInfo(addingDoneMessage);
            await notificationService.Send(addingDoneMessage);

            return orderId;
        }
        catch (Exception ex)
        {
            logger.LogError($"Exception thrown during add order operation. OrderId: {order.Id}", ex);
        }

        return null;
    }
}