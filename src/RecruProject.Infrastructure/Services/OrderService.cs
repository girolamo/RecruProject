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
    public async Task ProcessOrderAsync(int orderId)
    {
        logger.LogInfo($"Validating order. OrderId: {orderId}");
        var isOrderValid = orderValidator.IsValid(orderId);
        if (!isOrderValid)
        {
            logger.LogInfo($"Order validation failed. OrderId: {orderId}. Reason: OrderId cannot be zero or negative.");
            return;
        }
        logger.LogInfo($"Order validation passed. OrderId: {orderId}");
        
        logger.LogInfo($"Processing order started. OrderId: {orderId}");
        try
        {
            var orderName = await orderRepository.GetOrderAsync(orderId);
            
            var processingDoneMessage = $"Processing order finished. OrderId: {orderId} OrderName: {orderName}";
            logger.LogInfo(processingDoneMessage);
            await notificationService.Send(processingDoneMessage);
        }
        catch (Exception ex)
        {
            logger.LogError($"Exception thrown during processing orderId: {orderId}", ex);
        }
    }

    public async Task AddOrderAsync(Order order)
    {
        logger.LogInfo($"Validating order. OrderId: {order.Id}");
        var isOrderValid = orderValidator.IsValid(order.Id);
        if (!isOrderValid)
        {
            logger.LogInfo($"Order validation failed. OrderId: {order.Id}. Reason: OrderId cannot be zero or negative.");
            return;
        }
        logger.LogInfo($"Order validation passed. OrderId: {order.Id}");
        
        logger.LogInfo($"Adding new order. OrderId: {order.Id}, OrderName: {order.Description}.");
        try
        {
            await orderRepository.AddOrderAsync(order);
            
            var addingDoneMessage = $"New order added. OrderId: {order.Id}, OrderName: {order.Description}.";
            logger.LogInfo(addingDoneMessage);
            await notificationService.Send(addingDoneMessage);
        }
        catch (Exception ex)
        {
            logger.LogError($"Exception thrown during add order operation. OrderId: {order.Id}", ex);
        }
    }
}