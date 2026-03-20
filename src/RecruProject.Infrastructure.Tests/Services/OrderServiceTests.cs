using Moq;
using RecruProject.Core.Logger;
using RecruProject.Core.Models;
using RecruProject.Core.Repositories;
using RecruProject.Core.Services;
using RecruProject.Core.Validators;
using RecruProject.Infrastructure.Services;

namespace RecruProject.Infrastructure.Tests.Services;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly Mock<IOrderValidator> _mockOrderValidator;
    private readonly Mock<INotificationService> _notificationService;
    private readonly Mock<ILogger> _mockLogger;
    
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        _mockOrderValidator = new Mock<IOrderValidator>();
        _notificationService = new Mock<INotificationService>();
        _mockLogger = new Mock<ILogger>();
        
        _orderService = new OrderService(_mockOrderRepository.Object, _mockOrderValidator.Object, _notificationService.Object, _mockLogger.Object);
        
        _mockOrderValidator.Setup(f => f.IsValid(0)).Returns(false);
        _mockOrderValidator.Setup(f => f.IsValid(1)).Returns(true);
        _mockOrderValidator.Setup(f => f.IsValid(2)).Returns(true);

        _mockOrderRepository.Setup(f => f.GetOrderAsync(1)).ReturnsAsync("TestOrderName1");
        _mockOrderRepository.Setup(f => f.GetOrderAsync(2)).ThrowsAsync(new Exception("Test exception"));
    }

    [Fact]
    public async Task ProcessOrderAsync_ShouldSucceed_WhenOrderIdIsValid()
    {
        // Arrange
        
        // Act
        await _orderService.ProcessOrderAsync(1);

        // Assert
        _mockOrderValidator.Verify(f => f.IsValid(1), Times.Once);
        _mockOrderRepository.Verify(f => f.GetOrderAsync(1), Times.Once);
        _mockLogger.Verify(f => f.LogInfo(It.IsAny<string>()), Times.Exactly(4));
        _mockLogger.Verify(f => f.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
        _notificationService.Verify(f => f.Send(It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public async Task ProcessOrderAsync_ShouldFail_WhenOrderIdIsInvalid()
    {
        // Arrange
        
        // Act
        await _orderService.ProcessOrderAsync(0);

        // Assert
        _mockOrderValidator.Verify(f => f.IsValid(0), Times.Once);
        _mockOrderRepository.Verify(f => f.GetOrderAsync(0), Times.Never);
        _mockLogger.Verify(f => f.LogInfo(It.IsAny<string>()), Times.Exactly(2));
        _mockLogger.Verify(f => f.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
        _notificationService.Verify(f => f.Send(It.IsAny<string>()), Times.Never);
    }
    
    [Fact]
    public async Task ProcessOrderAsync_ShouldFail_WhenOrderIdIsValidButRepositoryThrowsException()
    {
        // Arrange
        
        // Act
        await _orderService.ProcessOrderAsync(2);

        // Assert
        _mockOrderValidator.Verify(f => f.IsValid(2), Times.Once);
        _mockOrderRepository.Verify(f => f.GetOrderAsync(2), Times.Once);
        _mockLogger.Verify(f => f.LogInfo(It.IsAny<string>()), Times.Exactly(3));
        _mockLogger.Verify(f => f.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        _notificationService.Verify(f => f.Send(It.IsAny<string>()), Times.Never);
    }
    
    [Fact]
    public async Task AddOrderAsync_ShouldSucceed_WhenOrderIsValid()
    {
        // Arrange
        var newOrder = new Order { Id = 1, Description = "TestOrderName" };
        
        // Act
        await _orderService.AddOrderAsync(newOrder);

        // Assert
        _mockOrderValidator.Verify(f => f.IsValid(1), Times.Once);
        _mockOrderRepository.Verify(f => f.AddOrderAsync(newOrder), Times.Once);
        _mockLogger.Verify(f => f.LogInfo(It.IsAny<string>()), Times.Exactly(4));
        _mockLogger.Verify(f => f.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
        _notificationService.Verify(f => f.Send(It.IsAny<string>()), Times.Once);
    }
    
    [Fact]
    public async Task AddOrderAsync_ShouldFail_WhenOrderIsInvalid()
    {
        // Arrange
        var newOrder = new Order { Id = 0, Description = "TestOrderName" };
        
        // Act
        await _orderService.AddOrderAsync(newOrder);

        // Assert
        _mockOrderValidator.Verify(f => f.IsValid(0), Times.Once);
        _mockOrderRepository.Verify(f => f.AddOrderAsync(newOrder), Times.Never);
        _mockLogger.Verify(f => f.LogInfo(It.IsAny<string>()), Times.Exactly(2));
        _mockLogger.Verify(f => f.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
        _notificationService.Verify(f => f.Send(It.IsAny<string>()), Times.Never);
    }
    
    [Fact]
    public async Task AddOrderAsync_ShouldFail_WhenOrderIdIsValidButRepositoryThrowsException()
    {
        // Arrange
        var newOrder = new Order { Id = 2, Description = "TestOrderName" };
        _mockOrderRepository.Setup(f => f.AddOrderAsync(newOrder)).ThrowsAsync(new Exception("Test exception"));
        
        // Act
        await _orderService.AddOrderAsync(newOrder);

        // Assert
        _mockOrderValidator.Verify(f => f.IsValid(2), Times.Once);
        _mockOrderRepository.Verify(f => f.AddOrderAsync(newOrder), Times.Once);
        _mockLogger.Verify(f => f.LogInfo(It.IsAny<string>()), Times.Exactly(3));
        _mockLogger.Verify(f => f.LogError(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        _notificationService.Verify(f => f.Send(It.IsAny<string>()), Times.Never);
    }
}