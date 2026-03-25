using AwesomeAssertions;
using RecruProject.Core.Models;
using RecruProject.Infrastructure.Repositories;

namespace RecruProject.Infrastructure.Tests.Repositories;

public class OrderRepositoryTests
{
    // I assume that the ConcurrentDictionary is an external resource and preloaded data can be used for test purpose
    // These tests are more like integration tests rather that unit tests because we have "technically" external resource in a form of ConcurrentDictionary
    private readonly OrderRepository _orderRepository;

    public OrderRepositoryTests()
    {
        _orderRepository = new OrderRepository();

    }
    
    [Fact]
    public async Task GetOrderAsync_ShouldReturnOrderName_WhenOrderExists()
    {
        // Arrange
        await _orderRepository.AddOrderAsync(new Order { Id = 1, Description = "Laptop"});
        
        // Act
        var result = await _orderRepository.GetOrderAsync(1);

        // Assert
        result.Id.Should().Be(1);
        result.Description.Should().Be("Laptop");
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetOrderAsync_ShouldReturnThrowException_WhenOrderIdIsZeroOrSmallerThanZero(int orderId)
    {
        // Arrange
        
        // Act
        var act = async () => await _orderRepository.GetOrderAsync(orderId);

        // Assert
        await act
            .Should().ThrowAsync<ArgumentOutOfRangeException>().WithParameterName($"{orderId}");
    }
    
    [Fact]
    public async Task GetOrderAsync_ShouldReturnThrowException_WhenOrderIdDoesNotExist()
    {
        // Arrange
        
        // Act
        var act = async () => await _orderRepository.GetOrderAsync(100);

        // Assert
        await act
            .Should().ThrowAsync<KeyNotFoundException>();
    }
}